using CleaningRobot.Enumeration;

namespace CleaningRobot.Utils
{
    public static class PredefinedSettings
    {
        public static readonly Dictionary<Command, int> DrainMap = new Dictionary<Command, int>()
        {
            { Command.TR, 1 },
            { Command.TL, 1 },
            { Command.C, 5 },
            { Command.A, 2 },
            { Command.B, 3 }
        };

        public static readonly Command[][] BackOffSequence = new Command[][] {
            new Command[] {Command.TR, Command.A, Command.TL},
            new Command[] {Command.TR, Command.A, Command.TR},
            new Command[] {Command.TR, Command.A, Command.TR},
            new Command[] {Command.TR, Command.B, Command.TR, Command.A},
            new Command[] {Command.TL, Command.TL, Command.A}
        };
    }
}
