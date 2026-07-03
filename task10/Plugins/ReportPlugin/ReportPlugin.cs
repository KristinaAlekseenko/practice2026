using System;
using PluginLib;

namespace ReportPlugin;

[PluginLoad]
[DependsOn(typeof(DatabasePlugin.DatabasePlugin))]
public class ReportPlugin : ICommand
{
    public void Execute()
    {
        Console.WriteLine("[Report] Отчёт сгенерирован");
    }
}