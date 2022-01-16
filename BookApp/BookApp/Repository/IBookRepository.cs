using BookApp.Model;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookApp.Repository
{
    public interface IBookRepository
    {
       List<BookModel> GetAllBooks(string kw, decimal? from, decimal? to, string sortBy, int page);
       Task<BookModel> GetBookById(int bookId);
       Task<int> AddBook(BookModel bookModel);
       Task UpdateBook(int bookId, BookModel bookModel);
       Task UpdateBookPatch(int bookId, JsonPatchDocument bookModel);
       Task DeleteBook(int bookId);
    }
}
