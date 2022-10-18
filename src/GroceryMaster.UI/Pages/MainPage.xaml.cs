using GroceryMaster.UI.ViewModels;


namespace GroceryMaster.UI.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(StoreEdit viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}