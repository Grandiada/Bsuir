using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bsuir.Web.Models
{
    public class ClientViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class ProductViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }
    }

    public class IndexViewModel
    {
        public List<ProductViewModel> Products { get; set; }
        public List<ClientViewModel> Clients { get; set; }

        public IndexViewModel()
        {
            Products = new List<ProductViewModel>();
            Clients = new List<ClientViewModel>();
        }
    }
}
