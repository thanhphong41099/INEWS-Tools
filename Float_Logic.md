# Logic Lọc Tin XML và Cách Ẩn Tin "Float"

## 1. Hàm xử lý logic lọc
Hàm chịu trách nhiệm quét danh sách XML và quyết định đưa tin nào vào DataTable nằm ở:

*   **File:** `API_iNews\App_Code\iNewsStory.cs`
*   **Class:** `ProcessingXMl2Class`
*   **Hàm:** `public DataTable GetDataRows(List<string> xmlString)`

## 2. Cơ chế hoạt động hiện tại
Trong hàm `GetDataRows`, code thực hiện vòng lặp `foreach` duyệt qua từng chuỗi XML. Tại đây, nó phân tích node `<head>` của XML để lấy thông tin.

Hiện tại, trong code cũ đã có sẵn logic để lọc (bỏ qua) các tin dạng `float` (tin trôi/tin nháp) hoặc `break` (dấu ngắt), nhưng đoạn code này **đang bị comment (vô hiệu hóa)**.

## 3. Cách thực hiện lọc tin "Float"
Dựa trên file mẫu `story_1.xml`, cấu trúc thẻ meta như sau:
```xml
<head>
    <meta break="1" float="false" ... />
    ...
</head>
```

Để ẩn các tin có thuộc tính `float="true"`, bạn cần bỏ comment (kích hoạt lại) đoạn code kiểm tra trong file `iNewsStory.cs`.

### Code cần sửa đổi (trong `GetDataRows`):

**Hiện tại (Đang bị khóa):**
```csharp
//170415-thanhth:Bo sung cai nay de no biet la story kieu float hay break thi ko doc du lieu
//bool isFloat = header.SelectSingleNode("meta").Attributes["float"] != null ? header.SelectSingleNode("meta").Attributes["float"].Value == "true" : false;
//bool isBreak = header.SelectSingleNode("meta").Attributes["break"] != null ? header.SelectSingleNode("meta").Attributes["break"].Value == "true" : false;
////bool isHold = header.SelectSingleNode("meta").Attributes["hold"] != null ? header.SelectSingleNode("meta").Attributes["hold"].Value == "true" : false;
//if (isFloat || isBreak)
//    continue;
```

**Cần sửa thành (Bỏ comment):**
```csharp
// Kiểm tra thẻ meta trong header
XmlNode metaNode = header.SelectSingleNode("meta");
if (metaNode != null)
{
    // Kiểm tra thuộc tính float
    bool isFloat = metaNode.Attributes["float"] != null && metaNode.Attributes["float"].Value == "true";
    
    // Kiểm tra thuộc tính break (thường cũng nên ẩn các dấu ngắt đoạn)
    bool isBreak = metaNode.Attributes["break"] != null && metaNode.Attributes["break"].Value == "true";

    // Nếu là float hoặc break thì bỏ qua (continue), không thêm vào DataTable
    if (isFloat || isBreak)
        continue;
}
```

## 4. Tóm tắt
Để hệ thống tự động loại bỏ các tin `float`, bạn chỉ cần vào file `API_iNews\App_Code\iNewsStory.cs` và **uncomment** (bỏ dấu `//`) ở đoạn logic kiểm tra `isFloat` trong hàm `GetDataRows`.
