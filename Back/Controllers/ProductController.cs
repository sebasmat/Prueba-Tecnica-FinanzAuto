using Microsoft.AspNetCore.Mvc;
using Back.Application.Services;
using Back.Application.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Back.Controllers;

[ApiController]
[Route("api/[controller]")] // La ruta será api/product
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService){
        _productService = productService;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult> GetAllPaginatedProducts(
        [FromQuery] string? search, 
        [FromQuery] int? categoryId, 
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10){
        var (items, total) = await _productService.GetPagedProductsAsync(search, categoryId, page, pageSize);
        
        // Devolvemos los datos y metadatos de paginación
        return Ok(new {
            TotalItems = total,
            CurrentPage = page,
            PageSize = pageSize,
            Data = items
        });
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetProductById(int id){
        var product = await _productService.GetByIdAsync(id);

        if(product == null){
            return NotFound($"No se encontró el producto con el ID {id}.");
        }
        
        return Ok(product);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ProductDTO>> CreateProduct([FromBody] CreateProductDTO dto){

        if (dto == null){
            return BadRequest("Los datos del producto son nulos.");
        }

        // Llamamos al servicio para crear el producto
        var createdProduct = await _productService.CreateProductAsync(dto);

        // Retornamos un 200 OK con el objeto creado
        return Ok(createdProduct);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<ProductDTO>> Put(int id, [FromBody] UpdateProductDTO dto){
        var updatedProduct = await _productService.UpdateProductAsync(id, dto);
        
        if (updatedProduct == null)
        {
            // Devuelve un HTTP 404 Not Found si el ID no existe
            return NotFound($"No se encontró el producto con el ID {id}."); 
        }

        return Ok(updatedProduct); 
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id){
        var success = await _productService.DeleteProductAsync(id);
        
        if (!success)
        {
            return NotFound($"No se encontró el producto con el ID {id}.");
        }

        return NoContent(); 
    }

    [Authorize]
    [HttpPost("generate")]
    public async Task<ActionResult> GenerateMassive(){
        // Iniciamos un cronómetro para ver cuánto tarda (opcional pero profesional)
        var watch = System.Diagnostics.Stopwatch.StartNew();
        
        int total = await _productService.GenerateMassiveProductsAsync(100000);
        
        watch.Stop();
        var elapsedMs = watch.Elapsed.TotalSeconds;

        return Ok(new {
            Message = $"Se generaron {total} productos exitosamente.",
            TimeInSeconds = elapsedMs,
            CategoryIdsUsed = "4 (SERVIDORES), 5 (CLOUD)"
        });
    }
}