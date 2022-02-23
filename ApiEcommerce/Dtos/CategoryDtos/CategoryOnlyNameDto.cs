using ApiEcommerce.Entities;
using System.Collections.Generic;

namespace ApiEcommerce.Dtos.CategoryDtos
{
    public class CategoryOnlyNameDto
    {
        public string Name { get; set; }

        public static List<string> BuildAsStringList(ICollection<ProductCategory> productCategories)
        {
            if (productCategories == null)
                return null;
            var result = new List<string>(productCategories.Count);
            foreach (var productCategory in productCategories)
            {
                result.Add(productCategory.Category?.Name);
            }

            return result;
        }
    }
}
