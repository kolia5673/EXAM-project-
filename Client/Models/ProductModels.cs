using System.Collections.Generic;

namespace Client.Models
{
    public class Product
    {
        public string Name { get; set; } = "";
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double Sum { get; set; }
    }

    public class SearchResponse
    {
        public List<Product> products { get; set; } = new List<Product>();
        public double total { get; set; }
    }
}