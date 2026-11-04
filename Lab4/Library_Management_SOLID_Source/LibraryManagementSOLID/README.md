# Library Management System - SOLID (C# Console)

Bài lab CSE422: quản lý sách, độc giả, mượn/trả và báo cáo, có phân tách component theo SOLID.

## Yêu cầu chạy

- Windows 10/11, Visual Studio 2022 (hoặc mới hơn) với workload **.NET desktop development**.
- .NET 8 SDK, hoặc dùng VS Code + .NET 8 SDK.
- Không cần SQL Server và không cần gói NuGet bên thứ ba.

## Mở trong Visual Studio

1. Giải nén file ZIP.
2. Visual Studio > **File > Open > Project/Solution**.
3. Chọn `LibraryManagementSOLID.csproj`.
4. Đợi VS nhận SDK .NET 8, sau đó bấm **Ctrl + F5** (chạy không debug).
5. Nhập số từ 0 đến 9 để sử dụng menu.

## Chạy bằng dòng lệnh

Tại thư mục chứa `.csproj`:

```powershell
dotnet restore
dotnet build
dotnet run
dotnet run -- --test
```

Lệnh `--test` chạy 9 bài kiểm tra tự động offline, không phụ thuộc thư viện test ngoài.

## Chức năng menu

| Menu | Chức năng |
|---|---|
| 1 | Danh sách sách, loại sách và số lượng còn lại |
| 2 | Thêm sách loại Printed Book, eBook hoặc Magazine |
| 3 | Tìm kiếm tên sách, không phân biệt chữ hoa/thường |
| 4 | Tìm kiếm thể loại, không phân biệt chữ hoa/thường |
| 5 | Danh sách độc giả |
| 6 | Thêm độc giả |
| 7 | Mượn một cuốn sách (giảm Quantity đi 1) |
| 8 | Trả một cuốn sách (tăng Quantity lên 1) |
| 9 | Báo cáo lịch sử mượn, bao gồm sách đã trả |
| 0 | Thoát |

## Dữ liệu mẫu

**Sách:**

- #1 Clean Code / Programming / Quantity: 3
- #2 Design Patterns / Programming / Quantity: 2
- #3 The Pragmatic Programmer / Programming / Quantity: 1
- #4 Database Systems / Database / Quantity: 0
- #5 C# Digital Handbook / Programming / Quantity: 2 eBook licenses
- #6 Science Monthly / Science / Quantity: 2 Magazine

**Độc giả:**

- #1 Nguyen Van An
- #2 Tran Thi Binh
- #3 Le Minh Chau

## Dùng thử nhanh

1. Chọn `1` xem sách.
2. Chọn `3`, nhập `code` để tìm theo tên (không phân biệt hoa/thường).
3. Chọn `7`, nhập Reader ID `1`, Book ID `1`. Số lượng sách #1 từ 3 xuống 2.
4. Lặp lại mượn 2 sách khác: ID `2` và `3`.
5. Thử mượn sách ID `5` cho reader `1`: chương trình chặn do giới hạn 3.
6. Chọn `8`, nhập `1` và `2` để trả sách #2.
7. Chọn `7`, nhập `1` và `5` để mượn eBook. Hợp lệ vì đã có chỗ trống.
8. Chọn `9` xem lịch sử mượn/trả.

## Quy ước dữ liệu và giới hạn

- Mỗi lần mượn tương ứng **1 bản sách / 1 license**. Mỗi reader được có **tối đa 3 loan đang hoạt động**.
- Một độc giả không được mượn hai bản của cùng một Book ID cùng lúc.
- `Quantity` là số bản hiện còn sẵn, không phải tổng số bản gốc.
- Với `EBook`, `Quantity` là số lượt mượn đồng thời (license) - cách mô hình hóa tối giản cho bài lab.
- Dữ liệu lưu **trong RAM**, thoát chương trình sẽ trở về dữ liệu mẫu. Muốn lưu vĩnh viễn, đổi implementation của repository sang EF Core/SQL Server mà không phải sửa logic dịch vụ.
- Thêm loại sách mới bằng `Book` subclass + `IBookFactory` implementation + đăng ký tại **Program.cs** (composition root); **không phải sửa** BookService/LendingService/ReportService.

## Cấu trúc

- `Models/`: Book, PrintedBook, EBook, Magazine, Reader, Loan.
- `Interfaces/`: service, repository, borrowing policy, factory, ID generator abstractions.
- `Repositories/`: 3 in-memory implementations.
- `Services/`: BookService, ReaderService, LendingService, ReportService.
- `Policies/`: MaxThreeBooksPolicy.
- `Factories/`: factories cho 3 loại sách.
- `Infrastructure/`: SequentialIdGenerator.
- `Data/`: SampleDataSeeder.
- `Tests/`: SmokeTests (chạy `--test`).
- `Docs/`: UML và tài liệu giải thích.

Xem thêm: `Docs/DESIGN_VI.md`, `Docs/TEST_CASES_VI.md` và `Docs/UML_ClassDiagram.mmd`.
