using CleaningRobot.Enumeration;
using CleaningRobot.Models;
using CleaningRobot.Utils;
using System.Drawing;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace CleaningRobot.Services
{
    public class PerformerService
    {
        // Imported program values
        public FacingPoint Start;
        public CellInfo?[][] RoomMap;
        public Queue<Command> Commands;

        // State management properties
        private bool Stuck = false;
        private int StuckCount = 0;
        private int StuckStep = 0;

        // Robot instance
        private Robot Robot;
      
        public PerformerService(JsonModel item) {
            Robot = Robot.Instance;
            Robot.Init(item.battery, item.start);
            Start = item.start;
            RoomMap = item.map;
            Commands = new Queue<Command>(item.commands);
        }

        public void PerformInstructions()
        {
            while (Commands.Count > 0)
            {
                PerformSingleInstruction();
            }
        }

        public string GetFinalState() => Robot.ToString();

        private void PerformSingleInstruction()
        {
            if (Stuck)
            {
                if(StuckCount == PredefinedSettings.BackOffSequence.Length)
                {
                    Console.WriteLine("Vacuum cleaner was stucked.");
                    Commands.Clear();
                    return;
                } else
                {
                    Console.WriteLine("backoff strategy : " + string.Join(",", PredefinedSettings.BackOffSequence[StuckCount]));
                }
                
            }

            var command = !Stuck ? Commands.Dequeue() : BackoffStrategyComand();
            Robot.DrainBattery(PredefinedSettings.DrainMap[command]);
            Console.WriteLine("command : " + command);

            switch (command)
            {
                case Command.TL:
                case Command.TR:
                    Robot.TurnFacing(command);
                    break;
                case Command.A:
                    MakeMove(false);
                    break;
                case Command.B:
                    MakeMove(true);
                    break;
                case Command.C:
                    Robot.AddCleaned();
                    break;
                default:
                    break;
            }

        }

        private void MakeMove(bool reverse)
        {
            int x = Robot.CurrentPosition.Position.X;
            int y = Robot.CurrentPosition.Position.Y;

            int direction = reverse ? -1 : 1;

            switch (Robot.CurrentPosition.Facing)
            {
                case Facing.E:
                    x = x - direction;
                    break;
                case Facing.N:
                    y = y - direction;
                    break;
                case Facing.W:
                    x = x + direction;
                    break;
                case Facing.S:
                    y = y + direction;
                    break;
            }

            // cleaner next position is set out of map
            if (x < 0 || x > RoomMap.Length || y < 0 || y > RoomMap[x].Length)
            {
                SetStucked();
            }
            else
            {
                // get info about next cell
                CellInfo? cellInfo = RoomMap[y][x];
                if (cellInfo is null or CellInfo.C)
                {
                    SetStucked();
                }
                else
                {
                    // if was stacked remove steps
                    if (!Stuck && StuckStep > 0)
                    {
                        StuckStep = 0;
                    }
                    Robot.ChangePosition(new Point(x, y));
                    Robot.AddVisited(new Point(x, y));
                }
            }
        }

        private void SetStucked()
        {
            // if was stacked and is still stucked, get another sequence
            if (StuckStep > 0)
            {
                StuckCount++;
                StuckStep = 0;
            }

            Stuck = true;
        }

        private Command BackoffStrategyComand()
        {
            Stuck = PredefinedSettings.BackOffSequence[StuckCount].Length - 1 != StuckStep;
            Command command = PredefinedSettings.BackOffSequence[StuckCount][StuckStep];
            StuckStep++;
            return command;
        }
    }
}
