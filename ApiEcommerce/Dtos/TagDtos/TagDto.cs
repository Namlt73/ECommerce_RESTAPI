using ApiEcommerce.Dtos.Handles;
using System.Collections.Generic;

namespace ApiEcommerce.Dtos.TagDtos
{
    public class TagDto : SuccessResponse
    {
        public long Id { get; set; }
        public List<string> ImageUrls { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        public static TagDto Build(Entities.Tag tag)
        {
            var imageUrls = new List<string>();
            if (tag.TagImgs != null)
            {
                foreach (var tagImage in tag.TagImgs)
                {
                    imageUrls.Add(tagImage.FilePath);
                }
            }

            return new TagDto
            {
                Id = tag.Id,
                Name = tag.Name,
                Description = tag.Description,
                ImageUrls = imageUrls
            };
        }
    }
}
