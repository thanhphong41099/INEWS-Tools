# Danh mục lớp & module chính

## Bản đồ chức năng
| Tệp/Lớp | Vai trò | Ghi chú workflow |
| --- | --- | --- |
| `Program.cs` | Khởi động ứng dụng WinForms (`Application.Run(new API())`). | Không chỉnh sửa trừ khi thay đổi form khởi động. |
| `API.cs` | Form chính: quản lý kết nối iNews, hiển thị rundown/story, xuất file. | `ConnectServerToLoadDataAsync` đọc app.config, tạo `ServerAPI` và `iNewsData`; `treeView1_*` điều phối việc load queue/story; các nút `Export*` dùng dữ liệu `Content`. |
| `ServerAPI.cs` | TCP server nội bộ. | `ListenForClients` đọc lệnh `GET_TREE`, `QUEUE|...`, sử dụng `iNewsData` để trả dữ liệu; event `Recieve` cập nhật status bar. |
| `ClientAPI.cs` | TCP client tiện ích cho `ClientForm`. | `GetDataFromCmd` gửi lệnh và trả về chuỗi phản hồi. |
| `iNews.cs` | Wrapper kết nối SOAP (class `iNews`) và tầng tiện ích (`iNewsData`). | `iNews` thiết lập `INEWSSystem`, `INEWSQueue`, đăng nhập bằng `Servername/User/Pass`; `iNewsData` cung cấp `GetFolderChildren`, `GetStoriesBoard`, `ChangedQueues`. |
| `ProcessingXMl2Class` | Chuyển NSML sang `DataTable`. | Sử dụng `FieldMapping` từ cấu hình `Fields`; lưu `Content` cho thao tác export. |
| `Utility.cs` | Hàm phụ trợ (split chuỗi, hiển thị XML). | `Utility.Split` dùng để phân tách queue root. |
| `ServerForm` | Form điều khiển server TCP. | Cho phép start/stop server, mở `API` hoặc `ClientForm`. |
| `ClientForm` | Form kiểm thử lệnh TCP. | Buttons `btnGetTree`, `btnSendCommand`; nút `btnGetStories` đang rỗng – nên xử lý hoặc ẩn. |
| `APIV4` | Form kế thừa dùng BarType. | Đọc `BarTypeConfiguration` để map scene/value. |
| `BarTypeConfiguration.cs` | Định nghĩa cấu trúc `BarTypeCollection`. | Phục vụ tính năng BarScene trong `APIV4`. |
| `WebServiceSettings.cs` | DTO chứa `ServerName`, `ServerBackup`, `UserName`, `Password`. | Được `API.ConnectServerToLoadDataAsync` khởi tạo từ app.config. |
| `app.config` | Cấu hình kết nối, thư mục xuất file và khóa regex. | Chỉnh sửa trước khi chạy; cân nhắc bỏ khóa `Phude` nếu không dùng. |

## Sự kiện UI quan trọng
| Control | Event | Mục đích |
| --- | --- | --- |
| `API.treeView1` | `BeforeExpand` | Lazy-load queue con bằng `iNewsData.GetFolderChildren`. |
|  | `AfterSelect` | Lấy stories cho rundown, cập nhật `dataGridView1`, `txtContent`, `txtTroiTin/Cuoi`. |
| `API.dataGridView1` | `CellClick`, `SelectionChanged` (qua `GetDataContent`) | Cập nhật `Content` đang xem. |
| `API.btnExportContentRaw` | `Click` | Lưu story hiện tại kèm tiêu đề. |
| `API.Export3TXTFiles` | `Click` | Gọi `ProcessContentWithCG`/`ProcessContentWithDD` để xuất rút tít & địa danh. |
| `API.btnXuatTroiTin` | `Click` | Gọi `LoadContentTroiTin`, `LoadContentTroiCuoi` rồi ghi ra file. |
| `API.btnExportAllRawContent` | `Click` | Duyệt toàn bộ rundown, xuất tổng hợp nội dung. |
| `ServerForm.btnStartServer` | `Click` | Mở `ServerAPI`. |
| `ClientForm.btnGetTree` | `Click` | Gửi lệnh `GET_TREE` và bind kết quả vào treeview. |

## Điểm vào workflow kết nối iNews
1. `API_Load` → `ConnectServerToLoadDataAsync`.
2. Hàm đọc `ServerIP`, `iNewsServer`, `iNewsServerBackup`, `iNewsUser`, `iNewsPass`, `QueuesRoot`, `QueuesChild`, `WorkingFolder`.
3. Tạo `ServerAPI(serverIP)` và `iNewsData(new WebServiceSettings{...})`.
4. Các phương thức trong bảng dưới sẽ gọi `iNewsData`:

| Phương thức | Input chính | Kết quả |
| --- | --- | --- |
| `iNewsData.GetFolderChildren(string folderParent)` | `folderParent` (node full name). | Danh sách child folder (string). |
| `iNewsData.GetStoriesBoard(string queueName)` | `queueName` (bao gồm hậu tố `QueuesChild` nếu cấu hình). | Danh sách story NSML. |
| `iNewsData.ChangedQueues()` | Không | Danh sách queue thay đổi (dùng cho giám sát). |

## Resource & config
- `WorkingFolder` + đường dẫn con (`Ruttit`, `Diadanh`, `TroiTin`, `TroiCuoi`, `Phude`).
- Các khóa regex `KeyTroiTin`, `KeyTroiCuoi` quyết định pattern tách nội dung.
- `BarScene`, `BarLocaltion` phục vụ `BarTypeConfiguration`.
- URL SOAP nằm trong `applicationSettings` (`API_iNews.Properties.Settings`).

## Thành phần nên rà soát
- `ProcessContentWithPhude` (không được gọi) – nên loại bỏ hoặc kích hoạt tính năng xuất phụ đề.
- `ClientForm.btnGetStories` chưa có logic – dễ gây nhầm lẫn cho người dùng kiểm thử.
- Case `GET_SCROLL_TEXT` trong `ServerAPI` trả chuỗi rỗng – cân nhắc cài đặt hoặc bỏ hẳn.
