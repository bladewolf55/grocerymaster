using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace GroceryMaster.UI.ViewModels;

public partial class AisleEdit : ObservableObject
{
    private readonly IGroceryDataService service;

    [ObservableProperty]
    private Aisle aisle;

    [ObservableProperty]
    private ObservableCollection<Item> items;

    public AisleEdit(Aisle aisle, IGroceryDataService service)
    {
        this.aisle = aisle;
        this.service = service;
        Refresh();
    }

    public List<Item> SelectedItems { get; set; } = new List<Item>();

    public void AddItem(string name)
    {
        Item item = new() { Name = name, AisleId = Aisle.AisleId };
        item = service.AddItem(item);
        service.SaveChanges();
        Items.Add(item);
        SetItemSequences();
    }

    public void ChangeItemSelection(Item Item, CheckedChangedEventArgs e)
    {
        if (e.Value == true)
        {
            SelectedItems.Add(Item);
        }
        else
        {
            SelectedItems.Remove(Item);
        }
    }

    public IEnumerable<Item> GetItems() => service.GetItems(aisle.AisleId);

    public void SetItemSequences()
    {
        for (int i = 1; i <= Items.Count; i++)
        {
            var item = Items[i - 1];
            item.Sequence = i;
            service.UpdateItem(item);
            service.SaveChanges();
        }
    }

    [RelayCommand]
    private async void AddItemWithPrompt()
    {
        string result = await Application.Current.MainPage.DisplayPromptAsync("Item entry", "Enter the Item name");
        if (result != null)
        {
            try
            {
                AddItem(result);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.GetBaseException().Message, "OK");
            }
        }
    }

    [RelayCommand]
    private void DeleteSelectedItems()
    {
        List<Item> deleted = new();
        try
        {
            foreach (var Item in SelectedItems)
            {
                service.DeleteItem(Item);
                service.SaveChanges();
                Items.Remove(Item);
                deleted.Add(Item);
            }
            SetItemSequences();
        }
        catch (Exception ex)
        {
        }
        finally { SelectedItems = SelectedItems.Except(deleted).ToList(); }
    }

    [RelayCommand]
    private void Refresh()
    {
        Items = new ObservableCollection<Item>(GetItems().OrderBy(a => a.Sequence));
    }
}