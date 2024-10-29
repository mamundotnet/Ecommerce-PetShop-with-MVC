using PetShop.Models.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace PetShop.Controllers
{
    public class SubCategoriesController : Controller
    {
        // GET: SubCategories
        ProductsDbContext db = new ProductsDbContext();

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name" : "";

            ViewBag.CurrentFilter = searchString;

            var subcategories = from s in db.SubCategories
                                select s;


            if (!String.IsNullOrEmpty(searchString))
            {
                subcategories = subcategories.Where(x => x.SubCategoryName.ToLower().StartsWith(searchString.ToLower()));

            }

            switch (sortOrder)
            {
                case "name":
                    subcategories = subcategories.OrderByDescending(s => s.SubCategoryName);

                    break;


                default:  // Name ascending 
                    subcategories = subcategories.OrderBy(s => s.SubCategoryName);

                    break;
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);

            return View(subcategories.ToPagedList(pageNumber, pageSize));

        }
        public ActionResult Create()
        {
            ViewBag.categoryList = db.Categories.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Create(SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                db.SubCategories.Add(subCategory);
                db.SaveChanges();
                return PartialView("_success");
            }
            ViewBag.categoryList = db.Categories.ToList();
            return View("subCategory");

        }
        public ActionResult Edit(int id)
        {
            var s = db.SubCategories.Find(id);
            ViewBag.categoryList = db.Categories.ToList();
            return View(s);
        }
        [HttpPost]
        public ActionResult Edit(SubCategory s)
        {
            if (ModelState.IsValid)
            {
                db.Entry(s).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return PartialView("_success");
            };
            ViewBag.categoryList = db.Categories.ToList();
            return View(s);
        }
        public ActionResult Delete(int id)
        {
            var sub = db.SubCategories.Find(id);
            return View(sub);
        }
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {

            var s = db.SubCategories.Find(id);
            db.Entry(s).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return PartialView("_success");
        }
    }
}