using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReFreshMVC.Models
{
    public class Product
    {
        [Required]
        public int ID { get; set; }
        [Required]
        [Display(Name = "Product SKU")]
        public int Sku { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Product Price")]
        [DataType(DataType.Currency)]
        public int Price { get; set; }
        public string Description { get; set; }
        [DataType(DataType.ImageUrl)]
        public string Image { get; set; }


    }
}
