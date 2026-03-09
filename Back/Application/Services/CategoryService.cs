using Microsoft.EntityFrameworkCore;
using Back.Infrastructure.Data;
using Back.Core.Entities;
using Back.Application.DTO;

namespace Back.Application.Services;

public class CategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CategoryDTO> CreateCategoryAsync(CreateCategoryDTO dto)
    {
        var category = new Category
        {
            CategoryName = dto.CategoryName,
            Description = dto.Description,
            Picture = dto.Picture
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return new CategoryDTO
        {
            CategoryID = category.CategoryID,
            CategoryName = category.CategoryName,
            Description = category.Description
        };
    }

    public async Task<List<CategoryDTO>> GetAllCategoriesAsync()
    {
        return await _context.Categories
            .Select(c => new CategoryDTO
            {
                CategoryID = c.CategoryID,
                CategoryName = c.CategoryName,
                Description = c.Description
            })
            .ToListAsync();
    }
}