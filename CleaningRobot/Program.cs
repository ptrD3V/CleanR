using CleaningRobot.Models;
using CleaningRobot.Services;
using CleaningRobot.Utils;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;

internal class Program
{
    private static void Main(string[] args)
    {
        PerformerService service = new PerformerService(LoadHelper.Load(args[0]));
        service.PerformInstructions();
        File.WriteAllText(args[1], service.GetFinalState());
    }
}