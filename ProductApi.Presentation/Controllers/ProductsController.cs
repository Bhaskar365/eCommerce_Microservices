using eCommerce.SharedLibrary.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Application.DTOs.Conversions;
using ProductApi.Application.Interfaces;

namespace ProductApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProduct productInterface) : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            var products = await productInterface.GetAllAsync();
            if (!products.Any())
                return (IEnumerable<ProductDTO>)NotFound("No products detected in database");

            var (_, list) = ProductConversion.FromEntity(null!, products);

            return list!.Any() ? list : (IEnumerable<ProductDTO>)NotFound("No products detected in database");
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id) 
        {
            var product = await productInterface.FindByIdAsync(id);
            if (product == null)
                return NotFound("Product requested not found");

            var (_product, _) = ProductConversion.FromEntity(product,null);
            return _product is not null ? Ok(_product) : NotFound("Product not found");
        }

        [HttpPost]
        public async Task<ActionResult<Response>> CreateProduct(ProductDTO product) 
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);



        }
    }
}
