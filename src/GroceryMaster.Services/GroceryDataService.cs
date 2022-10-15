using GroceryMaster.Data;
using GroceryMaster.Data.Models;

namespace GroceryMaster.Services;

public class GroceryDataService : IGroceryDataService
{
    private readonly GroceryMasterDbContext db;

    public GroceryDataService(GroceryMasterDbContext db)
    { this.db = db; }

    public Aisle AddAisle(Aisle aisle) => db.Aisles.Add(aisle).Entity;

    public Item AddItem(Item item) => db.Items.Add(item).Entity;

    public Store AddStore(Store store) => db.Stores.Add(store).Entity;

    public void DeleteAisle(Aisle aisle) => db.Aisles.Remove(aisle);

    public void DeleteItem(Item item) => db.Items.Remove(item);

    public void DeleteStore(Store store) => db.Stores.Remove(store);

    public Aisle? GetAisle(int id) => db.Aisles.Find(id);

    public IEnumerable<Aisle> GetAisles() => db.Aisles;

    public IEnumerable<Aisle> GetAisles(int storeId) => db.Aisles.Where(a => a.StoreId == storeId);

    public Item? GetItem(int id) => db.Items.Find(id);

    public IEnumerable<Item> GetItems() => db.Items;

    public IEnumerable<Item> GetItems(int aisleId) => db.Items.Where(a => a.AisleId == aisleId);

    public Store? GetStore(int id) => db.Stores.Find(id);

    public IEnumerable<Store> GetStores() => db.Stores;

    public int SaveChanges() => db.SaveChanges();

    public Aisle UpdateAisle(Aisle aisle)
    {
        var entry = db.Entry(aisle);
        entry.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        foreach (var item in entry.Entity.Items)
        {
            UpdateItem(item);
        }
        return entry.Entity;
    }

    public Item UpdateItem(Item item)
    {
        var entry = db.Entry(item);
        entry.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        return entry.Entity;
    }

    public Store UpdateStore(Store store)
    {
        var entry = db.Entry(store);
        entry.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        foreach (var aisle in entry.Entity.Aisles)
        {
            UpdateAisle(aisle);
        }
        return entry.Entity;
    }
}