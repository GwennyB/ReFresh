using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReFreshMVC.Models
{
    public class Product
    {
        public int ID { get; set; }
        public int Sku { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}
