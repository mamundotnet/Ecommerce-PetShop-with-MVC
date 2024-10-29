using PetShop.Models.InputModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PetShop.Models;
namespace PetShop.Controllers
{
    public class BrandsController : Controller
    {
        // GET: Brands
        private ProductsDbContext db = new ProductsDbContext();

        public ActionResult Index()
        {
            var brands = db.Brands.Include(f => f.Category).Include(f => f.SubCategory);
            return View(brands.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.categories = db.Categories.OrderBy(c => c.CategoryName);
            return View();
        }
        public JsonResult getSubCategories(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var list = db.SubCategories.Where(c => c.CategoryId == id).OrderBy(c => c.SubCategoryName);
            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Brand brand)
        {
            if (ModelState.IsValid)
            {
                db.Brands.Add(brand);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName", brand.CategoryId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "Id", "SubCategoryName", brand.SubCategoryId);
            return View(brand);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName", brand.CategoryId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "Id", "SubCategoryName", brand.SubCategoryId);
            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CategoryId,SubCategoryId,BrandName")] Brand brand
         )
        {
            if (ModelState.IsValid)
            {
                db.Entry(brand).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName", brand.CategoryId);
            ViewBag.SubCategoryId = new SelectList(db.SubCategories, "Id", "SubCategoryName", brand.SubCategoryId);
            return View(brand);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Brand brand = db.Brands.Find(id);
            db.Brands.Remove(brand);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}