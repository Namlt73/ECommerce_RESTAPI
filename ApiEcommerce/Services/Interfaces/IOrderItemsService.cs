using ApiEcommerce.Dtos.OrderDtos;
using ApiEcommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Services.Interfaces
{
    public interface IOrderItemsService
    {
        Task<Tuple<int, List<Order>>> GetOrderFromUser(User user = null, int page = 1, int pageSize = 6);
        Task<Order> Create(OrderDtoRequest formOrder, User user);
        int GetTotal(Order order);

        Task<Order> GetById(long id, bool includeAddress = false, bool includeUser = false,
            bool includeOrderItems = false);

        Task Delete(long id);
        Task<List<Order>> GetAllByUserId(long userId);
        Task<Tuple<int, List<Order>>> GetOrderByUser(long userId, int page = 1, int pageSize = 6);
        void Create(Order order);
        Task AddOrder(Order order);
        Task UpdateOrder(Order order);
        Task DeleteOrder(Order order);
    }
}
