using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookshop.Models.ViewModels
{
    public class ProductUserVM
    {
        // this VM will display the User details and Product details
        public ProductUserVM()
        {
            ProductList = new List<Book>();
        }

        public ApplicationUser ApplicationUser { get; set; }
        public IList<Book> ProductList { get; set; } // will be approached with for loop instead of foreach. foreach works for IEnuerable
    }
}
