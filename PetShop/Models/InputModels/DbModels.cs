using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PetShop.Models.InputModels;

namespace PetShop.Models.InputModels
{
    public class ProductsDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetailModel> OrderDetails { get; set; }
        public DbSet<Customers> Customers { get; set; }
        
    }
    
}