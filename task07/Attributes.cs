using System;
using System.Reflection;

namespace task07;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
public class DisplayNameAttribute : Attribute
{
    public string DisplayName { get; }

    public DisplayNameAttribute(string displayName)
    {
        DisplayName = displayName;
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class VersionAttribute : Attribute
{
    public int Major { get; }
    public int Minor { get; }

    public VersionAttribute(int major, int minor)
    {
        Major = major;
        Minor = minor;
    }
}

[DisplayName("Пример класса")]
[Version(1, 0)]
public class SampleClass
{
    [DisplayName("Числовое свойство")]
    public int Number { get; set; }

    [DisplayName("Тестовый метод")]
    public void TestMethod() { }

    public void AnotherMethod() { }
}

public static class ReflectionHelper
{
    public static void PrintTypeInfo(Type type)
    {
        var displayNameAttr = type.GetCustomAttribute<DisplayNameAttribute>();
        if (displayNameAttr != null)
        {
            Console.WriteLine($"Отображаемое имя класса: {displayNameAttr.DisplayName}");
        }

        var versionAttr = type.GetCustomAttribute<VersionAttribute>();
        if (versionAttr != null)
        {
            Console.WriteLine($"Версия класса: {versionAttr.Major}.{versionAttr.Minor}");
        }

        Console.WriteLine("\nМетоды:");
        foreach (var method in type.GetMethods())
        {
            var attr = method.GetCustomAttribute<DisplayNameAttribute>();
            if (attr != null)
            {
                Console.WriteLine($"  {attr.DisplayName} ({method.Name})");
            }
        }

        Console.WriteLine("\nСвойства:");
        foreach (var prop in type.GetProperties())
        {
            var attr = prop.GetCustomAttribute<DisplayNameAttribute>();
            if (attr != null)
            {
                Console.WriteLine($"  {attr.DisplayName} ({prop.Name})");
            }
        }
    }
}