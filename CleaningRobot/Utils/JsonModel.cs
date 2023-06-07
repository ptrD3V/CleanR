using CleaningRobot.Enumeration;
using CleaningRobot.Models;

namespace CleaningRobot.Utils
{
    public class JsonModel
    {
        public CellInfo?[][] map { get; set; }
        public FacingPoint start { get; set; }
        public Command[] commands { get; set; }
        public int battery { get; set; }
    }
}
