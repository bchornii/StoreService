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
            var parts = s.Split(',');
            if (parts.Length < 1)
            {
                return false;
            }

            int bookId = -1;            
            int authorId = -1;
            if(int.TryParse(parts[0], out bookId) ||
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