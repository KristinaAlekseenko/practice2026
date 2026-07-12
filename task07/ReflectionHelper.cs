using System;
using System.Reflection;
using System.Linq;

namespace task07
{
    public static class ReflectionHelper
    {
        public static void PrintTypeInfo(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            DisplayNameAttribute classDisplay = type.GetCustomAttribute<DisplayNameAttribute>();
            VersionAttribute classVersion = type.GetCustomAttribute<VersionAttribute>();

            if (classDisplay != null)
            {
                Console.WriteLine($"Отображаемое имя класса: {classDisplay.DisplayName}");
            }

            if (classVersion != null)
            {
                Console.WriteLine($"Версия класса: {classVersion._major}.{classVersion._minor}");
            }

            Console.WriteLine("Свойства:");
            PropertyInfo[] properties = type.GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo prop = properties[i];
                DisplayNameAttribute propAttr = prop.GetCustomAttribute<DisplayNameAttribute>();
                if (propAttr != null)
                {
                    Console.WriteLine($"{prop.Name}: {propAttr.DisplayName}");
                }
            }

            Console.WriteLine("Методы:");
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            MethodInfo[] methods = type.GetMethods(flags);
            foreach (MethodInfo method in methods)
            {
                DisplayNameAttribute methodAttr = method.GetCustomAttribute<DisplayNameAttribute>();
                if (methodAttr != null)
                {
                    Console.WriteLine($"{method.Name}: {methodAttr.DisplayName}");
                }
            }
        }
    }
}