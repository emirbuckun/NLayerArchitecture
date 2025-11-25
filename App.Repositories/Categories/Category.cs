using App.Repositories.Products;

namespace App.Repositories.Categories {
    public class Category : IAuditEntity {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public List<Product>? Products { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}