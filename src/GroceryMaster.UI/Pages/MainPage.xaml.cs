using GroceryMaster.UI.ViewModels;
using Microsoft.Maui.Controls;
using System;

namespace GroceryMaster.UI.Pages;

public partial class MainPage : ContentPage
{
    StoresEdit vm;
    public MainPage(StoresEdit vm)
    {
        InitializeComponent();
        BindingContext = vm;
        this.vm = vm;
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var store = ((CheckBox)sender).BindingContext as Store;
        vm.ChangeStoreSelection(store, e);
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        var store = ((Button)sender).BindingContext as Store;
        Application.Current.MainPage.Navigation.PushAsync(new StorePage(store), true);
    }
}