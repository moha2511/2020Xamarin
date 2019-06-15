using adamBozhiProj.Model;
using Newtonsoft.Json;
using Plugin.FilePicker.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace adamBozhiProj.ViewModel
{
    class EventListPageViewModel : INotifyPropertyChanged
    {
        FileData imagePath;
        Event eventModel = null;
        string currentId;
        INavigation navigation;
        ICommand LoadTasks;
        private ObservableCollection<Event> _events;

        public ObservableCollection<Event> events { get => _events; set { _events = value; OnPropertyChanged(); } }

        public EventListPageViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            LoadTasks = new Command(async func => await getEventsAsync());
        }

        private async Task getEventsAsync()
        {
            eventModel = new Event();
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("https://newmousleheventappp.herokuapp.com/event/get/allEvents");

                    var content = await client.GetStringAsync(client.BaseAddress);
                    events = JsonConvert.DeserializeObject<ObservableCollection<Event>>(content);

                    foreach (var item in this.events)
                    {
                        eventModel._id = item._id;
                        eventModel.startTime = String.Format("Start tid : {0,18}", item.startTime.Remove(21));
                        eventModel.endTime = String.Format("Slut tid : {0}", item.endTime.Remove(21));
                        eventModel.cost = String.Format("Pris for billet: {0}", item.cost);
                        eventModel.address.line = String.Format("Eventen finder sted: {0}", item.address.line);
                        eventModel.address.city = String.Format("By: {0}", item.address.city);
                    }

                }
                catch (Exception er)
                {
                    Console.Write(er);
                }
            }
        }

        private async Task navigateToAttendPage()
        {
            //await navigation.PushAsync(new attendPage(currentId));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
