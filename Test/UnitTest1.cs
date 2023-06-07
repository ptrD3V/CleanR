using Newtonsoft.Json.Linq;
using CleaningRobot.Enumeration;
using System.IO;

namespace Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var jsonFiles = Directory.EnumerateFiles("TestCases", "*.json");

            foreach (string currentFile in jsonFiles)
            {
                using (StreamReader r = new StreamReader(currentFile))
                {
                    string json = r.ReadToEnd();
                    JObject jo = JObject.Parse(json);
                    var map = jo["map"];
                    var start = jo["start"];
                    var commands = jo["commands"];
                    var battery = jo["battery"];

                    TestMap(map);
                    TestStart(start);
                    TestCommands(commands);
                    TestBattery(battery);
                }
            }
           
        }

        private void TestBattery(JToken battery)
        {
            Assert.NotNull(battery);

            var batteryValue = (int?)battery;

            Assert.NotNull(batteryValue);
            Assert.True(batteryValue > 0);
        }

        private void TestCommands(JToken commands)
        {
            Assert.NotNull(commands);

            foreach (JValue value in commands)
            {
                Assert.True(Enum.IsDefined(typeof(Command), value.ToString()));
            }
        }

        private void TestMap(JToken map)
        {
            Assert.NotNull(map);

            foreach (JArray arr in map)
            {
                foreach (JValue value in arr)
                {
                    // null string is supported
                    if(value.ToString() != "null")
                    {
                        Assert.True(Enum.IsDefined(typeof(CellInfo), value.ToString()));
                    }
                }
            }
        }

        private void TestStart(JToken start)
        {
            Assert.NotNull(start);

            var x = (int?)start?["X"];
            var y = (int?)start?["Y"];
            var facing = start?["facing"]?.ToObject<Facing>();

            Assert.NotNull(facing);
            Assert.NotNull(x);
            Assert.NotNull(y);
        }
    }
}