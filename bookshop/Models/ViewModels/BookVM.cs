using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookshop.Models.ViewModels
{
    public class BookVM
    {
        public Book Book { get; set; }
        public IEnumerable<SelectListItem> AuthorSelectList { get; set; }
        public IEnumerable<SelectListItem> GenreSelectList { get; set; }
    }
}
