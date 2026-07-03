using System;

namespace PluginLib;

public interface ICommand
{
    void Execute();
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class PluginLoadAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class DependsOnAttribute : Attribute
{
    public Type DependencyType { get; }

    public DependsOnAttribute(Type dependencyType)
    {
        DependencyType = dependencyType;
    }
}