using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeCraft.FxCop.MaxCollaborators;
using Microsoft.FxCop.Sdk;
using NUnit.Framework;

namespace CodeCraft.FxCop.Tests
{
    [TestFixture]
    public class MaxCollaboratorsRuleTests
    {
        [TestCase("TooManyCollabs", 1)]
        [TestCase("ThreeCollabs", 0)]
        public void ClassesWithMoreThanThreeCollaboratorsBreakRule(string typeName, int expectedProblemCount)
        {
            MaxCollaboratorsRule rule = new MaxCollaboratorsRule();
            rule.Check(GetTypeToCheck(typeName));
            Assert.That(rule.Problems.Count, Is.EqualTo(expectedProblemCount));
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

    public class TooManyCollabs
    {
        private Collab1 _collab1;
        private Collab2 _collab2;
        private Collab3 _collab3;
        private Collab4 _collab4;
    }

    public class ThreeCollabs
    {
        private Collab1 _collab1;
        private Collab2 _collab2;
        private Collab3 _collab3;
    }

    internal class Collab4
    {
    }

    internal class Collab3
    {
    }

    internal class Collab2
    {
    }

    internal class Collab1
    {
    }
}
