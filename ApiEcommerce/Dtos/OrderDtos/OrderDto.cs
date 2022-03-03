using ApiEcommerce.Dtos.AddressDtos;
using ApiEcommerce.Dtos.UserDtos;
using ApiEcommerce.Entities;
using ApiEcommerce.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Dtos.OrderDtos
{
    public class OrderDto
    {
        public long Id { get; set; }
        public string TrackingNumber { get; set; }
        public ShippingStatus OrderStatus { get; set; }
        public UserBasicInfor User { get; set; }
        public AddressDto Address { get; set; }


        public static OrderDto Build(Order order, bool includeUser = false)
        {
            var dto = new OrderDto
            {
                Id = order.Id,
                TrackingNumber = order.TrackingNumber,
                OrderStatus = order.OrderStatus,
                Address = AddressDto.Build(order.Address),
            };

            if (includeUser)
                dto.User = UserBasicInfor.Build(order.User);

            return dto;
        }
    }
}
