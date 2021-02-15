using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bookshop.Data;
using bookshop.Models;
using Microsoft.AspNetCore.Hosting;
using bookshop.Models.ViewModels;
using System.IO;

namespace bookshop.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment; // to access the WC.cs class where we store variables (section 4-12) 
        public BooksController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Book> objList = _db.Book.Include(u => u.Author).Include(u => u.Genre);
            return View(objList);
        }

        // GET: Upsert  /* upsert is create and edit in the same action */
        public IActionResult Upsert(int? id) // ? is for create/add functionality (because we are not passing id parameter, id is for edit functionality)
        {
            // display all of the categories in a dropdown input type in the View

            /* Right approach with ViewModels */

            BookVM bookVM = new BookVM()
            {
                Book = new Book(),
                AuthorSelectList = _db.Author.Select(i => new SelectListItem
                {
                    Text = i.FullName,
                    Value = i.Id.ToString()
                }),
                GenreSelectList = _db.Genre.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                // this is to create
                return View(bookVM);
            }
            else
            {
                // this is to edit
                bookVM.Book = _db.Book.Find(id);
                if (bookVM.Book == null)
                {
                    return NotFound();
                }
                return View(bookVM);
            }
        }

        // POST: Upsert 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(BookVM bookVM)
        {
            if (ModelState.IsValid)
            {
                // below is written approach to upload the image files FROM local directory TO wwwroot/images/product/

                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (bookVM.Book.Id == 0)
                {
                    // Creating...
                    string upload = webRootPath + WC.ImagePath; // where we want to store(upload) the images
                    string fileName = Guid.NewGuid().ToString(); // what is the FileName we want to give that will be uploaded in the folder... Guid is random name
                    string extension = Path.GetExtension(files[0].FileName); // get the extension from files

                    // actual upload functionality
                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    bookVM.Book.Image = fileName + extension;
                    _db.Book.Add(bookVM.Book);
                }
                else
                {
                    // Updating...
                    var objFromDb = _db.Book.AsNoTracking().FirstOrDefault(u => u.Id == bookVM.Book.Id); // retrieve data from the database. Because EF can track only 1 file at the time, we don't need to track this file from EF, and we use the method AsNoTracking()

                    if (files.Count > 0)
                    {
                        // upload the new file
                        string upload = webRootPath + WC.ImagePath; // where we want to store(upload) the images
                        string fileName = Guid.NewGuid().ToString(); // what is the FileName we want to give that will be uploaded in the folder... Guid is random name
                        string extension = Path.GetExtension(files[0].FileName); // get the extension from files

                        // remove the old file
                        var oldFile = Path.Combine(upload, objFromDb.Image); // get the path from the app

                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        // add the new file
                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        bookVM.Book.Image = fileName + extension;
                    }
                    else
                    {
                        // if the image is not edited, then save/keep the same image
                        bookVM.Book.Image = objFromDb.Image;
                    }
                    _db.Book.Update(bookVM.Book);
                }
                _db.SaveChanges(); // without this, the changes in the app will not be saved in the database
                return RedirectToAction("Index");
            }

            bookVM.AuthorSelectList = _db.Author.Select(i => new SelectListItem
            {
                Text = i.FullName,
                Value = i.Id.ToString()
            });

            bookVM.GenreSelectList = _db.Genre.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            return View(bookVM);
        }



        // GET: DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //eager loading (two queries into one execution), the 2 objects are executing from the Index action
            Book book = _db.Book.Include(u => u.Author).Include(u => u.Genre).FirstOrDefault(u => u.Id == id); // take care of the Author and Genre (two foreign keys)

            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST - DELETE 
        [HttpPost, ActionName("Delete")] // because the action name is DeletePost, with adding ActionName("Delete") we tell the compiler that DeletePost need to execute Delete action (above)
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Book.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;

            var oldFile = Path.Combine(upload, obj.Image);

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }

            _db.Book.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
