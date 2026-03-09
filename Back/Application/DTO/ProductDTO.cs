namespace Back.Application.DTO;

public class ProductDTO
{
    public int ProductID { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public string? CategoryName { get; set; } 

    public byte[]? CategoryPicture { get; set; } 
}