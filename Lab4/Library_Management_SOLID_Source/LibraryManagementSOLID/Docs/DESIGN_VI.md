# Thiết kế hệ thống quản lý thư viện theo SOLID

## 1. Phân tích bài toán

**Thực thể chính:** `Book` (sách), `Reader` (độc giả), `Loan` (phiếu mượn).

**Quy tắc nghiệp vụ:** Book ID và Reader ID duy nhất; Title, Author, Category không được trống; Quantity không âm; một người có tối đa ba phiếu mượn đang hoạt động; không mượn sách nếu Quantity = 0; trả sách phải có phiếu mượn hợp lệ.

**Dữ liệu:** in-memory để chạy demo độc lập. Chưa kết nối CSDL.

## 2. UML

- `UML_ClassDiagram.mmd`: class diagram chi tiết, chỉnh sửa trên Mermaid Live Editor hoặc môi trường hỗ trợ Mermaid.
- `UML_Overview.png` / `.svg`: biểu đồ tổng quan, vẽ bằng Graphviz từ `UML_Overview.dot`.
- Ký hiệu: mũi tên tam giác rỗng biểu thị inheritance/implementation; mũi tên thường biểu thị dependency; `Reader 1 — 0..* Loan` và `Book 1 — 0..* Loan` là liên kết nghiệp vụ.

## 3. Danh sách lớp và vai trò

| Lớp/interface | Trách nhiệm |
|---|---|
| `Book` | Dữ liệu sách, kiểm soát số lượng, abstraction cho các loại sách |
| `PrintedBook`, `EBook`, `Magazine` | Từng biến thể của Book (subtype) |
| `Reader` | Dữ liệu độc giả |
| `Loan` | Lưu lần mượn và trạng thái trả |
| `IBookRepository` / `InMemoryBookRepository` | Cổng truy xuất / lưu sách |
| `IReaderRepository` / `InMemoryReaderRepository` | Cổng truy xuất / lưu độc giả |
| `ILoanRepository` / `InMemoryLoanRepository` | Cổng truy xuất / lưu phiếu mượn |
| `IBookService` / `BookService` | Thêm, xem, tìm sách |
| `IReaderService` / `ReaderService` | Quản lý độc giả |
| `ILendingService` / `LendingService` | Mượn/trả và cập nhật kho |
| `IReportService` / `ReportService` | Báo cáo theo độc giả |
| `IBorrowingPolicy` / `MaxThreeBooksPolicy` | Quy tắc tối đa ba sách |
| `IBookFactory` / 3 factories | Tạo sách theo loại, hỗ trợ mở rộng |
| `IIdGenerator` / `SequentialIdGenerator` | Cấp ID tăng dần |
| `SampleDataSeeder` | Dữ liệu mẫu; `Program` là nơi lắp ráp dependency |

## 4. Nguyên lý SOLID

**S - Single Responsibility:** BookService chỉ quản lý danh mục; ReaderService chỉ quản lý độc giả; LendingService chỉ xử lý mượn/trả; ReportService chỉ tạo báo cáo; MaxThreeBooksPolicy chỉ giữ quy tắc số sách tối đa. Mỗi lớp có một lý do chính để thay đổi.

**O - Open/Closed:** Khi thêm AudioBook, tạo `AudioBook : Book` và `AudioBookFactory : IBookFactory`; đăng ký factory tại composition root. Không cần sửa logic BookService, LendingService hoặc ReportService. Khi đổi giới hạn mượn, có thể thêm implementation IBorrowingPolicy mới và cấu hình lại nơi lắp ráp.

**L - Liskov Substitution:** PrintedBook, EBook, Magazine thay thế Book trong mọi chỗ nhận Book. BorrowOne/ReturnOne giữ cùng quy tắc bất biến Quantity; eBook dùng số license đồng thời để phù hợp mô hình chung.

**I - Interface Segregation:** IBookService, IReaderService, ILendingService, IReportService được chia theo nghiệp vụ; IBookRepository, IReaderRepository, ILoanRepository nhỏ và riêng biệt. Không buộc lớp nào triển khai các thao tác không sử dụng.

**D - Dependency Inversion:** Các service nhận interface trong constructor. Ví dụ `LendingService(IBookRepository, IReaderRepository, ILoanRepository, IBorrowingPolicy, IIdGenerator)`, không khởi tạo trực tiếp InMemoryRepository trong service. Việc chọn concrete implementation nằm ở `Program`.

## 5. Luồng mượn và trả

### Borrow(readerId, bookId)

1. Tìm độc giả và sách từ repository.
2. Lấy số phiếu mượn còn hiệu lực của độc giả.
3. Gọi `IBorrowingPolicy.EnsureCanBorrow` (tối đa 3 và không mượn trùng Book ID).
4. `Book.BorrowOne()` kiểm tra số lượng > 0, sau đó trừ 1.
5. Tạo Loan với ID mới và thời gian mượn; thêm vào repository.
6. Nếu lưu phiếu mượn thất bại, khôi phục số lượng trong nhánh catch (rollback cơ bản cho bộ nhớ).

### Return(readerId, bookId)

1. Tìm một Loan đang hoạt động khớp readerId và bookId.
2. Nếu không có, báo lỗi; không tăng stock sai.
3. MarkReturned() ghi thời gian trả.
4. Book.ReturnOne() tăng stock 1.

### GenerateBorrowingReport()

Lặp qua tất cả độc giả; hiển thị số sách đang mượn và tất cả phiếu mượn theo thứ tự, gồm tên sách, thời điểm mượn và trạng thái Borrowing/Returned. Độc giả chưa mượn vẫn được liệt kê.

## 6. Mở rộng

- **AudioBook:** thêm lớp `AudioBook` kế thừa Book + factory mới, chỉ thêm đăng ký ở `Program`, không sửa service đã có.
- **Reservations:** thêm `Reservation`, `IReservationRepository`, `IReservationService`, `ReservationService`; có thể thêm chính sách ưu tiên mượn sách theo hàng chờ (cần điều chỉnh luồng điều phối ở điểm tích hợp, không sửa các lớp sách/reader/report).
- **SQL Server:** implement `IBookRepository`, `IReaderRepository`, `ILoanRepository` bằng EF Core; cấu hình DI tại Program, thay cho InMemory.
- **Xuất PDF / CSV:** thêm `IReportExporter` với PdfReportExporter/CsvReportExporter; ReportService tạo dữ liệu báo cáo, exporter lo định dạng.
- **GUI/ASP.NET:** thay Console ở lớp trình bày, tái sử dụng service và domain.

## 7. Kiểm thử và giới hạn

`dotnet run -- --test` chạy bộ test kiểm tra tìm kiếm, ID, mượn, trả, lỗi số lượng, hạn mức ba sách, trả rồi mượn thêm, lỗi ID, report và eBook.

Do lưu in-memory nên không có persistence hoặc concurrency. Nếu triển khai thực tế, cần CSDL, giao dịch DB và kiểm tra race condition với nhiều người cùng mượn; cần bổ sung authentication/authorization cho chức năng admin.
