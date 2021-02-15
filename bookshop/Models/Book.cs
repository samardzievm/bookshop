using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bookshop.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Display(Name ="Short Description")]
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        [Display(Name = "Year Published")]
        public int YearPublished { get; set; }
        [Range(1,int.MaxValue)]
        public double Price { get; set; }
        public string Image { get; set; }

        // foreign keys

        [Display(Name = "Author")]
        public int AuthorId { get; set; }
        [Display(Name = "Genre")]

        public int GenreId { get; set; }
        [ForeignKey("AuthorId")]
        public virtual Author Author { get; set; }
        [ForeignKey("GenreId")]
        public virtual Genre Genre { get; set; }
    }
}
