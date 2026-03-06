using ShopServer.Models;
using ShopServer.Services;

namespace ShopServer.Repositories
{
    public class ProductRepository
    {
        private readonly ILogger<ProductRepository> _logger;
        private List<Product> _products = new();

        public ProductRepository(ILogger<ProductRepository> logger)
        {
            _logger = logger;
        }

        public void LoadFromExcel(string path)
        {
            _logger.LogInformation("Завантаження Excel: {path}", path);
            var list = ExcelLoader.LoadFromXlsx(path);
            _products = list;
            _logger.LogInformation("Завантажено {count} записів", _products.Count);
        }

        public Product? GetExact(string name)
        {
            string norm = name.Trim().ToLowerInvariant();
            return _products.FirstOrDefault(p => p.NameNorm == norm);
        }

        public List<Product> FindPartial(string fragment)
        {
            string norm = fragment.Trim().ToLowerInvariant();
            return _products.Where(p => p.NameNorm.Contains(norm)).ToList();
        }

        public int Count => _products.Count;
    }
}