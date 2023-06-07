using CleaningRobot.Enumeration;
using System.Drawing;

namespace CleaningRobot.Models
{
    public class FacingPoint
    {
        public Point Position { get; set; }
        public Facing Facing { get; set; }

        public FacingPoint() { }

        public FacingPoint(Point position, Facing facing) 
        {
            Facing = facing;
            Position = position;
        }

        public override string ToString()
        {
            return "{ " + $"\"X\" : {Position.X}, \"Y\" : {Position.Y}, \"facing\" : \"{Facing}\"" + "}";
        }
    }
}
