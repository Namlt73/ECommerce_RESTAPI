using ApiEcommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Dtos.ProductDtos
{
    public class ProductBasicInfor
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }


        public static ProductBasicInfor Build(Product product)
        {
            return new ProductBasicInfor
            {
                Id = product.Id,
                Name = product.Name,
                Slug = product.Slug,
            };
        }
    }
}
