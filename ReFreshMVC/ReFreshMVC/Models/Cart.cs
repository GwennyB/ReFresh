using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReFreshMVC.Models
{
    public class Cart
    {
        // primary key
        [Required]
        public int ID { get; set; }

        // from user claim 'FullName'
        [Required]
        public string UserName { get; set; }

        // timestamp for cart closure / transaction complete
        public DateTime? Completed { get; set; }
        
        // join collection
        public ICollection<Order> Orders { get; set; }

        // total cost for all orders
        public int Total { get; set; }

    }
}
