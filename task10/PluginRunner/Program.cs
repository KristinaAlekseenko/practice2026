using System;
using PluginLib;
using PluginLoader;

class Program
{
    static void Main(string[] args)
    {
        string pluginsDir = args.Length > 0 ? args[0] : "./";

        var manager = new PluginManager();

        try
        {
            manager.LoadPlugins(pluginsDir);

            var pluginTypes = manager.PluginTypes;
            if (pluginTypes.Count == 0)
            {
                Console.WriteLine("Плагины не найдены");
                return;
            }

            Console.WriteLine("Найдены плагины:");
            foreach (var p in pluginTypes)
            {
                Console.WriteLine($"  {p.FullName}");
            }
            Console.WriteLine();

            var sortedPlugins = manager.GetSortedPlugins();

            Console.WriteLine("Порядок загрузки:");
            foreach (var p in sortedPlugins)
            {
                Console.WriteLine($"  {p.FullName}");
            }
            Console.WriteLine();

            var instances = manager.CreateInstances(sortedPlugins);
            manager.ExecuteAll(instances);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}