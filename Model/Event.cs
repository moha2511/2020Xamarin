namespace adamBozhiProj.Model
{

    public class Event
    {
        public object[] attendees { get; set; }
        public string _id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string cost { get; set; }
        public int ageLimit { get; set; }
        public string theme { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string imagePath { get; set; }
        public string creator { get; set; }
        public Address address { get; set; }
        public Contactperson contactPerson { get; set; }
        public int __v { get; set; }
    }

    public class Address
    {
        public string _id { get; set; }
        public string city { get; set; }
        public int zipCode { get; set; }
        public string line { get; set; }
    }

    public class Contactperson
    {
        public string _id { get; set; }
        public string contactEmail { get; set; }
        public string contactPhone { get; set; }
        public string contactName { get; set; }
    }

}
