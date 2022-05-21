using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecom.Models;
using ecom.Data.Base;
using Microsoft.EntityFrameworkCore;

namespace ecom.Data.Services
{
    public class OrdersService:IOrdersService
    {
        private readonly AppDbContext _context;
        public OrdersService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Order>> GetOrdersByUserIdAndRoleAsync(string userId, string userRole)
        {
            // var orders = await _context.Orders.Include(n => n.OrderItems).ThenInclude(n => n.Book).Where(n => n.UserId == userId).ToListAsync();

            var orders = await _context.Orders.Include(n => n.Address).Include(n => n.OrderItems).ThenInclude(n => n.Book).Include(n => n.User).ToListAsync();
            if(userRole != "Admin")
            {
                orders = orders.Where(n => n.UserId == userId).ToList();
            }

            //var orders = await _context.Orders.Include(n => n.OrderItems).ThenInclude(n => n.Book).Include(n => n.User).ToListAsync();

            // if(userRole != "Admin")
            // {
            //     orders = orders.Where(n => n.UserId == userId).ToList();
            // }

            return orders;
        }

        public async Task<Order> GetOrdersByUserIdOrderIDAsync(string userId, int orderId, string userRole)
        {
            if(userRole == "Admin")
            {
                var order = await _context.Orders.Include(n => n.Address).Include(n => n.OrderItems).ThenInclude(n => n.Book).Include(n => n.User).Where(n => (n.Id == orderId)).FirstOrDefaultAsync();
                return order;
            }

            var orders = await _context.Orders.Include(n => n.Address).Include(n => n.OrderItems).ThenInclude(n => n.Book).Include(n => n.User).Where(n => (n.UserId == userId) && (n.Id == orderId)).FirstOrDefaultAsync();
            return orders;
        }

        public async Task <List<Order>> GetOrdersAsync()
        {
            var orders = await _context.Orders.Include(n => n.Address).Include(n => n.OrderItems).ThenInclude(n => n.Book).OrderByDescending(a => a.Id).ToListAsync();
            return orders;
        }

        public async Task<Order> StoreOrderAsync(List<ShoppingCartItem> items, string userId, string userEmailAddress, ApplicationUser user)
        {
            var order = new Order()
            {
                UserId = userId,
                Email = userEmailAddress
            };
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            foreach (var item in items)
            {
                var orderItem = new OrderItem()
                {
                    Amount = item.Amount,
                    BookId = item.Book.Id,
                    OrderId = order.Id,
                    Price = item.Book.Price
                };
                await _context.OrderItems.AddAsync(orderItem);
            }
            await _context.SaveChangesAsync();

            var address = new Address()
            {
                OrderId = order.Id,
                FullName = user.FirstName +" "+ user.LastName,
                ShipAddress = user.Address,
                Apartment = user.Apartment,
                District = user.District,
                Postcode = user.Postcode,

            };
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();


            return order;
        }

        public async Task<Address> SaveAddressAsync(int orderId, ApplicationUser user)
        {
            var address = new Address()
            {
                OrderId = orderId,
                FullName = user.FirstName +" "+ user.LastName,
                ShipAddress = user.Address,
                Apartment = user.Apartment,
                District = user.District,
                Postcode = user.Postcode,

            };
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();


            return address;
        }

    }
}