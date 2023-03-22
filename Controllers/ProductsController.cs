using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiProducts.Data;
using static System.Reflection.Metadata.BlobBuilder;

namespace WebApiProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApiDbContext context;
        public ProductsController(ApiDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductItem>>> GetProductItem()
        {
            return Ok(await context.Products.ToListAsync());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<List<ProductItem>>> GetIdProductItem(int id)
        {
            return Ok(await context.Products.FindAsync(id));
        }
        [HttpGet("/Category")]
        public async Task<ActionResult<List<ProductItem>>> GetCategoryProductItem(string category)
        {
            var products = context.Products.Where(x => x.Category == category).ToList();
            if (category == null)
                return NotFound("Can't be null");
            else if(products.Count == 0)
                return NotFound($"Products with category '{category}' not found.");
            else
                return Ok(await context.Products.Where(x => x.Category == category).ToListAsync());
        }
        [HttpPost("/Add")]
        public async Task<ActionResult<List<ProductItem>>> AddProductItem(ProductItem productItem)
        {
            context.Products.AddAsync(productItem);
            await context.SaveChangesAsync();
            return Ok(await context.Products.ToListAsync());
        }
        [HttpPut("/Attribute")]
        public async Task<ActionResult<List<ProductItem>>> UpdateProductItem(ProductItem productItem)
        {
            var product = await context.Products.FindAsync(productItem.Id);
            if (product == null)
                return BadRequest("Ooops!!!");

            product.Name = productItem.Name;
            product.Count = productItem.Count;
            product.Category = productItem.Category;
            await context.SaveChangesAsync();
            return Ok(await context.Products.ToListAsync());
        }
        [HttpPut("{id}/Quantity")]
        public async Task<ActionResult<List<ProductItem>>> UpdateCountProductItem(int id, int count)
        {
            var product = await context.Products.FindAsync(id);
            product.Count = count;
            await context.SaveChangesAsync();
            return Ok(await context.Products.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<ProductItem>>> DeleteProductItem(int id)
        {
            var product = await context.Products.FindAsync(id);
            context.Products.Remove(product);
            await context.SaveChangesAsync();
            return Ok(await context.Products.ToListAsync());
        }

        [HttpGet("/OrderByCount")]
        public async Task<ActionResult<List<ProductItem>>> OrderByCountProductItem()
        {
            return Ok(context.Products.OrderBy(x => x.Count).ToList());
        }
    }
}
