using ApiEcommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Dtos.OrderDtos
{
    public class OrderItemDto
    {      
        public string Slug { get; set; }

        public int Price { get; set; }

        public string Name { get; set; }

        public static OrderItemDto Build(OrderItem orderItem)
        {
            return new OrderItemDto
            {
                Name = orderItem.Name,
                Price = orderItem.Price,
                Slug = orderItem.Slug
            };
        }
    }
}
