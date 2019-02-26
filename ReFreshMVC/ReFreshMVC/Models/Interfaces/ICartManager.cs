﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReFreshMVC.Models.Interfaces
{
    public interface ICartManager
    {
        Task<Cart> CreateCartAsync(string user);
        Task CloseCartAsync(Cart cart);
        

    }
}
