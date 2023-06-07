using CleaningRobot.Enumeration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace CleaningRobot.Models
{
    public sealed class Robot
    {
        private static readonly Robot instance = new Robot();
        public FacingPoint? CurrentPosition { get; private set; }
        public ISet<Point> Visited { get; set; }
        public ISet<Point> Cleaned { get; set; }
        private int battery;

        static Robot()
        {
        }

        private Robot()
        {
            Visited = new HashSet<Point>();
            Cleaned = new HashSet<Point>();
        }

        public static Robot Instance
        {
            get
            {
                return instance;
            }
        }

        public void Init(int battery, FacingPoint currPosition)
        {
            this.battery = battery;
            CurrentPosition = currPosition;
            AddVisited(CurrentPosition.Position);
        }

        public void DrainBattery(int value) => battery = this.battery - value;

        public void TurnFacing(Command command)
        {
            Facing f = CurrentPosition.Facing;
            int facingValues = Enum.GetNames(typeof(Facing)).Length;

            if (command.Equals(Command.TL))
            {
                f = (int)f != 0 ? (Facing)(int)f - 1 : (Facing)(int)facingValues - 1;
            }
            else if (command.Equals(Command.TR))
            {
                f = (int)f != facingValues - 1 ? (Facing)(int)f + 1 : 0;
            }

            CurrentPosition.Facing = f;
        }

        public void ChangePosition(Point position) => CurrentPosition.Position = position;

        public void AddCleaned() => Cleaned.Add(CurrentPosition.Position);

        public void AddVisited(Point position) => Visited.Add(position);

        public override string ToString()
        {
            return "{" +
                "\n\"visited\" : " + "[" + listToString(Visited) + "]" +
                ",\n\"cleaned\" : " + "[" + listToString(Cleaned) + "]" +
                ",\n\"final\" : " + CurrentPosition.ToString() +
                ",\n\"battery\" : " + battery +
            "\n}";
        }

        private string listToString(ISet<Point> set)
        {
            var list = new List<Point>(set);
            list.Reverse();
            return string.Join("",list.Select((item) => "{ \"X\" : " + item.X + ", \"Y\" : " + item.Y + " }").ToList());
        }
    }
}
