using MonkeyFinder6000.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MonkeyFinder6000
{
    public partial class MainPage : ContentPage
    {
        MonkeysViewModel viewModel;
        public MainPage()
        {
            InitializeComponent();

            viewModel = new MonkeysViewModel();
            BindingContext = viewModel;

            //some Win10 devices still doesn't support sliding to refreshing
            if(Device.RuntimePlatform == Device.UWP)
            {
                ToolbarItems.Add(new ToolbarItem("Refresh", "Assets/refresh.png", () =>
                {
                    if (viewModel.GetMonkeysCommand.CanExecute(null))
                        viewModel.GetMonkeysCommand.Execute(null);

                }));
            }
        }
    }
}
