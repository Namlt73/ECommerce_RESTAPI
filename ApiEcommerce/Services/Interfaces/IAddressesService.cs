using ApiEcommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Services.Interfaces
{
    public interface IAddressesService
    {
        Task<Tuple<int, List<Address>>> GetAddress();
        Task<Tuple<int, List<Address>>> GetAddressByUser(User user);



        Task<Address> GetAddressById(long? id);

        Task<Address> AddAddress(User user, string firstName, string lastname,
            string country, string city, string streetNumber, string zipCode);
    }
}
