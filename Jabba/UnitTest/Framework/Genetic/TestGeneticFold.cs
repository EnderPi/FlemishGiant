using System;
using System.Collections.Generic;
using System.Text;
using EnderPi.Genetics.Tree64Rng.Nodes;
using NUnit.Framework;

namespace UnitTest.Framework.Genetic
{
    public class TestGeneticFold
    {
        [Test]
        public void TestFold()
        {
            var root = new IntronNode(new AdditionNode(new StateOneNode(), new ConstantNode(0)));
            Assert.IsTrue(root.GetOperationCount() == 1);
            root.Fold();
            Assert.IsTrue(root.GetOperationCount() == 0);
        }

        [Test]
        public void TestFold3()
        {
            var root = new IntronNode(new AdditionNode(new ConstantNode(123), new ConstantNode(0)));
            Assert.IsTrue(root.GetOperationCount() == 1);
            root.Fold();
            Assert.IsTrue(root.GetOperationCount() == 0);
        }

        [Test]
        public void TestFold4()
        {
            var root = new IntronNode(new AdditionNode(new StateOneNode(), new SubtractNode(new StateOneNode(), new StateOneNode())));
            Assert.IsTrue(root.GetOperationCount() == 2);
            root.Fold();
            Assert.IsTrue(root.GetOperationCount() == 0);
        }


        [Test]
        public void TestFold2()
        {
            var root = new IntronNode(new AndNode(new StateOneNode(), new StateOneNode()));
            Assert.IsTrue(root.GetOperationCount() == 1);
            root.Fold();
            Assert.IsTrue(root.GetOperationCount() == 0);
        }




    }
}
