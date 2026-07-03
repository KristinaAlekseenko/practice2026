using System;
using PluginLib;

namespace DatabasePlugin;

[PluginLoad]
[DependsOn(typeof(LoggerPlugin.LoggerPlugin))]
public class DatabasePlugin : ICommand
{
    public void Execute()
    {
        Console.WriteLine("[Database] Подключение к базе данных установлено");
    }
}