using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GroceryMaster.UI.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections.ObjectModel;

namespace GroceryMaster.UI.ViewModels;

public partial class AisleItemsEdit : ObservableObject
{
    private readonly IGroceryDataService service;

    [ObservableProperty]
    private Store store;

    [ObservableProperty]
    private ObservableCollection<AisleItemGroup> aisles;

    public AisleItemsEdit(Store store, IGroceryDataService service)
    {
        this.store = store;
        this.service = service;
        Refresh();
    }

    private IEnumerable<Aisle> GetAisles() => service.GetAislesWithItems(store.StoreId);

    [RelayCommand]
    private void Refresh()
    {
        var aisles = GetAisles();
        // construct the grouped items as required by the list view
        IEnumerable<AisleItemGroup> groupedItems = aisles
            .Select(a => new AisleItemGroup(a.Name, a.Sequence, a.AisleId, a.Items.OrderBy(a => a.Sequence).ToList()))
            .OrderBy(a => a.Sequence);

        Aisles = new ObservableCollection<AisleItemGroup>(groupedItems);
    }

    public void SetItemSequences(List<Item> items)
    {
        for (int i = 1; i <= items.Count; i++)
        {
            var item = items[i - 1];
            item.Sequence = i;
            
            service.UpdateItem(item);
            //service.SaveChanges();
        }
    }


    public void DragDropItem(Item dragItem, Item dropItem)
    {
        var dragAisle = service.GetAisle(dragItem.AisleId);
        var dropAisle = service.GetAisle(dropItem.AisleId);

        bool aisleChanged = dragAisle.AisleId != dropAisle.AisleId;

        dragAisle.Items.Remove(dragItem);

        var items = dropAisle.Items.ToList();
        var dropIndex = items.IndexOf(dropItem) + 1;
        dragItem.AisleId = dropAisle.AisleId;
        items.Insert(dropIndex, dragItem);
        service.AddItem(dragItem);
        SetItemSequences(items);
        

        if (aisleChanged)
            SetItemSequences(dragAisle.Items.ToList());

        service.SaveChanges();
        Refresh();
    }
}
