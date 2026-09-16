public class Car
{
    public int Id { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public decimal Price { get; set; }
    public string Color { get; set; }
    public bool IsNew { get; set; }

    public List<Sale> Sales { get; set; } = new();
    public override string ToString()
    {
        return $"{Brand} {Model} | {Year} | {Price} | {Color} | New: {IsNew}";
    }
}