﻿using CodeCraft.FxCop.LargeClass;
using NUnit.Framework;

namespace CodeCraft.FxCop.Tests
{
    [TestFixture]
    public class LargeClassTests
    {
        [TestCase("ClassWithEightMethods", 0)]
        [TestCase("ClassWithNineMethods", 1)]
        [TestCase("LargeClassWithProperties", 1)]
        [TestCase("LargeInterface", 1)]
        public void TypesWithMoreThanTenMethodsOrPropertiesBreakRule(string typeName, int expectedProblemCount)
        {
            var rule = new LargeClassRule();
            rule.Check(AssemblyReader.GetType(typeName));
            Assert.AreEqual(expectedProblemCount, rule.Problems.Count);
        }
    }

    internal class ClassWithEightMethods
    {
        private void Foo1()
        {
        }

        private void Foo2()
        {
        }

        private void Foo3()
        {
        }

        private void Foo4()
        {
        }

        private void Foo5()
        {
        }

        private void Foo6()
        {
        }

        private void Foo7()
        {
        }

        private void Foo8()
        {
        }
    }

    internal class ClassWithNineMethods
    {
        private void Foo1()
        {
        }

        private void Foo2()
        {
        }

        private void Foo3()
        {
        }

        private void Foo4()
        {
        }

        private void Foo5()
        {
        }

        private void Foo6()
        {
        }

        private void Foo7()
        {
        }

        private void Foo8()
        {
        }

        private void Foo9()
        {
        }
    }

    internal class LargeClassWithProperties
    {
        private int Foo1
        {
            get { return 0; }
        }

        private int Foo2
        {
            get { return 0; }
        }

        private void Foo3()
        {
        }

        private void Foo4()
        {
        }

        private void Foo5()
        {
        }

        private void Foo6()
        {
        }

        private void Foo7()
        {
        }

        private void Foo8()
        {
        }

        private void Foo9()
        {
        }
    }

    internal interface LargeInterface
    {
        int Foo1 { get; }
        int Foo2 { get; }
        void Foo3();
        void Foo4();
        void Foo5();
        void Foo6();
        void Foo7();
        void Foo8();
        void Foo9();
    }
}