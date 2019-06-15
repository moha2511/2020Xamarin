using adamBozhiProj.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace adamBozhiProj
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class attendPage : ContentPage
    {
        string eventIdToAttend = "";
        public attendPage(Event eventIdToAttend)
        {
            InitializeComponent();
            this.eventIdToAttend = eventIdToAttend._id;
        }

        private async void Attend_Clicked(object sender, EventArgs e)
        {

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("https://newmousleheventappp.herokuapp.com/event/");



                    var htttAttendRequest = new StringContent(JsonConvert.SerializeObject(new { id = eventIdToAttend}));
                    var result = await client.PostAsync("addAttendee", htttAttendRequest);
                    await DisplayAlert("Til lykke", "Du er nu tilmeldt arrangementet🎉🎉", "OK");
                    await Navigation.PushAsync(new EventListPage());

                }
                catch (Exception er)
                {
                    Console.Write(er);
                }
            }
        }
    }
   
}