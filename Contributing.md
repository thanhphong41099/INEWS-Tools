# Hướng dẫn đóng góp cho INEWS Tools

## Yêu cầu môi trường
- Visual Studio 2019 hoặc 2022 với workload **.NET Desktop Development**.
- .NET Framework 4.8 SDK (được cài cùng Visual Studio).
- Khả năng truy cập tới máy chủ iNews Web Service (địa chỉ trong `app.config`).

## Chuẩn bị cấu hình
1. Sao chép `API_iNews/app.config` và cập nhật các khóa sau theo môi trường nội bộ:
   - `ServerIP`: địa chỉ TCP server nội bộ.
   - `iNewsServer`, `iNewsServerBackup`: endpoint SOAP.
   - `iNewsUser`, `iNewsPass`: tài khoản thử nghiệm.
   - `WorkingFolder`: thư mục ghi file tạm.
2. Không commit thông tin nhạy cảm lên repository công khai.

## Quy trình làm việc
1. Fork repository và tạo nhánh theo tính năng (ví dụ `feature/export-json`).
2. Thực hiện chỉnh sửa, bảo đảm giữ nguyên mô hình phân lớp (`API` UI ↔ `ServerAPI` ↔ `iNewsData`).
3. Kiểm tra thủ công (xem mục “Kiểm thử” bên dưới).
4. Cập nhật tài liệu (README/ProjectOverview/ModuleGuide) nếu thay đổi workflow kết nối hoặc cấu hình.
5. Tạo Pull Request mô tả rõ:
   - Mục đích thay đổi.
   - Ảnh hưởng tới workflow kết nối iNews (nếu có).
   - Bằng chứng kiểm thử (log, ảnh chụp màn hình UI).

## Tiêu chuẩn code
- **Naming**: giữ nguyên pattern event handler của WinForms (`btnXuatTroiTin_Click`, `treeView1_AfterSelect`).
- **Cấu hình**: đọc qua `ConfigurationManager.AppSettings`; không hard-code IP/tài khoản.
- **Bất đồng bộ**: sử dụng `Task.Run`/`async` cho thao tác SOAP hoặc IO dài; tránh chặn UI thread.
- **Xử lý lỗi**: log mọi exception thông qua `iNewsData.SentError` hoặc hiển thị `MessageBox`, đảm bảo người dùng biết trạng thái kết nối.
- **Web References**: nếu cập nhật wsdl, commit cả thư mục `Web References` để đồng bộ proxy.

## Kiểm thử đề xuất
- ✅ **Khởi động & kết nối**: chạy ứng dụng, kiểm tra status bar hiển thị IP và không có lỗi kết nối.
- ✅ **Lazy-load queue**: mở rộng vài node trong `treeView1`, đảm bảo dữ liệu tải đúng.
- ✅ **Tải story**: chọn rundown bất kỳ, xác minh `dataGridView1` hiển thị story và `txtContent` có dữ liệu.
- ✅ **Xuất file**: chạy từng nút export và kiểm tra file được tạo trong `WorkingFolder`.
- ✅ **Server TCP**: dùng `ClientForm` gửi `GET_TREE` và `QUEUE|<FullName>` để đảm bảo phản hồi hợp lệ.

## Liên hệ & hỗ trợ
- Đề xuất mọi câu hỏi kỹ thuật qua phần Issues của repository.
- Khi gặp lỗi kết nối iNews, cung cấp log `toolStripStatusLabel1` và giá trị cấu hình liên quan.
