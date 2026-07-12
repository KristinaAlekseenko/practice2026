using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

namespace task07
{
    [DisplayName("Тест")]
    [Version(2, 6)]
    public class SampleTestClass
    {
        public string Data { get; set; }

        public SampleTestClass(string initialData, int count)
        {
            Data = initialData;
        }

        [DisplayName("Метод обработки файлов")]
        public bool ProcessFile(string path, int mode)
        {
            return true;
        }
    }

    class Program
    {
        static void DisplayParameters(ParameterInfo[] parameters)
        {
            for (int idx = 0; idx < parameters.Length; idx++)
            {
                Console.Write($"{parameters[idx].ParameterType.Name} {parameters[idx].Name}");
                if (idx < parameters.Length - 1) Console.Write(", ");
            }
        }

        static void Main(string[] args)
        {
            string assemblyPath = args.Length > 0 ? args[0] : Assembly.GetExecutingAssembly().Location;

            if (!File.Exists(assemblyPath))
            {
                Console.WriteLine($"[ОШИБКА] Файл не найден: {assemblyPath}");
                return;
            }

            try
            {
                Assembly loadedAssembly = Assembly.LoadFrom(assemblyPath);
                Type[] loadedTypes;

                try
                {
                    loadedTypes = loadedAssembly.GetTypes();
                }
                catch (ReflectionTypeLoadException loadError)
                {
                    Console.WriteLine($"[ОШИБКА] Проблема загрузки типов: {loadError.Message}");
                    Console.WriteLine($"[ИНФО] Загружено типов: {loadError.Types.Length}, с ошибками: {loadError.LoaderExceptions.Length}");

                    loadedTypes = loadError.Types;

                    for (int i = 0; i < loadError.LoaderExceptions.Length; i++)
                    {
                        Console.WriteLine($"  > Ошибка #{i + 1}: {loadError.LoaderExceptions[i]?.Message}");
                    }

                    if (loadedTypes == null || loadedTypes.Length == 0)
                    {
                        Console.WriteLine("[ОШИБКА] Не загружено ни одного типа.");
                        return;
                    }
                }


                foreach (Type currentType in loadedTypes)
                {
                    if (currentType == null) continue;
                    if (!currentType.IsClass) continue;

                    Console.WriteLine($"Тип: {currentType.FullName}");
                    Console.WriteLine(new string('-', 40));

                    DisplayClassAttributes(currentType);
                    DisplayConstructors(currentType);
                    DisplayMethods(currentType);

                    Console.WriteLine();
                }
            }
            catch (Exception error)
            {
                Console.WriteLine($"[КРИТИЧЕСКАЯ ОШИБКА] {error.Message}");
            }
        }

        static void DisplayClassAttributes(Type classType)
        {
            var attributes = classType.GetCustomAttributes();
            bool hasAttributes = false;

            foreach (var attribute in attributes)
            {
                if (attribute.GetType().Name.StartsWith("Nullable"))
                    continue;

                if (!hasAttributes)
                {
                    Console.WriteLine("  Атрибуты класса:");
                    hasAttributes = true;
                }

                if (attribute is DisplayNameAttribute displayAttr)
                    Console.WriteLine($"    * DisplayName: \"{displayAttr.DisplayName}\"");
                else if (attribute is VersionAttribute versionAttr)
                    Console.WriteLine($"    * Version: v{versionAttr._major}.{versionAttr._minor}");
                else
                    Console.WriteLine($"    * {attribute.GetType().Name}");
            }
        }

        static void DisplayConstructors(Type classType)
        {
            ConstructorInfo[] ctors = classType.GetConstructors();

            if (ctors.Length > 0)
            {
                Console.WriteLine($"  Конструкторы ({ctors.Length}):");
                foreach (var constructor in ctors)
                {
                    Console.Write($"    - {classType.Name}(");
                    ParameterInfo[] ctorParams = constructor.GetParameters();
                    DisplayParameters(ctorParams);
                    Console.WriteLine(")");
                }
            }
        }

        static void DisplayMethods(Type classType)
        {
            BindingFlags searchFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodInfo[] classMethods = classType.GetMethods(searchFlags);

            List<MethodInfo> regularMethods = new List<MethodInfo>();
            foreach (var method in classMethods)
            {
                if (!method.IsSpecialName)
                    regularMethods.Add(method);
            }

            if (regularMethods.Count > 0)
            {
                Console.WriteLine($"  Методы ({regularMethods.Count}):");
                foreach (var method in regularMethods)
                {
                    Console.Write($"    - {method.ReturnType.Name} {method.Name}(");
                    ParameterInfo[] methodParams = method.GetParameters();
                    DisplayParameters(methodParams);
                    Console.WriteLine(")");

                    var methodAttributes = method.GetCustomAttributes();
                    foreach (var attr in methodAttributes)
                    {
                        if (attr is DisplayNameAttribute displayAttr)
                            Console.WriteLine($"      Атрибут: DisplayName = \"{displayAttr.DisplayName}\"");
                    }
                }
            }
        }
    }
}
