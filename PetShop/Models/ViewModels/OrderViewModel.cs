using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PetShop.Models.ViewModels
{
    public class OrderViewModel
    {
        [Key]
        public int OrderId { get; set; }
        [Display(Name = "Order Number")]
        public string OrderNumber { get; set; }
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        public double Quantity { get; set; }
        [Display(Name = "Unit Price")]
        public double UnitPrice { get; set; }
        public double Total { get; set; }
    }
}