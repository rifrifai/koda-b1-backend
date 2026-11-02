using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.Data;
public sealed class MemoryProductCatalog
{
	private readonly List<Product> _products = [];
	public IReadOnlyList<Product> GetProducts()
	{
		return _products;
	}
	public Product? GetProduct(string productID)
	{
		return _products.FirstOrDefault(p => string.Equals(p.ProductID, productID.Trim(), StringComparison.OrdinalIgnoreCase));
	}
}