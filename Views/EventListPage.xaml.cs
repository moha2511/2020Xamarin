using adamBozhiProj.Model;
using Newtonsoft.Json;
using Plugin.FilePicker.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace adamBozhiProj
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EventListPage : ContentPage
	{


        FileData imagePath;
        public ObservableCollection<Event> events { get; set; }
        public EventListPage ()
		{
			InitializeComponent ();
            getEventsAsync();
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
                    
                    foreach (var item in this.events)
                    {
                        item.startTime = String.Format("Start tid : {0,18}", item.startTime.Remove(21));
                        item.endTime = String.Format("Slut tid : {0}", item.endTime.Remove(21));
                        item.cost = String.Format("Pris for billet: {0}", item.cost);
                        item.address.line = String.Format("Eventen finder sted: {0}", item.address.line);
                        item.address.city = String.Format("By: {0}", item.address.city);
                    }
                    listView.ItemsSource = this.events;
                    
                }
                catch (Exception er)
                {
                    
                }
            }
        }

   

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            if (e.SelectedItem == null)
            {
                return;
            }
            Event tappedEvent = e.SelectedItem as Event;
            await Navigation.PushAsync(new attendPage(tappedEvent));
            ((ListView)sender).SelectedItem = null;
        }
    }
}