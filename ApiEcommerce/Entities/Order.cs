using ApiEcommerce.Helper;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Entities
{
    public class Order
    {
        public long Id { get; set; }
        [BindNever] public ICollection<OrderItem> OrderItems { get; set; }

        [Required(ErrorMessage = "Please enter the address")]

        public long? UserId { get; set; }
        public User User { get; set; }


        public long AddressId { get; set; }
        public Address Address { get; set; }
        public string TrackingNumber { get; set; }
        public ShippingStatus OrderStatus { get; set; }
        [NotMapped] 
        public decimal Sum { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
