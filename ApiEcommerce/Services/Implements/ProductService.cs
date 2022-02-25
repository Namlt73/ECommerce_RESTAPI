using ApiEcommerce.Data;
using ApiEcommerce.Dtos.CategoryDtos;
using ApiEcommerce.Dtos.ProductDtos;
using ApiEcommerce.Dtos.TagDtos;
using ApiEcommerce.Entities;
using ApiEcommerce.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Services.Implements
{
    public class ProductService : IProductsService
    {
        private readonly ApplicationDbContext _context;

        private readonly IStorageService _storageService;


        public ProductService(ApplicationDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task Create(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product> Create(string name, string description, IEnumerable<TagOnlyNameDto> tagOnlyNameDtos, IEnumerable<CategoryOnlyNameDto> categoryOnlyNameDtos, List<IFormFile> images)
        {
            var tags = new List<ProductTag>();
            var categories = new List<ProductCategory>();


            if (tagOnlyNameDtos == null)
                tagOnlyNameDtos = new List<TagOnlyNameDto>();

            if (categoryOnlyNameDtos == null)
                categoryOnlyNameDtos = new List<CategoryOnlyNameDto>();


            var product = new Product
            {
                Name = name,
                Description = description,
                //Slug = name.Slugify()
            };

            foreach (var tagName in tagOnlyNameDtos)
            {
                Tag tag = await _context.Tags.Where(t => t.Name == tagName.Name).FirstOrDefaultAsync();
                if (tag == null)
                {
                    // IS there a find or create?
                    tag = new Tag
                    {
                        Name = tagName.Name,
                    };
                    tags.Add(new ProductTag
                    {
                        Tag = tag,
                        Product = product
                    });
                }
            }

            foreach (var categoryName in categoryOnlyNameDtos)
            {
                Category category =
                    await _context.Categories.Where(t => t.Name == categoryName.Name).FirstOrDefaultAsync();
                if (category == null)
                {
                    category = new Category
                    {
                        Name = categoryName.Name,
                    };
                    categories.Add(new ProductCategory
                    {
                        Category = category,
                        Product = product
                    });
                }
            }

            ICollection<ProductImg> fileUploads = new List<ProductImg>(images.Count);
            foreach (IFormFile file in images)
            {
                FileUpload fileUpload = await _storageService.UploadFormFile(file, "products");

                fileUploads.Add(new ProductImg
                {
                    OriginalFileName = fileUpload.OriginalFileName,
                    FileName = fileUpload.FileName,
                    FilePath = fileUpload.FilePath,
                    FileSize = file.Length,
                });
            }


            product.ProductImages = fileUploads;
            product.ProductTags = tags;
            product.ProductCategories = categories;

            EntityEntry<Product> productEntity = await _context.Products.AddAsync(product);

            await _context.SaveChangesAsync();
            return productEntity.Entity;
        }

        public async Task<Product> Create(string name, string description, int price, int stock, 
            List<Tag> tags, List<Category> categories,List<IFormFile> images, 
            bool processTags = true, bool processCategories = true)
        {
            var product = new Product
            {
                Name = name,
                Description = description,
                Price = price,
                Stock = stock,
                Slug = name
            };

            ICollection<ProductTag> productTags = new List<ProductTag>();
            ICollection<ProductCategory> productCategories = new List<ProductCategory>();

            for (int i = 0; i < tags.Count; i++)
            {
                Tag tagElem;

                if (processTags)
                {
                    Tag tagAt = tags[i];

                    tagElem = await _context.Tags.Where(t => t.Name == tagAt.Name).FirstOrDefaultAsync();
                    if (tagElem == null)
                    {
                        tagElem = new Tag
                        {
                            Name = tagAt.Name,
                            Description = tagAt.Description
                        };
                    }
                }
                else
                {
                    tagElem = tags[i];
                }

                productTags.Add(new ProductTag
                {
                    Tag = tagElem,
                    Product = product
                });
            }

            // Find the categories, if they do not exist then create them
            for (int i = 0; i < categories.Count; i++)
            {
                Category category;

                // If categories have been already retrieved and checked if they already existed in the database
                if (processCategories)
                {
                    Category categoryAt = categories[i];
                    category = await _context.Categories.Where(t => t.Name == categoryAt.Name).FirstOrDefaultAsync();

                    //if the category not exist, then create it.
                    if (category == null)
                    {
                        category = new Category
                        {
                            Name = categoryAt.Name,
                            Description = categoryAt.Description
                        };
                    }
                }

                //if it exists then just retrieve it
                else
                {
                    category = categories[i];
                }

                productCategories.Add(new ProductCategory
                {
                    Category = category,
                    Product = product
                });
            }
            // Upload product-related images
            ICollection<ProductImg> fileUploads = new List<ProductImg>(images.Count);
            for (int i = 0; i < images.Count; i++)
            {
                var file = images[i];
                FileUpload fileUpload = await _storageService.UploadFormFile(file, "products");

                fileUploads.Add(new ProductImg
                {
                    OriginalFileName = fileUpload.OriginalFileName,
                    FileName = fileUpload.FileName,
                    FilePath = fileUpload.FilePath,
                    FileSize = file.Length,
                });
            }


            product.ProductImages = fileUploads;
            product.ProductTags = productTags;
            product.ProductCategories = productCategories;

            EntityEntry<Product> productEntityEntry = await _context.Products.AddAsync(product);

            await _context.SaveChangesAsync();
            return productEntityEntry.Entity;
        }
    

        public async Task<int> Delete(long id)
        {
            Product product = await GetById(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                return await _context.SaveChangesAsync();
            }

            return 0;
        }

        public async Task<int> Delete(Product product)
        {
            _context.Products.Remove(product);
            return await _context.SaveChangesAsync();
        }
        

        public async Task<List<Product>> GetAll()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Tuple<int, List<Product>>> GetByCategory(string category)
        {
            IQueryable<Product> queryable = _context.Products.Include(a => a.ProductCategories)
                .Include(p => p.ProductImages)
                .Where(a => a.ProductCategories.Any(p =>
                    p.Category.Name.Contains(category, StringComparison.OrdinalIgnoreCase)));
            var count = queryable.Count();

            List<Product> products = await queryable.ToListAsync();

            return Tuple.Create(count, products);
        }

        public async Task<Product> GetById(long id, bool onlyIfPublished = false)
        {
            var product = await _context.Products
                .Include(p => p.Comments)
                .ThenInclude(c => c.User).FirstOrDefaultAsync(p => p.Id == id);

            if (product != null)
            {
                return await Task.FromResult(product);
            }
            else
            {
                return await Task.FromResult<Product>(null);
            }
        }


        public async Task<Tuple<int, List<Product>>> GetBySearchTerm(string search)
        {
            var queryable = _context.Products.Where(a =>
                    a.Description.Contains(search) ||
                    a.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
            
            var count = queryable.Count();

            var products = await queryable.OrderByDescending(a => a.PublishAt)
                .ThenByDescending(a => a.UpdatedAt)
                .Include(a => a.Comments)
                .Include(a => a.ProductCategories)
                .ThenInclude(ac => ac.Category).ToListAsync();


            return Tuple.Create(count, products);
        }

        public async Task<Product> GetProductBySlug(string slug)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Slug.Equals(slug, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task SaveProduct(Product product)
        {
            if (product.Id == 0)
            {
                _context.Products.Add(product);
            }
            else
            {
                Product dbEntry = _context.Products.FirstOrDefault(p => p.Id == product.Id);
                if (dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Description = product.Description;
                    dbEntry.Price = product.Price;
                    dbEntry.ProductCategories = product.ProductCategories;
                }
            }

            await _context.SaveChangesAsync();
        }


        public async Task<Product> Update(string slug, ProductDto productDto)
        {
            var product = await _context.Products.Include(a => a.ProductCategories)
                .Include(a => a.ProductTags)
                .Where(x => x.Slug == slug).FirstOrDefaultAsync();

            if (product == null)
            {
                return null;
            }

            product.Name = productDto.Name.Trim();
            product.Slug = productDto.Name.Trim();
            product.Description = productDto.Description;


            if (productDto.Categories != null && productDto.Categories.Any())
            {
                foreach (var category in productDto.Categories)
                {
                    Category cat= _context.Categories
                        .SingleOrDefault(p => p.Name.Equals(category.Name));

                    if (cat == null)
                    {
                        cat = new Category
                        {
                            Name = category.Name,
                        };

                        _context.Categories.Add(cat);
                        await _context.SaveChangesAsync();
                    }

                    // If the product is not already assigned to the category, it will be assigned to the category.
                    if (!product.ProductCategories.Any(p => p.Product == product && p.Category == cat))
                    {
                        product.ProductCategories.Add(new ProductCategory
                        {
                            Product = product,
                            Category = cat
                        });
                    }
                }
            }


            if (_context.ChangeTracker.Entries().First(x => x.Entity == product).State == EntityState.Modified)
            {
                product.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return product;
        }
    }
}
