using CleaningRobot.Enumeration;
using CleaningRobot.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing;

namespace CleaningRobot.Utils
{
    public class JsonObjectConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(JsonModel));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);

            try
            {
                // null string resolver
                var map = new List<List<CellInfo?>>();
                var _map = jo["map"];
                foreach (JArray arr in _map)
                {
                    var subList = new List<CellInfo?>();
                    foreach (JValue value in arr)
                    {
                        var val = ((string)value == "null") ? null : value;
                        subList.Add(val != null ? val.ToObject<CellInfo?>() : null);
                    }
                    map.Add(subList);
                }

                var x = (int?)jo["start"]?["X"];
                var y = (int?)jo["start"]?["Y"];
                var facing = jo["start"]?["facing"]?.ToObject<Facing>();

                var start = (x != null && y != null && facing != null) ? new FacingPoint(new Point((int)x, (int)y), (Facing)facing) : new FacingPoint();
                var commands = jo["commands"]?.ToObject<Command[]>();
                var battery = (int?)jo["battery"];

                JsonModel model = new JsonModel();
                model.start = start;
                model.commands = commands;
                model.battery = (battery != null) ? (int)battery : 0;
                model.map = map.Select(m => m.ToArray()).ToArray();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
