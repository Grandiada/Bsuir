using Bsuir.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Bsuir.Core.Models;
using Bsuir.Core.Services;
using Newtonsoft.Json;

namespace Bsuir.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductService _productService;
        private readonly ClientService _clientService;

        public HomeController(
            ClientService clientService,
            ProductService productService)
        {
            _productService = productService;
            _clientService = clientService;
        }
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var clients = await _clientService.GetAsync(cancellationToken);
            var products = await _productService.GetAsync(cancellationToken);
            var model = new IndexViewModel();

            foreach (var client in clients)
            {
                model.Clients.Add(new ClientViewModel
                {
                    Id = client.Id,
                    Name = client.Name
                });
            }

            foreach (var product in products)
            {
                model.Products.Add(new ProductViewModel
                {
                    Name = product.Tittle,
                    Price = product.Price
                });
            }

            return View(model);
        }

        public async Task<IActionResult> AddProduct(ProductViewModel product, CancellationToken cancellationToken)
        {
            await _productService.AddAsync(new Product { Tittle = product.Name, Price = product.Price }, cancellationToken);
            return Ok();
        }

        public async Task<IActionResult> AddClient(ClientViewModel client, CancellationToken cancellationToken)
        {
            var clientAdded = await _clientService.AddAsync(new Client { Name = client.Name }, cancellationToken);
            return Json(clientAdded.Id);
        }

        public sealed class BuyRequest
        {
            [JsonProperty("id")]
            public int Id { get; set; }
            [JsonProperty("total")]
            public decimal Total { get; set; }
        }

        public async Task<IActionResult> Buy(BuyRequest request, CancellationToken cancellationToken)
        {
            var client = await _clientService.GetByIdAsync(request.Id, cancellationToken);
            client.Total += request.Total;
            await _clientService.UpdateAsync(client, new[] { nameof(Client.Total) }, cancellationToken);

            return Ok();
        }

        public async Task<IActionResult> GetDiscount(int id, CancellationToken cancellationToken)
        {
            var client = await _clientService.GetByIdAsync(id, cancellationToken);
            decimal discount = 0;

            if (client.Total >= 5000)
                discount = 10;
            else if (client.Total >= 3000 && client.Total < 5000)
                discount = 7;
            else if (client.Total >= 1000 && client.Total < 3000)
                discount = 5;
            else
                discount = 0;

            return Json(discount);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
