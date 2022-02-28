using ApiEcommerce.Dtos.AddressDtos;
using ApiEcommerce.Dtos.Handles;
using ApiEcommerce.Dtos.UserDtos;
using ApiEcommerce.Entities;
using ApiEcommerce.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Dtos.OrderDtos
{
    public class OrderDetailDto : SuccessResponse
    {
        public long Id { get; set; }
        public string TrackingNumber { get; set; }
        public ShippingStatus OrderStatus { get; set; }
        public UserBasicInfor User { get; set; }
        public AddressDto Address { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; }

        public static OrderDetailDto Build(Order order, bool includeUser= false)
        {
            var orderItemDtos = new List<OrderItemDto>(order.OrderItems.Count);

            foreach (var orderItem in order.OrderItems)
            {
                orderItemDtos.Add(OrderItemDto.Build(orderItem));
            }

            var dto = new OrderDetailDto
            {
                Id = order.Id,
                TrackingNumber = order.TrackingNumber,
                OrderStatus = order.OrderStatus,
                Address = AddressDto.Build(order.Address),
                OrderItems = orderItemDtos
            };

            return dto;
        }
    }
}
