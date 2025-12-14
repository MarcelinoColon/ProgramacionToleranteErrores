namespace ProductRepository_Exception
{
    public class ProductRepository
    {
        private List<Product> _products = new List<Product>()
        {
            new Product(1, "Laptop"),
            new Product(2, "Smartphone"),
            new Product(3, "Tablet")
        };

        public Product GetProductById(int id)
        {
            if(id <= 0)
            {
                throw new KeyNotFoundException("El Id del producto no debe ser menor a 0");
            }

            var product = _products.FirstOrDefault(p => p.id == id);

            if (product == null)
            {
                throw new KeyNotFoundException("Producto no existente");
            }

            return product;
        }
    }

    public record Product(int id, string Name);
}
