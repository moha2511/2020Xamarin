using adamBozhiProj.Model;
using Newtonsoft.Json;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Map = Xamarin.Forms.Maps.Map;

namespace adamBozhiProj
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapsPage : ContentPage
	{
        Geocoder geoCoder;
        public ObservableCollection<Event> events { get; set; }
        List<Pin> pinsForEvents = new List<Pin>();
        Map map = new Map();

        public MapsPage ()
		{
			InitializeComponent ();
            getEventsAsync();
            geoCoder = new Geocoder();
		}



        private async void getEventsAsync()
        {

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("https://newmousleheventappp.herokuapp.com/event/get/allEvents");

                    var content = await client.GetStringAsync(client.BaseAddress);
                    events = JsonConvert.DeserializeObject<ObservableCollection<Event>>(content);
                    initializeMapsAsync();
                }
                catch (Exception er)
                {
                    Console.Write(er);
                }
            }

        }

        public async Task<Position> GetLocationFromAddress(string address)
        {
            IEnumerable<Position> addressPositions = await geoCoder.GetPositionsForAddressAsync(address);
            return addressPositions.FirstOrDefault();
        }

        private async void initializeMapsAsync()
        {

            try
            {

                foreach (Event eventInList in events)
                {
                    Position newPosition = await GetLocationFromAddress(eventInList.address.line + " " + eventInList.address.city);

                    pinsForEvents.Add(new Pin()
                    {
                        Position = newPosition,
                        Label = eventInList.name,
                        Address = eventInList.address.line,
                         Type = PinType.Place
                    });
                }

                foreach (Pin pinsToEvents in pinsForEvents)
                {
                    map.Pins.Add(pinsToEvents);
                }
                

                var stack = new StackLayout { Spacing = 0 };
                stack.Children.Add(map);
                getCurrentLocation();
                Content = stack;
            }
            catch(Exception e)
            {
                Console.Write(e);
            }

            }
        protected async void getCurrentLocation()
        {
            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync();
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude),
                                                         Distance.FromMiles(3)));
        }

        protected override void OnAppearing()
        {
            getCurrentLocation();
        }
    }
}