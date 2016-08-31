using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceDomain.DTOs
{
    public class pBookDto
    {        
        public int BookId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public decimal? Price { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        public DateTime? PublishDate { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int? AuthorId { get; set; }        
    }
}