using ApiEcommerce.Dtos.Handles;
using System.Collections.Generic;

namespace ApiEcommerce.Dtos.CategoryDtos
{
    public class CategoryDto : SuccessResponse
    {
        public long Id { get; set; }
        public List<string> ImageUrls { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        public static CategoryDto Build(Entities.Category category)
        {
            var imageUrls = new List<string>();
            if (category.CategoryImages != null)
            {
                foreach (var tagImage in category.CategoryImages)
                {
                    imageUrls.Add(tagImage.FilePath);
                }
            }

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageUrls = imageUrls
            };
        }
    }
}
