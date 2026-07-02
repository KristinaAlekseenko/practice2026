using System;
using System.Reflection;
using System.Linq;

class MetadataViewer
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Укажите путь к DLL");
            return;
        }

        string dllPath = args[0];

        if (!System.IO.File.Exists(dllPath))
        {
            Console.WriteLine($"Файл не найден: {dllPath}");
            return;
        }

        Assembly assembly = Assembly.LoadFrom(dllPath);

        foreach (Type type in assembly.GetTypes())
        {
            if (type.IsClass && !type.IsAbstract)
            {
                PrintClassInfo(type);
            }
        }
    }

    static void PrintClassInfo(Type type)
    {
        Console.WriteLine($"{type.FullName}");

        var attrs = type.GetCustomAttributes();
        if (attrs.Any())
        {
            Console.WriteLine("Атрибуты:");
            foreach (var attr in attrs)
            {
                Console.WriteLine($"  {attr.GetType().Name}");
            }
        }

        Console.WriteLine("Конструкторы:");
        foreach (var ctor in type.GetConstructors())
        {
            PrintMethodInfo(ctor);
        }

        Console.WriteLine("Методы:");
        foreach (var method in type.GetMethods())
        {
            PrintMethodInfo(method);
        }

        Console.WriteLine();
    }

    static void PrintMethodInfo(MethodBase method)
    {
        Console.WriteLine($"  {method.Name}");

        var parameters = method.GetParameters();
        if (parameters.Length > 0)
        {
            Console.WriteLine("    Параметры:");
            foreach (var param in parameters)
            {
                Console.WriteLine($"      {param.ParameterType.Name} {param.Name}");
            }
        }
    }
}