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

		[HttpPost]
		[ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
		public ActionResult<ProductResponse> Create([FromBody] ProductRequest request)
		{
			try
			{
				var product = new Product(request.ProductID, request.Name, request.Price, request.Stock);
				_catalog.CreateProduct(product);

				return CreatedAtAction(
					actionName: nameof(GetProduct),
					routeValues: new {productID = product.ProductID},
					value: ProductResponse.FromModel(product)
				);
			}
			catch (ArgumentException e)
			{
				return Problem(
					statusCode: StatusCodes.Status400BadRequest,
					title: "Invalid Product Data",
					detail: e.Message
				);
			}
		}

		[HttpDelete("{productID}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
		public IActionResult Delete([FromRoute] string productID)
		{
			var product = _catalog.GetProduct(productID);

			if (product is null)
			{
				return Problem(
					statusCode: StatusCodes.Status404NotFound,
					title: "Product Not Found!",
					detail: $"No product was not found in ID {productID}"
				);
			}
			_catalog.RemoveProduct(productID);

			return NoContent();
		}

		[HttpPut("{productID}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
		public ActionResult<ProductResponse> Update(
			[FromRoute] string productID, [FromBody] UpdateProductRequest request)
			{
				var exitingProduct = _catalog.GetProduct(productID);
				if (exitingProduct is null)
				{
					return Problem(statusCode: StatusCodes.Status404NotFound, title: "Product not found", detail: $"No product was found for ID {productID}.");
				}

				try
				{
					var updateProdek = new Product(productID, request.Name, request.Price, request.Stock);

					_catalog.UpdateProduct(productID, updateProdek);

					return Ok(ProductResponse.FromModel(updateProdek));
				}
				catch (ArgumentException e)
				{
					return Problem(
						statusCode: StatusCodes.Status400BadRequest,
						title: "Invalid product data!",
						detail: e.Message);
				}
			}

		// [HttpPost]
		// [ProducesResponseType(typeof(IEnumerable<ProductResponse>), StatusCodes.Status201Created)]
		// [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
		// public ActionResult<IEnumerable<ProductResponse>> CreateProducts([FromBody] IEnumerable<CreateProductDto> requests)
		// {
		// 	var responses = new List<ProductResponse>();

		// 	foreach (var request in requests)
		// 	{
		// 		var existingProduct = _catalog.GetProduct(request.ProductID);
		// 		if (existingProduct is not null)
		// 		{
		// 			return Problem(
		// 				statusCode: StatusCodes.Status400BadRequest,
		// 				title: "Product Already Exists",
		// 				detail: $"Product with ID {request.ProductID} already exists. Bulk insert aborted."
		// 			);
		// 		}

		// 		var product = _catalog.AddProduct(request);
				
		// 		responses.Add(ProductResponse.FromModel(product));
		// 	}

		// 	return CreatedAtAction(
		// 		nameof(GetAll), 
		// 		null, 
		// 		responses
		// 	);
		// }

	}
}