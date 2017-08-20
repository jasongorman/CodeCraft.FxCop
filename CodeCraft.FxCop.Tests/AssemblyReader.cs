using System;
using System.Linq;
using System.Reflection;
using Microsoft.FxCop.Sdk;

namespace CodeCraft.FxCop.Tests
{
    public class AssemblyReader
    {
        private static AssemblyNode _assemblyNode;

        public static AssemblyNode GetAssembly()
        {
            if (_assemblyNode == null)
            {
                _assemblyNode = AssemblyNode.GetAssembly(Assembly.GetExecutingAssembly().Location, true, true, true);
            }
            return _assemblyNode;
        }

        public static TypeNode GetType(string typeName)
        {
            return GetAssembly().Types.FirstOrDefault(t => t.Name.Name == typeName);
        }

        public static TypeNode GetType(Type type)
        {
            return GetAssembly().GetType(Identifier.For(type.Namespace), Identifier.For(type.Name));
        }

        public static Method GetMethodByName(Type declaringType, string methodName)
        {
            return (Method) GetType(declaringType).Members.FirstOrDefault(member => member.Name.Name == methodName);
        }
    }
}