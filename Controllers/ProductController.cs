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

		// [HttpPost]
		// [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
		// [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
		// public ActionResult<ProductResponse> CreateProduct([FromBody] CreateProductDto request)
		// {
		// 	var isExist = _catalog.GetProduct(request.ProductID);
		// 	if (isExist is not null)
		// 	{
		// 		return Problem(
		// 			statusCode: StatusCodes.Status400BadRequest,
		// 			title: "Product ID conflict",
		// 			detail: $"Product with ID {request.ProductID} already exists."
		// 		);
		// 	}

		// 	var newProduct = _catalog.AddProduct(request);

		// 	var response = ProductResponse.FromModel(newProduct);

		// 	return CreatedAtAction(
		// 		nameof(GetProduct), 
		// 		new { productID = newProduct.ProductID }, 
		// 		response
		// 	);
		// }
		[HttpPost]
		[ProducesResponseType(typeof(IEnumerable<ProductResponse>), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
		public ActionResult<IEnumerable<ProductResponse>> CreateProducts([FromBody] IEnumerable<CreateProductDto> requests)
		{
			var responses = new List<ProductResponse>();

			foreach (var request in requests)
			{
				// 1. Validasi duplikasi ID
				var existingProduct = _catalog.GetProduct(request.ProductID);
				if (existingProduct is not null)
				{
					return Problem(
						statusCode: StatusCodes.Status400BadRequest,
						title: "Product Already Exists",
						detail: $"Product with ID {request.ProductID} already exists. Bulk insert aborted."
					);
				}

				// 2. Simpan ke catalog
				var product = _catalog.AddProduct(request);
				
				// 3. Tambahkan ke list response
				responses.Add(ProductResponse.FromModel(product));
			}

			// 4. Kembalikan semua produk yang berhasil dibuat
			// (Untuk bulk insert, biasanya kita kembalikan URL koleksi/list produknya)
			return CreatedAtAction(
				nameof(GetAll), 
				null, 
				responses
			);
		}

	}
}