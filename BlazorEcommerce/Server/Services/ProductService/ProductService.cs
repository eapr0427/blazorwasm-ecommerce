﻿using System;
using BlazorEcommerce.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Server.Services.ProductService
{
	public class ProductService : IProductService
	{
        private readonly DataContext _context;

        public ProductService(DataContext context)
		{
            _context = context;
		}

        public async Task<ServiceResponse<List<Product>>> GetProductsAsync()
        {
            var products = await _context.Products
                .Include(p =>  p.Variants).ToListAsync();
            var response = new ServiceResponse<List<Product>>()
            {
                Data = products
            };

            return response;
        }

        public async Task<ServiceResponse<Product>> GetProductAsync(int productId)
        {
            try
            {

                var response = new ServiceResponse<Product>();
                var product = await _context.Products
                    .Include(p => p.Variants)
                    .ThenInclude(v => v.ProductType)
                    .FirstOrDefaultAsync(p => p.Id == productId);

                if (product == null)
                {
                    response.Success = false;
                    response.Message = "Sorry, but this product doesn't exist.";
                }
                else
                {
                    response.Data = product;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsByCategory(string categoryUrl)
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products
                .Where(p => p.Category.Url.ToLower().Equals(categoryUrl.ToLower()))
                .Include(p => p.Variants)
                .ToListAsync()
            };
            return response;
        }
    }
}

