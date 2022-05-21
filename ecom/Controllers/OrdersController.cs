using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ecom.Data.Services;
using ecom.Data.Cart;
using ecom.Data.ViewModels;
using Microsoft.AspNetCore.Identity;
using ecom.Models;
using ecom.Data.Static;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ecom.Data;

namespace ecom.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IBooksService _booksService;
        private readonly ShoppingCart _shoppingCart;
        private readonly IOrdersService _ordersService;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly AppDbContext _context;

        public OrdersController(IBooksService booksService, ShoppingCart shoppingCart, IOrdersService ordersService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext context)
        {
            _booksService = booksService;
            _shoppingCart = shoppingCart;
            _ordersService = ordersService;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [Route("cart")]
        public IActionResult Index()
        {
            var items = _shoppingCart.GetShoppingCartItems();

            _shoppingCart.ShoppingCartItems = items;

            var response = new ShoppingCartVM()
            {
                shoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };

            return View("cart", response);
        }
        [Route("/addtoshoppingcart")]
        public async Task<RedirectToActionResult> AddToShoppingCart(int id, int? qty)
        {
            var item = await _booksService.GetBookByIdAsync(id);
            if(item != null)
            {   if(qty == null){
                    _shoppingCart.AddItemToCart(item);
                }
                else{
                    if(qty == 1)
                    {
                    _shoppingCart.AddItemToCart(item);
                    }
                    else{
                        int i;
                        for(i =1; i <= qty; i++)
                        {
                            _shoppingCart.AddItemToCart(item);
                        }
                    }
                }
            }
            return RedirectToAction(nameof(Index));

        }
        [Route("removecartitems/")]
        public async Task<RedirectToActionResult> RemoveFromShoppingCart(int id, int? qty)
        {
            var item = await _booksService.GetBookByIdAsync(id);
            if(item != null)
            {   
                 if(qty != null)
                {
                    int i;
                    for(i =1; i <= qty; i++)
                    {
                        _shoppingCart.RemoveItemFromCart(item);
                    }
                }
                else{
                     _shoppingCart.RemoveItemFromCart(item);
                }
            }
            return RedirectToAction(nameof(Index));

        }
        [Route("checkout")]
        public IActionResult Checkout()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            if(items.Count == 0) return RedirectToAction("Shop", "Home");
            _shoppingCart.ShoppingCartItems = items;

            var cartdata = new ShoppingCartVM()
            {
                shoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };
            ViewBag.cartdata = cartdata;
            if(User.Identity.IsAuthenticated)
            {
                if(User.IsInRole("User"))
                {
                    var userId = _userManager.GetUserId(User);
                    var user = _context.Users.Where(n => n.Id == userId).FirstOrDefault();
                    var res = new LoggedOrderVM()
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Address = user.Address,
                        Apartment = user.Apartment,
                        District = user.District,
                        Postcode = user.Postcode,
                    };
                    ViewBag.atribute = "disabled readonly";
                    return View("checkoutLogged", res);
                }
            }
            var response = new RegisterVm();
            return View("checkout", response);
        }

        [HttpPost("order/checkout")]
        public async Task<IActionResult> OrderSave(RegisterVm registerVm)
        {
            if(!ModelState.IsValid) 
            {
                var items = _shoppingCart.GetShoppingCartItems();
                //if(items.Count == 0) return RedirectToAction("Shop", "Home");
                _shoppingCart.ShoppingCartItems = items;

                var cartdata = new ShoppingCartVM()
                {
                    shoppingCart = _shoppingCart,
                    ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
                };
                ViewBag.cartdata = cartdata;

                return View("Checkout", registerVm);
            }
            
            var user = await _userManager.FindByEmailAsync(registerVm.EmailAddress);
            //return RedirectToAction(nameof(Index));
            if(user != null)
            {
                TempData["Error"] = "This email address is already in use.";
                return RedirectToAction(nameof(Checkout), registerVm);
            }
            var newuser = new ApplicationUser()
            {
                FirstName = registerVm.FirstName,
                LastName = registerVm.LastName,
                Address = registerVm.Address,
                Apartment = registerVm.Apartment,
                District = registerVm.District,
                Postcode = registerVm.Postcode,
                PhoneNumber = registerVm.PhoneNumber,
                Email = registerVm.EmailAddress,
                UserName = registerVm.EmailAddress
            };

            var newUserResponse = await _userManager.CreateAsync(newuser, registerVm.Password);
            
            if(newUserResponse.Succeeded)
            {
                var reguser = await _userManager.FindByEmailAsync(registerVm.EmailAddress);
                await _userManager.AddToRoleAsync(newuser, UserRoles.User);

                
                var result = await _signInManager.PasswordSignInAsync(reguser, registerVm.Password, false, false);

                var cartitems = _shoppingCart.GetShoppingCartItems();
                string userId = newuser.Id;
                string userEmailAddress = newuser.Email;
                
                var order = await _ordersService.StoreOrderAsync(cartitems, userId, userEmailAddress, newuser);



                await _shoppingCart.ClearShoppingCartAsync();
                
                return RedirectToAction(nameof(OrderConfirmed), new { orderId = order.Id });
            
            }
            else{
                var items = _shoppingCart.GetShoppingCartItems();
                //if(items.Count == 0) return RedirectToAction("Shop", "Home");
                _shoppingCart.ShoppingCartItems = items;

                var cartdata = new ShoppingCartVM()
                {
                    shoppingCart = _shoppingCart,
                    ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
                };
                ViewBag.cartdata = cartdata;
                return View("Checkout", registerVm);
            }
            
            
        }


        [HttpPost("order/checkout/user")]
        [Authorize]
        public async Task<IActionResult> OrderSaveUser(LoggedOrderVM loggedOrderVM)
        {
            if(!ModelState.IsValid) 
            {
                var items = _shoppingCart.GetShoppingCartItems();
                if(items.Count == 0) return RedirectToAction("Shop", "Home");
                _shoppingCart.ShoppingCartItems = items;

                var cartdata = new ShoppingCartVM()
                {
                    shoppingCart = _shoppingCart,
                    ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
                };
                ViewBag.cartdata = cartdata;

                return View("checkoutLogged", loggedOrderVM);
            }
            var email = _userManager.GetUserName(User);
            var user = await _userManager.FindByEmailAsync(email);
            
            //return RedirectToAction(nameof(Index));
            if(user == null)
            {
                TempData["Error"] = "Something wrong! Please try sign out and sign in again.";
                return RedirectToAction(nameof(Checkout), loggedOrderVM);
            }
            
            user.Address = loggedOrderVM.Address;
            user.Apartment = loggedOrderVM.Apartment;
            user.District = loggedOrderVM.District;
            user.Postcode = loggedOrderVM.Postcode;

            var cartitems = _shoppingCart.GetShoppingCartItems();
            string userId = _userManager.GetUserId(User);
            string userEmailAddress = email;
            
            var order = await _ordersService.StoreOrderAsync(cartitems, userId, userEmailAddress, user);
            //var address = await _ordersService.SaveAddress(order.Id, user);
            await _shoppingCart.ClearShoppingCartAsync();
            
            return RedirectToAction(nameof(OrderConfirmed), new { orderId = order.Id });



        }

        [Route("order/invoice")]
        [Authorize]
        public async Task<IActionResult> OrderConfirmed(int? orderid)
        {
            if(orderid == null) return View("NotFound");
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            //string userEmailAddress = User.FindFirstValue(ClaimTypes.Email);
            //await _ordersService.StoreOrderAsync()
            
            var order = await _ordersService.GetOrdersByUserIdOrderIDAsync(userId, (int)orderid, userRole);
            if(order == null) return View("NotFound");
            
            return View("orderConfirmed", order);
            
        }

        [Route("myorders")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> MyOrders()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var orders = await  _ordersService.GetOrdersByUserIdAndRoleAsync(userId, userRole);
            return View("myOrders", orders);
        }

        
        
    }
}