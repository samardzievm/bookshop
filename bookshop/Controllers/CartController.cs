using bookshop.Data;
using bookshop.Models;
using bookshop.Models.ViewModels;
using bookshop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace bookshop.Controllers
{
    [Authorize] 
    public class CartController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment; // with this, we will not save the edited information about the purchased products

        [BindProperty] // this will automatically assign to the actions when called the property (ProductUserVM)
        public ProductUserVM ProductUserVM { get; set; }

        public CartController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            // retreive all the products we listed in our shopping cart (from Session)
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                // session exists
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            // find all distinct items in the shopping cart
            List<int> prodInCart = shoppingCartList.Select(i => i.ProductId).ToList(); // list of product ids
            // retreive all of the products that matches the id from above
            IEnumerable<Book> prodList = _db.Book.Where(u => prodInCart.Contains(u.Id)); // all information from the matched id


            return View(prodList);
        }

        // When we click the continue button, this action will be called
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            // with this action, we will be Redirected to a new View that will display the User details and Product details
            return RedirectToAction(nameof(Summary));
        }

        // remove item from shopping cart
        public IActionResult Remove(int id)
        {
            // get all the items
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                // session exists
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            // remove the item that was clicked
            shoppingCartList.Remove(shoppingCartList.FirstOrDefault(u => u.ProductId == id));

            // after that line, we need to update the session
            HttpContext.Session.Set(WC.SessionCart, shoppingCartList); // instead of GET, now it is SET

            return RedirectToAction(nameof(Index));
        }


        // GET: SUMMARY
        public IActionResult Summary()
        {
            // with this action, we want to display User Name, User Email, User Phone Number

            var claimsIdentity = (ClaimsIdentity)User.Identity; // get the identity of the user
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier); // match the user
            // var userId = User.FindFirstValue(ClaimTypes.Name); // get the name of the user

            // now we need the shopping cart details
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                // session exists
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            List<int> prodInCart = shoppingCartList.Select(i => i.ProductId).ToList(); // list of product ids
            // retreive all of the products that matches the id from above
            IEnumerable<Book> prodList = _db.Book.Where(u => prodInCart.Contains(u.Id)); // all information from the matched id

            ProductUserVM = new ProductUserVM()
            {
                ApplicationUser = _db.ApplicationUser.FirstOrDefault(u => u.Id == claim.Value),
                ProductList = prodList.ToList() // very important, this will display the items in summary
            };

            return View(ProductUserVM);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [ActionName("Summary")]
        public IActionResult SummaryPost()
        {
            // with this action, we will be Redirected to a new View that will display the User details and Product details
            return RedirectToAction(nameof(InquiryConfirmation));
        }


        public IActionResult InquiryConfirmation() 
        {
            // with this action, we will clear the current ACTIVE SESSION and display the view
            HttpContext.Session.Clear();
            return View();
        }
    }
}

