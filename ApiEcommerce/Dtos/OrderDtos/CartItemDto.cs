using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Dtos.OrderDtos
{
    public class CartItemDto
    {
        public long Id { get; set; }
        public int Quantity { get; set; }
    }
}
