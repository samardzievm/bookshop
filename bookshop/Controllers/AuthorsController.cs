using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bookshop.Data;
using bookshop.Models;

namespace bookshop.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AuthorsController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Author> objList = _db.Author;
            return View(objList);
        }

        // GET: /Authors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST - CREATE 
        [HttpPost]
        [ValidateAntiForgeryToken] // security token 
        public IActionResult Create(Author obj)
        {
            if (ModelState.IsValid)
            {
                _db.Author.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        // GET: EDIT
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _db.Author.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        // POST - EDIT 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Author obj)
        {
            if (ModelState.IsValid)
            {
                _db.Author.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        // GET: DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _db.Author.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        // POST - DELETE 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Author.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Author.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
