using ApiEcommerce.Dtos.PageDtos;
using ApiEcommerce.Entities;
using ApiEcommerce.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Dtos.AddressDtos
{
    public class ListAddressesDto : PaginationDto
    {
        public IEnumerable<AddressDto> Addresses { get; set; }


        public static ListAddressesDto Build(List<Address> addresses, string basePath,
            int currentPage, int pageSize, int totalItemCount)
        {
            var addressDtos = new List<AddressDto>(addresses.Count);

            foreach (var address in addresses)
                addressDtos.Add(AddressDto.Build(address));


            return new ListAddressesDto
            {
                Pagination = new PaginationModel(addresses.Count, basePath, currentPageNumber: currentPage, requestedPageSize: pageSize,
                    totalItemCount: totalItemCount),
                Addresses = addressDtos
            };
        }
    }
}
