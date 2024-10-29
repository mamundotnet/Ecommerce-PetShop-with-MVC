using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PetShop.Models.InputModels;

namespace PetShop.Models.InputModels
{
    public class Customers
    {
        [Key]
        public Guid CustomersId { get; set; }
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required, Display(Name ="Email")]
        public string Email { get; set; }
        [Required, Display(Name = "Phone")]
        public string PhoneNumber { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, DataType(DataType.Password), Compare("Password")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
      
        
    }
    
}