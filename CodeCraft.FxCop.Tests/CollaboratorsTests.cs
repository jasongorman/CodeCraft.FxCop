using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace CodeCraft.FxCop.Tests
{
    [TestFixture]
    public class CollaboratorsTests
    {
        [Test]
        public void FindsFieldCollaborator()
        {
            CheckIfCollaboratorFound("ThisClass", "FieldType");
        }

        [Test]
        public void FindsParameterCollaborator()
        {
            CheckIfCollaboratorFound("ThisClass", "ParamType");
        }

        [Test]
        public void FindsLocalCollaborator()
        {
            CheckIfCollaboratorFound("ThisClass", "LocalType");
        }

        [Test]
        public void FindsReturnTypeOfInternalMethodCall()
        {
            CheckIfCollaboratorFound("ThisClass", "ReturnType");
        }

        private void CheckIfCollaboratorFound(string typeName, string collaboratorTypeName)
        {
            Collaborators collaborators = new Collaborators();
            TypeNode type = GetTypeToCheck(typeName);
            Assert.That(collaborators.GetCollaboratorsFor(type)
                .ToList()
                .Exists(c => c.Name.Name == collaboratorTypeName));
        }

        private TypeNode GetTypeToCheck(string typeName)
        {
            AssemblyNode assembly = AssemblyNode.GetAssembly(this.GetType().Module.Assembly.Location, true, true, true);
            TypeNode typeNode = GetTypeByName(assembly, typeName);
            return typeNode;
        }

        private TypeNode GetTypeByName(AssemblyNode assembly, string typeName)
        {
            return assembly.Types.FirstOrDefault(t => t.Name.Name == typeName);
        }
    }

    internal class ThisClass
    {
        private FieldType _field;

        internal void Foo(ParamType param)
        {
            LocalType local = new LocalType();
            CreateReturnType().Foo();
        }

        private ReturnType CreateReturnType()
        {
            return new ReturnType();
        }
    }

    internal class ReturnType
    {
        public void Foo()
        {
        }
    }

    internal class LocalType
    {
    }

    internal class ParamType
    {
    }

    internal class FieldType
    {
    }
}
