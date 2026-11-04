# Kịch bản kiểm thử (Test Cases)

Chạy tự động: `dotnet run -- --test`. Chạy thủ công: `dotnet run`.

| ID | Bước kiểm tra | Kết quả mong đợi |
|---|---|---|
| TC01 | Menu 1 khi khởi động | 6 sách mẫu, ID 1-6 |
| TC02 | Menu 5 khi khởi động | 3 độc giả mẫu, ID 1-3 |
| TC03 | Menu 3 nhập `clean` | Tìm thấy Clean Code |
| TC04 | Menu 4 nhập `PROGRAM` | Tìm thấy sách Category = Programming |
| TC05 | Menu 2 thêm Printed Book | Book ID mới tự tăng; xuất hiện trong Menu 1 |
| TC06 | Menu 6 thêm độc giả | Reader ID mới tự tăng |
| TC07 | Mượn sách ID 1 cho Reader 1 | Quantity của #1 giảm từ 3 xuống 2 |
| TC08 | Mượn sách ID 4 có Quantity=0 | Bị từ chối, Quantity vẫn 0 |
| TC09 | Reader 1 mượn sách ID 1, 2, 3 rồi 5 | Lần mượn thứ 4 bị từ chối |
| TC10 | Reader 1 trả sách ID 2 sau TC09; mượn ID 5 | Trả thành công và mượn ID 5 thành công |
| TC11 | Trả sách chưa mượn | Báo lỗi, Quantity không tăng |
| TC12 | Mượn hai lần cùng Book ID khi chưa trả | Bị từ chối |
| TC13 | Menu 9 sau mượn/trả | Có tên độc giả, tiêu đề sách và Borrowing/Returned |
| TC14 | Mượn sách #5 eBook | Giảm 1 license giống Book; chương trình hoạt động bình thường |

**Ghi chú:** 9 nhóm kiểm tra tự động nằm trong `Tests/SmokeTests.cs`; bảng TC là checklist test thủ công và mong đợi. Chưa đưa ra tuyên bố kiểm thử đã pass trên máy của bạn: hãy chạy để ghi nhận kết quả thực tế/screenshot trước khi nộp.
