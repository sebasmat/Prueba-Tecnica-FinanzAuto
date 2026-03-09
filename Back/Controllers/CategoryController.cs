using Microsoft.AspNetCore.Mvc;
using Back.Application.Services;
using Back.Application.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Back.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly CategoryService _categoryService;

    public CategoryController(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<CategoryDTO>>> Get()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        return Ok(categories);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CategoryDTO>> Post([FromBody] CreateCategoryDTO dto)
    {
        if (string.IsNullOrEmpty(dto.CategoryName))
            return BadRequest("El nombre de la categoría es obligatorio.");

        var newCategory = await _categoryService.CreateCategoryAsync(dto);
        return Ok(newCategory);
    }
}