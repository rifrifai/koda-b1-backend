using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Data;
public sealed class MemoryProductCatalog
{
	private readonly List<Product> _products = [];
	public IReadOnlyList<Product> GetProducts()
	{
		return _products.ToArray();
	}
	public Product? GetProduct(string productID)
	{
		return _products.FirstOrDefault(p => string.Equals(p.ProductID, productID.Trim(), StringComparison.OrdinalIgnoreCase));
	} 

	public void CreateProduct(Product product)
	{
		if (_products.Any(p => p.ProductID == product.ProductID))
		{
			throw new ArgumentException("Product code already exists.");
		}
		_products.Add(product);
	}

	public void RemoveProduct (string productID)
	{
		var product = GetProduct(productID);
		if (product is not null)
			_products.Remove(product);
	}

	public void UpdateProduct(string productID, Product updatedProduct)
	{
		var existingProduct = GetProduct(productID);
		if (existingProduct is not null)
		{
			var index = _products.IndexOf(existingProduct);

			_products[index] = updatedProduct;
		}
	}

	// public Product AddProduct(CreateProductDto dto)
	// {
	// 	var newProduct = new Product(
	// 		dto.ProductID.Trim(),
	// 		dto.Name,
	// 		dto.Price,
	// 		dto.Stock
	// 	);

	// 	_products.Add(newProduct);

	// 	return newProduct;
	// }
}