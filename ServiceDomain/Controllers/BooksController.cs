using DataDomainModels;
using ServiceDomain.DTOs;
using ServiceDomain.Filters;
using ServiceDomain.HttpActionResultFactory;
using ServiceRepository;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http;

namespace ServiceDomain.Controllers
{
    [RoutePrefix("api/books")]
    public class BooksController : ApiController
    {
        IUnitOfWorkFactory _unitOfWork;
        public BooksController()
        {
            _unitOfWork = new UnitOfWorkFactory();
        }

        [HttpGet]
        [Route("all")]
        public async Task<IHttpActionResult> GetBooks()
        {
            using (var uow = _unitOfWork.CreateEfUnitOfWork())
            {
                var books_dms = await uow.Books.GetBooks();
                if(books_dms == null)
                {
                    return NotFound();
                }
                return Ok(books_dms.Select(dm => new gBookDto
                {
                    Author = dm.Author,
                    Genre = dm.Genre,
                    Title = dm.Title
                }).ToList());
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetBook(int id)
        {           
            using(var uow = _unitOfWork.CreateEfUnitOfWork())
            {
                var book_dm = await uow.Books.GetBook(id);
                if(book_dm == null)
                {
                    return NotFound();
                }
                return Ok(new gBookDto
                {
                    Author = book_dm.Author,
                    Genre = book_dm.Genre,
                    Title = book_dm.Title
                });
            }
        }

        [HttpGet]
        [Route("neg/{id:int}")]
        public async Task<IHttpActionResult> GetBookNegotiation(int id)
        {
            using (var uow = _unitOfWork.CreateEfUnitOfWork())
            {
                var book_dm = await uow.Books.GetBook(id);
                if(book_dm == null)
                {
                    return NotFound();
                }
                return new CustomResult<gBookDto>(this, new gBookDto
                {
                    Author = book_dm.Author,
                    Genre = book_dm.Genre,
                    Title = book_dm.Title
                });
            }
        }

        [Route("{id:int}/details")]
        public async Task<IHttpActionResult> GetBookDetail(int id)
        {
            using(var uow = _unitOfWork.CreateEfUnitOfWork())
            {
                var bookDetail_dm = await uow.Books.GetBookDetails(id);
                if(bookDetail_dm == null)
                {
                    return NotFound();
                }
                return Ok(bookDetail_dm.Select(dm => new gBookDetailDto
                {
                    Title = dm.Title,
                    Description = dm.Description,
                    Genre = dm.Genre,
                    Price = dm.Price,
                    PublishDate = dm.PublishDate,
                    Author = dm.Author
                }));
            }
        }

        [Route("{genre}")]
        public async Task<IHttpActionResult> GetBooksByGenre(string genre)
        {
            using(var uow = _unitOfWork.CreateEfUnitOfWork())
            {
                var books_dm = await uow.Books.GetBooksByGenre(genre);
                if(books_dm == null)
                {
                    return NotFound();
                }
                return Ok(books_dm.Select(dm => new gBookDto
                {
                    Author = dm.Author,
                    Genre = dm.Genre,
                    Title = dm.Title
                }).ToList());
            }
        }

        [Route("authors/{authorId}/books")]
        public HttpResponseMessage GetBooksByAuthor(int authorId)
        {
            using (var uow = _unitOfWork.CreateEfUnitOfWork())
            {
                var books_dm = Task.Run(() => uow.Books.GetBooksByAuthor(authorId)).Result;
                return Request.CreateResponse(HttpStatusCode.OK, books_dm.Select(dm => new gBookDto
                {
                    Author = dm.Author,
                    Title = dm.Title,
                    Genre = dm.Genre
                }).ToList());
            }               
        }

        [Route("date/{pubdate:datetime:regex(\\d{4}-\\d{2}-\\d{2})}")]
        [Route("date/{*pubdate:datetime:regex(\\d{4}/\\d{2}/\\d{2})}")]
        public async Task<HttpResponseMessage> GetBooks(DateTime pubdate)
        {
            using(var uow = _unitOfWork.CreateEfUnitOfWork())
            {
                var books_dm = await uow.Books.GetBooksByDate(pubdate);
                return Request.CreateResponse(HttpStatusCode.OK, books_dm.Select(dm => new gBookDto
                {
                    Author = dm.Author,
                    Genre = dm.Genre,
                    Title = dm.Title
                }).ToList());
            }
        }

        [HttpGet]
        [Route("formBook/{id}/{name}")]
        public async Task<IHttpActionResult> GetBooks(int id,string name, IPrincipal principal, uBookDto book)
        {            
            return Ok(await Task.FromResult(book.BookId + " : " + book.Title + " : " + book.Genre + " : " + book.AuthorId));
        }

        [HttpPost]
        [Route("upsert")]
        [ValidateModel]
        public async Task<IHttpActionResult> UpsertBook([FromBody] pBookDto book)
        {
            using(var uow = _unitOfWork.CreateEfUnitOfWork())
            {
                uow.Books.UpsertBook(new pBookDm
                {
                    BookId = book.BookId,
                    AuthorId = book.AuthorId,
                    Description = book.Description,
                    Genre = book.Genre,
                    Price = book.Price,
                    PublishDate = book.PublishDate,
                    Title = book.Title                    
                });
                await uow.Complete();
            }
            return Ok();
        }

        [HttpGet]
        [Route("principal")]
        public async Task<IHttpActionResult> GetPrincipal(IPrincipal principal)
        {
            return Ok(await Task.FromResult<string>(string.Format("Name = {0}", principal.Identity.Name)));
        }
    }
}
