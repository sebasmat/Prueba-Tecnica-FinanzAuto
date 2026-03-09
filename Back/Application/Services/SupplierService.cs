using Microsoft.EntityFrameworkCore;
using Back.Infrastructure.Data;
using Back.Core.Entities;
using Back.Application.DTO;

namespace Back.Application.Services;

public class SupplierService
{
    private readonly AppDbContext _context;

    public SupplierService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<SupplierDTO>> GetAllAsync()
    {
        return await _context.Suppliers
            .Select(s => new SupplierDTO {
                SupplierID = s.SupplierID,
                CompanyName = s.CompanyName,
                City = s.City,
                Country = s.Country
            }).ToListAsync();
    }

    public async Task<SupplierDTO> CreateAsync(SupplierDTO dto)
    {
        var supplier = new Supplier {
            CompanyName = dto.CompanyName,
            ContactName = dto.ContactName,
            ContactTitle = dto.ContactTitle,
            Address = dto.Address,
            City = dto.City,
            Region = dto.Region,
            PostalCode = dto.PostalCode,
            Country = dto.Country,
            Phone = dto.Phone,
            Fax = dto.Fax,
            HomePage = dto.HomePage
        };

        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();

        dto.SupplierID = supplier.SupplierID;
        return dto;
    }
}