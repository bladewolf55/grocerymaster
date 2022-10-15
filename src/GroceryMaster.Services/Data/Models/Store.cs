namespace GroceryMaster.Data.Models;

public class Store
{
    public int StoreId { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<Aisle> Aisles { get; set; } = new HashSet<Aisle>();

}
