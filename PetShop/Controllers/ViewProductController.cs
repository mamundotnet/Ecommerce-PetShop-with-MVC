using PetShop.Models.InputModels;
using PetShop.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace PetShop.Controllers
{
    public class ViewProductController : Controller
    {
        // GET: ViewProduct
        ProductsDbContext db;

        List<ShoppingCartModel> ListOfCart = new List<ShoppingCartModel>();
        List<RetriveProductView> ListofProduct = new List<RetriveProductView>();

        public ViewProductController()
        {
            db = new ProductsDbContext();
            ListOfCart = new List<ShoppingCartModel>();
            ListofProduct = new List<RetriveProductView>();
        }
        //[Authorize]
        public ActionResult Index()
        {
            IEnumerable<RetriveProductView> product = (
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

        [HttpPost]


        public JsonResult CartView(int Id)
        {

            ShoppingCartModel objShoppingCartModel = new ShoppingCartModel();
            Product objPro = db.Products.SingleOrDefault(Model => Model.ProductsId == Id);
            ProductImage objImg = db.ProductImages.SingleOrDefault(Model => Model.ProductsId == Id);
            if (Session["CartCounter"] != null)
            {
                ListOfCart = Session["CartItem"] as List<ShoppingCartModel>;
            }
            if (ListOfCart.Any(x => x.Id == Id))
            {
                objShoppingCartModel = ListOfCart.Single(x => x.Id == Id);
                objShoppingCartModel.Quantity = objShoppingCartModel.Quantity + 1;
                objShoppingCartModel.Total = objShoppingCartModel.Quantity * objShoppingCartModel.UnitPrice;
            }
            else
            {
                objShoppingCartModel.Id = Id;
                objShoppingCartModel.ImagePath = objImg.Images;
                objShoppingCartModel.ProductsName = objPro.ProductsName;
                objShoppingCartModel.Quantity = 1;
                objShoppingCartModel.UnitPrice = objPro.UnitPrice;
                objShoppingCartModel.Total = objPro.UnitPrice;
                ListOfCart.Add(objShoppingCartModel);
            }

            Session["CartCounter"] = ListOfCart.Count;
            Session["CartItem"] = ListOfCart;
            return Json(new { Success = true, Counter = ListOfCart.Count }, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult ShoppingCart()
        {
            ListOfCart = Session["CartItem"] as List<ShoppingCartModel>;
            return View(ListOfCart);
        }
        [HttpPost]
        [ActionName("ShoppingCart")]
        public ActionResult AddOrder()
        {
            int OrderId = 0;

            ListOfCart = Session["CartItem"] as List<ShoppingCartModel>;
            Order ord = new Order
            {
                OrderDate = DateTime.Now,
                OrderNumber = String.Format("{0:ddmmyyyyHHmmsss}", DateTime.Now)
            };
            db.Orders.Add(ord);
            db.SaveChanges();
            OrderId = ord.OrderId;

            foreach (var item in ListOfCart)
            {
                OrderDetailModel orderDetail = new OrderDetailModel();
                orderDetail.Total = item.Total;
                orderDetail.ProductId = item.Id.ToString();
                orderDetail.OrderId = OrderId;
                orderDetail.Quantity = item.Quantity;
                orderDetail.UnitPrice = item.UnitPrice;
                db.OrderDetails.Add(orderDetail);
                db.SaveChanges();
            }

            Session["CartCounter"] = null;

            Session["CartItem"] = null;

            return PartialView("_orderResult");
        }


        //[Authorize]
        public ActionResult ShowProduct(int id)
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
        //[Authorize]
        public ActionResult Category(string sortOrder, string searchString, int? page)
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
        //[Authorize]
        public ActionResult Brand(string sortOrder, /*string currentFilter,*/ string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name" : "";

            ViewBag.CategorySortParm = String.IsNullOrEmpty(sortOrder) ? "category" : "";
            ViewBag.SubCategorySortParm = String.IsNullOrEmpty(sortOrder) ? "subcategory" : "";

            ViewBag.CurrentFilter = searchString;

            var brand = from s in db.Brands
                        select s;
            var category = from c in db.Categories select c;
            var subcategory = from s in db.SubCategories select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                brand = brand.Where(x => x.BrandName.ToLower().StartsWith(searchString.ToLower()));

            }
            switch (sortOrder)
            {
                case "name":
                    brand = brand.OrderBy(b => b.BrandName);
                    break;


                default:  // Name des_cending 
                    brand = brand.OrderByDescending(s => s.BrandName);
                    category = category.OrderByDescending(c => c.CategoryName);
                    subcategory = subcategory.OrderByDescending(s => s.SubCategoryName);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(brand.ToPagedList(pageNumber, pageSize));
        }
        //[Authorize]
        public ActionResult SubCategory(string sortOrder, string currentFilter, string searchString, int? page)
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
    }
}