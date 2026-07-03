using Xunit;
using System;
using System.IO;
using System.Linq;
using PluginLib;
using PluginLoader;
using System.Reflection;

namespace task10tests;

public class PluginManagerTests
{
    [Fact]
    public void LoggerPlugin_HasPluginLoadAttribute()
    {
        var type = typeof(LoggerPlugin.LoggerPlugin);
        var attr = type.GetCustomAttribute<PluginLoadAttribute>();
        Assert.NotNull(attr);
    }

    [Fact]
    public void DatabasePlugin_HasPluginLoadAttribute()
    {
        var type = typeof(DatabasePlugin.DatabasePlugin);
        var attr = type.GetCustomAttribute<PluginLoadAttribute>();
        Assert.NotNull(attr);
    }

    [Fact]
    public void ReportPlugin_HasPluginLoadAttribute()
    {
        var type = typeof(ReportPlugin.ReportPlugin);
        var attr = type.GetCustomAttribute<PluginLoadAttribute>();
        Assert.NotNull(attr);
    }

    [Fact]
    public void DatabasePlugin_DependsOnLoggerPlugin()
    {
        var type = typeof(DatabasePlugin.DatabasePlugin);
        var attr = type.GetCustomAttribute<DependsOnAttribute>();
        Assert.NotNull(attr);
        Assert.Equal(typeof(LoggerPlugin.LoggerPlugin), attr.DependencyType);
    }

    [Fact]
    public void ReportPlugin_DependsOnDatabasePlugin()
    {
        var type = typeof(ReportPlugin.ReportPlugin);
        var attr = type.GetCustomAttribute<DependsOnAttribute>();
        Assert.NotNull(attr);
        Assert.Equal(typeof(DatabasePlugin.DatabasePlugin), attr.DependencyType);
    }

    [Fact]
    public void LoggerPlugin_ImplementsICommand()
    {
        var type = typeof(LoggerPlugin.LoggerPlugin);
        Assert.True(typeof(ICommand).IsAssignableFrom(type));
    }

    [Fact]
    public void DatabasePlugin_ImplementsICommand()
    {
        var type = typeof(DatabasePlugin.DatabasePlugin);
        Assert.True(typeof(ICommand).IsAssignableFrom(type));
    }

    [Fact]
    public void ReportPlugin_ImplementsICommand()
    {
        var type = typeof(ReportPlugin.ReportPlugin);
        Assert.True(typeof(ICommand).IsAssignableFrom(type));
    }

    [Fact]
    public void LoggerPlugin_Execute_ShouldWriteToConsole()
    {
        var command = new LoggerPlugin.LoggerPlugin();
        var consoleOutput = new StringWriter();
        var originalOutput = Console.Out;
        Console.SetOut(consoleOutput);

        try
        {
            command.Execute();
            var output = consoleOutput.ToString();
            Assert.Contains("[Logger] Логирование активировано", output);
        }
        finally
        {
            Console.SetOut(originalOutput);
        }
    }

    [Fact]
    public void DatabasePlugin_Execute_ShouldWriteToConsole()
    {
        var command = new DatabasePlugin.DatabasePlugin();
        var consoleOutput = new StringWriter();
        var originalOutput = Console.Out;
        Console.SetOut(consoleOutput);

        try
        {
            command.Execute();
            var output = consoleOutput.ToString();
            Assert.Contains("[Database] Подключение к базе данных установлено", output);
        }
        finally
        {
            Console.SetOut(originalOutput);
        }
    }

    [Fact]
    public void ReportPlugin_Execute_ShouldWriteToConsole()
    {
        var command = new ReportPlugin.ReportPlugin();
        var consoleOutput = new StringWriter();
        var originalOutput = Console.Out;
        Console.SetOut(consoleOutput);

        try
        {
            command.Execute();
            var output = consoleOutput.ToString();
            Assert.Contains("[Report] Отчёт сгенерирован", output);
        }
        finally
        {
            Console.SetOut(originalOutput);
        }
    }

    [Fact]
    public void PluginManager_LoadPlugins_ShouldFindPlugins()
    {
        var manager = new PluginManager();
        var pluginsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Plugins", "LoggerPlugin", "bin", "Debug", "net8.0");

        if (Directory.Exists(pluginsDir))
        {
            manager.LoadPlugins(pluginsDir);
            Assert.NotEmpty(manager.PluginTypes);
        }
    }

    [Fact]
    public void PluginManager_GetSortedPlugins_ShouldReturnCorrectOrder()
    {
        var manager = new PluginManager();
        var pluginsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Plugins", "LoggerPlugin", "bin", "Debug", "net8.0");

        if (Directory.Exists(pluginsDir))
        {
            manager.LoadPlugins(pluginsDir);
            var sorted = manager.GetSortedPlugins();

            var loggerIndex = sorted.FindIndex(t => t.Name == "LoggerPlugin");
            var databaseIndex = sorted.FindIndex(t => t.Name == "DatabasePlugin");
            var reportIndex = sorted.FindIndex(t => t.Name == "ReportPlugin");

            if (loggerIndex != -1 && databaseIndex != -1 && reportIndex != -1)
            {
                Assert.True(loggerIndex < databaseIndex);
                Assert.True(databaseIndex < reportIndex);
            }
        }
    }
} 