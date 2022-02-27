using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Dtos.OrderDtos
{
    public class OrderDtoRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string CardNumber { get; set; }

        public string Address { get; set; }

        public long? AddressId { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string PhoneNumber { get; set; }

        public String ZipCode { get; set; }

        public Collection<CartItemDto> CartItems { get; set; }
    }   
}
