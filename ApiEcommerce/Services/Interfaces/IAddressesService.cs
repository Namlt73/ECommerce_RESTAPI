using ApiEcommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Services.Interfaces
{
    public interface IAddressesService
    {
        Task<Tuple<int, List<Address>>> GetAddress(int page = 1, int pageSize = 6);
        Task<Tuple<int, List<Address>>> GetAddressByUser(User user, int page, int pageSize);



        Task<Address> GetAddressById(long? id);

        Task<Address> AddAddress(User user, string firstName, string lastname,
            string country, string city, string streetNumber, string zipCode);
    }
}
