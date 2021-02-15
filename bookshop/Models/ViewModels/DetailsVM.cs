using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookshop.Models.ViewModels
{
    public class DetailsVM
    {
        public DetailsVM()
        {
            // if you don't want to do this in the Controller, you can do that here
            Book = new Book();
        }
        public Book Book { get; set; }
        public bool ExistsInCart { get; set; }
    }
}
