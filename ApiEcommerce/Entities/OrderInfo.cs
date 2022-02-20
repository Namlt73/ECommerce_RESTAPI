using ApiEcommerce.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Entities
{
    public class OrderInfo
    {
        public long Id { get; set; }
        public Address Address { get; set; }
        public string TrackingNumber { get; set; }
        public ShippingStatus ShippingStatus { get; set; }
    }
}
