using ApiEcommerce.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEcommerce.Data
{
    public class ApplicationDbContext : IdentityDbContext<
            User,
            Role,
            long,
            IdentityUserClaim<long>,
            UserRole,
            IdentityUserLogin<long>,
            IdentityRoleClaim<long>,
            IdentityUserToken<long>>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        public DbSet<TagImg> TagImages { get; set; }
        public DbSet<CategoryImg> CategoryImages { get; set; }
        public DbSet<FileUpload> FileUploads { get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults


            modelBuilder.Entity<User>()
                .HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Logins)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Roles)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Slug).IsRequired();
                entity.HasIndex(p => p.Slug).IsUnique(true);
            });



            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Description);

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.HasKey(e => new { e.CategoryId, e.ProductId });

                entity.Property(e => e.CategoryId);

                entity.Property(e => e.ProductId);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductCategories)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.ProductCategories)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.UserId);

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasOne(o => o.User).WithMany(u => u.Orders)
                    .HasForeignKey(o => o.UserId).IsRequired(false)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(o => o.OrderItems)
                    .WithOne(oi => oi.Order)
                    .HasForeignKey(o => o.OrderId).IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                

                entity.HasOne(o => o.Address).WithMany((string)null)
                    .HasForeignKey(o => o.AddressId).IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasOne(oi => oi.User)
                    .WithMany((string)null).HasForeignKey(oi => oi.UserId)
                    .IsRequired(false).OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(oi => oi.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(oi => oi.OrderId).IsRequired(true)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.Description);

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<ProductTag>(entity =>
            {
                entity.HasKey(e => new { e.TagId, e.ProductId });

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductTags)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.ProductTags)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });


            modelBuilder.Entity<TagImg>(entity =>
            {
                entity.HasBaseType<FileUpload>();
                entity.HasOne(ti => ti.Tag)
                    .WithMany(t => t.TagImgs)
                    .HasForeignKey(ti => ti.TagId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CategoryImg>(entity =>
            {
                entity.HasBaseType<FileUpload>();
                entity.HasOne(ti => ti.Category)
                    .WithMany(t => t.CategoryImages)
                    .HasForeignKey(ti => ti.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ProductImg>(entity =>
            {
                entity.HasBaseType<FileUpload>();
                entity.HasOne(pi => pi.Product)
                    .WithMany(t => t.ProductImages)
                    .HasForeignKey(ti => ti.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
