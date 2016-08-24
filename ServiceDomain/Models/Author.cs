using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceDomain.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Street { get; set; }
        public long Zip { get; set; }
        public ICollection<Book> Books { get; set; }

        public Author()
        {
            Books = new List<Book>();
        }
    }
}