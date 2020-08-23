using BookStore.API.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.API.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Years { get; set; }
        public string ISBN { get; set; }
        public string Summary { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }


        public AuthorDTO Author { get; set; }
        public int AuthorId { get; set; }
    }

    [NotMapped]
    public class BookCreateDTO
    {
        public string Title { get; set; }
        public int Years { get; set; }
        public string ISBN { get; set; }
        public string Summary { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }

        [Required]
        public int AuthorId { get; set; }
    }

    [NotMapped]
    public class BookUpdateDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Years { get; set; }
        public string ISBN { get; set; }
        public string Summary { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }

        [Required]
        public int AuthorId { get; set; }
    }
}
