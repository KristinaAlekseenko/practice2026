using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace task05
{
    public class ClassAnalyzer
    {
        private readonly Type _classType;

        public ClassAnalyzer(Type classType)
        {
            _classType = classType ?? throw new ArgumentNullException(nameof(classType), "Type cannot be null");
        }

        public IEnumerable<string> GetPublicMethods()
        {
            var methods = _classType.GetMethods();
            var result = new List<string>();
            foreach (var m in methods)
            {
                result.Add(m.Name);
            }
            return result;
        }

        public IEnumerable<string> GetMethodParams(string methodName)
        {
            if (string.IsNullOrWhiteSpace(methodName))
                return Array.Empty<string>();

            var targetMethod = _classType.GetMethod(methodName);
            if (targetMethod == null)
                return Array.Empty<string>();

            var paramDescriptions = new List<string>();
            foreach (var p in targetMethod.GetParameters())
            {
                paramDescriptions.Add($"{p.ParameterType.Name} {p.Name}");
            }

            var signature = $"{targetMethod.ReturnType.Name} {methodName}({string.Join(", ", paramDescriptions)})";
            return new[] { signature };
        }

        public IEnumerable<string> GetAllFields()
        {
            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            var fields = _classType.GetFields(flags);
            var fieldNames = new string[fields.Length];
            for (int i = 0; i < fields.Length; i++)
            {
                fieldNames[i] = fields[i].Name;
            }
            return fieldNames;
        }

        public IEnumerable<string> GetProperties()
        {
            var props = _classType.GetProperties();
            var names = new List<string>();
            foreach (var prop in props)
            {
                names.Add(prop.Name);
            }
            return names;
        }

        public bool HasAttribute<T>() where T : Attribute
        {
            var attributes = _classType.GetCustomAttributes(typeof(T), true);
            return attributes.Length > 0;
        }
    }
}
