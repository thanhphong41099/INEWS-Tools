# API_iNews

Ứng dụng WinForms kết nối tới máy chủ iNews để truy xuất dữ liệu và vận hành các dịch vụ API.

## Cấu hình

Các thông số kết nối và đường dẫn được cấu hình trong `API_iNews/app.config`. Ứng dụng đọc trực tiếp từ `appSettings` thông qua `ConfigurationManager.AppSettings`.

## Chạy ứng dụng

Mở solution `News2025.sln` trong Visual Studio và thiết lập `API_iNews` làm startup project. Khi chạy, ứng dụng mở trực tiếp form `API` để thao tác với dữ liệu từ iNews.
