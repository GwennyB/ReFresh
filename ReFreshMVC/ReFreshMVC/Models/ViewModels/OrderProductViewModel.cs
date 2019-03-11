using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReFreshMVC.Models.ViewModels
{
    public class OrderProductViewModel
    {
        [Required]
        public int ProductID { get; set; }

        [Required]
        public int CartID { get; set; }

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

        [Required]
        [Display(Name = "Quantity Available")]
        public int QtyAvail { get; set; }
        public string Description { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Image { get; set; }

        [Required]
        [Display(Name = "Category")]
        public Categories Category { get; set; }

        [Required]
        [Display(Name = "Contains Meat?")]
        public bool Meaty { get; set; }

        [Required]
        [Display(Name = "Quantity")]
        [DataType(DataType.Currency)]
        public int Qty { get; set; }

    }
}
