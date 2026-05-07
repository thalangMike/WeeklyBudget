using MauiAppSandbox.Models;
using MauiAppSandbox.PageModels;

namespace MauiAppSandbox.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}