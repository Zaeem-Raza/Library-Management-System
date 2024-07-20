using System.IO;
using System.Net;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DAL
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public bool IsAvailable { get; set; }

        // Constructor
        public Book(int bookId, string title, string author, string genre, bool status)
        {
            BookId = bookId;
            Title = title;
            Author = author;
            Genre = genre;
            IsAvailable = status;
        }

    }

    public class Borrower
    {
        public int BorrowerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public Borrower(int borrowerId, string name, string email)
        {
            BorrowerId = borrowerId;
            Name = name;
            Email = email;
        }
       
    }

    public class Transaction
    {
        public int TransactionId { get; set; }
        public int BookId { get; set; }
        public int BorrowerId { get; set; }
        public DateTime Date { get; set; }
        public bool IsBorrowed { get; set; }

        public Transaction(int transactionId, int bookId, int borrowerId, DateTime date, bool isBorrowed)
        {
            TransactionId = transactionId;
            BookId = bookId;
            BorrowerId = borrowerId;
            Date = date;
            IsBorrowed = isBorrowed;
        }

    }

    public class DataAccess
    {
        //Lists of Books,Borrowers and Transactions in The Library
        private  List<Book> BooksList = new List<Book>();
        private  List<Borrower> BorrowersList = new List<Borrower>();
        private  List<Transaction> TransactionsList = new List<Transaction>();


        // Book operations
        public  void AddBook(Book book)
        {
            // if the book is Already present, do not add
            if (IsBookPresent(book.BookId))
            {
                Console.WriteLine($"Book with ID {book.BookId} is already present, cannot be added.");
            }
            else
            {
                BooksList.Add(book);
                Console.WriteLine($"Book added: {book.Title}");
            }


        }

        public void RemoveBook(int bookId)
        {
            bool IsRemoved = false;

            for (int i = 0; i < BooksList.Count; i++)
            {
                if (BooksList[i].BookId == bookId)
                {
                    BooksList.RemoveAt(i);
                    Console.WriteLine($"Book with ID {bookId} has been removed.");
                    IsRemoved = true;
                    break;
                }
            }
            // book not found
            if (!IsRemoved)
            {
                Console.WriteLine($"Book with ID {bookId} does not exist.");
            }
        }

        public void UpdateBook(Book book)
        {
            bool IsPresent = false;

            for (int i = 0; i < BooksList.Count; i++)
            {
                if (BooksList[i].BookId == book.BookId)
                {
                    BooksList[i].Title = book.Title;
                    BooksList[i].Author = book.Author;
                    BooksList[i].Genre = book.Genre;
                    BooksList[i].IsAvailable = book.IsAvailable;
                    IsPresent = true;
                    Console.WriteLine($"Book with ID {book.BookId} has been updated.");
                    break;
                }
            }
            // book not found
            if (!IsPresent)
            {
                Console.WriteLine($"Book with ID {book.BookId} does not exist, cannot update.");
            }

        }

        public  List<Book> GetAllBooks()
        {
            return BooksList;
        }

        public Book GetBookById(int bookId)
        {
            foreach(Book b in BooksList)
            {
                if (b.BookId == bookId) return b;
            }
            return null;

        }

        public List<Book> SearchBooks(string query)
        {
            
           List<Book> RequiredBooks=new List<Book>();
            for(int i = 0; i < BooksList.Count; i++)
            {
                if (BooksList[i].Title==query || BooksList[i].Author==query || BooksList[i].Genre == query)
                {
                    RequiredBooks.Add(BooksList[i]);
                }
            }
            return RequiredBooks;
        }


        // Borrower operations
        public void RegisterBorrower(Borrower borrower)
        {
            if (IsBorrowerPresent(borrower.BorrowerId) || IsEmailPresent(borrower))
            {
                Console.WriteLine($"Borrower with this ID  or Email already exists, cannot register.");
            }
            else
            {
                BorrowersList.Add(borrower);
                Console.WriteLine($"Borrower registered: {borrower.Name}");
            }
        }

        public void UpdateBorrower(Borrower borrower)
        {
            bool IsPresent = false;

            for (int i = 0; i < BorrowersList.Count; i++)
            {
                if (BorrowersList[i].BorrowerId == borrower.BorrowerId)
                {
                    BorrowersList[i].Name = borrower.Name;
                    BorrowersList[i].Email = borrower.Email;
                    IsPresent = true;
                    Console.WriteLine($"Borrower with ID {borrower.BorrowerId} has been updated.");
                    break;
                }
            }

            if (!IsPresent)
            {
                Console.WriteLine($"Borrower with ID {borrower.BorrowerId} does not exist, cannot update.");
            }
        }

        public void DeleteBorrower(int borrowerId)
        {
            bool IsDeleted = false;

            for (int i = 0; i < BorrowersList.Count; i++)
            {
                if (BorrowersList[i].BorrowerId == borrowerId)
                {
                    BorrowersList.RemoveAt(i);
                    Console.WriteLine("Borrower has been deleted\n");
                    IsDeleted = true;
                    break;
                }
            }

            if (!IsDeleted)
            {
                Console.WriteLine("Borrower with this Id does not exist\n");
            }
        }

        // Transaction operations
        public  void RecordTransaction(Transaction transaction)
        {
            
            if (!IsBorrowerPresent(transaction.BorrowerId))
            {
                Console.WriteLine("This Borrower is not registered\n");
            }
            else if (!IsBookPresent(transaction.BookId))
            {
                Console.WriteLine("This Book does not exist in our system\n");
            }
            else
            {
                Book BookToBeBorrowed = GetBookById(transaction.BookId);
                if (transaction.IsBorrowed)
                {
                    // Borrowing the book
                    if (BookToBeBorrowed.IsAvailable)
                    {
                        BookToBeBorrowed.IsAvailable = false;
                        TransactionsList.Add(transaction);
                        Console.WriteLine($"The book with id {BookToBeBorrowed.BookId} has been borrowed");
                    }
                    else
                    {
                        Console.WriteLine($"The book with id {BookToBeBorrowed.BookId} is not available, so you cannot borrow it");
                    }
                }
                else
                {
                    // Returning the book
                    if (BookToBeBorrowed.IsAvailable)
                    {
                    Console.WriteLine($"The book with id {transaction.BookId} has not been borrowed at the moment");
                    }
                    else
                    {
                        // check this is the same person who borrowed this Book
                        if (BorrowedBook(transaction.BorrowerId, transaction.BookId))
                        { 
                        BookToBeBorrowed.IsAvailable=true;
                        TransactionsList.Add(transaction);
                        Console.WriteLine($"The book with id {BookToBeBorrowed.BookId} has been returned\n");
                        }
                        else
                        {
                            Console.WriteLine("This Borrower did not borrow this book\n");
                        }
                    }
                }
            }
        }

        public  List<Transaction> GetBorrowedBooksByBorrower(int borrowerId)
        {
            List<Transaction> BorrowedTransactions=new List<Transaction> ();
            for(int i = 0; i < TransactionsList.Count; i++)
            {
                if (TransactionsList[i].BorrowerId == borrowerId)
                {
                    BorrowedTransactions.Add(TransactionsList[i]);
                }
            }
            return BorrowedTransactions;
            
        }


        //helping Function
        public  bool IsBookPresent(int Id)
        {
            // search in the list if the book already exists

            for(int i=0;i<BooksList.Count;i++)
            {
                if (BooksList[i].BookId == Id) return true;
            }
            return false;


        }
        public bool IsBorrowerPresent(int Id)
        {
            // search in the list if the borrower already exists

            for (int i = 0; i < BorrowersList.Count; i++)
            {
                if (BorrowersList[i].BorrowerId == Id) return true;
            }
            return false;
        }

        private bool IsEmailPresent(Borrower borrower)
        {
            // for unique Email
            foreach(Borrower b in BorrowersList)
            {
                if (b.Email == borrower.Email) return true;
            }
            return false;
        }
        public int NumberOfBooks()
        {
            return BooksList.Count;
        }
        public void ShowCurrentBooks()
        {
            // make sure that number of Books are greater than 0
            List<Book> books = GetAllBooks();
            foreach (Book book in books)
            {
                    Console.WriteLine($"Book Id : {book.BookId}, Title : {book.Title}, Author : {book.Author}, Genre : {book.Genre}, IS Available : {book.IsAvailable}");
            }

            
        }
        private bool BorrowedBook(int BorrowerId,int BookId)
        {
            // is this book borrowed by the borrower with the id 
            foreach(Transaction t in TransactionsList)
            {
                if(t.BorrowerId==BorrowerId && t.BookId == BookId)
                {
                    return true;
                }
            }
            return false;
        }
   
        // function to retriev data from files to lists
        public void  FetchData()
        {
            FetchBooks();
            FetchBorrowers();
            FetchTransactions();
        }
       
        private  void FetchBooks()
        {
            // fetch data from files
            string FilePath = "Books.txt";
            if (File.Exists(FilePath))
            {
                FileStream fin = new FileStream(FilePath,FileMode.Open, FileAccess.Read);
                StreamReader reader=new StreamReader(fin);
                string line;
                while((line=reader.ReadLine()) != null)
                {
                    // read until end of file
                    string[] parts = line.Split(',');
                    if (parts.Length == 5)
                    {
                        int bookId = int.Parse(parts[0]);
                        string title = parts[1];
                        string author = parts[2];
                        string genre = parts[3];
                        bool isAvailable = bool.Parse(parts[4]);

                        Book book = new Book(bookId, title, author, genre, isAvailable);
                        BooksList.Add(book);
                    }
                }
                reader.Close();
                fin.Close();
            }

        }

        private void FetchBorrowers()
        {
            // fetch data from files
            string FilePath = "Borrowers.txt";
            if (File.Exists(FilePath))
            {
                FileStream fin = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(fin);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // read until end of file
                    string[] parts = line.Split(',');
                    if (parts.Length == 3)
                    {
                        int borrowerId = int.Parse(parts[0]);
                        string name = parts[1];
                        string email = parts[2];

                        Borrower borrower = new Borrower(borrowerId, name, email);
                        BorrowersList.Add(borrower);
                    }
                }
                reader.Close();
                fin.Close();
            }
            
        }


        private void FetchTransactions()
        {
            // fetch data from files
            string FilePath = "Transactions.txt";
            if (File.Exists(FilePath))
            {
                FileStream fin = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(fin);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // read until end of file
                    string[] parts = line.Split(',');
                    if (parts.Length == 5)
                    {
                        int transactionId = int.Parse(parts[0]);
                        int bookId = int.Parse(parts[1]);
                        int borrowerId = int.Parse(parts[2]);
                        DateTime date = DateTime.Parse(parts[3]);
                        bool isBorrowed = bool.Parse(parts[4]);

                        Transaction transaction = new Transaction(transactionId, bookId, borrowerId, date, isBorrowed);
                        TransactionsList.Add(transaction);
                    }
                }
                reader.Close();
                fin.Close();
            }
           
        }

        // function to store data from lists to files
        public void StoreData()
        {
            StoreBooks();
            StoreBorrowers();
            StoreTransactions();
        }


        private void StoreBooks()
        {
            // store data from lists to Files
            FileStream fout = new FileStream("Books.txt", FileMode.Create);
            StreamWriter writer = new StreamWriter(fout);
            foreach(Book b in BooksList)
            {
                string data = $"{b.BookId},{b.Title},{b.Author},{b.Genre},{b.IsAvailable}";
                writer.WriteLine(data);
            }
            writer.Close();
            fout.Close();

        }
        private void StoreBorrowers()
        {
            // store data from lists to Files
            FileStream fout = new FileStream("Borrowers.txt", FileMode.Create);
            StreamWriter writer = new StreamWriter(fout);
            foreach (Borrower b in BorrowersList)
            {
                string data = $"{b.BorrowerId},{b.Name},{b.Email}";
                writer.WriteLine(data);
            }
            writer.Close();
            fout.Close();
        }
        private void StoreTransactions()
        {
            // store data from lists to Files
            FileStream fout = new FileStream("Transactions.txt", FileMode.Create);
            StreamWriter writer = new StreamWriter(fout);
            foreach (Transaction t in TransactionsList)
            {
                string data = $"{t.TransactionId},{t.BookId},{t.BorrowerId},{t.Date},{t.IsBorrowed}";
                writer.WriteLine(data);
            }
            writer.Close();
            fout.Close();
        }


    }
}
