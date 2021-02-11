using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bookshop.Models
{
    public class Author
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        // merge first and last name

        public string FullName { get {
                return String.Format("{0} {1}", FirstName, LastName);
            } }
    }
}
