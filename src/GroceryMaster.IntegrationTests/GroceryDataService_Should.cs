using FluentAssertions.Execution;
using GroceryMaster.Data;
using GroceryMaster.Data.Models;
using GroceryMaster.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GroceryMaster.IntegrationTests;

public class GroceryDataService_Should
{
    GroceryMasterDbContext context;
    GroceryDataService service;

    public GroceryDataService_Should()
    {
        service = GetNewService(true);
    }

    ~GroceryDataService_Should() {
        context.Database.EnsureDeleted();
    }

    private GroceryDataService GetNewService(bool initDb)
    {
        // arrange
        DbContextOptions options = new DbContextOptionsBuilder()
            .UseSqlite("Data Source=grocerymaster_test.db;Cache=Shared;")
            .Options;
        context = new GroceryMasterDbContext(options);
        if (initDb)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
        service = new(context);
        return service;
    }

    [Fact]
    public void Pass_the_smoke_tests()
    {
        Store store = new()
        {
            StoreId = 1,
            Name = "a",
            Aisles = new[]
            {
                new Aisle
                { AisleId = 11, Name = "b", Sequence = 1, Items = new[]
                    {
                        new Item { ItemId = 111, Name = "c", Sequence = 1, IsToBuy = true, IsComplete = true, IsFavorite = true }
                    }
                }
             }
        };

        // Create
        service.AddStore(store);
        service.SaveChanges();

        Aisle aisle;
        Item item;

        // Retrieve and Assert
        using (new AssertionScope()) {
            var stores = service.GetStores();
            stores.Should().HaveCount(1);
            store = service.GetStore(1);
            store.Should().NotBeNull();
            store.Name.Should().Be("a");            

            // aisle
            store.Aisles.Should().HaveCount(1);
            store.Aisles?.First()?.Items.Should().HaveCount(1);
            var aisles = service.GetAisles(1);
            aisles.Should().HaveCount(1);
            aisle = service.GetAisle(11);
            aisle.Should().NotBeNull();
            aisle.Name.Should().Be("b");
            aisle.Sequence.Should().Be(1);

            // item
            aisle.Items.Should().HaveCount(1);
            var items = service.GetItems(11);
            items.Should().HaveCount(1);
            item = service.GetItem(111);
            item.Should().NotBeNull();
            item.Name.Should().Be("c");
            item.Sequence.Should().Be(1);
            item.IsToBuy.Should().BeTrue();
            item.IsComplete.Should().BeTrue();
            item.IsFavorite.Should().BeTrue();
        }

        // Update current context 
        using (new AssertionScope())
        {
            store.Name = "z";
            aisle.Name = "y";
            aisle.Sequence = 99;
            item.Name = "x";
            item.Sequence = 98;
            item.IsToBuy = false;
            item.IsComplete = false;
            item.IsFavorite = false;
            service.SaveChanges();

            store = service.GetStore(1);
            store.Name.Should().Be("z");
            aisle = service.GetAisle(11);
            aisle.Name.Should().Be("y");
            aisle.Sequence.Should().Be(99);
            item = service.GetItem(111);
            item.Name.Should().Be("x");
            item.Sequence.Should().Be(98);
            item.IsToBuy.Should().BeFalse();
            item.IsComplete.Should().BeFalse();
            item.IsFavorite.Should().BeFalse();
        }

        // Update using new context 
        using (new AssertionScope())
        {
            // new service instance
            service = GetNewService(false);
            store.Name = "a";
            aisle.Name = "b";
            aisle.Sequence = 1;
            item.Name = "c";
            item.Sequence = 1;
            item.IsToBuy = false;
            item.IsComplete = false;
            item.IsFavorite = false;
            // new service, so explicitly update. graph has been maintained.
            service.UpdateStore(store);
            service.SaveChanges();

            store = service.GetStore(1);
            store.Name.Should().Be("a");
            aisle = service.GetAisle(11);
            aisle.Name.Should().Be("b");
            aisle.Sequence.Should().Be(1);
            item = service.GetItem(111);
            item.Name.Should().Be("c");
            item.Sequence.Should().Be(1);
            item.IsToBuy.Should().BeFalse();
            item.IsComplete.Should().BeFalse();
            item.IsFavorite.Should().BeFalse();
        }

        // Delete using store graph
        using (new AssertionScope())
        {
            // delete a child
            service.DeleteItem(item);
            // delete a parent with children
            service.DeleteStore(store);
            service.SaveChanges();
            service.GetStores().Should().BeEmpty();
            service.GetAisles().Should().BeEmpty();
            service.GetItems().Should().BeEmpty();
        }

        // Delete with new service instance
        using (new AssertionScope()) {

            // readd store
            service.AddStore(store);
            service.SaveChanges();
            
            // new service instance
            service = GetNewService(false);
            service.DeleteStore(store);
            service.SaveChanges();
            service.GetStores().Should().BeEmpty();
            service.GetAisles().Should().BeEmpty();
            service.GetItems().Should().BeEmpty();

        }
    }
}