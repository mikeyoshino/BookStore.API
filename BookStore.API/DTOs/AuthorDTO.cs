using BookStore.API.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.API.DTOs
{
    public class AuthorDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }

        public virtual IList<BookDTO> Books { get; set; }
    }

    [NotMapped]
    public class AuthorCreateDTO
    { 
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Bio { get; set; }
    }

    [NotMapped]
    public class AuthorUpdateDTO
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Bio { get; set; }

        public virtual IList<BookDTO> Books { get; set; }
    }
}
