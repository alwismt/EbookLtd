using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ecom.Models;
using ecom.Data.Cart;

namespace ecom.Data.ViewModels

{
    public class ShoppingCartVM
    {
        public ShoppingCart shoppingCart { get; set; }

        public double ShoppingCartTotal { get; set; }
    }
}