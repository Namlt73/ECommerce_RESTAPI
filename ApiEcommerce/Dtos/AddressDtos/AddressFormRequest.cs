using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Dtos.AddressDtos
{
    public class AddressFormRequest
    {
        public string Lastname { get; set; }
        public string Country { get; set; }
        public string FirstName { get; set; }
        public string City { get; set; }
        public string StreetAddress { get; set; }
        public string ZipCode { get; set; }
    }
}
