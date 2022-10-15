using GroceryMaster.Data.Models;

namespace GroceryMaster.Services;

public interface IGroceryDataService
{
    /// <summary>
    /// Add a <see cref="Aisle"/>
    /// </summary>
    /// <param name="aisle"></param>
    /// <returns><see cref="Aisle"/></returns>
    Aisle AddAisle(Aisle aisle);

    /// <summary>
    /// Add a <see cref="Item"/>
    /// </summary>
    /// <param name="item"></param>
    /// <returns><see cref="Item"/></returns>
    Item AddItem(Item item);

    /// <summary>
    /// Add a <see cref="Store"/>
    /// </summary>
    /// <param name="store"></param>
    /// <returns><see cref="Store"/></returns>
    Store AddStore(Store store);

    /// <summary>
    /// Delete a <see cref="Aisle"/>
    /// </summary>
    /// <param name="aisle"></param>
    void DeleteAisle(Aisle aisle);

    /// <summary>
    /// Delete a <see cref="Item"/>
    /// </summary>
    /// <param name="item"></param>
    void DeleteItem(Item item);

    /// <summary>
    /// Delete a <see cref="Store"/>
    /// </summary>
    /// <param name="store"></param>
    void DeleteStore(Store store);

    /// <summary>
    /// Get a <see cref="Aisle"/>
    /// </summary>
    /// <param name="id"></param>
    /// <returns>An <see cref="Aisle"/> or null</returns>
    Aisle? GetAisle(int id);

    /// <summary>
    /// Get all <see cref="IEnumerable{Aisle}"/>
    /// </summary>
    /// <returns><see cref="IEnumerable{Aisle}"/></returns>
    IEnumerable<Aisle> GetAisles();

    /// <summary>
    /// Get a <see cref="IEnumerable{Aisle}"/>
    /// </summary>
    /// <param name="storeId"></param>
    /// <returns>A <see cref="IEnumerable{Aisle}"/></returns>
    IEnumerable<Aisle> GetAisles(int storeId);

    /// <summary>
    /// Get a <see cref="Item"/>
    /// </summary>
    /// <param name="id"></param>
    /// <returns>A <see cref="Item"/></returns>
    Item? GetItem(int id);

    /// <summary>
    /// Get all <see cref="IEnumerable{Item}"/>
    /// </summary>
    /// <returns><see cref="IEnumerable{Item}"/></returns>
    IEnumerable<Item> GetItems();

    /// <summary>
    /// Get a <see cref="IEnumerable{Item}"/>
    /// </summary>
    /// <param name="aisleId"></param>
    /// <returns>A <see cref="IEnumerable{Item}"/></returns>
    IEnumerable<Item> GetItems(int aisleId);

    /// <summary>
    /// Gets a <see cref="Store"/>
    /// </summary>
    /// <param name="id"></param>
    /// <returns>A <see cref="Store"/> or null</returns>
    Store? GetStore(int id);

    /// <summary>
    /// Gets a <see cref="IEnumerable{Store}"/>
    /// </summary>
    /// <returns>A <see cref="IEnumerable{Store}"/></returns>
    IEnumerable<Store> GetStores();

    /// <summary>
    /// Saves pending changes
    /// </summary>
    /// <returns>The number of changed records, if available</returns>
    int SaveChanges();

    /// <summary>
    /// Updates a <see cref="Aisle"/>
    /// </summary>
    /// <param name="aisle"></param>
    /// <returns>The modified <see cref="Aisle"/></returns>
    Aisle UpdateAisle(Aisle aisle);

    /// <summary>
    /// Updates a <see cref="Item"/>
    /// </summary>
    /// <param name="item"></param>
    /// <returns>The modified <see cref="Item"/></returns>
    Item UpdateItem(Item item);

    /// <summary>
    /// Updates a <see cref="Store"/>
    /// </summary>
    /// <param name="store"></param>
    /// <returns>The modified <see cref="Store"/></returns>
    Store UpdateStore(Store store);
}