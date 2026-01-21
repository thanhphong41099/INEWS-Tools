✦ Pogic lấy tin từ server iNews:

  1. Luồng xử lý tổng quan (Flow)

  Quy trình lấy tin đi theo các bước sau:
   1. Khởi động & Kết nối (`API.cs`):
       * Khi Form load (API_Load), hàm ConnectServerToLoadDataAsync được gọi.
       * Hàm này khởi tạo ServerAPI (để lắng nghe các request TCP nội bộ/bên ngoài) và quan trọng hơn là cấu hình đối tượng iNewsData với thông tin server, user, pass.
       * Sau đó gọi LoadTreeQueuesAsync để lấy danh sách các thư mục/queue gốc (Root) và hiển thị lên TreeView.

   2. Người dùng chọn Queue (`treeView1_AfterSelect`):
       * Khi người dùng click vào một node trên TreeView.
       * Hệ thống gọi iData.GetStoriesBoard(name) (trong App_Code/iNews.cs) để lấy danh sách tin bài của queue đó.

   3. Giao tiếp với iNews Server (`iNews.cs`):
       * Mỗi lần gọi GetStoriesBoard, code sẽ tạo một kết nối mới (new iNews(settings)).
       * Thực hiện lệnh SetCurrentQueue để trỏ vào queue cần lấy.
       * Thực hiện lệnh GetStories với tham số NumberOfStoriesToGet = 240 và IsStoryBodyIncluded = true để lấy nội dung chi tiết (định dạng NSML).

   4. Xử lý dữ liệu XML (`iNewsStory.cs`):
       * Dữ liệu trả về (dạng list các chuỗi XML/NSML) được đưa vào ProcessingXMl2Class.GetDataRows.
       * Class này parse XML, trích xuất các trường (Title, ID...) và nội dung (Content) để đổ vào DataTable.

   5. Hiển thị (`API.cs`):
       * DataTable được gán vào dataGridView1.
       * Các hàm xử lý nội dung (ProcessContentWithCG, ProcessContentWithDD) chạy để tách lọc thông tin đồ họa, địa danh từ nội dung thô.

  2. Các điểm đáng chú ý trong logic (Code Review)

  Dựa trên code hiện tại, có một số điểm bạn cần lưu ý về logic:

   * Logic Lazy Load bị tắt:
      Trong API.cs, sự kiện treeView1_BeforeExpand (dùng để load thư mục con khi mở cây thư mục) đã bị comment (vô hiệu hóa).
      Tuy nhiên, trong lúc tạo node cha (LoadTreeStoriesAsync), code vẫn add một node con giả là "Loading...".
      => Hệ quả: Người dùng sẽ thấy dấu mũi tên để mở thư mục, nhưng khi mở ra chỉ thấy chữ "Loading..." và không có thư mục con nào được tải thêm. Code hiện tại chỉ hỗ trợ lấy tin ở
  cấp thư mục đã được load ban đầu.

   * Hardcoded Index khi Parse XML (`iNewsStory.cs`):
      Trong hàm GetDataRows:
   1     //Get content
   2     XmlNode content = root.ChildNodes[2];
      Code đang mặc định node chứa nội dung luôn nằm ở vị trí index số 2 (ChildNodes[2]).
      => Rủi ro: Nếu cấu trúc NSML từ iNews thay đổi (ví dụ có thêm node metadata ở đầu), logic này sẽ lấy sai node hoặc gây lỗi IndexOutOfRangeException. Nên dùng SelectSingleNode
  hoặc kiểm tra tên node.

   * Giới hạn số lượng tin:
      Trong iNews.cs -> GetStoriesBoard:
   1     storyType.NumberOfStoriesToGet=240;
      => Giới hạn: Code chỉ lấy tối đa 240 tin đầu tiên. Không thấy logic phân trang (pagination) hay kiểm tra xem còn tin không. Nếu queue có 300 tin, 60 tin cũ hơn sẽ không hiển
  thị.

   * Tạo kết nối liên tục:
      Trong iNewsData, mỗi hàm (GetStoriesBoard, GetFolderChildren...) đều khởi tạo using (iNews inews = new iNews(settings)).
      => Hiệu năng: Điều này an toàn để tránh giữ kết nối chết, nhưng sẽ tạo overhead (độ trễ) do phải bắt tay (handshake) và login lại với server iNews mỗi lần người dùng click chọn
  bản tin.

   * ServerAPI (TCP Server):
      Ứng dụng này không chỉ là Client mà còn đóng vai trò là một Server trung gian (Port 3000) thông qua ServerAPI.cs. Nó cho phép các ứng dụng khác gửi lệnh (ví dụ: GET_TREE,
  Tên_Queue) qua TCP để lấy dữ liệu iNews mà ứng dụng này đã kết nối.

  3. Kết luận
  Logic đi từ UI -> iNewsData -> WebService -> Parse XML -> UI là mạch lạc. Tuy nhiên, phần mở rộng cây thư mục (BeforeExpand) đang bị tắt và cách parse XML dựa vào index cố định là
  những điểm có thể gây lỗi tiềm ẩn.

  ## 1. Nội dung gốc lấy từ iNews là XML hay DataTable?
   * Chính xác là XML (dạng chuỗi NSML).
   * Tại bước 3 (trong iNews.cs), server trả về danh sách các câu chuyện dưới dạng thuộc tính StoryAsNSML.
   * Tại API.cs, biến queues chứa một List<string>, trong đó mỗi string là một đoạn mã XML nguyên bản của một bản tin. Lúc này chưa hề có DataTable.

  ## 2. Key "Fields" (ví dụ: `title,page-number`) được xử lý ở đâu?
  Key này được dùng làm cầu nối để trích xuất dữ liệu từ XML và đổ vào DataTable. Quá trình này diễn ra trong hàm GetDataRows của file App_Code\iNewsStory.cs.

  Cụ thể quy trình xử lý của biến Fields như sau:

   1. Tạo Cột cho DataTable:
      Đầu tiên, code tách chuỗi Fields ra thành mảng các từ khóa (ví dụ: ["title", "page-number"]). Sau đó, nó tạo các cột trong DataTable tương ứng với các tên này.
      Code: AddColumn(tbl, k); (Dòng 38 iNewsStory.cs)

   2. Dùng làm từ khóa tìm kiếm trong XML:
      Sau đó, code duyệt qua file XML gốc. Nó dùng chính các từ khóa trong Fields để tìm thẻ XML tương ứng bằng lệnh XPath.
      Code: XmlNode temp = body.SelectSingleNode("string[@id='" + k + "']"); (Dòng 66 iNewsStory.cs)

  Kết luận:
  Cấu hình Fields tác động vào cả hai:
   1. Nó quyết định XML sẽ bị "nhặt" ra những thông tin nào (ví dụ: tìm thẻ có id='page-number').
   2. Nó quyết định cấu trúc của DataTable kết quả sẽ gồm những cột nào để chứa thông tin vừa nhặt được.
