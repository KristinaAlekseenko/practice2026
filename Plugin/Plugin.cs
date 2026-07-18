using CommandLib;
using System;

namespace Plugin
{
    [PluginLoad("")]
    public class AnalyticsCommand : ICommand
    {
        public void Execute()
        {
            Console.WriteLine("Тестовый плагин.");
        }

    }

}