using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.API.Data
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public int Years { get; set; }
        public string ISBN { get; set; }
        public string Summary { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }


        public Author Author { get; set; }
        [ForeignKey("AuthorId")]
        public int AuthorId { get; set; }
    }
}
