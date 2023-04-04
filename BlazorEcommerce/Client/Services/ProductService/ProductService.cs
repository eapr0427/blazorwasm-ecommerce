using System;
using System.Net.Http;

namespace BlazorEcommerce.Client.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public event Action ProductsChanged;

        public List<Product> Products { get; set; } = new List<Product>();


        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }


        public async Task GetProducts(string? categoryUrl = null)
        {
            var result = categoryUrl == null ?
                await _httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product")
                : await _httpClient.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/category/{categoryUrl}");

            if (result != null && result.Data != null)
                Products = result.Data;

            ProductsChanged.Invoke();

        }

        public async Task<ServiceResponse<Product>> GetProduct(int productId)
        {
            var result = await _httpClient.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/{productId}");
            return result;
        }
    }
}

