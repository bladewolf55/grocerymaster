using GroceryMaster.Data.Models;
using GroceryMaster.UI.ViewModels;
using GroceryMaster.Services;
using System.Collections.ObjectModel;

namespace GroceryMaster.UnitTests;

public class StoresEdit_Should
{
    private IGroceryDataService service;
    private StoresEdit storesEdit;

    public StoresEdit_Should()
    {
        service = Substitute.For<IGroceryDataService>();
        storesEdit = new(service);
    }

    [Fact]
    public void Add_a_store()
    {
        // arrange
        string name = "Blamo";
        Store store = new()
        {
            Name = name,
        };

        service.AddStore((Arg.Is<Store>(a => a.Name == name))).Returns(store);

        // act
        storesEdit.AddStore(name);

        // assert
        service.Received().AddStore(Arg.Is<Store>(a => a.Name == name));
        service.Received().SaveChanges();
        storesEdit.Stores.Should().HaveCount(1);
    }

    [Fact]
    public void Delete_select_stores()
    {
        // arrange
        Store store1 = new() { StoreId = 1 };
        Store store2 = new() { StoreId = 2 };
        Store store3 = new() { StoreId = 3 };
        List<Store> stores = new() { store1, store2, store3 };
        storesEdit.Stores = new ObservableCollection<Store>(stores);
        storesEdit.SelectedStores = stores.Take(2).ToList();

        // act
        storesEdit.DeleteSelectedStoresCommand.Execute(null);

        // assert
        service.Received().DeleteStore(store1);
        service.Received().DeleteStore(store2);
        storesEdit.Stores.Should().HaveCount(1);
    }

    [Fact]
    public void Get_a_store_list()
    {
        // arrange
        List<Store> stores = new()
        {
            new Store { StoreId = 1},
            new Store { StoreId = 2}
        };

        service.GetStores().Returns(stores);

        // act
        var result = storesEdit.GetStores();

        // assert
        result.Should().HaveCount(2);
    }
}