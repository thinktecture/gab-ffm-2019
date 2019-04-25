using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Serverless.Azure.WebJobs.Extensions.SqlServer;
using Serverless.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Serverless
{
    public class ProductsApi
    {
        [FunctionName("GetProducts")]
        public IActionResult GetProducts(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "products")]
            HttpRequest req,
            [SqlServer(Query = "SELECT ID, Name FROM Products ORDER BY Name")]
            IEnumerable<Product> products)
        {
            return new OkObjectResult(products.Select(p => new { p.Id, p.Name }));
        }

        [FunctionName("GetProductDetails")]
        public IActionResult GetProductDetails(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "products/{id}")]
            HttpRequest req,
            [SqlServer(Query = "SELECT * FROM Products WHERE ID={id}")]
            Product product)
        {
            return new OkObjectResult(product);
        }
    }
}
