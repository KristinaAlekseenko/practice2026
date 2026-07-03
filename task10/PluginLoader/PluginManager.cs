using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using PluginLib;

namespace PluginLoader;

public class PluginManager
{
    private readonly List<Type> _pluginTypes = new();
    private readonly Dictionary<Type, List<Type>> _dependencies = new();

    public IReadOnlyList<Type> PluginTypes => _pluginTypes.AsReadOnly();

    public void LoadPlugins(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            throw new DirectoryNotFoundException($"Папка не найдена: {directoryPath}");
        }

        var dllFiles = Directory.GetFiles(directoryPath, "*.dll");

        foreach (var dll in dllFiles)
        {
            try
            {
                var assembly = Assembly.LoadFrom(dll);
                foreach (var type in assembly.GetTypes())
                {
                    if (IsPlugin(type))
                    {
                        _pluginTypes.Add(type);
                        _dependencies[type] = GetDependencies(type);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки {dll}: {ex.Message}");
            }
        }
    }

    public List<Type> GetSortedPlugins()
    {
        return TopologicalSort(_pluginTypes, _dependencies);
    }

    public List<ICommand> CreateInstances(List<Type> pluginTypes)
    {
        var instances = new List<ICommand>();
        foreach (var type in pluginTypes)
        {
            try
            {
                var instance = (ICommand)Activator.CreateInstance(type);
                instances.Add(instance);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка создания {type.Name}: {ex.Message}");
            }
        }
        return instances;
    }

    public void ExecuteAll(List<ICommand> commands)
    {
        foreach (var command in commands)
        {
            try
            {
                command.Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка выполнения {command.GetType().Name}: {ex.Message}");
            }
        }
    }

    private bool IsPlugin(Type type)
    {
        return type.IsClass &&
               !type.IsAbstract &&
               typeof(ICommand).IsAssignableFrom(type) &&
               type.GetCustomAttribute<PluginLoadAttribute>() != null;
    }

    private List<Type> GetDependencies(Type type)
    {
        return type.GetCustomAttributes<DependsOnAttribute>()
                   .Select(a => a.DependencyType)
                   .ToList();
    }

    private List<Type> TopologicalSort(List<Type> types, Dictionary<Type, List<Type>> dependencies)
    {
        var result = new List<Type>();
        var visited = new HashSet<Type>();

        void Visit(Type type)
        {
            if (visited.Contains(type)) return;
            visited.Add(type);

            if (dependencies.TryGetValue(type, out var deps))
            {
                foreach (var dep in deps)
                {
                    if (types.Contains(dep))
                    {
                        Visit(dep);
                    }
                }
            }

            result.Add(type);
        }

        foreach (var type in types)
        {
            Visit(type);
        }

        return result;
    }
}