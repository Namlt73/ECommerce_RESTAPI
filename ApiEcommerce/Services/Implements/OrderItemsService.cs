using ApiEcommerce.Data;
using ApiEcommerce.Dtos.OrderDtos;
using ApiEcommerce.Entities;
using ApiEcommerce.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Services.Implements
{
    public class OrderItemsService : IOrderItemsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductsService _productsService;
        private readonly IAddressesService _addressesService;

        public OrderItemsService(ApplicationDbContext context, IProductsService productsService,
            IAddressesService addressesService)
        {
            _context = context;
            _productsService = productsService;
            _addressesService = addressesService;
        }
        public async Task<Order> Create(OrderDtoRequest formOrder, User user)
        {
            if (formOrder.CartItems == null)
                return null;
            var order = new Order();
            Address address;
            if (user != null && formOrder.AddressId != null)
            {
                address = await _addressesService.GetAddressById(formOrder.AddressId);
                if (address?.User?.Id != user.Id)
                    throw new Exception("You can not use this address for your order");
            }
            else if (formOrder.AddressId == null)
            {
                address = new Address
                {
                    StreetAddress = formOrder.Address,
                    FirstName = formOrder.FirstName,
                    LastName = formOrder.LastName,
                    ZipCode = formOrder.ZipCode,
                    City = formOrder.City,
                    Country = formOrder.Country
                };
                if (user != null)
                    address.User = user;
            }
            else
            {
                throw new Exception("Error! An error occurred. Please try again later");
            }

            order.Address = address;
            if (user != null)
                order.User = user;


            var orderItems = new List<OrderItem>();
            IEnumerable<long> productIds = formOrder.CartItems.Select(c => c.Id);


            List<Product> products = await _productsService.GetByIdInRetrieveNamePriceAndSlug(productIds);

            if (products.Count != formOrder.CartItems.Count)
                return null;

            for (int i = 0; i < products.Count; i++)
            {
                var product = products[i];
                orderItems.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = formOrder.CartItems[i].Quantity,
                    Price = product.Price,
                    Name = product.Name,
                    Slug = product.Slug,
                    User = user
                });
            }

            order.OrderItems = orderItems;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public void Create(Order order)
        {
            _context.AttachRange(order.OrderItems.Select(p => p.Product));
            if (order.Id == 0)
            {
                _context.Orders.Add(order);
            }

            _context.SaveChanges();
        }

        public async Task Delete(long id)
        {
            Order order = await GetById(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Order>> GetAllByUserId(long userId)
        {
            return await _context.Orders.Where(o => o.User.Id == userId).ToListAsync();
        }

        public async Task<Order> GetById(long id, bool includeAddress = false, bool includeUser = false, bool includeOrderItems = false)
        {
            IQueryable<Order> queryable = _context.Orders.Where(o => o.Id == id);
            if (includeUser)
                queryable = queryable.Include(o => o.User);

            if (includeAddress)
                queryable = queryable.Include(o => o.Address);

            if (includeOrderItems)
                queryable = queryable.Include(o => o.OrderItems);


            return await queryable.FirstAsync();
        }

        public async Task<Tuple<int, List<Order>>> GetOrderByUser(long userId, int page = 1, int pageSize = 5)
        {
            IQueryable<Order> queryable = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(orderItem => orderItem.Product)
                .Include(o => o.Address)
                .Where(o => o.User.Id == userId);


            var count = queryable.Count();
            List<Order> orders = await queryable
                .Skip((page - 1) * pageSize).Take(pageSize)
                .ToListAsync();

            return Tuple.Create(count, orders);
        }

        public async Task<Tuple<int, List<Order>>> GetOrderFromUser(User user = null, 
            int page = 1, int pageSize = 6)
        {
            IQueryable<Order> queryable = _context.Orders.Include(o => o.OrderItems).ThenInclude(p => p.Product);
            if (user != null)
            {
                queryable = queryable.Where(o => o.User == user);
            }

            var count = queryable.Count();
            List<Order> orders = await queryable
                .Skip((page - 1) * pageSize).Take(pageSize)
                .ToListAsync();
            return Tuple.Create(count, orders);
        }

        public int GetTotal(Order order)
        {
            var total = 0;
            foreach (var item in order.OrderItems)
            {
                total += (item.Product.Price * item.Quantity);
            }

            return total;
        }
        public async Task AddOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrder(Order order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}
