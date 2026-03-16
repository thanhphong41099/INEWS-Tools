# Kế Hoạch Triển Khai Client Kết Nối TCP Server iNews

Tài liệu này mô tả chi tiết luồng logic và định dạng dữ liệu để các Client (Python, Node.js, C#, v.v.) có thể kết nối và lấy dữ liệu an toàn từ TCP Server của hệ thống iNews.

## 1. Nguyên Tắc Cốt Lõi

* **Mô hình giao tiếp:** Stateless (Phi trạng thái), Short-lived (Vòng đời ngắn).
* **Quy trình 1 vòng đời:** Mỗi một yêu cầu (Request) tương ứng với 1 phiên kết nối duy nhất: `Mở Socket -> Gửi lệnh -> Nhận kết quả -> Đóng Socket`.
* **Không Keep-Alive:** Không duy trì Socket sau khi đã nhận xong dữ liệu. Server tự động đóng luồng sau khi phản hồi.
* **Giới hạn Payload:** Chiều dài lệnh gửi đi **tuyệt đối không vượt quá 1024 bytes** (do hạn chế bộ đệm phía Server).

## 2. Tiêu Chuẩn Dữ Liệu

* **Encoding:** `UTF-8` hoặc `ASCII`.
* **Cú pháp lệnh (Command Request):** 
  
  **A. Lệnh lấy tin (Core):**
  * Cú pháp chuẩn: `QUEUE|<Tên_Queue>`
  * Ví dụ hợp lệ: `QUEUE|VTV4.04_VO_BAN_TIN.BAN_TIN_9H00` (Có thể dùng `|` hoặc `#` làm ký tự phân cách).
  * *Dữ liệu trả về:* Chuỗi XML thuần chứa thông tin từ iNews (định dạng `DataTable`).

  **B. Các lệnh phụ trợ khác hiện có:**
  * `TRIGGER_EXTRACT|<Tên_Queue>`: Yêu cầu Server tự động tóm lấy nội dung Queue và xuất thành file `videoID_list.xml` lưu tại ổ cứng của Server (ví dụ: `D:\TEST\XML\...`). **Server sẽ trả về chữ "OK" ngay lập tức**, sau đó tiến trình xuất file mới diễn ra ngầm.
  * `GET_TREE`: Yêu cầu lấy cấu trúc thư mục từ cấu hình `QueuesRoot`. Trả về danh sách chuỗi thư mục, phân cách bằng dấu `|`.
  * `RESTART_SV`: Yêu cầu Server tự khởi động lại (gọi file `restart.bat`). Trả về mã thoát (ExitCode).
  * `TESTDATA`: Yêu cầu lấy dữ liệu giả lập (dùng cho môi trường dev).
  * `GET_SCROLL_TEXT`: Hiện tại trả về chuỗi rỗng (không chứa logic thiết thực).

## 3. Luồng Thực Thi (Workflow)

Client cần thực hiện tuần tự 4 bước sau cho **MỖI** lần muốn lấy dữ liệu:

### Bước 1: Khởi tạo kết nối (Connect)
* Mở TCP Socket tới IP của Server và **Port 3000**.
* Thiết lập **Connect Timeout** (VD: 5 giây). Nếu thất bại, Server đang bảo trì hoặc lỗi mạng.
* **Xác nhận:** Nếu mở Socket không sinh ra Exception/Timeout, Server đã sẵn sàng nhận lệnh (Không có bước Handshake thông báo từ Server).

### Bước 2: Gửi lệnh (Send Command)
* Chuẩn bị mảng byte từ chuỗi lệnh: `QUEUE|...`
* **Kiểm tra an toàn:** Nếu mảng byte > 1024 bytes -> Hủy thao tác để tránh lỗi tràn bộ đệm ở Server.
* Đẩy (Write) mảng byte vào Network Stream.

### Bước 3: Lắng nghe phản hồi (Receive Data)
* Ngay sau khi Write, chuyển sang vòng lặp đọc (Read) từ stream.
* Gom dần các mảng bytes trả về vào một bộ đệm (Buffer).
* Vòng lặp kết thúc khi hàm đọc trả về `0` byte (do Server gọi `stream.Close()` sau khi gửi xong).
* Thiết lập **Read Timeout** (VD: 15 giây) để tránh Client bị treo nếu Server xử lý quá lâu.

### Bước 4: Xử lý dựa trên loại lệnh (Phân loại logic)
* **Nếu gọi lệnh `QUEUE` (Nhận Full Raw XML):**
  * Lắp ráp các mảng bytes thành chuỗi XML hoàn chỉnh.
  * **Bắt buộc:** Client tự gọi thủ tục `Close/Dispose` Socket.
  * Tự thiết kế module Parser để ép kiểu XML thành các trường mong muốn (`video-id`, `title`,...) ngay tại Client.
* **Nếu gọi lệnh `TRIGGER_EXTRACT` (Uỷ quyền cho Server xuất XML gọp):**
  * Dữ liệu nhận được chỉ là mảng byte chứa đúng chữ `"OK"`.
  * Client đóng Socket kết nối.
  * Do phần mềm iNews cần vài giây để kéo tin bài từ mạng Đài, nên **Client phải bắt đầu một vòng lặp Sleep (Polling)** để canh me thư mục chứa file đích (ví dụ `\\IP_Server\D$\TEST\XML\Tên_Queue\videoID_list.xml`).
  * Liên tục kiểm tra sự tồn tại của file này (mỗi 1 giây/lần). Nếu thấy file và đảm bảo file không bị khóa (File Lock) do đang ghi dở, thì Client tiến hành copy file đó đi xử lý. Sau khi dùng xong, Client **phải xóa file đó đi** để chuẩn bị cho lần Trigger tiếp theo.

## 4. Bảng Xử Lý Ngoại Lệ

| Tình Huống | Nguyên Nhân (Từ Server) | Hành Động Của Client |
| :--- | :--- | :--- |
| **Connection Refused** | Dịch vụ trên cổng 3000 chưa chạy. | Ghi log, thử lại (Retry) sau 10-30s. |
| **Timeout khi Read()** | Server treo hoặc mạng chậm. | Chủ động ngắt Socket, báo lỗi lấy dữ liệu. |
| **Buffer > 1024 Bytes** | Client gửi tên Queue quá dài. | Validate trước khi gửi, bỏ qua lệnh này. |
| **XML không hợp lệ/Rỗng**| Lệnh sai định dạng, thiếu `\|` hoặc `#`. | Kiểm tra lại cú pháp tự sinh của Client. |

## 5. Mã Giả Tham Chiếu (Pseudocode)

```python
FUNCTION FetchQueueFromINews(ip_address, queue_path):
    PORT = 3000
    CONNECT_TIMEOUT = 5000 # 5 giây
    READ_TIMEOUT = 15000   # 15 giây
    
    command_str = "QUEUE|" + queue_path
    command_bytes = EncodeToBytes(command_str, "UTF-8")
    
    # Kiểm tra giới hạn 1024 bytes của Server
    IF Length(command_bytes) > 1024:
        RETURN ERROR "Lệnh vượt quá 1024 bytes"

    TRY:
        # Bước 1: Kết nối
        socket = tcp.Open(ip_address, PORT, CONNECT_TIMEOUT)
        socket.SetReadTimeout(READ_TIMEOUT)
        
        # Bước 2: Gửi lệnh
        socket.Write(command_bytes)
        
        # Bước 3: Đọc phản hồi
        response_bytes = []
        WHILE True:
            chunk = socket.Read(Buffer_Size=4096)
            IF chunk_is_empty OR socket_is_closed:
                BREAK
            response_bytes.Append(chunk)
            
        # Bước 4: Dọn dẹp và xử lý nghiệp vụ
        response_string = DecodeToString(response_bytes, "UTF-8")
        
        # Nếu đang trigger lệnh tự động xuất file bên C#
        IF response_string == "OK":
            socket.Close()
            RETURN WaitAndGetXmlFile(queue_path) # Hàm Polling theo dõi thư mục
            
        # Nếu đang gọi lệnh thông thường (ví dụ: QUEUE)
        RETURN ParseXmlToData(response_string)

    CATCH Exception AS e:
        Log("Lỗi giao tiếp TCP: " + e.Message)
        RETURN NULL
        
    FINALLY:
        # Bắt buộc đóng kết nối
        IF socket IS_OPEN:
            socket.Close()

# Hàm vệ tinh dùng chung cho thiết kế "Lệnh TCP 1 chiều"
FUNCTION WaitAndGetXmlFile(queue_path):
    # Đường dẫn ví dụ (đã được map ổ cứng mạng)
    file_path = "\\\\192.168.1.100\\Shared_XML\\" + queue_path + "\\videoID_list.xml"
    
    # Canh me thư mục tối đa 30s
    max_retries = 30 
    WHILE max_retries > 0:
        IF FileExists(file_path):
            TRY:
                # Đọc file để kiểm tra xem Server C# đã nhả khóa chưa
                data = ReadFile(file_path)
                DeleteFile(file_path) # Xóa ngay sau khi đọc xong
                RETURN data
            CATCH FileLockException:
                # File đang ghi dở -> Bỏ qua, đợi chu kỳ sau
                Pass
                
        Sleep(1000) # Đợi 1 giây
        max_retries = max_retries - 1
        
    RETURN ERROR "Timeout: Không thấy file XML được sinh ra!"
```
