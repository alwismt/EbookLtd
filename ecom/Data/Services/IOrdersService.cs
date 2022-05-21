using ecom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecom.Data.Services
{
    public interface IOrdersService
    {
        //Task<Order> StoreOrderAsync(List<ShoppingCartItem> items, string userId, string userEmailAddress);
        Task<Order> StoreOrderAsync(List<ShoppingCartItem> items, string userId, string userEmailAddress, ApplicationUser user);
        Task<List<Order>> GetOrdersByUserIdAndRoleAsync(string userId, string userRole);
        //Task<List<Order>> GetOrdersByUserIdAsync(string userId);
        Task<List<Order>> GetOrdersAsync();
        Task<Order> GetOrdersByUserIdOrderIDAsync(string userId, int OrderId, string userRole);

        //Task GetUser(string userId);

        //Task<Address> SaveAddressAsync(int orderId, ApplicationUser user);

    }
}