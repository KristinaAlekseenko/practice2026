using System;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.Linq;

namespace task11;

public static class CalculatorGenerator
{
    public static object CreateCalculator()
    {
        string code = @"
using System;

public class Calculator
{
    public int Add(int a, int b) => a + b;
    public int Minus(int a, int b) => a - b;
    public int Mul(int a, int b) => a * b;
    public int Div(int a, int b) => a / b;
}";

        var syntaxTree = CSharpSyntaxTree.ParseText(code);

        var references = new List<MetadataReference>();
        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (!asm.IsDynamic && !string.IsNullOrEmpty(asm.Location))
            {
                references.Add(MetadataReference.CreateFromFile(asm.Location));
            }
        }

        var compilation = CSharpCompilation.Create(
            "DynamicCalculator",
            new[] { syntaxTree },
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        using var ms = new System.IO.MemoryStream();
        var result = compilation.Emit(ms);

        if (!result.Success)
        {
            foreach (var diagnostic in result.Diagnostics)
            {
                Console.WriteLine($"Ошибка: {diagnostic.GetMessage()}");
            }
            return null;
        }

        ms.Seek(0, System.IO.SeekOrigin.Begin);
        var loadedAssembly = Assembly.Load(ms.ToArray());

        Type calcType = loadedAssembly.GetType("Calculator");
        return Activator.CreateInstance(calcType);
    }
}

public class CalculatorWrapper
{
    private readonly object _calculator;
    private readonly Type _type;

    public CalculatorWrapper(object calculator)
    {
        _calculator = calculator;
        _type = calculator.GetType();
    }

    public int Add(int a, int b)
    {
        return (int)_type.GetMethod("Add").Invoke(_calculator, new object[] { a, b });
    }

    public int Minus(int a, int b)
    {
        return (int)_type.GetMethod("Minus").Invoke(_calculator, new object[] { a, b });
    }

    public int Mul(int a, int b)
    {
        return (int)_type.GetMethod("Mul").Invoke(_calculator, new object[] { a, b });
    }

    public int Div(int a, int b)
    {
        try
        {
            return (int)_type.GetMethod("Div").Invoke(_calculator, new object[] { a, b });
        }
        catch (System.Reflection.TargetInvocationException ex)
        {
            if (ex.InnerException is System.DivideByZeroException)
                throw ex.InnerException;
            throw;
        }
    }
}