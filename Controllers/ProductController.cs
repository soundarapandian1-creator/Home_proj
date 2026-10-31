using Microsoft.AspNetCore.Mvc;
using simpleApiDemo.Models;

namespace simpleApiDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private static List<Product> products = new()
        {
            new Product { Id = 1, Name = "Laptop", Price = 75000 },
            new Product { Id = 2, Name = "Phone", Price = 25000 }
        };

        /// <summary>
        /// Retrieves all products from the product list.
        /// </summary>
        /// <returns>
        /// An IEnumerable of all Product objects with HTTP status code 200 (OK).
        /// Returns an empty collection if no products exist.
        /// </returns>
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            return Ok(products);
        }

        /// <summary>
        /// Retrieves a single product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product to retrieve.</param>
        /// <returns>
        /// The Product object with HTTP status code 200 (OK) if found.
        /// Returns HTTP status code 404 (Not Found) if no product matches the provided ID.
        /// </returns>
        [HttpGet("{id}")]
        public ActionResult<Product> GetById(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            return Ok(product);
        }

       

        /// <summary>
        /// Updates an existing product with new information.
        /// </summary>
        /// <param name="id">The unique identifier of the product to update.</param>
        /// <param name="updatedProduct">The Product object containing the new Name and Price values.</param>
        /// <returns>
        /// The updated Product object with HTTP status code 200 (OK) if successfully updated.
        /// Returns HTTP status code 404 (Not Found) if no product matches the provided ID.
        /// </returns>
        /// <remarks>
        /// The product ID cannot be changed; only Name and Price fields are updated.
        /// </remarks>
        [HttpPut("{id}")]
        public ActionResult<Product> Update(int id, [FromBody] Product updatedProduct)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            product.Name = updatedProduct.Name;
            product.Price = updatedProduct.Price;
            return Ok(product);
        }

        /// <summary>
        /// Creates a new product and adds it to the product list.
        /// </summary>
        /// <param name="product">The Product object to create, containing Name and Price values.</param>
        /// <returns>
        /// The newly created Product object with an auto-generated ID and HTTP status code 201 (Created).
        /// The response includes a Location header pointing to the new product resource.
        /// Returns HTTP status code 401 (Unauthorized) if the required API key is missing or invalid.
        /// </returns>
        /// <remarks>
        /// This method requires authentication via the X-API-Key header.
        /// The product ID is auto-generated sequentially based on the highest existing ID.
        /// Authentication check is performed before product creation.
        /// </remarks>
        [HttpPost]
        public ActionResult<Product> Create([FromBody] Product product)
        {
            // Authentication logic - check for API key
            if (!Request.Headers.TryGetValue("X-API-Key", out var apiKey))
            {
                return Unauthorized(new { message = "API key is required" });
            }

            const string validApiKey = "secret-api-key-12345";
            if (apiKey != validApiKey)
            {
                return Unauthorized(new { message = "Invalid API key" });
            }

            product.Id = (products.Any() ? products.Max(p => p.Id) : 0) + 1;
            products.Add(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        /// <summary>
        /// Deletes a product from the product list by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product to delete.</param>
        /// <returns>
        /// HTTP status code 204 (No Content) if the product is successfully deleted.
        /// Returns HTTP status code 404 (Not Found) if no product matches the provided ID.
        /// </returns>
        /// <remarks>
        /// Once deleted, a product cannot be recovered from this API.
        /// The deletion operation does not return the deleted product object.
        /// </remarks>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();

            products.Remove(product);
            return NoContent();
        }
    }
}
/// comments ends here..
