using ApiEcommerce.Dtos.CategoryDtos;
using ApiEcommerce.Dtos.TagDtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Dtos.ProductDtos
{
    public class ProductDto
    {

        [JsonProperty(PropertyName = "name")]
        [Required]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "body")]
        [Required]
        public string Body { get; set; }

        [JsonProperty(propertyName: "description")]
        [Required]
        public string Description { get; set; }

        public DateTime PublishedOn { get; set; }


        [JsonProperty(PropertyName = "categories")]
        public IEnumerable<CategoryOnlyNameDto> Categories { get; set; }

        [JsonProperty(PropertyName = "tags")] public IEnumerable<TagOnlyNameDto> Tags { get; set; }

        public DateTime PublishAt { get; set; }
    }
}
