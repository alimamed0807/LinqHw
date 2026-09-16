public class Sale
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public int CustomerId { get; set; }
    public DateTime SaleDate { get; set; }
    public decimal SalePrice { get; set; }

    public Car Car { get; set; }
    public Customer Customer { get; set; }
}