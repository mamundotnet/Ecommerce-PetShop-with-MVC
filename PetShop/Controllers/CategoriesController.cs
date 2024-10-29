using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PetShop.Models.InputModels;
using PagedList;


namespace PetShop.Controllers
{
    public class CategoriesController : Controller
    {
        // GET: Categories
        private ProductsDbContext db = new ProductsDbContext();

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name" : "";
            ViewBag.CurrentFilter = searchString;

            var categories = from s in db.Categories
                             select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                categories = categories.Where(x => x.CategoryName.ToLower().StartsWith(searchString.ToLower()));
            }
            switch (sortOrder)
            {
                case "name":
                    categories = categories.OrderByDescending(s => s.CategoryName);
                    break;

                default:  // Name ascending 
                    categories = categories.OrderBy(s => s.CategoryName);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(categories.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Category cat)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(cat);
                db.SaveChanges();
                return PartialView("_success"); 
            }
            return PartialView("_error");
        }
        public ActionResult Edit(int id)
        {
            var category = db.Categories.Find(id);
            return View(category);
        }
        [HttpPost]
        public ActionResult Edit(Category c)
        {
            if (ModelState.IsValid)
            {
                db.Entry(c).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return PartialView("_success");
            }
            return View(c);
        }
        public ActionResult Delete(int id)
        {
            var category = db.Categories.Find(id);
            return View(category);
        }
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            var cat = db.Categories.Find(id);
            db.Categories.Remove(cat);
            db.SaveChanges();
            return PartialView("_success");
        }
    }
}