using System;
using System.IO;
using System.Reflection;
using CommandLib;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Динамическая загрузка команд\n");

        string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileSystemCommands.dll");

        if (!File.Exists(dllPath))
        {
            Console.WriteLine($"Файл {dllPath} не найден.");
            Console.WriteLine("Сначала соберите проект FileSystemCommands.");
            return;
        }

        Console.WriteLine($"Загружаем библиотеку: {dllPath}\n");

        Assembly assembly = Assembly.LoadFrom(dllPath);

        var dirSizeType = assembly.GetType("FileSystemCommands.DirectorySizeCommand");
        if (dirSizeType != null)
        {
            Console.WriteLine("DirectorySizeCommand");
            string path = args.Length > 0 ? args[0] : ".";
            var command = (ICommand)Activator.CreateInstance(dirSizeType, path);
            command.Execute();
            Console.WriteLine();
        }

        var findFilesType = assembly.GetType("FileSystemCommands.FindFilesCommand");
        if (findFilesType != null)
        {
            Console.WriteLine("FindFilesCommand");
            string path = args.Length > 0 ? args[0] : ".";
            string mask = args.Length > 1 ? args[1] : "*.*";
            var command = (ICommand)Activator.CreateInstance(findFilesType, path, mask);
            command.Execute();
            Console.WriteLine();
        }
    }
}