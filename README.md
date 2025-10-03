# INEWS Tools

## Tổng quan dự án
INEWS Tools là ứng dụng WinForms dành cho biên tập viên khai thác rundown từ hệ thống iNews. Chương trình hỗ trợ:

- Duyệt cây rundown và xem chi tiết từng story theo node queue của iNews.
- Trích xuất nhanh các trường biên tập (rút tít, địa danh, phần "trôi", nội dung thô) ra tệp văn bản cấu hình sẵn.
- Cung cấp dịch vụ TCP nội bộ (ServerAPI) để client khác yêu cầu dữ liệu rundown/story theo thời gian thực.

Đối tượng sử dụng chính là nhóm biên tập chương trình truyền hình cần theo dõi rundown và chuẩn bị nội dung CG/Phụ đề.

## Cấu trúc thư mục
| Đường dẫn | Mô tả |
| --- | --- |
| `News2025.sln` | Solution duy nhất của dự án WinForms. |
| `API_iNews/Program.cs` | Điểm vào (`Main`) mở form `API`. |
| `API_iNews/API*.cs` | Form chính: dựng cây queue, tải story, preview nội dung và nút xuất file. |
| `API_iNews/App_Code/` | Lớp nghiệp vụ: SOAP client (`iNews`, `iNewsData`), TCP (`ServerAPI`, `ClientAPI`), parser (`ProcessingXMl2Class`), cấu hình bar. |
| `API_iNews/Web References/` | Proxy SOAP sinh từ iNews (`INEWSSystem`, `INEWSQueue`, `INEWSStory`). |
| `API_iNews/app.config` | Toàn bộ tham số kết nối server, thông tin đăng nhập, thư mục xuất file và khóa lọc nội dung. |
| `API_iNews/ServerForm*`, `ClientForm*`, `APIV4*` | Form phụ điều khiển server, client kiểm thử và giao diện kế thừa với BarType. |
| `API_iNews/Properties/` | Assembly info, resource và settings (bao gồm URL web service). |

## Cấu hình kết nối iNews
Mọi thông số kết nối được nạp khi `API` khởi động thông qua `ConfigurationManager.AppSettings`. Những khóa quan trọng:

| Khóa | Vai trò |
| --- | --- |
| `ServerIP` | IP mà `ServerAPI` lắng nghe TCP nội bộ. |
| `iNewsServer` / `iNewsServerBackup` | Hostname/IP dịch vụ iNews Web Service (chính & dự phòng). |
| `iNewsUser` / `iNewsPass` | Tài khoản đăng nhập web service iNews. |
| `QueuesRoot` | Danh sách queue gốc (phân tách `;`) để dựng cây rundown. |
| `QueuesChild` | Queue con mặc định đi kèm mỗi rundown. |
| `WorkingFolder` | Thư mục gốc xuất file text. |
| `Ruttit`, `Diadanh`, `TroiTin`, `TroiCuoi`, `Phude` | Đường dẫn tương đối dưới `WorkingFolder` cho từng loại file xuất. |
| `KeyTroiTin`, `KeyTroiCuoi` | Mẫu khóa (dùng regex) trích phần trôi tin/trôi cuối từ nội dung story. |

Các URL SOAP (`INEWSSystem`, `INEWSQueue`, `INEWSStory`) nằm trong mục `applicationSettings` và có thể chỉnh sửa trong Visual Studio (Project Settings \> Web Settings).

## Workflow kết nối tới iNews
1. `API.ConnectServerToLoadDataAsync` đọc `ServerIP`, khởi động `ServerAPI` và đăng ký sự kiện nhận tin.
2. Hàm này đồng thời xây dựng `WebServiceSettings` từ `iNewsServer`, `iNewsServerBackup`, `iNewsUser`, `iNewsPass` rồi tạo `iNewsData`.
3. Khi cần truy xuất, `iNewsData` khởi tạo `TTDH.iNews`, thiết lập `INEWSSystem` & `INEWSQueue` với `CookieContainer` chung và timeout 5 giây.
4. `iNews` gọi `INEWSSystem.Connect` bằng `ConnectType` chứa `Servername`, `Username`, `Password`. Nếu lỗi kết nối và có `ServerBackup`, hàm thử lại.
5. Sau khi kết nối thành công, các phương thức như `GetFolderChildren` và `GetStoriesBoard` sử dụng proxy trong `Web References` (SOAP) để lấy danh sách folder/story theo queue name.
6. `GetStoriesBoard` gọi `SetCurrentQueue` trên `INEWSQueue`, sau đó `GetStories` với tham số `IsStoryBodyIncluded=true` để lấy NSML; chuỗi kết quả được `ProcessingXMl2Class` chuyển thành `DataTable` và hiển thị lên UI.

Mọi lỗi kết nối hoặc SOAP exception sẽ được chuyển về UI thông qua sự kiện `iNewsData.SentError` để hiển thị trên `statusStrip`.

## Build/Cài đặt/Chạy trên Windows
1. Cài Visual Studio 2019/2022 với workload **.NET Desktop Development**.
2. Mở `News2025.sln`, Visual Studio sẽ tự đặt `API_iNews` làm startup project.
3. Chỉnh sửa `API_iNews/app.config` theo môi trường của bạn (IP server, tài khoản iNews, thư mục xuất file).
4. Build solution ở cấu hình `Debug` hoặc `Release`. Khi build, VS sẽ tự phục hồi các Web Reference đã cấu hình.
5. Nhấn **F5** để chạy. Form `API` sẽ tự khởi động, kết nối đến iNews, dựng cây rundown và mở dịch vụ TCP.

## Hướng dẫn sử dụng chương trình
### Duyệt rundown & xem story
- Mở các node trong `treeView1` để lazy-load queue con. Mỗi node chạy `iNewsData.GetFolderChildren` tương ứng.
- Chọn một rundown để tải stories. `dataGridView1` hiển thị danh sách story, `txtContent` preview chi tiết.

### Xuất dữ liệu biên tập
- **Xuất tất cả ra file**: trích phần CG (`##CG`) và địa danh (`##DD`) từ `Content`, ghi vào các file `Ruttit`, `Diadanh` (UTF-8).
- **Xuất trôi tin - trôi cuối**: dùng khóa trong `KeyTroiTin`, `KeyTroiCuoi` để lọc nội dung, ghi vào `TroiTin` và `TroiCuoi`.
- **Xuất nội dung gốc**: lưu toàn bộ story (kèm tên trong ngoặc vuông) qua `SaveFileDialog`.
- **Xuất toàn bộ nội dung**: duyệt toàn cây rundown, gộp nội dung từng story và ghi ra tệp duy nhất.

### Công cụ phụ trợ
- `ServerForm`: điều khiển `ServerAPI`, mở form chính hoặc client test.
- `ClientForm`: gửi lệnh TCP (`GET_TREE`, `QUEUE|…`) để xác thực dữ liệu trả về từ server.
- `APIV4`: phiên bản giao diện cũ có hỗ trợ cấu hình BarType.

## Dependencies & công nghệ
- **.NET Framework 4.8**, C# WinForms.
- `System.Configuration`, `System.Data`, `System.Net`, `System.IO`, `System.Text.RegularExpressions`.
- Proxy SOAP sinh từ wsdl iNews (INEWSSystem, INEWSQueue, INEWSStory).
- Không sử dụng cơ sở dữ liệu cục bộ; dữ liệu dạng NSML được chuyển sang `DataTable` trong bộ nhớ.

## Lưu ý tối ưu
- Hàm `ProcessContentWithPhude` chưa được sử dụng; có thể cân nhắc loại bỏ cùng khóa `Phude` trong cấu hình.
- Nút "Get Stories" trên `ClientForm` không có xử lý, nên bổ sung logic hoặc ẩn để tránh nhầm lẫn.

