using Microsoft.AspNetCore.Mvc;
using Back.Application.Services;
using Back.Application.DTO;
using Microsoft.AspNetCore.Authorization;

namespace Back.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SupplierController : ControllerBase
{
    private readonly SupplierService _supplierService;

    public SupplierController(SupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<SupplierDTO>>> Get()
    {
        return Ok(await _supplierService.GetAllAsync());
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<SupplierDTO>> Post([FromBody] SupplierDTO dto)
    {
        var created = await _supplierService.CreateAsync(dto);
        return Ok(created);
    }
}