using GroceryMaster.Maui.Maui.ViewModels;


namespace GroceryMaster.Maui.Maui.Pages
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage(StoreEdit viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}