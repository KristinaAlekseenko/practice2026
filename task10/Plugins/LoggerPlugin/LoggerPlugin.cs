using System;
using PluginLib;

namespace LoggerPlugin;

[PluginLoad]
public class LoggerPlugin : ICommand
{
    public void Execute()
    {
        Console.WriteLine("[Logger] Логирование активировано");
    }
}