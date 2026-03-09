namespace Back.Application.DTO;

public class UpdateProductDTO
{
    public string ProductName { get; set; } = string.Empty;
    public int? CategoryID { get; set; }
    public decimal UnitPrice { get; set; }
    public short UnitsInStock { get; set; }
    public string? QuantityPerUnit { get; set; }
    public bool Discontinued { get; set; } 
}