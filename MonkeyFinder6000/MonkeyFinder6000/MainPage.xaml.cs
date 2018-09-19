using MonkeyFinder6000.ViewModel;
using Xamarin.Forms;

namespace MonkeyFinder6000
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            if(Device.RuntimePlatform == Device.UWP)
            {
                ToolbarItems.Add(new ToolbarItem()
                {
                    Text = "Refresh",
                    Icon = "Assets/refresh.png",
                    Command = ((MonkeysViewModel)BindingContext).GetMonkeysCommand
                });
            }
        }
    }
}
