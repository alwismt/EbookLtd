using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ecom.Data.Services;
using ecom.Data.Cart;
using ecom.Data.ViewModels;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
    
namespace ecom.Controllers
{
    [Route("admin")]
    [Authorize(Roles = "Admin")]
    public class AdminOrdersController : Controller
    {
        private readonly IBooksService _booksService;
        private readonly IOrdersService _ordersService;
        
        public AdminOrdersController(IBooksService booksService, IOrdersService ordersService)
        {
            _booksService = booksService;
            _ordersService = ordersService;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("lk-LK");
        }


        [Route("orders")]
        public async Task<IActionResult> Orders()
        {
            var orders = await  _ordersService.GetOrdersAsync();
            return View("orders", orders);
        }


    }
}