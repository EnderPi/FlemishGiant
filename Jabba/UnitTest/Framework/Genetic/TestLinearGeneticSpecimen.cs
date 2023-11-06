using EnderPi.Genetics;
using EnderPi.Genetics.Linear8099;
using EnderPi.Genetics.Tree64Rng.Nodes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.Framework.Genetic
{
    public class TestLinearGeneticSpecimen
    {
        [Test]
        public void TestValid()
        {
            string programText = @"MUL S1, 1234567;
                               MUL S1, 1234567;
                               LOP 2, 3;
                               XOR OP, S1;";
            var program = LinearGeneticHelper.Parse(programText);
            var cleanedProgram = Machine8099ProgramCleaner.FoldProgram(program.ToArray());
            var specimen = new LinearRngSpecimen(cleanedProgram);
            Assert.IsTrue(specimen.IsValid(new GeneticParameters(), out string errors));
        }

        [Test]
        public void TestValid2()
        {
            string programText = @"MUL S1, 1234567;
                               MUL S1, 1234567;
                               LOP 2, 3;
                               MUL S1, 1234567;
                               LOP 1, 2;                                
                               XOR OP, S1;";
            var program = LinearGeneticHelper.Parse(programText);
            var cleanedProgram = Machine8099ProgramCleaner.FoldProgram(program.ToArray());
            var specimen = new LinearRngSpecimen(cleanedProgram);
            Assert.IsTrue(specimen.IsValid(new GeneticParameters(), out string errors));
        }

        [Test]
        public void TestValid3()
        {
            string programText = @"MUL S1, 1234567;
                               MUL S1, 1234567;
                               LOP 2, 3;
                               MUL S1, 1234567;
                               LOP 2, 2;                                
                               XOR OP, S1;";
            var program = LinearGeneticHelper.Parse(programText);
            var cleanedProgram = Machine8099ProgramCleaner.FoldProgram(program.ToArray());
            var specimen = new LinearRngSpecimen(cleanedProgram);
            Assert.IsFalse(specimen.IsValid(new GeneticParameters(), out string errors));
        }

    }
}
