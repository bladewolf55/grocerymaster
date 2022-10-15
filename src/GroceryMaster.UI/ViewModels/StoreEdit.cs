using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
//using Xamarin.CommunityToolkit;

namespace GroceryMaster.Maui.Maui.ViewModels;

public partial class StoreEdit: ObservableObject
{
    readonly IGroceryDataService service;
    Store selectedStore;

    public ObservableCollection<Store> Stores { get; private set; }

    public Store SelectedStore 
    { 
        get => selectedStore;
        set => SetProperty(ref selectedStore, value);        
    }

    public AsyncRelayCommand<Store> SelectedCommand { get; }

    async Task Selected(Store store)
    {
        if (store == null)
            return;
        await Application.Current.MainPage.DisplayAlert("Selected", store.Name, "OK");
    }

    public StoreEdit(IGroceryDataService service)
    {
        this.service = service;
        Stores = new ObservableCollection<Store>(GetStores());
        SelectedCommand = new AsyncRelayCommand<Store>(Selected);
        //SelectedStore = Stores.FirstOrDefault();
        //OnPropertyChanged(nameof(SelectedStore));
    }


    public IEnumerable<Store> GetStores() => service.GetStores();
 
    

}
