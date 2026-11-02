using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
	public sealed class Product(string productID, string name, decimal price, int stock)
	{
		public string ProductID { get; set; } = productID;
		public string Name { get; set; } = name;
		public decimal Price { get; set; } = price;
		public int Stock { get; set; } = stock;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(7);
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(7);

		public string GetInfo()
		{
			return $"ID: {ProductID}, Name: {Name}, Price: {Price}, Stock: {Stock}";
		}
	}
}