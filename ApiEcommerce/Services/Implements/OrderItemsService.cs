using ApiEcommerce.Data;
using ApiEcommerce.Dtos.OrderDtos;
using ApiEcommerce.Entities;
using ApiEcommerce.Services.Interfaces;
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
        public Task<Order> Create(OrderDtoRequest formOrder, User user)
        {
            throw new NotImplementedException();
        }

        public void Create(Order order)
        {
            throw new NotImplementedException();
        }

        public Task Delete(long id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> GetAllByUserId(long userId)
        {
            throw new NotImplementedException();
        }

        public Task<Order> GethById(long id, bool includeAddress = false, bool includeUser = false, bool includeOrderItems = false)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<int, List<Order>>> GetOrderByUser(long userId)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<int, List<Order>>> GetOrderFromUser(User user = null)
        {
            throw new NotImplementedException();
        }

        public int GetTotal(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
