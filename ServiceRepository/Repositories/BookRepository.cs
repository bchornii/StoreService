using ServiceDataAccess;
using System.Threading.Tasks;
using System;
using DataAccess.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using DataDomainModels;
using System.Linq.Expressions;

namespace ServiceRepository
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<IEnumerable<gBookDm>> GetBooks();
        Task<gBookDm> GetBook(int id);
        Task<IEnumerable<gBookDetailDm>> GetBookDetails(int id);
        Task<IEnumerable<gBookDm>> GetBooksByGenre(string genre);
        Task<IEnumerable<gBookDm>> GetBooksByAuthor(int authorId);
        Task<IEnumerable<gBookDm>> GetBooksByDate(DateTime publishDate);
        void UpsertBook(pBookDm book);

    }
    public class BookRepository : Repository<Book>, IBookRepository
    {
        private static readonly Expression<Func<Book, gBookDm>> AsBookDto = x => new gBookDm
        {
            Title = x.Title,
            Author = x.Author.Name,
            Genre = x.Genre
        };

        public BookRepository(IStoreServiceContext context) :
            base(context) { }

        public async Task<IEnumerable<gBookDm>> GetBooks()
        {
            return await _context.Authors
                           .Join(_context.Books,
                                 a => a.AuthorId,
                                 b => b.AuthorId,
                                 (a, b) => new gBookDm
                                 {
                                     Author = a.Name,
                                     Genre = b.Genre,
                                     Title = b.Title
                                 }).ToListAsync();
        }

        public async Task<gBookDm> GetBook(int id)
        {
            return await _context.Books.Include(b => b.Author)
                                 .Where(b => b.BookId == id)
                                 .Select(AsBookDto)
                                 .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<gBookDetailDm>> GetBookDetails(int id)
        {
            return await _context.Books
                                 .Where(b => b.AuthorId == id)
                                 .Select(b => new gBookDetailDm
                                 {
                                     Title = b.Title,
                                     Description = b.Description,
                                     Genre = b.Genre,
                                     Price = b.Price,
                                     PublishDate = b.PublishDate,
                                     Author = b.Author.Name
                                 }).ToListAsync();
        }  

        public async Task<IEnumerable<gBookDm>> GetBooksByGenre(string genre)
        {
            return await _context.Books
                           .Where(b => b.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase))
                           .Select(b => new gBookDm
                           {
                               Author = b.Author.Name,
                               Genre = b.Genre,
                               Title = b.Title
                           }).ToListAsync();
        }

        public async Task<IEnumerable<gBookDm>> GetBooksByAuthor(int authorId)
        {
            return await _context.Books
                                 .Where(b => b.AuthorId == authorId)
                                 .Select(AsBookDto)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<gBookDm>> GetBooksByDate(DateTime publishDate)
        {
            return await _context.Books
                                 .Where(b => DbFunctions.TruncateTime(b.PublishDate) == DbFunctions.TruncateTime(publishDate))
                                 .Select(AsBookDto)
                                 .ToListAsync();                                 
        }

        public void UpsertBook(pBookDm book)
        {
            var b = new Book
            {
                BookId = book.BookId,
                Title = book.Title,
                Price = book.Price.Value,
                Genre = book.Genre,
                PublishDate = book.PublishDate.Value,
                Description = book.Description,
                AuthorId = book.AuthorId.Value
            };

            _context.Entry(b).State = (b.BookId != 0) ? 
                EntityState.Modified : EntityState.Added;            
        }
    }
}
