using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReFreshMVC.Models.ViewModels
{
    public class CreditCardViewModel
    {
        public string Number { get; set; }
        public string ExpDate { get; set; }
        public Cart Cart { get; set; }
    }
}
