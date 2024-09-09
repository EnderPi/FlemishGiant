using EnderPi.Genetics;
using EnderPi.Random;
using EnderPi.Random.Test;
using EnderPi.SystemE;
using RngGenetics.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RngGenetics
{
    public partial class GeneticSimulationPanel : UserControl
    {
        /// <summary>
        /// A delegate just used to marshal events to the main form UI thread.
        /// </summary>
        private delegate void FormDelegate();

        /// <summary>
        /// The whole model.
        /// </summary>
        private GeneticSimulationFormPoco _simulation;

        /// <summary>
        /// Handle to a cancellation source.
        /// </summary>
        private CancellationTokenSource _source;

        /// <summary>
        /// The number of specimens evaluated.
        /// </summary>
        private long _specimensEvaluated;

        /// <summary>
        /// Current generation.
        /// </summary>
        private long _generation;

        /// <summary>
        /// The current best specimen.
        /// </summary>
        private IGeneticSpecimen _best;

        /// <summary>
        /// For syncing with the background.
        /// </summary>
        public object _padlock = new object();
        public GeneticSimulationPanel()
        {
            InitializeComponent();
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

        /// <summary>
        /// Draws the image of the best in the picture box.
        /// </summary>
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

        /// <summary>
        /// Starts the simulation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            //toolStripProgressBarMain.Style = ProgressBarStyle.Marquee;
            //toolStripStatusLabel1.Text = "Running...";

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
            _simulation.RngTests = [];
            foreach (var t in checkedListBoxGeneticTests.CheckedItems)
            {
                var test = t as IIncrementalRandomTest;
                _simulation.RngTests.Add(test);
            }
            var parameters = new GeneticParameters
            {
                AllowAdditionNodes = checkedListBoxOperations.GetItemChecked(0),
                AllowSubtractionNodes = checkedListBoxOperations.GetItemChecked(1),
                AllowMultiplicationNodes = checkedListBoxOperations.GetItemChecked(2),
                AllowDivisionNodes = checkedListBoxOperations.GetItemChecked(3),
                AllowRemainderNodes = checkedListBoxOperations.GetItemChecked(4),
                AllowRightShiftNodes = checkedListBoxOperations.GetItemChecked(5),
                AllowLeftShiftNodes = checkedListBoxOperations.GetItemChecked(6),
                AllowRotateRightNodes = checkedListBoxOperations.GetItemChecked(7),
                AllowRotateLeftNodes = checkedListBoxOperations.GetItemChecked(8),
                AllowAndNodes = checkedListBoxOperations.GetItemChecked(9),
                AllowOrNodes = checkedListBoxOperations.GetItemChecked(10),
                AllowXorNodes = checkedListBoxOperations.GetItemChecked(11),
                AllowNotNodes = checkedListBoxOperations.GetItemChecked(12),
                AllowXorShiftRightNodes = checkedListBoxOperations.GetItemChecked(13),
                AllowRotateMultiplyNodes = checkedListBoxOperations.GetItemChecked(14),
                AllowLoopNodes = checkedListBoxOperations.GetItemChecked(15),
                InitialNodes = (int)numericUpDownInitialAdds.Value,
                FeistelRounds = (int)numericUpDownGeneticFeistelRounds.Value,
                KeyTypeForFeistel = (FeistelKeyType)comboBoxGeneticFeistelType.SelectedItem
            };
            _simulation.SimulationParameters = parameters;
        }

        /// <summary>
        /// This simulation runs the thread in the background.
        /// </summary>
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

        /// <summary>
        /// Cleanup that runs when the simulation finishes.
        /// </summary>
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
            //toolStripProgressBarMain.Style = ProgressBarStyle.Continuous;
            //toolStripProgressBarMain.Value = 0;
            //toolStripStatusLabel1.Text = "";
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
        private void TimerUpdateVisual_Tick(object sender, EventArgs e)
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

        private void ComboBoxGeneticType_SelectedIndexChanged(object sender, EventArgs e)
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

    }
}
