using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using CommandLib;

namespace CommandRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            const string libraryFile = "FileSystemCommands.dll";
            string libraryPath = LocateLibrary(libraryFile);

            if (string.IsNullOrEmpty(libraryPath))
            {
                Console.WriteLine($"Не найден файл динамической библиотеки: {libraryFile}");
                return;
            }

            try
            {
                RunAllCommands(libraryPath);
            }
            catch (Exception error)
            {
                Console.WriteLine($"Ошибка: {error.Message}");
            }
        }

        static string LocateLibrary(string fileName)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string directLocation = Path.Combine(basePath, fileName);
            if (File.Exists(directLocation))
                return directLocation;

            string relativeLocation = Path.Combine(basePath, "..", "..", "..", "..", "FileSystemCommands", "bin", "Debug", "net10.0", fileName);
            if (File.Exists(relativeLocation))
                return relativeLocation;

            return null;
        }

        static void RunAllCommands(string assemblyPath)
        {
            Assembly loadedAssembly = Assembly.LoadFrom(assemblyPath);
            string testDir = PrepareTestDirectory();
            PopulateTestFiles(testDir);

            var availableCommands = DiscoverCommands(loadedAssembly);
            Console.WriteLine($"Найдено команд: {availableCommands.Count}");

            foreach (var cmdType in availableCommands)
            {
                InvokeCommand(cmdType, testDir);
            }

            DestroyTestDirectory(testDir);
        }

        static string PrepareTestDirectory()
        {
            string directory = Path.Combine(Path.GetTempPath(), $"RunnerTestDir_{Guid.NewGuid()}");
            Directory.CreateDirectory(directory);
            Console.WriteLine($"Тестовая директория: {directory}");
            return directory;
        }

        static void PopulateTestFiles(string folder)
        {
            File.WriteAllText(Path.Combine(folder, "document.txt"), "abcdefg");
            File.WriteAllText(Path.Combine(folder, "text.txt"), "Text file");
            Console.WriteLine("Созданы файлы: document.txt, text.txt\n");
        }

        static List<Type> DiscoverCommands(Assembly assembly)
        {
            return assembly.GetTypes().Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract).ToList();
        }

        static void InvokeCommand(Type commandType, string workingDir)
        {
            Console.WriteLine(commandType.Name);
            ICommand cmd = InstantiateCommand(commandType, workingDir);

            if (cmd is not null)
            {
                ExecuteAndPrint(cmd);
            }

            Console.WriteLine();
        }

        static ICommand InstantiateCommand(Type cmdType, string directory)
        {
            try
            {
                if (cmdType.Name == "DirectorySizeCommand")
                {
                    var obj = (ICommand)Activator.CreateInstance(cmdType, new object[] { directory });
                    Console.WriteLine($"Создан экземпляр DirectorySizeCommand с директорией: {directory}");
                    return obj;
                }

                if (cmdType.Name == "FindFilesCommand")
                {
                    var obj = (ICommand)Activator.CreateInstance(cmdType, new object[] { directory, "*.txt" });
                    Console.WriteLine($"Создан экземпляр FindFilesCommand с директорией: {directory} и маской: *.txt");
                    return obj;
                }

                return (ICommand)Activator.CreateInstance(cmdType);
            }
            catch (Exception error)
            {
                Console.WriteLine($"Ошибка: {error.Message}");
                return null;
            }
        }

        static void ExecuteAndPrint(ICommand cmd)
        {
            try
            {
                Console.WriteLine("Выполнение команды:");
                cmd.Execute();
                Console.WriteLine("Команда выполнена.");
            }
            catch (Exception error)
            {
                Console.WriteLine($"Ошибка: {error.Message}");
            }
        }

        static void DestroyTestDirectory(string path)
        {
            try
            {
                Directory.Delete(path, true);
                Console.WriteLine($"Директория удалена: {path}");
            }
            catch (Exception error)
            {
                Console.WriteLine($"Ошибка: {error.Message}");
            }
        }
    }
}
