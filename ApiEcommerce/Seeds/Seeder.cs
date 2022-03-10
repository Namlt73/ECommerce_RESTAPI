using ApiEcommerce.Data;
using ApiEcommerce.Entities;
using ApiEcommerce.Helper;
using ApiEcommerce.Services.Interfaces;
using Bogus;
using Bogus.DataSets;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiEcommerce.Seeds
{
    public class Seeder
    {
        private static async Task SeedAuthenticatedUsersAndRole(IServiceProvider services)
        {
            var  faker = new Faker();
            using var serviceScope = services.GetRequiredService<IServiceScopeFactory>().CreateScope();

            IConfigurationService configurationService = services.GetService<IConfigurationService>();

            UserManager<User> userManager =  serviceScope.ServiceProvider.GetService<UserManager<User>>();
            RoleManager<Role> roleManager = serviceScope.ServiceProvider.GetService<RoleManager<Role>>();

            string standardUserRoleName = configurationService.GetStandardUserRoleName();

            IdentityResult result = IdentityResult.Success;
            if (!(await roleManager.RoleExistsAsync(standardUserRoleName)))
            {
                result = await roleManager.CreateAsync(new Role(standardUserRoleName));
                if (!result.Succeeded)
                {
                    throw new Exception();
                }
            }

            if (result.Succeeded)
            {
                ApplicationDbContext applicationDbContext =
                    serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var usersCount = applicationDbContext.Users.Count();
                var usersToSeed = 10;
                usersToSeed -= usersCount;



                for (int i = 0; i < usersToSeed; i++)
                {
                    var user = new User
                    {
                        FirstName = faker.Name.FirstName(),
                        LastName = faker.Name.LastName(),
                        UserName = faker.Internet.UserName(faker.Name.FirstName(), faker.Name.LastName()),
                        Email = faker.Internet.Email()
                    };
                    result = await userManager.CreateAsync(user, "DefaultPassword");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, standardUserRoleName);
                    }
                }
            }
        }

        private static async Task SeedAdminUserAndRole(IServiceProvider services)
        {
            using var serviceScope = services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            IConfigurationService configurationService = services.GetService<IConfigurationService>();
            UserManager<User> userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
            RoleManager<Role> roleManager = serviceScope.ServiceProvider.GetService<RoleManager<Role>>();

            string adminUserName = configurationService.GetAdminUserName();
            string adminFirstName = configurationService.GetAdminFirstName();
            string adminLastName = configurationService.GetAdminLastName();
            string adminEmail = configurationService.GetAdminEmail();
            string adminPassword = configurationService.GetAdminPassword();
            string adminRoleName = configurationService.GetAdminRoleName();
            {
                IdentityResult authRoleCreated = IdentityResult.Success;
                if (await roleManager.FindByNameAsync(adminRoleName) == null)
                {
                    authRoleCreated = await roleManager.CreateAsync(new Role(adminRoleName));
                }

                if (await userManager.FindByNameAsync(adminUserName) == null && authRoleCreated.Succeeded)
                {
                    var user = new User
                    {
                        FirstName = adminFirstName,
                        LastName = adminLastName,
                        UserName = adminUserName,
                        Email = adminEmail
                    };

                    IdentityResult result = await userManager.CreateAsync(user, adminPassword);

                    if (result.Succeeded)
                    {
                        result = await userManager.AddToRoleAsync(user, adminRoleName);
                        if (!result.Succeeded)
                            throw new ThreadStateException();
                    }
                    else
                    {
                        throw new Exception("Failed to Create User");
                    }
                }
            }
        }


        private static async Task SeedTags(IServiceProvider services)
        {
            using var serviceScope = services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            ApplicationDbContext context = services.GetRequiredService<ApplicationDbContext>();
            var tagCount = await context.Tags.CountAsync();
            var tagsToSeed = 5;


            tagsToSeed -= tagCount;
            if (tagsToSeed <= 0)
                return;
            var faker = new Faker<Tag>()
                .RuleFor(t => t.Name, f => f.Lorem.Word())
                .RuleFor(t => t.Description, f => f.Lorem.Sentences(1))
                .FinishWith((fake, tagInstance) =>
                {
                    var numberOfImages = fake.Random.Int(min: 1, max: 3);
                    ICollection<TagImg> fileUploads = new List<TagImg>(numberOfImages);
                    for (var i = 0; i < numberOfImages; i++)
                    {
                        var fileName = fake.System.FileName("png");
                        fileUploads.Add(new TagImg
                        {
                            OriginalFileName = fake.System.FileName("png"),
                            FileName = fileName,
                            FilePath = fake.Image.LoremPixelUrl(LoremPixelCategory
                                .Business), //"/images/tags/" + fileName,
                                FileSize = fake.Random.Long(min: 1500, max: 20000),
                            IsFeaturedImage = i == 0
                        });
                    }

                    tagInstance.TagImgs = fileUploads;
                });

            List<Tag> tags = faker.Generate(tagsToSeed);
            context.Tags.AddRange(tags);
            await context.SaveChangesAsync();
        }

        private static async Task SeedCategories(IServiceProvider services)
        {
            using var serviceScope = services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            ApplicationDbContext context = services.GetRequiredService<ApplicationDbContext>();
            var categoryCount = await context.Categories.CountAsync();
            var categoriesToSeed = 5;
            categoriesToSeed -= categoryCount;
            if (categoriesToSeed <= 0)
                return;
            var faker = new Faker<Category>()
                .RuleFor(t => t.Name, f => f.Lorem.Word())
                .RuleFor(t => t.Description, f => f.Lorem.Sentences(2));


            List<Category> categories = faker.Generate(categoriesToSeed);

            foreach (var category in categories)
            {
                var fake = new Faker();

                var numberOfImages = fake.Random.Int(min: 1, max: 3);
                ICollection<CategoryImg> fileUploads = new List<CategoryImg>(numberOfImages);
                for (var i = 0; i < numberOfImages; i++)
                {
                    var fileName = fake.System.FileName("png");
                    fileUploads.Add(new CategoryImg
                    {
                        OriginalFileName = fake.System.FileName("png"),
                        FileName = fileName,
                        FilePath = fake.Image.LoremPixelUrl(LoremPixelCategory
                            .Business),
                        FileSize = fake.Random.Long(min: 1500, max: 20000),
                    });
                }

                category.CategoryImages = fileUploads;
            }

            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();
        }

        private static async Task SeedRatings(IServiceProvider services)
        {
            using var serviceScope = services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            IConfigurationService settingsService =
                serviceScope.ServiceProvider.GetService<IConfigurationService>();
            ApplicationDbContext context = services.GetRequiredService<ApplicationDbContext>();

            var faker = new Faker();
            var ratingCount = await context.Ratings.CountAsync();
            var ratingSeed = 10;
            ratingSeed -= ratingCount;
            for (int i = ratingCount; i < ratingSeed; i++)
            {
                User user = await context.Users.OrderBy(a => Guid.NewGuid()).FirstAsync();
                var product = await context.Products.Include(p => p.Ratings)
                    .Where(p => p.Ratings.All(comment =>
                        comment.User != user)) // Find any whouse commenters do not include our user;
                    .FirstAsync();
                context.Ratings.Add(new Rating
                {
                    Value = faker.Random.Int(min: 1, max: 5),
                    User = user,
                    Product = product,
                });

                await context.SaveChangesAsync();
            }
        }


        private static async Task SeedAddresses(IServiceProvider services)
        {
            using var serviceScope = services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            IConfigurationService settingsService =
                serviceScope.ServiceProvider.GetService<IConfigurationService>();
            ApplicationDbContext context = services.GetRequiredService<ApplicationDbContext>();


            var faker = new Faker<Entities.Address>()
                .RuleFor(c => c.StreetAddress, f => f.Address.StreetAddress())
                .RuleFor(a => a.Country, f => f.Address.County())
                .RuleFor(a => a.City, f => f.Address.City())
                .RuleFor(a => a.ZipCode, f => f.Address.ZipCode())
                .FinishWith(async (f, address) =>
                {
                    var user = await context.Users.OrderBy(a => Guid.NewGuid()).FirstAsync();

                    address.FirstName = user.FirstName;
                    address.LastName = user.LastName;
                    address.UserId = user.Id;
                });

            var addressesCount = await context.Addresses.CountAsync();
            var commentsToSeed = 35;
            commentsToSeed -= addressesCount;

            if (commentsToSeed > 0)
            {
                List<Entities.Address> addresses = faker.Generate(commentsToSeed);
                context.Addresses.AddRange(addresses);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedComments(IServiceProvider services)
        {
            using var serviceScope = services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            IConfigurationService settingsService =
                serviceScope.ServiceProvider.GetService<IConfigurationService>();
            ApplicationDbContext context = services.GetRequiredService<ApplicationDbContext>();


            var faker = new Faker<Comment>()
                .RuleFor(c => c.Content, f => f.Lorem.Sentences(f.Random.Number(1, 3)))
                .FinishWith(async (f, c) =>
                {
                    if (f.Random.Bool(0.75f))
                        c.Rating = f.Random.Int(min: 1, max: 5);


                    var user = await context.Users.OrderBy(a => Guid.NewGuid()).FirstAsync();


                    Product product = await context.Products.Include(p => p.Comments)
                        .OrderBy(a => Guid.NewGuid()).FirstAsync();


                    c.Product = product;
                    c.User = user;
                });

            var commentsCount = await context.Comments.CountAsync();
            var commentsToSeed = 35;
            commentsToSeed -= commentsCount;

            if (commentsToSeed > 0)
            {
                List<Comment> comments = faker.Generate(commentsToSeed);
                context.Comments.AddRange(comments);
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedProducts(IServiceProvider services)
        {
            using var serviceScope = services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            IConfigurationService settingsService =
                serviceScope.ServiceProvider.GetService<IConfigurationService>();
            ApplicationDbContext context = services.GetRequiredService<ApplicationDbContext>();
            var productsCount = await context.Products.CountAsync();
            var productsToSeed = 35;
            productsToSeed -= productsCount;
            if (productsToSeed <= 0)
                return;

            UserManager<User> userManager =
                serviceScope.ServiceProvider.GetService<UserManager<User>>();
            RoleManager<Role> roleManager =
                serviceScope.ServiceProvider.GetService<RoleManager<Role>>();

            var faker = new Faker<Product>()
                .RuleFor(a => a.PublishAt, f => f.Date
                    .Between(DateTime.Now.AddYears(-3), DateTime.Now.AddYears(1)))
                .RuleFor(a => a.Name, f => f.Commerce.ProductName())
                .RuleFor(a => a.Description, f => f.Lorem.Sentences(2))
                .RuleFor(p => p.Price,
                    f => f.Random.Int(min: 50,
                        max: 1000))
                .RuleFor(p => p.Stock, f => f.Random.Int(min: 0, max: 2500))
                .FinishWith(async (f, aproductInstance) =>
                {
                    ICollection<ProductTag> productTags = new List<ProductTag>();
                    productTags.Add(new ProductTag
                    {
                        Product = aproductInstance,
                        ProductId = aproductInstance.Id,
                        Tag = await context.Tags.OrderBy(t => Guid.NewGuid()).FirstAsync()
                    }
                    );
                    aproductInstance.ProductTags = productTags;

                    ICollection<ProductCategory> productCategories = new List<ProductCategory>();
                    productCategories.Add(new ProductCategory
                    {
                        Product = aproductInstance,
                        ProductId = aproductInstance.Id,
                        Category = await context.Categories.OrderBy(t => Guid.NewGuid()).FirstAsync()
                    }
                    );
                    aproductInstance.ProductCategories = productCategories;

                    aproductInstance.Slug = aproductInstance.Name.Slugify();


                    var numberOfImages = f.Random.Int(min: 1, max: 3);
                    ICollection<ProductImg> fileUploads = new List<ProductImg>(numberOfImages);
                    for (var i = 0; i < numberOfImages; i++)
                    {
                        var fileName = f.System.FileName("png");
                        fileUploads.Add(new ProductImg
                        {
                            OriginalFileName = f.System.FileName("png"),
                            FileName = fileName,
                            FilePath = f.Image.LoremPixelUrl(LoremPixelCategory
                                .Business), // "/images/products/" + fileName,
                                FileSize = f.Random.Long(min: 1500, max: 20000),
                            IsFeaturedImage = i == 0
                        });
                    }

                    aproductInstance.ProductImages = fileUploads;
                });


            List<Product> products = faker.Generate(productsToSeed);
            products.ForEach(a =>
            {
                context.Products.Add(a);
            });
            EntityEntry<Product> entry = context.Products.Add(products[0]);
            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        }


        public static async Task SeedOrders(IServiceProvider services)
        {
            using var serviceScope = services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            IConfigurationService settingsService =
                serviceScope.ServiceProvider.GetService<IConfigurationService>();
            ApplicationDbContext context = services.GetRequiredService<ApplicationDbContext>();

            Array values = Enum.GetValues(typeof(ShippingStatus));
            var random = new Random();


            var faker = new Faker<Order>()
                .RuleFor(o => o.TrackingNumber, f => f.Random.AlphaNumeric(16))
                .RuleFor(o => o.OrderStatus, f => (ShippingStatus)f.Random.Int(min: 1, max: values.Length))
                .FinishWith(async (fk, order) =>
                {
                        // User
                        var orderingUser =
                        fk.Random.Bool(
                            0.75f) // 75% change we create an order with an authenticated user, 25% change of guest user making the order
                            ? await context.Users.Include(u => u.Addresses).OrderBy(a => Guid.NewGuid())
                                .FirstAsync()
                            : null;
                    order.User = orderingUser;


                        // Address
                        if (orderingUser?.Addresses?.Count > 0)
                    {
                        if (orderingUser.Addresses?.Count > 0 && orderingUser.Addresses.GetType() == typeof(IList))
                            order.Address =
                                ((IList<Entities.Address>)orderingUser.Addresses)[
                                    fk.Random.Int(min: 0, max: orderingUser.Addresses.Count)];
                        else
                            order.Address = await context.Addresses.Where(a => a.User == orderingUser)
                                .OrderBy(e => Guid.NewGuid())
                                .FirstAsync();
                    }
                    else
                    {
                        order.Address = new Entities.Address
                        {
                                // we may have a user but with 0 Addresses
                                UserId = orderingUser?.Id,
                            FirstName = fk.Name.FirstName(),
                            LastName = fk.Name.LastName(),
                            StreetAddress = fk.Address.StreetAddress(),
                            City = fk.Address.City(),
                            Country = fk.Address.Country(),
                            ZipCode = fk.Address.ZipCode()
                        };
                    }


                        // Seed OrderItems
                        var product = context.Products.OrderBy(p => Guid.NewGuid()).First();

                        // OrderItems
                        ICollection<OrderItem> orderItems = new List<OrderItem>();
                    for (int i = 0; i < fk.Random.Int(min: 1, max: 20); i++)
                    {
                        orderItems.Add(new OrderItem
                        {
                            User = orderingUser,
                            Order = order,
                                // OrderId = order.Id,
                                Slug = product.Slug,
                            Name = product.Name,
                            Product = await context.Products.OrderBy(p => Guid.NewGuid()).FirstAsync(),
                            Price = Math.Max(10, fk.Random.Int(min: -20, max: 20) + product.Price),
                            Quantity = fk.Random.Int(min: 1, max: 10)
                        });
                    }

                    order.OrderItems = orderItems;
                });

            var ordersCount = await context.Orders.CountAsync();
            var ordersToSeed = 35;
            ordersToSeed -= ordersCount;

            if (ordersToSeed > 0)
            {
                List<Order> orders = faker.Generate(ordersToSeed);
                context.Orders.AddRange(orders);
                await context.SaveChangesAsync();
            }
        }

        public static async Task Seed(IServiceProvider services)
        {
            await SeedAdminUserAndRole(services);
            await SeedAuthenticatedUsersAndRole(services);
            await SeedTags(services);
            await SeedCategories(services);
            await SeedProducts(services);
            await SeedComments(services);
            await SeedAddresses(services);
            await SeedOrders(services);
        }
    }
}
