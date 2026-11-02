using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
	public sealed record class CreateProductDto(
		[Required]
		[StringLength(5, ErrorMessage = "ProductID can not be more than 5 char!")]
		string ProductID,

		[Required]
		[StringLength(10, ErrorMessage = "Name can not be more than 10 character!")]
		string Name,

		[Required]
		[Range(10_000, 1000_000, ErrorMessage = "Price must be between 10_000 and 1000_000")]
		decimal Price,

		[Required]
		[Range(0, 100, ErrorMessage = "Stock must be between 0 and 100")]
		int Stock
	);
}