namespace App.Services.Products {
    public record UpdateProductRequest(string Id, string Name, decimal Price, int Stock);
}