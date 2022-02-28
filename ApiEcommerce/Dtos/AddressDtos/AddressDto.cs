using ApiEcommerce.Dtos.Handles;
using ApiEcommerce.Dtos.UserDtos;
using ApiEcommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Dtos.AddressDtos
{
    public class AddressDto : SuccessResponse
    {      
        public UserBasicInfor User { get; set; }

        public long Id { get; set; }
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }

        public static AddressDto Build(Address address, bool includeUser = false)
        {
            var addressDto = new AddressDto
            {
                Id = address.Id,
                City = address.City,
                Country = address.Country,
                ZipCode = address.ZipCode,
                FirstName = address.FirstName,
                LastName = address.LastName,
                Address = address.StreetAddress
            };

            if (includeUser)
                addressDto.User = UserBasicInfor.Build(address.User);
            return addressDto;
        }
    }
}
