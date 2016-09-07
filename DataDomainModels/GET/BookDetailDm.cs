using System;

namespace DataDomainModels
{
    public class gBookDetailDm
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public DateTime PublishDate { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Author { get; set; }
    }
}