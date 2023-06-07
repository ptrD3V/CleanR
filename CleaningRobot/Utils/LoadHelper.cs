using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleaningRobot.Utils
{
    public static class LoadHelper
    {
        public static JsonModel Load(string path)
        {
            JsonModel result;
            using (StreamReader r = new StreamReader(path))
            {
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                settings.Converters.Add(new JsonObjectConverter());
                string json = r.ReadToEnd();
                result = JsonConvert.DeserializeObject<JsonModel>(json, settings);
            }

            return result;
        }
    }
}
