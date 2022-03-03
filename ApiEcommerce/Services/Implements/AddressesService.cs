using ApiEcommerce.Data;
using ApiEcommerce.Entities;
using ApiEcommerce.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Services.Implements
{
    public class AddressesService : IAddressesService
    {
        private readonly ApplicationDbContext _context;

        public AddressesService(ApplicationDbContext context)
        {
            _context = context;
        }
        private static async Task<Tuple<int, List<Address>>> GetAddressFromQueryable(IQueryable<Address> queryable, 
            int page, int pageSize)
        {
            var count = await queryable.CountAsync();
            var addresses = await queryable
                .Skip((page - 1) * pageSize).Take(pageSize)
                .Include(a => a.User)
                .ToListAsync();

            return await Task.FromResult(Tuple.Create(count, addresses));
        }
        public async Task<Address> AddAddress(User user, string firstName, string lastname, string country, string city, string streetNumber, string zipCode)
        {
            var address = new Address
            {
                FirstName = firstName,
                LastName = lastname,
                User = user,
                Country = country,
                City = city,
                StreetAddress = streetNumber,
                ZipCode = zipCode
            };

            _context.Addresses.Add(address);

            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<Tuple<int, List<Address>>> GetAddress(int page = 1, int pageSize = 6)
        {
            var queryable = _context.Addresses;
            return await GetAddressFromQueryable(queryable, page, pageSize);
        }

        public async Task<Address> GetAddressById(long? id)
        {
            return await _context.Addresses.FindAsync(id);
        }

        public async Task<Tuple<int, List<Address>>> GetAddressByUser(User user, int page, int pageSize)
        {
            var count = _context.Addresses.Count(a => a.User.Id == user.Id);
            var queryable = _context.Addresses.Where(a => a.User == user)
                .Include(a => a.User);
            return await GetAddressFromQueryable(queryable, page, pageSize);
        }
    }
}
