using GroceryMaster.UI.ViewModels;
using Microsoft.Maui.Controls;
using System;

namespace GroceryMaster.UI.Pages;

public partial class MainPage : ContentPage
{
    private IGroceryDataService service;
    private StoresEdit vm;

    public MainPage(StoresEdit vm, IGroceryDataService service)
    {
        InitializeComponent();
        BindingContext = vm;
        this.vm = vm;
        this.service = service;
    }

    private void ButtonOldPage_Clicked(object sender, EventArgs e)
    {
        var store = ((Button)sender).BindingContext as Store;
        var vm = new StoreEdit(store, service);
        Application.Current.MainPage.Navigation.PushAsync(new StorePage(vm, service), true);
    }

    private void ButtonNewPage_Clicked(object sender, EventArgs e)
    {
        var store = ((Button)sender).BindingContext as Store;
        var vm = new AisleItemsEdit(store, service);
        Application.Current.MainPage.Navigation.PushAsync(new AisleItemsPage(vm, service), true);
    }



    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var store = ((CheckBox)sender).BindingContext as Store;
        vm.ChangeStoreSelection(store, e);
    }
}