using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShop.Models.InputModels
{
    public class Category
    {
        public int Id { get; set; }
        [Required,Display(Name ="Category")]
        public string CategoryName { get; set; }
        [Display(Name ="Description")]
        public string CategoryDescription { get; set; }
        //nev
        public virtual ICollection<Brand> Brands { get; set; }=new List<Brand>();
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
        public virtual ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
    }
}