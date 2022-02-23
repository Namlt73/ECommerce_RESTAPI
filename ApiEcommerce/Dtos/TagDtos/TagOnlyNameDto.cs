using ApiEcommerce.Entities;
using System.Collections.Generic;

namespace ApiEcommerce.Dtos.TagDtos
{
    public class TagOnlyNameDto
    {
        public string Name { get; set; }

        public static List<string> BuildAsStringList(IEnumerable<ProductTag> productTags)
        {
            if (productTags == null)
                return null;
            var result = new List<string>(20);
            foreach (var productTag in productTags)
            {
                result.Add(productTag?.Tag?.Name);
            }

            return result;
        }
    }
}
