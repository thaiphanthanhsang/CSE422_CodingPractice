using LibraryManagementSystem.Models;

Book book = new Book("B001", "C# Programming", "John Smith", 2024, 5);

book.DisplayInfo();

Member member = new Member(
    "M001", "Nguyen Van A", "a@gmail.com");

PremiumMember premium = new PremiumMember(
    "M002", "Tran Van B", "b@gmail.com",
    DateTime.Today.AddYears(1), 10);

member.DisplayInfo();
premium.DisplayInfo();

Library library1 = new Library();

Library library2 = new Library("FPT Library", new List<Book> { book });

Library library3 = new Library(library2);

library1.DisplayLibraryInfo();
library2.DisplayLibraryInfo();
library3.DisplayLibraryInfo();