using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs;
public record class ProductRequest(string ProductID, string Name, decimal Price, int Stock);