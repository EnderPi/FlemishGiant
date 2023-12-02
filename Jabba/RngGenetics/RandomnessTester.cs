using EnderPi.Cryptography;
using EnderPi.Genetics;
using EnderPi.Genetics.Linear8099;
using EnderPi.Genetics.Tree64Rng;
using EnderPi.Random;
using EnderPi.Random.Test;
using EnderPi.SystemE;
using EnderPi.Task;
using RngGenetics.Data;
using RngGenetics.Helper;
using RngGenetics.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace RngGenetics
{
    public partial class RandomnessTester : UserControl
    {
        /// <summary>
        /// Cancellation token source for testing.
        /// </summary>
        private CancellationTokenSource _sourceRngTesting;

        /// <summary>
        /// A delegate just used to marshal events to the main form UI thread.
        /// </summary>
        private delegate void FormDelegate();

        public RandomnessTester()
        {
            InitializeComponent();
            comboBoxRngTestingType.Items.AddRange(new object[] { SpecimenType.TreeUnconstrained64, SpecimenType.LinearUnconstrained, SpecimenType.Feistel, SpecimenType.LinearPseudoRandomFunction, SpecimenType.LinearPrfThreeFunction, SpecimenType.Feistel128, SpecimenType.Feistel128Explicit });
            comboBoxRngTestingType.SelectedIndex = 0;
            UIHelperMethods.AddTestsToCheckedListBoxes(checkedListBoxRngTestingTests);
            comboBoxKeyType.Items.AddRange(new object[] { FeistelKeyType.Prime, FeistelKeyType.Hash, FeistelKeyType.Integer });
            comboBoxKeyType.SelectedIndex = 1;



        }

        private void buttonStartTesting_Click(object sender, EventArgs e)
        {
            _sourceRngTesting = new CancellationTokenSource();
            IRandomEngine engine = null;
            try
            {
                engine = GetEngineFromForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error compiling expression!  {ex}", "Compilation error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            textBoxFitnessRngTesting.Text = null;
            textBoxTestsPassedRngTesting.Text = null;
            pictureBoxRngTesting.Image = null;
            textBoxDescriptionRngTesting.Text = null;

            progressBarRngTesting.Style = ProgressBarStyle.Marquee;

            var parameters = GetParametersFromForm();

            var thread = new Thread(() => RunRngTest(engine, parameters));
            thread.IsBackground = true;
            thread.Start();
        }

        private IRandomEngine GetEngineFromForm()
        {
            IRandomEngine engine = null;
            if ((SpecimenType)comboBoxRngTestingType.SelectedItem == SpecimenType.TreeUnconstrained64)
            {
                engine = new DynamicRandomEngine(textBoxStateExpressionRngTesting.Text, textBoxOutputRngTesting.Text);
            }
            else if ((SpecimenType)comboBoxRngTestingType.SelectedItem == SpecimenType.LinearUnconstrained)
            {
                var commands = LinearGeneticHelper.Parse(textBoxStateExpressionRngTesting.Text);
                engine = new LinearGeneticEngine(commands);
            }
            else if ((SpecimenType)comboBoxRngTestingType.SelectedItem == SpecimenType.Feistel)
            {
                int rounds = (int)numericUpDownFeistelRounds.Value;
                uint[] keys = GetKeys(rounds, (FeistelKeyType)comboBoxKeyType.SelectedItem);
                engine = new Feistel64Engine(textBoxStateExpressionRngTesting.Text, rounds, keys);
            }
            else if ((SpecimenType)comboBoxRngTestingType.SelectedItem == SpecimenType.LinearPseudoRandomFunction)
            {
                var commands = LinearGeneticHelper.Parse(textBoxStateExpressionRngTesting.Text);
                engine = new LinearRandomFunctionEngine(commands);
            }
            else if ((SpecimenType)comboBoxRngTestingType.SelectedItem == SpecimenType.LinearPrfThreeFunction)
            {
                var commands = LinearGeneticHelper.Parse(textBoxStateExpressionRngTesting.Text);
                engine = new LinearPrfThreeFunctionEngine(commands);
            }
            else if ((SpecimenType)comboBoxRngTestingType.SelectedItem == SpecimenType.Feistel128)
            {
                int rounds = (int)numericUpDownFeistelRounds.Value;
                var commands = LinearGeneticHelper.Parse(textBoxStateExpressionRngTesting.Text);
                engine = new Feistel128Engine(rounds, commands);
            }
            else if ((SpecimenType)comboBoxRngTestingType.SelectedItem == SpecimenType.Feistel128Explicit)
            {
                int rounds = (int)numericUpDownFeistelRounds.Value;
                int rotate = int.Parse(textBoxStateExpressionRngTesting.Text);
                engine = new RandomFeistel128(rounds, rotate);
            }

            return engine;
        }
        
        private List<IIncrementalRandomTest> GetRandomnessTestsFromForm()
        {
            var tests = new List<IIncrementalRandomTest>();
            foreach (var t in checkedListBoxRngTestingTests.CheckedItems)
            {
                var test = t as IIncrementalRandomTest;
                tests.Add(test);
            }
            return tests;
        }

        private void RunRngTest(IRandomEngine engine, RandomTestParameters parameters)
        {
            RandomnessTest simulation = null;
            try
            {
                simulation = new RandomnessTest(engine, _sourceRngTesting.Token, parameters);
                if (parameters.TestAsHash)
                {
                    Invoke(new FormDelegate(() => pictureBoxRngTesting.Image = GeneticHelper.GetImage(new HashWrapper(engine.DeepCopy()), 1)));
                }
                else
                {
                    Invoke(new FormDelegate(() => pictureBoxRngTesting.Image = GeneticHelper.GetImage(engine.DeepCopy(), 1)));
                }
                simulation.CheckpointPassed += RngCheckpointPassed;
                simulation.Start();
            }
            catch (Exception ex)
            {
                Logging.LogError(ex.ToString());
            }
            finally
            {
                try
                {
                    Invoke(new FormDelegate(() => RngTestingFinished(simulation, engine)));
                }
                catch (Exception ex)
                {
                    Logging.LogError(ex.ToString());
                }
            }


        }

        private RandomTestParameters GetParametersFromForm()
        {
            long maxFitness = (long)numericUpDownMaxFitnessRngTesting.Value;
            var seed = (ulong)numericUpDownSeed.Value;
            var testAsHash = checkBoxRngTestAsHash.Checked;
            var tests = GetRandomnessTestsFromForm();
            var parameters = new RandomTestParameters() { MaxFitness = maxFitness, Seed = seed, TestAsHash = testAsHash, Tests = tests };
            return parameters;
        }

        private uint[] GetKeys(int rounds, FeistelKeyType selectedItem)
        {
            uint[] result = new uint[rounds];
            if (selectedItem == FeistelKeyType.Hash)
            {
                var hash = new EnderPi.Random.RandomNumberGenerator(new RandomHash());
                hash.Seed(0);
                for (int i = 0; i < rounds; i++)
                {
                    result[i] = hash.Nextuint();
                }
            }
            else if (selectedItem == FeistelKeyType.Prime)
            {
                for (int i = 0; i < rounds; i++)
                {
                    result[i] = Primes.FirstPrimes[i];
                }
            }
            else if (selectedItem == FeistelKeyType.Integer)
            {
                for (int i = 0; i < rounds; i++)
                {
                    result[i] = (uint)(i + 1);
                }
            }
            return result;
        }

        private void RngCheckpointPassed(object sender, RandomnessTestEventArgs e)
        {
            Invoke(new FormDelegate(() => RngTestingCheckpoint(e)));
        }

        private void RngTestingCheckpoint(RandomnessTestEventArgs e)
        {
            textBoxFitnessRngTesting.Text = e.Iterations.ToString("N0");
        }

        private void RngTestingFinished(RandomnessTest simulation, IRandomEngine engine)
        {
            textBoxFitnessRngTesting.Text = simulation.Iterations.ToString("N0");
            textBoxTestsPassedRngTesting.Text = simulation.TestsPassed.ToString();
            progressBarRngTesting.Style = ProgressBarStyle.Blocks;
            textBoxDescriptionRngTesting.Text = simulation.GetFailedTestsDescription();
        }

        private void comboBoxRngTestingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((SpecimenType)comboBoxRngTestingType.SelectedItem)
            {
                case SpecimenType.TreeUnconstrained64:
                    labelFieldTwo.Visible = true;
                    textBoxOutputRngTesting.Visible = true;
                    labelFieldOne.Text = "State Function Expression";
                    break;
                case SpecimenType.LinearUnconstrained:
                    labelFieldTwo.Visible = false;
                    textBoxOutputRngTesting.Visible = false;
                    labelFieldOne.Text = "Generation Program";
                    break;
                case SpecimenType.Feistel:
                    labelFieldTwo.Visible = false;
                    textBoxOutputRngTesting.Visible = false;
                    labelFieldOne.Text = "Round Function";
                    break;
            }
        }
        private void buttonStopTesting_Click(object sender, EventArgs e)
        {
            if (_sourceRngTesting != null && !_sourceRngTesting.IsCancellationRequested)
            {
                _sourceRngTesting.Cancel();
            }
        }
        
        private void RunLoopTest()
        {
            string program = textBoxStateExpressionRngTesting.Text;
            long maxFitness = (long)numericUpDownMaxFitnessRngTesting.Value;
            var parameters = GetParametersFromForm();
            var sb = new StringBuilder();
            try
            {
                for (int i = 1; i < 64; i++)
                {
                    var stringthisProgram = program.Replace("qqq", i.ToString());
                    var commands = LinearGeneticHelper.Parse(stringthisProgram);
                    var engine = new LinearRandomFunctionEngine(commands);
                    RandomnessTest simulation = null;

                    simulation = new RandomnessTest(engine, _sourceRngTesting.Token, parameters);
                    simulation.CheckpointPassed += RngCheckpointPassed;
                    simulation.Start();
                    sb.AppendLine($"Rotate {i}, Fitness {simulation.Iterations}");
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex.ToString());
            }
            finally
            {
                try
                {
                    Invoke(new FormDelegate(() => RngLoopTestingFinished(sb)));
                }
                catch (Exception ex)
                {
                    Logging.LogError(ex.ToString());
                }
            }
        }

        private void RngLoopTestingFinished(StringBuilder sb)
        {
            textBoxFitnessRngTesting.Text = "";
            textBoxTestsPassedRngTesting.Text = "";
            progressBarRngTesting.Style = ProgressBarStyle.Blocks;
            textBoxDescriptionRngTesting.Text = sb.ToString();
        }

        private void buttonAddTestingTask_Click(object sender, EventArgs e)
        {
            //create a testing task, push to queue.
            var task = new BackgroundTask();
            //***********************            




            var backgroundRandomnessTask = new BackgroundRandomnessTask();
            backgroundRandomnessTask.Engine = GetEngineFromForm();
            //***********************
            using (var context = new EFCoreTestContext())
            {
                context.BackgroundTasks.Add(task);
                context.SaveChanges();
            }
            //notify user task added

        }


    }
}
