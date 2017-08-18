using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CodeCraft.FxCop.MaxCollaborators;
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
        public void FindsReturnTypeOfMethodCall()
        {
            CheckIfCollaboratorFound("ThisClass", "ReturnType1");
            CheckIfCollaboratorFound("ThisClass", "ReturnType2");
        }

        [Test]
        public void DoesntIncludeItselfAsCollaborator()
        {
            CollaboratorCount collaboratorCount = new CollaboratorCount();
            TypeNode type = GetTypeToCheck("ThisClass");
            Assert.That(!collaboratorCount.GetCollaboratorsFor(type)
                .ToList()
                .Exists(c => c.Name.Name == "ThisClass"));
        }

        [Test]
        public void DoesntIncludeBaseClasses()
        {
            CollaboratorCount collaboratorCount = new CollaboratorCount();
            TypeNode type = GetTypeToCheck("ThisClass");
            Assert.That(!collaboratorCount.GetCollaboratorsFor(type)
                .ToList()
                .Exists(c => c.Name.Name == "BaseClass"));
        }


        private void CheckIfCollaboratorFound(string typeName, string collaboratorTypeName)
        {
            CollaboratorCount collaboratorCount = new CollaboratorCount();
            TypeNode type = GetTypeToCheck(typeName);
            Assert.That(collaboratorCount.GetCollaboratorsFor(type)
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

    internal class ThisClass : BaseClass
    {
        private FieldType _field;
        private ThisClass _self;

        internal void Foo(ParamType param)
        {
            LocalType local = new LocalType();
            CreateReturnType().Foo().Bar();
            base.Fum();
        }

        private ReturnType1 CreateReturnType()
        {
            return new ReturnType1();
        }
    }

    internal class BaseClass
    {
        public void Fum()
        {
        }
    }

    internal class ReturnType1
    {
        public ReturnType2 Foo()
        {
            return new ReturnType2();
        }
    }

    internal class ReturnType2
    {
        public void Bar()
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
