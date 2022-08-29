using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using EnderPi.Genetics.Linear8099;
using EnderPi.Genetics.Linear8099.Commands;

namespace UnitTest.Framework.Genetic
{
    public class Test8099ProgramCleaner
    {
        [Test]
        public void TestFoldMultiply()
        {
            string programText = @"MUL S1, 1234567;
                               MUL S1, 1234567;";
            var program = LinearGeneticHelper.Parse(programText);
            var cleanedProgram = Machine8099ProgramCleaner.FoldProgram(program.ToArray());
            Assert.IsTrue(cleanedProgram.Count == 1);
            var firstCommand = cleanedProgram[0] as MultiplyConstant;
            Assert.IsTrue(firstCommand != null);
            Assert.IsTrue(firstCommand.Constant == (1234567UL*1234567UL));            
        }


        [Test]
        public void TestFoldRol()
        {
            string programText = @"ROL S1, 12;
                               ROL S1, 13;";
            var program = LinearGeneticHelper.Parse(programText);
            var cleanedProgram = Machine8099ProgramCleaner.FoldProgram(program.ToArray());
            Assert.IsTrue(cleanedProgram.Count == 1);
            var firstCommand = cleanedProgram[0] as RotateLeftConstant;
            Assert.IsTrue(firstCommand != null);
            Assert.IsTrue(firstCommand.Constant == 25);
        }
    }
}
