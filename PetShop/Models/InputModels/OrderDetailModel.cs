using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShop.Models.InputModels
{
    public class OrderDetailModel
    {
        [Key]
        public int OrderDetailsId { get; set; }
        public int OrderId { get; set; }
        public string ProductId { get; set; }
        public double Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double Total { get; set; }

        

    }
}