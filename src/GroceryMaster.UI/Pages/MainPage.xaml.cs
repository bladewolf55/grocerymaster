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

    private void Button_Clicked(object sender, EventArgs e)
    {
        var store = ((Button)sender).BindingContext as Store;
        var vm = new StoreEdit(store, service);
        Application.Current.MainPage.Navigation.PushAsync(new StorePage(vm, service), true);

        // TEST
        //Application.Current.MainPage.Navigation.PushAsync(new TestPage(), true);
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var store = ((CheckBox)sender).BindingContext as Store;
        vm.ChangeStoreSelection(store, e);
    }
}