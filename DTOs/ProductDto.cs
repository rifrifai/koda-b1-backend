using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
	public record class ProductDto(string ProductID, string Name, decimal Price, int Stock, DateTime CreatedAt, DateTime UpdatedAt);
}