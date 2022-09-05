using Newtonsoft.Json;


public class Event
{
    public int id;
    public string name;
    public string program;
    public string dateStart;
    public string dateEnd;
    public string url;
    public string owner;
}


//EventList simulates an interface with a database. Rather than storing
//data here, it would be much better to store it on disk in a database,
//rather than in memory
public class EventList 
{
    private List<Event> events;

    public EventList(List<Event> events)
    {
        this.events = events;
    }

    private string CalculateDays(Event e)
    {
        DateTime start = DateTime.Parse(e.dateStart);
        DateTime end = DateTime.Parse(e.dateEnd);

        System.TimeSpan diff = end.Subtract(start);

        int days = diff.Days + 1;

        return days.ToString();
    }

    public string GetEventByID(int id)
    {
        Event selectedEvent = null;

        foreach(Event e in events)
        {
            if(e.id == id)
            {
                selectedEvent = e;
                break;
            }
        }

        if (selectedEvent != null)
        {
            Newtonsoft.Json.Linq.JObject o = Newtonsoft.Json.Linq.JObject.FromObject(new
            {
                name = selectedEvent.name,
                days = CalculateDays(selectedEvent),
                websiteUrl = selectedEvent.url
            });
            
            return o.ToString();
        }
        else
        {
            return "No event found";
        }
    }
}


class Exercise
{
    static int GetInput()
    {
        bool goodInput = false;
        int requestID = 0;


        while (goodInput == false)
        {
            Console.WriteLine("Input requested id (-1 to exit)");
            int temp;
            if (int.TryParse(Console.ReadLine(), out temp))
            {
                goodInput = true;
                requestID = temp;
            }
            else
            {
                Console.WriteLine("Invalid input, try again.");
            }
        }

        return requestID;
    }

    static void Main(string[] args)
    {
        EventList events;
        using (StreamReader reader = new StreamReader("..\\..\\..\\input.json"))
        {
            Console.WriteLine("Reading input.json");
            string json = reader.ReadToEnd();
            events = new EventList(JsonConvert.DeserializeObject<List<Event>>(json));
        }


        int requestID = GetInput();

        while (requestID != -1)
        {
            //Instructions are a bit unclear whether you want to pass the whole url
            //that looks like this: http://localhost:7065/api/GetEvent/100
            //and have me get the id out from the end, so I am assuming that is done 
            //before it gets to my code, otherwise that would be here.

            string result = events.GetEventByID(requestID);

            Console.WriteLine(result);

            requestID = GetInput();
        }
    }
}