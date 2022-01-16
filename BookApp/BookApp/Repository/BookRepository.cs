using AutoMapper;
using BookApp.Data;
using BookApp.Model;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApp.Repository
{
    public class BookRepository: IBookRepository
    {
        private readonly BookStoreContext _context;
        private readonly IMapper _mapper;

        public static int PAGE_SIZE { get; set; } = 1;
        public BookRepository(BookStoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public List<BookModel> GetAllBooks(string kw, decimal? from, decimal? to, string sortBy, int page)
        {
            var records = _context.Books.AsQueryable();

            if (!string.IsNullOrEmpty(kw))
            {
                records = records.Where(x => x.Title.Contains(kw));
            }

            if (from.HasValue && to.HasValue)
            {
                records = records.Where(x => x.Price >= from && x.Price <= to);
            }
            if (from.HasValue)
            {
                records = records.Where(x => x.Price >= from);
            }
            if (to.HasValue)
            {
                records = records.Where(x => x.Price <= from);
            }

            

            #region Sort
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "price_desc":
                        records = records.OrderByDescending(x => x.Price);
                        break;
                    case "price":
                        records = records.OrderBy(x => x.Price);
                        break;
                    case "title_desc":
                        records = records.OrderByDescending(x => x.Price);
                        break;
                    default:
                        records = records.OrderBy(x => x.Title);
                        break;
                }
            }
            #endregion
/*
            #region Paging
            records = records.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);
            #endregion*/
         /*   var result = records.Select(x => new BookModel
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Price = x.Price
            });*/

            var result = PaginatedList<Book>.Create(records, page, PAGE_SIZE);

            return result.Select(x => new BookModel
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Price = x.Price
            }).ToList();

            /*var records = await _context.Books.ToListAsync();
            return _mapper.Map<List<BookModel>>(records);*/


        }

        public async Task<BookModel> GetBookById(int bookId)
        {
            /*       var records = await _context.Books.Where(x => x.Id == bookId).Select(x => new BookModel()
                   {
                       Id = x.Id,
                       Title = x.Title,
                       Description = x.Description
                   }).FirstOrDefaultAsync();

                   return records;*/
            var book = await _context.Books.FindAsync(bookId);
            return _mapper.Map<BookModel>(book);
        }

        public async Task<int> AddBook(BookModel bookModel)
        {
            var book = new Book()
            {
                Title = bookModel.Title,
                Description = bookModel.Description
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return book.Id;
        }

        public async Task UpdateBook(int bookId, BookModel bookModel)
        {
            /*   var book = await _context.Books.FindAsync(bookId);
               if (book != null)
               {
                   book.Title = bookModel.Title;
                   book.Description = bookModel.Description;

                   await _context.SaveChangesAsync();
               }

               await _context.SaveChangesAsync();*/
            var book = new Book()
            {
                Id = bookId,
                Title = bookModel.Title,
                Description = bookModel.Description
            };
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookPatch(int bookId, JsonPatchDocument bookModel)
        {
            var book = await _context.Books.FindAsync(bookId);
            if(book != null)
            {
                bookModel.ApplyTo(book);
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteBook(int bookId)
        {
            var book = new Book() { Id = bookId };

            _context.Books.Remove(book);

            await _context.SaveChangesAsync();
        }
    }
}
