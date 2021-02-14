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
    public class GenresController : Controller
    {
        private readonly ApplicationDbContext _db;
        public GenresController(ApplicationDbContext db)
        {
            _db = db;
        }
        // GET: /Genres
        public IActionResult Index()
        {
            IEnumerable<Genre> objList = _db.Genre;
            return View(objList);
        }
        // GET: /Genres/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Genre obj)
        {
            _db.Genre.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: EDIT
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _db.Genre.Find(id);

            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        // POST - EDIT 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Genre obj)
        {
            if (ModelState.IsValid)
            {
                _db.Genre.Update(obj);
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

            var obj = _db.Genre.Find(id);

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
            var obj = _db.Genre.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            _db.Genre.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
