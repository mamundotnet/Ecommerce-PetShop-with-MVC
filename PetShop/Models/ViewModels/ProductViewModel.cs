using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PetShop.Models.ViewModels
{
    public class ProductViewModel
    {
        [Key, Display(Name = "ID")]
        public int ProductsId { get; set; }
        [Required, Display(Name = "Product Name")]
        public string ProductsName { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [Display(Name = "Sub Category")]
        public int SubCategoryId { get; set; }
        [Display(Name = "Brand")]
        public int BrandId { get; set; }

        [Display(Name = "Per Unit")]
        public string QuantityPerUnit { get; set; }

        [Column(TypeName = "money"), Display(Name = "Unit Price")]
        public double UnitPrice { get; set; }
        [Display(Name = "Stock Quantity")]
        public double Quantity { get; set; }

        [Display(Name = "Stock In")]
        public bool StockInStatus { get; set; }
        public DateTime StoreDate { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public string Images { get; set; }
        public HttpPostedFileBase Picture { get; set; }
    }
}