using PetShop.Models.InputModels;
using PetShop.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PetShop.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        ProductsDbContext db = new ProductsDbContext();

        // GET: Products

        [Authorize]
        public ActionResult Index()
        {

            var product = (
                               from p in db.Products
                               join pi in db.ProductImages on p.ProductsId equals pi.ProductsId
                               select new RetriveProductView
                               {
                                   ProductsId = p.ProductsId,
                                   ProductsName = p.ProductsName,
                                   CategoryName = p.Category.CategoryName,
                                   SubCategoryName = p.SubCategory.SubCategoryName,
                                   BrandName = p.Brand.BrandName,
                                   QuantityPerUnit = p.QuantityPerUnit,
                                   UnitPrice = p.UnitPrice,
                                   QuantityInStock = pi.QuantityInStock,
                                   StockInStatus = pi.StockInStatus,
                                   Description = pi.Description,
                                   StoreDate = pi.StoreDate,
                                   Images = pi.Images

                               }).ToList();

            return View(product);
        }
        public ActionResult Orders()
        {
            var order = db.Orders.ToList();
            return View(order);
        }
        public ActionResult OrderDetails()
        {
            var orderdetails = (from o in db.Orders
                                join od in db.OrderDetails on o.OrderId equals od.OrderId
                                select new OrderViewModel
                                {
                                    OrderId = o.OrderId,
                                    OrderNumber = o.OrderNumber,
                                    Quantity = od.Quantity,
                                    UnitPrice = od.UnitPrice,
                                    Total = od.Total
                                }).ToList();
            return View(orderdetails);
        }
        public ActionResult Create()
        {
            ViewBag.categoryList = db.Categories.ToList();
            ViewBag.brandList = db.Brands.ToList();
            ViewBag.subCategoryList = db.SubCategories.ToList();
            return View();
        }

        public JsonResult getSubCategories(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var list = db.SubCategories.Where(c => c.CategoryId == id).OrderBy(c => c.SubCategoryName);

            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult getBrands(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var list = db.Brands.Where(c => c.SubCategoryId == id).OrderBy(c => c.BrandName);

            return Json(list.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(ProductViewModel pv)
        {
            string msg = "";
            using (var context = new ProductsDbContext())
            {
                using (DbContextTransaction dbtran = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (ModelState.IsValid)
                        {
                            if (pv.Picture != null)
                            {
                                string filepath = Path.Combine("~/ProductPicture", Guid.NewGuid().ToString() + Path.GetExtension(pv.Picture.FileName));
                                pv.Picture.SaveAs(Server.MapPath(filepath));

                                Product pro = new Product
                                {
                                    ProductsId = pv.ProductsId,
                                    ProductsName = pv.ProductsName,
                                    CategoryId = pv.CategoryId,
                                    SubCategoryId = pv.SubCategoryId,
                                    BrandId = pv.BrandId,
                                    QuantityPerUnit = pv.QuantityPerUnit,
                                    UnitPrice = pv.UnitPrice,

                                };
                                ProductImage pi = new ProductImage
                                {
                                    ProductsId = pv.ProductsId,
                                    QuantityInStock = pv.Quantity,
                                    StockInStatus = pv.StockInStatus,
                                    Description = pv.Description,
                                    StoreDate = pv.StoreDate,
                                    Images = filepath
                                };
                                db.Products.Add(pro);
                                db.ProductImages.Add(pi);
                                db.SaveChanges();
                                return PartialView("_success");

                            }
                        }
                    }

                    catch (DbEntityValidationException ex)
                    {
                        dbtran.Rollback();
                        msg = ex.Message;
                        ViewBag.msg = msg;
                    }
                }
            }

            ViewBag.categoryList = db.Categories.ToList();
            ViewBag.brandList = db.Brands.ToList();
            ViewBag.subCategoryList = db.SubCategories.ToList();
            return View(pv);
        }

        public ActionResult Edit(int id)
        {

            Product p = db.Products.Find(id);
            ProductImage pi = db.ProductImages.Find(id);
            ProductViewModel pvm = new ProductViewModel
            {
                ProductsId = p.ProductsId,
                ProductsName = p.ProductsName,
                CategoryId = (int)p.CategoryId,
                SubCategoryId = (int)p.SubCategoryId,
                BrandId = (int)p.BrandId,
                UnitPrice = p.UnitPrice,
                QuantityPerUnit = p.QuantityPerUnit,
                Quantity = pi.QuantityInStock,
                StockInStatus = pi.StockInStatus,
                Description = pi.Description,
                StoreDate = (DateTime)pi.StoreDate,
                Images = pi.Images
            };
            ViewBag.categoryList = db.Categories.ToList();
            ViewBag.brandList = db.Brands.ToList();
            ViewBag.subCategoryList = db.SubCategories.ToList();
            return View(pvm);
        }

        [HttpPost]
        public ActionResult Edit(ProductViewModel pv)
        {
            if (ModelState.IsValid)
            {
                string filepath = pv.Images;
                if (pv.Picture != null)
                {
                    filepath = Path.Combine("~/ProductPicture", Guid.NewGuid().ToString() + Path.GetExtension(pv.Picture.FileName));
                    pv.Picture.SaveAs(Server.MapPath(filepath));

                    Product pro = new Product
                    {
                        ProductsId = pv.ProductsId,
                        ProductsName = pv.ProductsName,
                        CategoryId = pv.CategoryId,
                        SubCategoryId = pv.SubCategoryId,
                        BrandId = pv.BrandId,
                        QuantityPerUnit = pv.QuantityPerUnit,
                        UnitPrice = pv.UnitPrice,

                    };
                    ProductImage pi = new ProductImage
                    {
                        ProductsId = pv.ProductsId,
                        QuantityInStock = pv.Quantity,
                        StockInStatus = pv.StockInStatus,
                        Description = pv.Description,
                        StoreDate = pv.StoreDate,
                        Images = filepath
                    };
                    db.Entry(pro).State = System.Data.Entity.EntityState.Modified;
                    db.Entry(pi).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return PartialView("_success");

                }
                else
                {
                    Product pro = new Product
                    {
                        ProductsId = pv.ProductsId,
                        ProductsName = pv.ProductsName,
                        CategoryId = pv.CategoryId,
                        SubCategoryId = pv.SubCategoryId,
                        BrandId = pv.BrandId,
                        QuantityPerUnit = pv.QuantityPerUnit,
                        UnitPrice = pv.UnitPrice,

                    };
                    ProductImage pi = new ProductImage
                    {
                        ProductsId = pv.ProductsId,
                        QuantityInStock = pv.Quantity,
                        StockInStatus = pv.StockInStatus,
                        Description = pv.Description,
                        StoreDate = pv.StoreDate,
                        Images = filepath

                    };
                    db.Entry(pro).State = System.Data.Entity.EntityState.Modified;
                    db.Entry(pi).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return PartialView("_success");
                }

            }

            ViewBag.categoryList = db.Categories.ToList();
            ViewBag.brandList = db.Brands.ToList();
            ViewBag.subCategoryList = db.SubCategories.ToList();
            return View(pv);
        }
        public ActionResult Delete(int id)
        {

            Product p = db.Products.Find(id);
            ProductImage pi = db.ProductImages.Find(id);
            RetriveProductView pvm = new RetriveProductView
            {
                ProductsId = p.ProductsId,
                ProductsName = p.ProductsName,
                CategoryName = p.Category.CategoryName,
                SubCategoryName = p.SubCategory.SubCategoryName,
                BrandName = p.Brand.BrandName,
                QuantityPerUnit = p.QuantityPerUnit,
                UnitPrice = p.UnitPrice,
                QuantityInStock = (double)pi.QuantityInStock,
                StockInStatus = (bool)pi.StockInStatus,
                Description = pi.Description,
                StoreDate = (DateTime)pi.StoreDate,
                Images = pi.Images
            };
            return View(pvm);
        }
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            Product p = db.Products.Find(id);
            ProductImage pi = db.ProductImages.Find(id);
            string file_name = pi.Images;
            string path = Server.MapPath(file_name);
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                file.Delete();
            }
            db.Products.Remove(p);
            db.ProductImages.Remove(pi);
            db.SaveChanges();
            return PartialView("_success");

        }
    }
}