using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.DTOs;
public sealed record ProductResponse(string ProductID, string Name, decimal Price, int Stock)
{
    public static ProductResponse FromModel(Product product) => new(product.ProductID, product.Name, product.Price, product.Stock);
}