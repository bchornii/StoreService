using ServiceDomain.Context;
using ServiceDomain.DTOs;
using ServiceDomain.Filters;
using ServiceDomain.HttpActionResultFactory;
using ServiceDomain.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ServiceDomain.Controllers
{
    [RoutePrefix("api/books")]
    public class BooksController : ApiController
    {
        IDbContextFactory _factory;
        public BooksController()
        {
            _factory = new DbContextFactory();
        }

        private static readonly Expression<Func<Book, GET_BookDto>> AsBookDto = x => new GET_BookDto
        {
            Title = x.Title,
            Author = x.Author.Name,
            Genre = x.Genre
        };

        [HttpGet]
        [Route("all")]
        public IEnumerable<GET_BookDto> GetBooks()
        {
            using(var context = _factory.CreateContext())
            {
                return context.Authors
                              .Join(context.Books,
                                    a => a.AuthorId,
                                    b => b.AuthorId,
                                    (a, b) => new GET_BookDto
                                    {
                                        Author = a.Name,
                                        Genre = b.Genre,
                                        Title = b.Title                                                                              
                                    }).ToList();                                                                                               
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetBook(int id)
        {
            using (var context = _factory.CreateContext())
            {
                GET_BookDto book = await context.Books.Include(b => b.Author)
                    .Where(b => b.BookId == id)
                    .Select(AsBookDto)
                    .FirstOrDefaultAsync();
                if (book == null)
                {
                    return NotFound();
                }

                return Ok(book);
            }
        }

        [HttpGet]
        [Route("neg/{id:int}")]
        public async Task<IHttpActionResult> GetBookNegotiation(int id)
        {
            using(var context = _factory.CreateContext())
            {
                var book = await context.Books.Include(b => b.Author)
                                        .Where(b => b.BookId == id)
                                        .Select(AsBookDto)
                                        .FirstOrDefaultAsync();                                              

                if (book == null)
                {
                    return NotFound();
                }
                return new CustomResult<GET_BookDto>(this, book);
            }
        }

        [Route("{id:int}/details")]
        public async Task<IHttpActionResult> GetBookDetail(int id)
        {
            using(var context = _factory.CreateContext())
            {
                var book = await context.Books
                                        .Where(b => b.AuthorId == id)
                                        .Select(b => new GET_BookDetailDto
                                        {
                                            Title = b.Title,
                                            Description = b.Description,
                                            Genre = b.Genre,
                                            Price = b.Price,
                                            PublishDate = b.PublishDate,
                                            Author = b.Author.Name
                                        }).ToListAsync();

                if (book == null)
                {
                    return NotFound();
                }
                return Ok(book);
            }
        }

        [Route("{genre}")]
        public IEnumerable<GET_BookDto> GetBooksByGenre(string genre)
        {
            using(var context = _factory.CreateContext())
            {
                return context.Books
                              .Where(b => b.Genre.Equals(genre,StringComparison.OrdinalIgnoreCase))
                              .Select(b => new GET_BookDto
                              {
                                  Author = b.Author.Name,
                                  Genre = b.Genre,
                                  Title = b.Title
                              }).ToList();
            }
        }

        [Route("authors/{authorId}/books")]
        public HttpResponseMessage GetBooksByAuthor(int authorId)
        {
            using(var context = _factory.CreateContext())
            {
                var books = context.Books
                              .Where(b => b.AuthorId == authorId)
                              .Select(AsBookDto)
                              .ToList();
                return Request.CreateResponse(HttpStatusCode.OK, books);
            }
        }

        [Route("date/{pubdate:datetime:regex(\\d{4}-\\d{2}-\\d{2})}")]
        [Route("date/{*pubdate:datetime:regex(\\d{4}/\\d{2}/\\d{2})}")]
        public HttpResponseMessage GetBooks(DateTime pubdate)
        {
            using (var context = _factory.CreateContext())
            {
                var books = context.Books
                              .Where(b => DbFunctions.TruncateTime(b.PublishDate) ==
                                          DbFunctions.TruncateTime(pubdate))
                              .Select(AsBookDto)
                              .ToList();
                return Request.CreateResponse(HttpStatusCode.OK, books);
            }
        }

        [HttpPost]
        [Route("upsert")]
        [ValidateModel]
        public async Task<IHttpActionResult> UpsertBook([FromBody] POST_BookDto book)
        {
            using(var contex = _factory.CreateContext())
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
                contex.Entry(b).State = (b.BookId != 0) ? 
                    EntityState.Modified : EntityState.Added;
                await contex.SaveChangesAsync();
            }
            return Ok();
        }
    }
}
