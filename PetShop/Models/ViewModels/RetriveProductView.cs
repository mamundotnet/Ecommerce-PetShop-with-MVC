using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PetShop.Models.ViewModels
{
    public class RetriveProductView
    {
        [Key, Display(Name = "ID")]
        public int ProductsId { get; set; }
        [Required, Display(Name = "Product Name")]
        public string ProductsName { get; set; }
        [Display(Name = "Category")]
        public string CategoryName { get; set; }
        [Display(Name = "Sub Category")]

        public string SubCategoryName { get; set; }
        [Display(Name = "Brand")]
        public string BrandName { get; set; }
        [Display(Name = "Per Unit")]
        public string QuantityPerUnit { get; set; }
        [Column(TypeName = "money"), Display(Name = "Unit Price")]
        public double UnitPrice { get; set; }
        [Display(Name = "Stock Quantity")]
        public double QuantityInStock { get; set; }

        [Display(Name = "Status")]
        public bool StockInStatus { get; set; }

        [Display(Name = "Store Date"), Column(TypeName = "date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StoreDate { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
        public string Images { get; set; }
    }
}