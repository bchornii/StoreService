using ServiceDomain.ModelBinders;
using System.Web.Http.ModelBinding;

namespace ServiceDomain.DTOs
{
    //[TypeConverter(typeof(BooksTypeConverter))]
    [ModelBinder(typeof(BooksModelBinder))]
    public class uBookDto
    {
        public int? BookId { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public int? AuthorId { get; set; }

        public static bool TryParse(string s, out uBookDto result)
        {
            result = null;
            var parts = new string[4];
            if (!string.IsNullOrEmpty(s) && s.Split(',').Length <= 4)
            {
                s.Split(',').CopyTo(parts, 0);
            }
            else
            {
                return false;
            }

            var bookId = -1;
            var authorId = -1;
            if (int.TryParse(parts[0], out bookId) ||
               !string.IsNullOrEmpty(parts[1]) ||
               !string.IsNullOrEmpty(parts[2]) ||
               int.TryParse(parts[3], out authorId))
            {
                result = new uBookDto
                {
                    BookId = bookId,
                    Title = parts[1],
                    Genre = parts[2],
                    AuthorId = authorId
                };
                return true;
            }
            return false;
        }
    }
}
