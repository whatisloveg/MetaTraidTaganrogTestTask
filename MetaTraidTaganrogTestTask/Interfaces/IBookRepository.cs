using MetaTraidTaganrogTestTask.Models;

namespace MetaTraidTaganrogTestTask.Interfaces
{
    public interface IBookRepository
    {
        List<Book> GetAllBooks();
        void AddBook(Book book);
        void EditBookDescription(int bookId, string description);
        void TakeBook(int bookId, int clientId);
        void ReturnBook(int bookId);
    }
}
