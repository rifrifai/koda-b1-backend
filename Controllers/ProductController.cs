using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public sealed class ProductController : ControllerBase
	{
		private readonly MemoryProductCatalog _catalog;
		public ProductController(MemoryProductCatalog catalog)
		{
			_catalog = catalog;
		}

		[HttpGet]
		[ProducesResponseType(typeof(IReadOnlyList<ProductResponse>), StatusCodes.Status200OK)]
		public ActionResult<IReadOnlyList<Product>> GetAll()
		{
			var products =  _catalog.GetProducts().Select(ProductResponse.FromModel).ToArray();

			return Ok(products);
		}

		[HttpGet("{productID}")]
		[ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
		public ActionResult<ProductResponse> GetProduct([FromRoute] string productID)
		{
			var product = _catalog.GetProduct(productID);
			if (product is null)
				return Problem(
					statusCode: StatusCodes.Status404NotFound,
					title: "Product does not exist",
					detail: $"No product was found for ID {productID}"
				);
			return Ok(ProductResponse.FromModel(product));
		}
	}
}