using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence.Products {
    public class CategoryConfiguration : IEntityTypeConfiguration<Category> {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Category> builder) {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        }
    }
}