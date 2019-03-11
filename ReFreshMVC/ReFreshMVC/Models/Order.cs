using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReFreshMVC.Models
{
    public class Order
    {
        [Required]
        public int CartID { get; set; }

        [Required]
        public int ProductID { get; set; }

        [Required]
        [Display(Name = "Quantity")]
        [DataType(DataType.Currency)]
        public int Qty { get; set; }

        [Required]
        [Display(Name = "Total Price")]
        [DataType(DataType.Currency)]
        public int ExtPrice { get; set; }


        // Navigation Properties
        public Cart Cart { get; set; }
        public Product Product { get; set; }

    }
}
