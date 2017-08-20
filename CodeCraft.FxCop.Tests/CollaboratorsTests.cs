using System.Linq;
using CodeCraft.FxCop.MaxCollaborators;
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
            var collaboratorCount = new CollaboratorCount();
            var type = AssemblyReader.GetType("ThisClass");
            Assert.That(!collaboratorCount.GetCollaboratorsFor(type)
                .ToList()
                .Exists(c => c.Name.Name == "ThisClass"));
        }

        [Test]
        public void DoesntIncludeBaseClasses()
        {
            var collaboratorCount = new CollaboratorCount();
            var type = AssemblyReader.GetType("ThisClass");
            Assert.That(!collaboratorCount.GetCollaboratorsFor(type)
                .ToList()
                .Exists(c => c.Name.Name == "BaseClass"));
        }

        private void CheckIfCollaboratorFound(string typeName, string collaboratorTypeName)
        {
            var collaboratorCount = new CollaboratorCount();
            var type = AssemblyReader.GetType(typeName);
            Assert.That(collaboratorCount.GetCollaboratorsFor(type)
                .ToList()
                .Exists(c => c.Name.Name == collaboratorTypeName));
        }
    }

    internal class ThisClass : BaseClass
    {
        private FieldType _field;
        private ThisClass _self;

        internal void Foo(ParamType param)
        {
            var local = new LocalType();
            CreateReturnType().Foo().Bar();
            Fum();
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