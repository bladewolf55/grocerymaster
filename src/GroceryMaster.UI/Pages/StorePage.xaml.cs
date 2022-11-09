using GroceryMaster.UI.ViewModels;
using System.Collections.ObjectModel;

namespace GroceryMaster.UI.Pages;

public partial class StorePage : ContentPage
{
    private IGroceryDataService service;
    private StoreEdit vm;

    public StorePage(StoreEdit vm, IGroceryDataService service)
    {
        InitializeComponent();
        BindingContext = vm;
        this.vm = vm;
        this.service = service;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        var aisle = ((Button)sender).BindingContext as Aisle;
        //var vm = new AisleEdit(store, service);
        //Application.Current.MainPage.Navigation.PushAsync(new AislePage(vm), true);
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var aisle = ((CheckBox)sender).BindingContext as Aisle;
        vm.ChangeAisleSelection(aisle, e);
    }

    private void collectionView_ReorderCompleted(object sender, EventArgs e)
    {
        vm.SetAisleSequences();
    }
}