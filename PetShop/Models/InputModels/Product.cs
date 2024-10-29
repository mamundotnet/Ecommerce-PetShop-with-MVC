using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PetShop.Models.InputModels
{
    public class Product
    {
        [Key]
        public int ProductsId { get; set; }
        public string ProductsName { get; set; }
        [ForeignKey("Category"),Display(Name ="Category")]
        public int CategoryId { get; set; }
        [ForeignKey("SubCategory"), Display(Name = "Sub Category")]
        public int SubCategoryId { get; set; }
        [ForeignKey("Brand"), Display(Name = "Brand")]
        public int BrandId { get; set; }
        public string QuantityPerUnit { get; set; }

        public double UnitPrice { get; set; }
        //nev

        public virtual Brand Brand { get; set; }
        public virtual Category Category { get; set; }
        public virtual ProductImage ProductImage { get; set; }
        public virtual SubCategory SubCategory { get; set; }
        

    }
}