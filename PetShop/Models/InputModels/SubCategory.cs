using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShop.Models.InputModels
{
    public class SubCategory
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string SubCategoryName { get; set; }
        //nev
        public virtual ICollection<Brand> Brands { get; set; } = new List<Brand>();
        public virtual Category Category { get; set; }
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}