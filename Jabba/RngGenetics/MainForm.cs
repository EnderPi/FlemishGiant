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
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace RngGenetics
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// The whole model.
        /// </summary>
        private GeneticSimulationFormPoco _simulation;

        /// <summary>
        /// Handle to a cancellation source.
        /// </summary>
        private CancellationTokenSource _source;

        //private CancellationTokenSource _sourceRngTesting;

        /// <summary>
        /// A delegate just used to marshal events to the main form UI thread.
        /// </summary>
        private delegate void FormDelegate();

        /// <summary>
        /// The number of specimens evaluated.
        /// </summary>
        private long _specimensEvaluated;

        /// <summary>
        /// Current generation.
        /// </summary>
        private long _generation;

        private IGeneticSpecimen _best;

        public object _padlock = new object();

        public MainForm()
        {
            InitializeComponent();
            toolStripStatusLabel1.Text = "";
            _simulation = new GeneticSimulationFormPoco();
            _simulation.SpecimenEvaluated += SpecimenEvaluated;
            _simulation.GenerationFinished += GenerationFinished;
            foreach (var specimenType in Enum.GetValues(typeof(SpecimenType)))
            {
                comboBoxGeneticType.Items.Add(specimenType);
            }
            comboBoxGeneticType.SelectedIndex = 2;            
            comboBoxGeneticFeistelType.Items.AddRange(new object[] { FeistelKeyType.Prime, FeistelKeyType.Hash, FeistelKeyType.Integer });
            comboBoxGeneticFeistelType.SelectedIndex = 1;            
            for (int i = 0; i < checkedListBoxOperations.Items.Count; i++)
            {
                if (i != 3 && i != 4)
                {
                    checkedListBoxOperations.SetItemChecked(i, true);
                }
            }
            UIHelperMethods.AddTestsToCheckedListBoxes(checkedListBoxGeneticTests);            
        }
                
        /// <summary>
        /// Syncing this form with the model.  Probably not great, probably just need to read the model?
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerationFinished(object sender, SimulationEventArgs e)
        {
            Interlocked.Exchange(ref _generation, e.Generation);
        }

        /// <summary>
        /// Syncing the form with the model.  Probably not great, probably just need to read the model?
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpecimenEvaluated(object sender, SimulationEventArgs e)
        {
            if (_best == null)
            {
                lock (_padlock)
                {
                    if (_best == null)
                    {
                        _best = _simulation.GetNextBetterRng(null);
                        if (_best != null)
                        {
                            Invoke(new FormDelegate(() => PopulatePictureBox()));
                        }
                    }
                }
            }
            Interlocked.Exchange(ref _specimensEvaluated, e.SpecimensEvaluated);
        }

        private void PopulatePictureBox()
        {
            try
            {
                if (_simulation.TestAsHash)
                {
                    pictureBoxMain.Image = GeneticHelper.GetImage(new HashWrapper(_best.GetEngine()), 1);
                }
                else
                {
                    pictureBoxMain.Image = GeneticHelper.GetImage(_best.GetEngine(), 1);
                }
                textBoxBestDescription.Text = _best.GetDescription();
                textBoxGeneration.Text = _best.Generation.ToString();
                textBoxFitness.Text = _best.Fitness.ToString("N0");
                textBoxOperations.Text = _best.Operations.ToString("N0");
                textBoxTestsPassed.Text = _best.TestsPassed.ToString("N0");
            }
            catch (Exception ex)
            {
                Logging.LogError(ex.ToString());
            }
        }

        private void buttonRunSimulation_Click(object sender, EventArgs e)
        {
            _source = new CancellationTokenSource();
            _best = null;
            _specimensEvaluated = 0;
            _generation = 0;

            BindFormToModel();
            EnableControls(true);

            textBoxGeneration.Text = null;
            textBoxFitness.Text = null;
            pictureBoxMain.Image = null;
            textBoxBestDescription.Text = null;
            textBoxOperations.Text = null;
            textBoxTestsPassed.Text = null;
            dataGridViewAverageFitness.Rows.Clear();
            toolStripProgressBarMain.Style = ProgressBarStyle.Marquee;
            toolStripStatusLabel1.Text = "Running...";

            var thread = new Thread(RunSimulation);
            thread.IsBackground = true;
            thread.Start();

            timerUpdateUI.Enabled = true;
            timerUpdateVisual.Enabled = true;
        }

        /// <summary>
        /// Binds entry data from the form to the model.
        /// </summary>
        private void BindFormToModel()
        {
            _simulation.SpecimensPerGeneration = (int)numericUpDownSpecimensPerGeneration.Value;
            _simulation.NumberOfGenerationsForConvergence = (int)numericUpDownConvergenceAge.Value;
            _simulation.MutationChance = (double)numericUpDownMutationRate.Value;
            _simulation.SpecimensPerTournament = (int)numericUpDownSpecimensPerTournament.Value;
            _simulation.Threads = (int)numericUpDownThreads.Value;
            _simulation.MaxFitness = (long)numericUpDownMaxFitness.Value;
            _simulation.StateOneConstraint = textBoxStateOneFunction.Text;
            _simulation.RngSpecimenType = (SpecimenType)comboBoxGeneticType.SelectedItem;
            _simulation.SelectionPressure = (double)numericUpDownSelectionPressure.Value;
            _simulation.TestAsHash = checkBoxTestAsHash.Checked;
            _simulation.RngTests = new List<IIncrementalRandomTest>();
            foreach (var t in checkedListBoxGeneticTests.CheckedItems)
            {
                var test = t as IIncrementalRandomTest;
                _simulation.RngTests.Add(test);
            }
            var parameters = new GeneticParameters();
            parameters.AllowAdditionNodes = checkedListBoxOperations.GetItemChecked(0);
            parameters.AllowSubtractionNodes = checkedListBoxOperations.GetItemChecked(1);
            parameters.AllowMultiplicationNodes = checkedListBoxOperations.GetItemChecked(2);
            parameters.AllowDivisionNodes = checkedListBoxOperations.GetItemChecked(3);
            parameters.AllowRemainderNodes = checkedListBoxOperations.GetItemChecked(4);
            parameters.AllowRightShiftNodes = checkedListBoxOperations.GetItemChecked(5);
            parameters.AllowLeftShiftNodes = checkedListBoxOperations.GetItemChecked(6);
            parameters.AllowRotateRightNodes = checkedListBoxOperations.GetItemChecked(7);
            parameters.AllowRotateLeftNodes = checkedListBoxOperations.GetItemChecked(8);
            parameters.AllowAndNodes = checkedListBoxOperations.GetItemChecked(9);
            parameters.AllowOrNodes = checkedListBoxOperations.GetItemChecked(10);
            parameters.AllowXorNodes = checkedListBoxOperations.GetItemChecked(11);
            parameters.AllowNotNodes = checkedListBoxOperations.GetItemChecked(12);
            parameters.AllowXorShiftRightNodes = checkedListBoxOperations.GetItemChecked(13);
            parameters.AllowRotateMultiplyNodes = checkedListBoxOperations.GetItemChecked(14);
            parameters.AllowLoopNodes = checkedListBoxOperations.GetItemChecked(15);
            parameters.InitialNodes = (int)numericUpDownInitialAdds.Value;
            parameters.FeistelRounds = (int)numericUpDownGeneticFeistelRounds.Value;
            parameters.KeyTypeForFeistel = (FeistelKeyType)comboBoxGeneticFeistelType.SelectedItem;
            _simulation.SimulationParameters = parameters;
        }

        private void RunSimulation()
        {
            try
            {
                _simulation.Run(_source.Token);
            }
            finally
            {
                Invoke(new FormDelegate(SimulationFinished));
            }
        }

        private void SimulationFinished()
        {
            timerUpdateUI.Enabled = false;
            timerUpdateVisual.Enabled = false;
            _best = _simulation.Best;
            PopulatePictureBox();
            dataGridViewAverageFitness.SuspendLayout();
            dataGridViewAverageFitness.Rows.Clear();
            for (int i = _simulation._allSpecimens.Count - 1; i >= 0; i--)
            {
                dataGridViewAverageFitness.Rows.Add(i, _simulation._allSpecimens[i].Average(x => x.Fitness).ToString("N0"));
            }
            dataGridViewAverageFitness.ResumeLayout();
            toolStripProgressBarMain.Style = ProgressBarStyle.Continuous;
            toolStripProgressBarMain.Value = 0;
            toolStripStatusLabel1.Text = "";
            textBoxFailures.Text = _simulation.GetFailureOccurences();
            EnableControls(false);
            _source.Dispose();
        }

        /// <summary>
        /// Standard form method to enable or disable controls.
        /// </summary>
        /// <param name="isRunning"></param>
        private void EnableControls(bool isRunning)
        {
            buttonRunSimulation.Enabled = !isRunning;
            numericUpDownSpecimensPerTournament.Enabled = !isRunning;
            numericUpDownSpecimensPerGeneration.Enabled = !isRunning;
            numericUpDownMutationRate.Enabled = !isRunning;
            numericUpDownConvergenceAge.Enabled = !isRunning;
            numericUpDownThreads.Enabled = !isRunning;
            buttonStop.Enabled = isRunning;
            numericUpDownMaxFitness.Enabled = !isRunning;
            comboBoxGeneticType.Enabled = !isRunning;
            textBoxStateOneFunction.Enabled = !isRunning && comboBoxGeneticType.SelectedItem is SpecimenType.TreeStateConstrained64;
            numericUpDownSelectionPressure.Enabled = !isRunning;
            numericUpDownInitialAdds.Enabled = !isRunning;
            numericUpDownGeneticFeistelRounds.Enabled = !isRunning;
            comboBoxGeneticFeistelType.Enabled = !isRunning;
        }

        /// <summary>
        /// Cancels the source.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (_source != null && !_source.IsCancellationRequested)
            {
                _source.Cancel();
            }
        }

        /// <summary>
        /// Timer event to update the UI on total specimens generated and current generation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerUpdateUI_Tick(object sender, EventArgs e)
        {
            var specimens = Interlocked.Read(ref _specimensEvaluated);
            var currentGeneration = Interlocked.Read(ref _generation);
            textBoxSpecimensEvaluated.Text = specimens.ToString("N0");
            textBoxCurrentGeneration.Text = currentGeneration.ToString("N0");
        }

        /// <summary>
        /// Update the UI with the next specimen.  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerUpdateVisual_Tick(object sender, EventArgs e)
        {
            var result = _simulation.GetNextBetterRng(_best);
            if (result != null)
            {
                _best = result;
                PopulatePictureBox();
            }
            lock (_simulation._specimensPadlock)
            {
                dataGridViewAverageFitness.SuspendLayout();
                dataGridViewAverageFitness.Rows.Clear();
                for (int i = _simulation._allSpecimens.Count - 1; i >= 0; i--)
                {
                    dataGridViewAverageFitness.Rows.Add(i, _simulation._allSpecimens[i].Average(x => x.Fitness).ToString("N0"));
                }
                dataGridViewAverageFitness.ResumeLayout();
                textBoxFailures.Text = _simulation.GetFailureOccurences();
            }
        }

        private void comboBoxGeneticType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxGeneticType.SelectedItem)
            {
                case SpecimenType.TreeUnconstrained64:
                    textBoxStateOneFunction.Enabled = false;
                    break;
                case SpecimenType.TreeStateConstrained64:
                    textBoxStateOneFunction.Enabled = true;
                    break;
            }
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (tabControlMain.SelectedTab == tabPage2)
            {
                PopulateLogs();
            }
        }

        private void PopulateLogs()
        {
            dataGridViewLog.SuspendLayout();
            dataGridViewLog.Rows.Clear();
            var logMessages = Logging.GetLogMessages();
            foreach (var logmessage in logMessages)
            {
                dataGridViewLog.Rows.Add(logmessage.Id, logmessage.TimeStamp.ToShortTimeString(), logmessage.Message);
            }
            dataGridViewLog.ResumeLayout();
        }
        
        //private void buttonPushToTesting_Click(object sender, EventArgs e)
        //{
        //    lock (_padlock)
        //    {
        //        if (_best is Tree64RngSpecimen bestTree)
        //        {
        //            comboBoxRngTestingType.SelectedIndex = 0;
        //            textBoxStateExpressionRngTesting.Text = bestTree.StateRoot.Evaluate();
        //            textBoxOutputRngTesting.Text = bestTree.OutputRoot.Evaluate();
        //            tabControlMain.SelectedTab = tabPage3;
        //        }
        //        if (_best is LinearRngSpecimen bestLinear)
        //        {
        //            comboBoxRngTestingType.SelectedIndex = 1;
        //            textBoxStateExpressionRngTesting.Text = LinearGeneticHelper.PrintProgram(bestLinear.GetGenerationProgram());
        //            tabControlMain.SelectedTab = tabPage3;
        //        }
        //    }
        //}

        private void buttonRefreshLogs_Click(object sender, EventArgs e)
        {
            PopulateLogs();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //ok, so we need differential uniformity of a by one-shifted s-box, a bunch of "random" derangements, and then maybe some specific, like mul xsr, etc
            Func<ulong, ulong> func = (ulong x) =>
            {
                ulong[] constants = new ulong[] { 15794028255413092319, 18442280127147387751, 12729309401716732454, 7115307147812511645, 5302139897775218427, 14101262115410449445, 12208502135646234746, 18372937549027959272, 4458685334906369756, 3585144267928700831 };
                ulong y = 1234567;
                for (int i = 0; i < 7; i++)
                {
                    y ^= constants[i];
                    x ^= y;
                    x = 4236690171739396961 * BitOperations.RotateLeft(x, 11);
                }
                return x;
            };
            var prf = new ActualPrf(123);
            Func<ulong, ulong> function = (ulong x) =>
            {
                var sha = SHA256.Create();
                var bytes = BitConverter.GetBytes(x);
                var result = sha.ComputeHash(bytes);
                var answer = BitConverter.ToUInt64(result, 0);
                return answer;
            };

            //var result = BitHelper.CharacterizeSbox(function);
            var line = BitHelper.GetLinearity10(x => prf.F(x), out double a);
            var du = BitHelper.GetDu10(x => prf.F(x), out double b);
            var sb = new StringBuilder();
            sb.AppendLine($"Linearity - {line}");
            sb.AppendLine($"Differential Uniformity - {du}");
            //sb.AppendLine("Characterization of one-way compression 64-bit sbox");
            //sb.Append(result.ToString());
            MessageBox.Show(sb.ToString());



        }


        private void button2_Click_old(object sender, EventArgs e)
        {
            //ok, so we need differential uniformity of a by one-shifted s-box, a bunch of "random" derangements, and then maybe some specific, like mul xsr, etc
            #region test
            var shiftedSbox = new byte[256];
            for (int i = 0; i < shiftedSbox.Length; i++)
            {
                shiftedSbox[i] = (byte)((i + 1) & 255);
            }
            var shiftedUniformity = BitHelper.GetDifferentialUniformity(shiftedSbox, out var d);
            var linearityShifted = BitHelper.GetLinearity(shiftedSbox, out var f);
            var shuffledSbox = new byte[256];
            for (int i = 0; i < shuffledSbox.Length; i++)
            {
                shuffledSbox[i] = (byte)i;
            }
            var ran = new RandomHash();
            var q = new EnderPi.Random.RandomNumberGenerator(ran);
            q.SeedRandom();
            int shuffledUniformity = 0;
            int shuffledLinearity = 0;
            for (int i = 0; i < 20; i++)
            {
                q.Shuffle(shuffledSbox);
                shuffledUniformity += BitHelper.GetDifferentialUniformity(shuffledSbox, out double ave);
                shuffledLinearity += BitHelper.GetLinearity(shuffledSbox, out double ave2);
            }
            shuffledUniformity /= 20;
            shuffledLinearity /= 20;
            MessageBox.Show($"Shuffled Linearity = {shuffledLinearity}\nShuffled DU = {shuffledUniformity}");
            var rin = BitHelper.GetDifferentialUniformity(BitHelper._SBox, out var b);
            var linearityrin = BitHelper.GetLinearity(BitHelper._SBox, out var c);

            var romuSBox = new byte[256];
            for (int i = 0; i < shiftedSbox.Length; i++)
            {
                int temp = i;
                for (int j = 0; j < 5; j++)
                {
                    temp = (temp << 3) | (temp >> 5);
                    temp &= 255;
                    temp = temp * 197;
                    temp = temp & 255;
                    temp ^= i;
                }
                romuSBox[i] = (byte)(temp);
            }
            var romuUniformity = BitHelper.GetDifferentialUniformity(romuSBox, out var a);
            var romuLinear = BitHelper.GetLinearity(romuSBox, out var h);
            MessageBox.Show($"Differential Uniformity of shifted box is {shiftedUniformity}\nDifferential Uniformity of shuffled box is {shuffledUniformity}\nDifferential Uniformity of rijndael box is {rin}\nDifferential Uniformity of romu 3,17 box is {romuUniformity}\nLinearity of Romu 3,17 box is {romuLinear}\nLinearity of shifted S-Box is {linearityShifted}\nLinearity of rijndael S-box is {linearityrin}");
            #endregion



            //var sb = new StringBuilder();
            //sb.AppendLine("Rounds,Differential Uniformity,Average DU, Linearity,Average Linearity (10 bit)");
            //for (int i = 1; i < 16; i++)
            //{
            //    int[] SBox = GetSBoxFeistel10(i);
            //    var diff = BitHelper.GetDifferentialUniformity10(SBox, out double aveDiff);
            //    var linearity = BitHelper.GetLinearity10(SBox, out double aveLin);
            //    sb.AppendLine($"{i},{diff},{aveDiff},{linearity},{aveLin}");
            //}
            //using (var dlg = new SaveFileDialog())
            //{
            //    if (dlg.ShowDialog() == DialogResult.OK)
            //    {
            //        File.WriteAllText(dlg.FileName, sb.ToString());
            //    }
            //}


            //var multiplySBox = new byte[256];
            //for (int i = 0; i < 256; i++)
            //{
            //    int q = 7;
            //    for (int j = 0; j < 1; j++)
            //    {
            //        q ^= (byte)i;
            //        q = (byte)((q << 3) | (q >> 5));
            //        q = (byte)(q * 113);                    
            //    }
            //    multiplySBox[i] = (byte)(q);
            //}
            //var linear = BitHelper.GetLinearity(multiplySBox, out double ave);
            //var dif = BitHelper.GetDifferentialUniformity(multiplySBox, out ave);
            //MessageBox.Show($"Linearity of multiplying by 113, rot 2, lop 3 is {linear}\nDifferential Uniformity is {dif}");



        }

        private byte[] GetSBox(int mask, int rotate, int multiply)
        {
            var result = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                var val = mask;
                for (int j = 0; j < 17; j++)
                {
                    val ^= i;
                    val = (val << rotate) ^ (val >> (8 - rotate));
                    val &= 255;
                    val *= multiply;
                    val &= 255;
                }
                result[i] = (byte)val;
            }
            return result;

        }

        private byte[] GetSBoxFeistel(int rounds)
        {
            var result = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                int _state = i;
                int right = _state & 63;
                int left = _state >> 4;
                int temp;
                for (int j = 0; j < rounds; j++)
                {
                    temp = right;
                    right = left ^ RoundFunction(right, j + 1);
                    left = temp;
                }
                result[i] = (byte)(((left) << 4) | right);
            }
            return result;

        }

        private int[] GetSBoxFeistel10(int rounds)
        {
            var result = new int[1024];
            for (int i = 0; i < 1024; i++)
            {
                int _state = i;
                int right = _state & 127;
                int left = _state >> 5;
                int temp;
                for (int j = 0; j < rounds; j++)
                {
                    temp = right;
                    right = left ^ RoundFunction10(right, j + 1);
                    left = temp;
                }
                result[i] = (byte)(((left) << 5) | right);
            }
            return result;

        }

        public int RoundFunction(int val, int key)
        {
            //k + x + rotate(x,x,)
            int returnValue = (val + key) & 63;
            int rotated = (val << (val & 3)) | (val >> (4 - (val & 3)));
            rotated = rotated & 63;
            returnValue = returnValue + rotated;
            returnValue &= 63;
            return returnValue;
        }

        public int RoundFunction10(int val, int key)
        {
            //k + x + rotate(x,x,)
            int returnValue = (val + key) & 127;
            int rotated = (val << (val % 5)) | (val >> (5 - (val % 5)));
            rotated = rotated & 127;
            returnValue = returnValue + rotated;
            returnValue &= 127;
            return returnValue;
        }

        private void buttonSmallGenerator_Click(object sender, EventArgs e)
        {
            var generator = new SmallArxaGenerator() { AdditiveConstant = Convert.ToUInt16(numericUpDownAdditiveConstant.Value) };
            var sb = new StringBuilder();
            sb.AppendLine($"AdditiveConstant {generator.AdditiveConstant}");
            sb.AppendLine($"Rotate, Xsr, Period,MinimumCount,MaximumCount,AverageCount");
            for (int rotate = 1; rotate < 16; rotate++)
            {
                for (int xsr = 1; xsr < 16; xsr++)
                {
                    generator.XsrConstant = xsr;
                    generator.RotateConstant = rotate;
                    generator.Reset();
                    long[] frequency = new long[65536];
                    ulong counter = 0;
                    do
                    {
                        frequency[generator.Next()]++;
                        counter++;
                    } while (generator.StateTwo != 0 || generator.StateOne != 0);
                    long max = frequency.Max();
                    long min = frequency.Min();
                    double average = frequency.Average();
                    sb.AppendLine($"{rotate},{xsr},{counter},{min},{max},{average}");
                }
            }
            using (var dlg = new SaveFileDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(dlg.FileName, sb.ToString());
                }
            }
        }
        
        private void buttonFourBitSBoxes_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            byte[] sBox = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                sBox[i] = (byte)i;
            }
            var rng = new EnderLcg();
            var generator = new EnderPi.Random.RandomNumberGenerator(rng);
            generator.SeedRandom();
            sb.AppendLine("sbox,DU,ave,lin,ave (lower better)");
            for (int j = 0; j < 1000; j++)
            {
                generator.Shuffle(sBox);
                double averagedu, averagelin;
                var du = BitHelper.GetDifferentialUniformityFourBit(sBox, out averagedu);
                var lin = BitHelper.GetLinearityFourBit(sBox, out averagelin);
                var sb2 = new StringBuilder();
                for (int i = 0; i < sBox.Length; i++)
                {
                    char val = (char)(sBox[i] + (int)'A');
                    sb2.Append(val);
                }
                var sBoxObject = new SboxFourBits() { Sbox = sb2.ToString(), DifferentialUniformity = du, Linearity = lin };
                using (var context = new EFCoreTestContext())
                {
                    context.Add(sBoxObject);
                    context.SaveChanges();
                }                
            }            
        }

        private void buttonGet8BitFeistelStrength_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            sb.AppendLine("rounds,DU,ave,lin,ave (lower better)");
            for (int j = 0; j < 16; j++)
            {
                var sBox = BitHelper.Get8BitSBoxFrom4Bit(j);
                double averagedu, averagelin;
                var du = BitHelper.GetDifferentialUniformity(sBox, out averagedu);
                var lin = BitHelper.GetLinearity(sBox, out averagelin);
                sb.AppendLine($"{j},{du},{averagedu},{lin},{averagelin}");
            }
            using (var dlg = new SaveFileDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(dlg.FileName, sb.ToString());
                }
            }
        }
        
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlMain.SelectedIndex == 3)
            {
                using (var context = new EFCoreTestContext())
                {
                    var bestSBoxes = context.Sboxes.OrderBy(x => x.DifferentialUniformity).ThenBy(y => y.Linearity).Take(10).ToList();
                    dataGridViewSboxes.Rows.Clear();
                    dataGridViewSboxes.SuspendLayout();
                    foreach (var item in bestSBoxes)
                    {
                        dataGridViewSboxes.Rows.Add(item.Id, item.Sbox, item.DifferentialUniformity, item.Linearity);
                    }
                    dataGridViewSboxes.ResumeLayout();
                }
            }
        }

        
    }
}
