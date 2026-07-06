using System;
using System.IO;
using System.Reflection;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Загрузка команд из DLL ===\n");

        string dllPath = Path.Combine(Directory.GetCurrentDirectory(), "FileSystemCommands.dll");

        if (!File.Exists(dllPath))
        {
            Console.WriteLine($"ОШИБКА: Файл {dllPath} не найден.");
            Console.WriteLine("Соберите проект FileSystemCommands.");
            return;
        }

        try
        {
            var assembly = Assembly.LoadFrom(dllPath);
            var commandTypes = assembly.GetTypes()
                .Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .ToList();

            if (commandTypes.Count == 0)
            {
                Console.WriteLine("Команды не найдены.");
                return;
            }

            foreach (var type in commandTypes)
            {
                try
                {
                    Console.WriteLine($"Команда: {type.Name}");
                    object command = null;

                    if (type.Name == "DirectorySizeCommand")
                    {
                        Console.Write("Введите путь: ");
                        string path = Console.ReadLine()?.Trim();

                        if (string.IsNullOrEmpty(path))
                        {
                            Console.WriteLine("ОШИБКА: Путь не может быть пустым.");
                            continue;
                        }

                        command = Activator.CreateInstance(type, path);
                    }
                    else if (type.Name == "FindFilesCommand")
                    {
                        Console.Write("Введите путь: ");
                        string path = Console.ReadLine()?.Trim();

                        if (string.IsNullOrEmpty(path))
                        {
                            Console.WriteLine("ОШИБКА: Путь не может быть пустым.");
                            continue;
                        }

                        Console.Write("Введите маску: ");
                        string mask = Console.ReadLine()?.Trim();

                        if (string.IsNullOrEmpty(mask))
                        {
                            Console.WriteLine("ОШИБКА: Маска не может быть пустой.");
                            continue;
                        }

                        command = Activator.CreateInstance(type, path, mask);
                    }

                    if (command != null)
                    {
                        ((ICommand)command).Execute();
                    }
                    else
                    {
                        Console.WriteLine($"ОШИБКА: Не удалось создать {type.Name}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ОШИБКА: {ex.Message}");
                }

                Console.WriteLine();
            }
        }
        catch (FileLoadException ex)
        {
            Console.WriteLine($"ОШИБКА загрузки DLL: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ОШИБКА: {ex.Message}");
        }
    }
}