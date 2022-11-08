using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GroceryMaster.UI.Pages;
using System.Collections.ObjectModel;

namespace GroceryMaster.UI.ViewModels;

public partial class StoresEdit : ObservableObject
{
    private readonly IGroceryDataService service;

    #region "Properties"

    [ObservableProperty]
    ObservableCollection<Store> stores;

    public List<Store> SelectedStores { get; set; } = new List<Store>();

    #endregion

    #region "Constructors"

    public StoresEdit(IGroceryDataService service)
    {
        this.service = service;
        Refresh();
    }

    #endregion

    #region "Methods"
    public IEnumerable<Store> GetStores() => service.GetStores();

    [RelayCommand]
    private void Refresh()
    {
        Stores = new ObservableCollection<Store>(GetStores());
    }

    [RelayCommand]
    private async void AddStore()
    {
        string result = await Application.Current.MainPage.DisplayPromptAsync("Store entry", "Enter the store name");
        if (result != null)
        {
            try
            {
                Store store = new() { Name = result };
                store = service.AddStore(store);
                service.SaveChanges();
                Stores.Add(store);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.GetBaseException().Message, "OK");
            }
        }
    }

    public void ChangeStoreSelection(Store store, CheckedChangedEventArgs e)
    {
        if (e.Value == true)
        {
            SelectedStores.Add(store);
        }
        else
        {
            SelectedStores.Remove(store);
        }
    }

    [RelayCommand]
    private void DeleteSelectedStores()
    {
        List<Store> deleted = new();
        try
        {
            foreach (var store in SelectedStores)
            {
                service.DeleteStore(store);
                service.SaveChanges();
                Stores.Remove(store);
                deleted.Add(store);
            }
        }
        catch (Exception ex)
        {

        }
        finally { SelectedStores = SelectedStores.Except(deleted).ToList(); }
    }
    #endregion

}