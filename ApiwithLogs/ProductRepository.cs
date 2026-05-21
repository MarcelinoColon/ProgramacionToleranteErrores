namespace ApiwithLogs
{
    public static class ProductRepository
    {
        public static readonly List<Product> products = new()
        {
            new Product(1, "Keyboard", 29.99m),
            new Product(2, "Mouse", 19.99m),
            new Product(3, "Monitor", 199.99m),
            new Product(4, "CPU", 499.99m),
            new Product(5, "Orejeras", 49.99m)
        };
    }
}
