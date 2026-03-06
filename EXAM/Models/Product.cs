namespace ShopServer.Models
{
    public class Product
    {
        public string Найменування { get; set; } = "";
        public decimal Ціна { get; set; }
        public int Кількість { get; set; }
        public string NameNorm => Найменування.Trim().ToLowerInvariant();
    }
}