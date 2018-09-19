using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;
using QuickType;
using Xamarin.Forms;
using Xamarin.Essentials;

using System.Linq;

namespace MonkeyFinder6000.ViewModel
{
    public class MonkeysViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Monkey> Monkeys { get; }
        public ICommand GetMonkeysCommand { get; }
        public ICommand GetClosestCommand { get; }
        public MonkeysViewModel()
        {
            Title = "Monkey Finder";
            Monkeys = new ObservableRangeCollection<Monkey>();
            GetMonkeysCommand = new Command(async () => await GetMonkeysAsync());
            GetClosestCommand = new Command(async () => await GetClosestAsync());
        }

        async Task GetClosestAsync()
        {
            if (Monkeys.Count == 0)
                return;

            if (IsBusy)
                return;
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if(location == null)
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
                }

                var first = Monkeys.OrderBy(m => location.CalculateDistance(
                    new Location(m.Latitude, m.Longitude), DistanceUnits.Miles))
                    .FirstOrDefault();

                await Application.Current.MainPage.DisplayAlert("", first.Name + " " +
                    first.Location, "OK");
            
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Something is wrong",
                       "Unable to get location! :(", "OK");
            }
        }

        async Task GetMonkeysAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                var client = new HttpClient();
                var json = await client.GetStringAsync("https://montemagno.com/monkeys.json");
                var all = Monkey.FromJson(json);
                Monkeys.ReplaceRange(all);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Something is wrong",
                    "OH MY GOODNESS! :(", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }


    }
}
