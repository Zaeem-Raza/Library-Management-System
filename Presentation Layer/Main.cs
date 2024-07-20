using DAL;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Presentation_Layer
{
    public class Input
    {
        public static Book InputBook()
        {
            int Id;
            string Title, Author, Genre;

            Console.WriteLine("\nEnter Book id : ");
            Id=int.Parse(Console.ReadLine());
            while (Id <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a valid positive integer for Book id:");
                Id = int.Parse(Console.ReadLine());
            }
            // Input and Validate
            Console.WriteLine("\nEnter Book Title : ");
            Title = Console.ReadLine();
            while (string.IsNullOrEmpty(Title))
            {
                Console.WriteLine("Title cannot be empty. Enter Book Title:");
                Title = Console.ReadLine();
            }

            Console.WriteLine("\nEnter Book Author : ");
            Author = Console.ReadLine();
            while (string.IsNullOrEmpty(Author))
            {
                Console.WriteLine("Author cannot be empty. Enter Book Author:");
                Author = Console.ReadLine();
            }

            Console.WriteLine("\nEnter Genre of the Book : ");
            Genre = Console.ReadLine();
            while (string.IsNullOrEmpty(Genre))
            {
                Console.WriteLine("Genre cannot be empty. Enter Genre of the Book:");
                Genre = Console.ReadLine();
            }

       
            // the newly added book is assumed to be available
            Book newBook = new Book(Id, Title, Author, Genre, true);
            return newBook;
        }

        public static Book InputUpdate(int Id,bool status)
        {
            string Title, Author, Genre;
            bool Available = status;

            Console.WriteLine("\nEnter Book Title : ");
            Title = Console.ReadLine();
            while (string.IsNullOrEmpty(Title))
            {
                Console.WriteLine("Title cannot be empty. Enter Book Title:");
                Title = Console.ReadLine();
            }

            Console.WriteLine("\nEnter Book Author : ");
            Author = Console.ReadLine();
            while (string.IsNullOrEmpty(Author))
            {
                Console.WriteLine("Author cannot be empty. Enter Book Author:");
                Author = Console.ReadLine();
            }

            Console.WriteLine("\nEnter Genre of the Book : ");
            Genre = Console.ReadLine();
            while (string.IsNullOrEmpty(Genre))
            {
                Console.WriteLine("Genre cannot be empty. Enter Genre of the Book:");
                Genre = Console.ReadLine();
            }

            Book newBook = new Book(Id, Title, Author, Genre, Available);
            return newBook;
        }

        public  static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            // Check for @ and at least one dot
            if (!email.Contains('@') || !email.Contains('.'))
                return false;

            // Check for more than 4 lowercase letters
            int lowercaseCount = 0;
            foreach (char c in email)
            {
                if (char.IsLower(c))
                {
                    lowercaseCount++;
                }
            }

            if (lowercaseCount <= 4)
                return false;

            return true;
        }

        public static Borrower InputBorrower()
        {
            int Id;
            string Name, Email;

            // Input and validations
            Console.WriteLine("\nEnter Borrower ID: ");
            while (!int.TryParse(Console.ReadLine(), out Id) || Id <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a valid positive integer for Borrower ID:");
            }
            
            Console.WriteLine("\nEnter Borrower Name: ");
            Name = Console.ReadLine();
            while (string.IsNullOrEmpty(Name))
            {
                Console.WriteLine("Name cannot be empty. Enter Borrower Name:");
                Name = Console.ReadLine();
            }

            Console.WriteLine("\nEnter Borrower Email: ");
            Email = Console.ReadLine();
            while (!Input.IsValidEmail(Email))
            {
                Console.WriteLine("Invalid email format. Enter Borrower Email:");
                Email = Console.ReadLine();
            }

            Borrower newBorrower = new Borrower(Id, Name, Email);
            return newBorrower;
        }

        public static Borrower UpdateBorrower(int id)
        {
         
            string Name, Email;
            Console.WriteLine("\nEnter Borrower Name: ");
            Name = Console.ReadLine();
            while (string.IsNullOrEmpty(Name))
            {
                Console.WriteLine("Name cannot be empty. Enter Borrower Name:");
                Name = Console.ReadLine();
            }

            Console.WriteLine("\nEnter Borrower Email: ");
            Email = Console.ReadLine();
            while (!Input.IsValidEmail(Email))
            {
                Console.WriteLine("Invalid email format. Enter Borrower Email:");
                Email = Console.ReadLine();
            }

            Borrower newBorrower = new Borrower(id, Name, Email);
            return newBorrower;
        }

        public static int GetCount()
        {
            // Count.txt contains current transactions
            string FilePath = "Transactions.txt";
            int count = 0;
            if(File.Exists(FilePath))
            {

            FileStream fin=new FileStream(FilePath,FileMode.Open, FileAccess.Read);
            StreamReader reader=new StreamReader(fin);
            while(reader.ReadLine() != null)
            {
                count++;
            }

            reader.Close();
            fin.Close();
            }
            return count;  

        }


        
    }

    public class Main
    {
        
        public static void Menu()
        {
            int choice = 0;
            DataAccess library = new DataAccess();
            library.FetchData();
            int BookId;

            do
            {
                Console.Clear();
                Console.WriteLine("Library Management System");
                Console.WriteLine("1. Add a new book");
                Console.WriteLine("2. Remove a book");
                Console.WriteLine("3. Update a book");
                Console.WriteLine("4. Register a new borrower");
                Console.WriteLine("5. Update a borrower");
                Console.WriteLine("6. Delete a borrower");
                Console.WriteLine("7. Borrow a book");
                Console.WriteLine("8. Return a book");
                Console.WriteLine("9. Search for book");
                Console.WriteLine("10. View All Books");
                Console.WriteLine("11. View borrowed books by borrower");
                Console.WriteLine("12. Exit");
                Console.Write("Select an option: ");
                choice = int.Parse(Console.ReadLine());
                if (choice < 1 || choice > 12)
                {
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        Book Book = Input.InputBook();
                        library.AddBook(Book);
                        break;
                    case 2:
                        if (library.NumberOfBooks() == 0)
                        {
                            Console.WriteLine("There are No books in The Library at this Moment\n");
                        }
                        else
                        {

                            library.ShowCurrentBooks();
                            Console.WriteLine("Enter the Id of the Book you want to remove\n");
                            int Id = int.Parse(Console.ReadLine());
                            library.RemoveBook(Id); 
                        }
                        break;
                    case 3:
                        if (library.NumberOfBooks() != 0)
                        {
                        library.ShowCurrentBooks();
                        Console.WriteLine("Enter the Id of the Book you want to Update");
                        BookId = int.Parse(Console.ReadLine());
                        Book b=library.GetBookById(BookId);
                        if (b == null)
                        {
                            Console.WriteLine($"Book with Id {BookId} does not exist in the Library");
                        }
                        else
                        {
                            Book BookToBeUpdated=Input.InputUpdate(BookId,b.IsAvailable);
                            library.UpdateBook(BookToBeUpdated);
                        }
                        }
                        else
                        {
                            Console.WriteLine("There are No books in The Library at this Moment\n");
                        }

                        break;
                    case 4:
                        Borrower newBorrower= Input.InputBorrower();
                        library.RegisterBorrower(newBorrower);
                        break;
                    case 5:
                        Console.WriteLine("Enter the Id of the Borrower you want to Update");
                        int BorrowerId = int.Parse(Console.ReadLine());
                        Borrower BorrowerToBeUpdated = Input.UpdateBorrower(BorrowerId);
                        library.UpdateBorrower(BorrowerToBeUpdated);
                        break;
                    case 6:
                        Console.WriteLine("Enter The Id of the Borrower you want to Delete : ");
                        int BorrowerToBeDeleted = int.Parse(Console.ReadLine());
                        library.DeleteBorrower(BorrowerToBeDeleted);
                            break;
                    case 7:
                        // borrow a book
                        if (library.NumberOfBooks() == 0)
                        {
                            Console.WriteLine("There are No books in The Library at this Moment\n");
                        }
                        else
                        {
                            library.ShowCurrentBooks();
                            Console.WriteLine("Enter The Borrower Id: ");
                            BorrowerId = int.Parse(Console.ReadLine());
                            Console.WriteLine("Enter The Book Id you want to Borrow: ");
                            BookId= int.Parse(Console.ReadLine());
                            int Count= Input.GetCount()+1;
                            Transaction BorrowTransaction = new Transaction(Count, BookId, BorrowerId, DateTime.Now, true);
                            library.RecordTransaction(BorrowTransaction);
                        }
                        break;
                    case 8:
                        if (library.NumberOfBooks() == 0)
                        {
                            Console.WriteLine("There are No books in The Library at this Moment\n");
                        }
                        else
                        {

                            library.ShowCurrentBooks();
                            Console.WriteLine("Enter The Borrower Id: ");
                            BorrowerId = int.Parse(Console.ReadLine());
                            Console.WriteLine("Enter The Book Id you want to Return: ");
                            BookId = int.Parse(Console.ReadLine());
                            int Count = Input.GetCount() + 1;
                            Transaction ReturnTransaction = new Transaction(Count, BookId, BorrowerId, DateTime.Now, false);
                            library.RecordTransaction(ReturnTransaction);
                        }
                            break;
                    case 9:
                        if (library.NumberOfBooks() == 0)
                        {
                            Console.WriteLine("There are No books in The Library at this Moment\n");
                        }
                        else
                        {

                        Console.WriteLine("Enter query( Title/ Atuthor/ Genre ) of Book to be searched: ");
                        string query = Console.ReadLine();
                        if (string.IsNullOrEmpty(query))
                        {
                            Console.WriteLine("Enter Valid query\n");
                        }
                        else
                        {
                                List<Book> FoundBooks = library.SearchBooks(query);
                                if (FoundBooks.Count == 0)
                                {
                                    Console.WriteLine("There are No books with this Information at the moment\n");
                                }
                                else
                                {
                                    foreach (Book book in FoundBooks)
                                    {
                                        Console.WriteLine($"Book Id : {book.BookId}, Title : {book.Title}, Author : {book.Author}, Genre : {book.Genre}, IS Available : {book.IsAvailable}");
                                    }

                                }
                            }
                        }
                            break;
                    case 10:
                        if (library.NumberOfBooks() == 0)
                        {
                            Console.WriteLine("There are No books in The Library at this Moment\n");
                        }
                        else
                        {
                            library.ShowCurrentBooks();
                        }
                        break;
                    case 11:
                        Console.WriteLine("Enter The Borrower Id :  ");
                        BorrowerId=int.Parse(Console.ReadLine());
                        if (!library.IsBorrowerPresent(BorrowerId))
                        {
                            Console.WriteLine("This Borrower does not exist in our System\n");
                        }
                        else
                        {
                            List<Transaction> Transactions = library.GetBorrowedBooksByBorrower(BorrowerId);
                            if (Transactions.Count == 0)
                            {
                                Console.WriteLine("This Borrower has not borrowed any book at this moment\n");
                            }
                            else
                            {
                                List<Book> Books = new List<Book>();
                                // Using Dictionary to keep record of borrowed and returned Books
                                Dictionary<int, bool> bookStatus = new Dictionary<int, bool>();
                                foreach (Transaction transaction in Transactions)
                                {
                                    // If the book is borrowed, set its status to true (borrowed)
                                    // If the book is returned, set its status to false (returned)
                                    bookStatus[transaction.BookId] = transaction.IsBorrowed;
                                }

                                // Now,Retrive Books that are currently borrowed
                                foreach (var entry in bookStatus)
                                {
                                    if (entry.Value) // If the book is currently borrowed
                                    {
                                        Book borrowedBook = library.GetBookById(entry.Key);
                                        if (borrowedBook != null)
                                        {
                                            Books.Add(borrowedBook);
                                        }
                                    }
                                }
                                if (Books.Count == 0)
                                {
                                    // there must be some book/s that were return
                                    Console.WriteLine("This Borrower has not borrowed any books at this moment\n");
                                }
                                else
                                {
                                    Console.WriteLine("Books currently borrowed by Borrower ID " + BorrowerId + ":");
                                    foreach (Book book in Books)
                                    {
                                        Console.WriteLine($"Book ID: {book.BookId}, Title: {book.Title}, Author: {book.Author}, Genre: {book.Genre}");
                                    }
                                }
                            }
                        }
                        break;
                    case 12:
                        Console.WriteLine("Exiting...");
                        break;
                }
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                        library.StoreData();
            } while (choice != 12);
        }
    }
}
