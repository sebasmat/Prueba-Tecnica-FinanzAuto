using Microsoft.EntityFrameworkCore;
using Back.Infrastructure.Data;
using Back.Core.Entities;
using Back.Application.DTO;
using Bogus;

namespace Back.Application.Services;

public class ProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductDTO>> GetAllProductsAsync(){
        // Traemos los productos de la BD, incluimos la categoría y mapeamos al DTO
        return await _context.Products
            .Select(p => new ProductDTO
            {
                ProductID = p.ProductID,
                ProductName = p.ProductName,
                UnitPrice = p.UnitPrice,
                CategoryPicture = null
            })
            .ToListAsync();
    }

    public async Task<ProductDTO?> GetByIdAsync(int id) {
        return await _context.Products
            .Where(p => p.ProductID == id)
            .Include(p => p.Category)
            .Select(p => new ProductDTO {
                ProductID = p.ProductID,
                ProductName = p.ProductName,
                UnitPrice = p.UnitPrice,
                CategoryPicture = p!.Category!.Picture // ¡Aquí sí lo mapeamos!
            }).FirstOrDefaultAsync();
    }

    public async Task<(List<ProductDTO> items, int total)> GetPagedProductsAsync(string? search, int? categoryId, int page, int pageSize){
        var query = _context.Products.AsQueryable();

        // 1. Filtros y Búsqueda
        if (!string.IsNullOrEmpty(search))
            query = query.Where(p => p.ProductName.Contains(search));

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryID == categoryId);

        // 2. Contar total para la paginación
        int total = await query.CountAsync();

        // 3. Paginación (Skip y Take)
        var items = await query
            .OrderBy(p => p.ProductID)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductDTO {
                ProductID = p.ProductID,
                ProductName = p.ProductName,
                UnitPrice = p.UnitPrice
            })
            .ToListAsync();

        return (items, total);
    }

    public async Task<ProductDTO> CreateProductAsync(CreateProductDTO dto){
        // 1. Mapeo manual del DTO a la Entidad (Core Entity)
        var product = new Product
        {
            ProductName = dto.ProductName,
            CategoryID = dto.CategoryID,
            SupplierID = dto.SupplierID, // Nuevo campo del modelo
            UnitPrice = dto.UnitPrice,
            UnitsInStock = dto.UnitsInStock,
            UnitsOnOrder = dto.UnitsOnOrder, // Nuevo campo
            ReorderLevel = dto.ReorderLevel, // Nuevo campo
            QuantityPerUnit = dto.QuantityPerUnit,
            Discontinued = false // Por defecto un producto nuevo no está descontinuado
        };

        // 2. Persistencia en la base de datos PostgreSQL
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        // 3. Retornamos el ProductDTO con el ID generado por la BD
        return new ProductDTO
        {
            ProductID = product.ProductID,
            ProductName = product.ProductName,
            UnitPrice = product.UnitPrice
        };
    }

    // MÉTODO PARA ACTUALIZAR (PUT)
    public async Task<ProductDTO?> UpdateProductAsync(int id, UpdateProductDTO dto)
    {
        // 1. Buscamos si el producto existe en la base de datos
        var product = await _context.Products.FindAsync(id);
        
        // Si no existe, devolvemos null para que el controlador maneje el error 404
        if (product == null) return null;

        // 2. Actualizamos los campos
        product.ProductName = dto.ProductName;
        product.CategoryID = dto.CategoryID;
        product.UnitPrice = dto.UnitPrice;
        product.UnitsInStock = dto.UnitsInStock;
        product.QuantityPerUnit = dto.QuantityPerUnit;
        product.Discontinued = dto.Discontinued;

        // 3. Guardamos los cambios. EF Core es inteligente y solo hará el UPDATE de lo que cambió
        await _context.SaveChangesAsync();

        // 4. Devolvemos el DTO actualizado
        return new ProductDTO
        {
            ProductID = product.ProductID,
            ProductName = product.ProductName,
            UnitPrice = product.UnitPrice
        };
    }

    // MÉTODO PARA ELIMINAR (DELETE)
    public async Task<bool> DeleteProductAsync(int id){
        var product = await _context.Products.FindAsync(id);
        
        if (product == null) return false;

        // Marcamos la entidad para ser eliminada
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task<int> GenerateMassiveProductsAsync(int count){
        // 1. Configuramos los IDs que ya existen en tu base de datos
        var categoryIds = new[] { 4, 5 }; // SERVIDORES y CLOUD
        var supplierId = 1; // Tecnología Avanzada S.A.

        // 2. Configuramos Bogus para inventar productos
        var faker = new Faker<Product>()
            .RuleFor(p => p.ProductName, f => f.Commerce.ProductName())
            .RuleFor(p => p.UnitPrice, f => f.Random.Decimal(10, 5000))
            .RuleFor(p => p.UnitsInStock, f => (short)f.Random.Number(0, 500))
            .RuleFor(p => p.CategoryID, f => f.PickRandom(categoryIds))
            .RuleFor(p => p.SupplierID, f => supplierId)
            .RuleFor(p => p.Discontinued, f => false)
            .RuleFor(p => p.QuantityPerUnit, f => f.Commerce.ProductAdjective());

        int batchSize = 5000; // Tamaño del "camión" (lote)
        int totalInserted = 0;

        // 3. Desactivamos el seguimiento para máxima velocidad
        _context.ChangeTracker.AutoDetectChangesEnabled = false;

        for (int i = 0; i < count / batchSize; i++)
        {
            var products = faker.Generate(batchSize); // Genera 5,000 productos en memoria
            
            _context.Products.AddRange(products); // Los sube al "camión"
            await _context.SaveChangesAsync(); // Dispara el camión a la base de datos
            
            // Limpiamos la memoria para que el siguiente lote no pese
            _context.ChangeTracker.Clear();
            totalInserted += batchSize;
        }

        // Volvemos a activar el seguimiento por defecto
        _context.ChangeTracker.AutoDetectChangesEnabled = true;

        return totalInserted;
    }
}