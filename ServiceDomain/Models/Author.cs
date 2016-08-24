using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceDomain.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Book> Books { get; set; }

        public Author()
        {
            Books = new List<Book>();
        }
    }
}