using EnderPi.Genetics;
using EnderPi.Genetics.Tree64Rng;
using EnderPi.Random;
using EnderPi.Random.Test;
using EnderPi.System;
using System;
using System.Linq;
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

        private CancellationTokenSource _sourceRngTesting;

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
            foreach(var specimenType in Enum.GetValues(typeof(SpecimenType)))
            {
                comboBoxGeneticType.Items.Add(specimenType);
            }
            comboBoxGeneticType.SelectedIndex = 0;
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
                lock(_padlock)
                {
                    if (_best == null)
                    {
                        _best = _simulation.GetNextBetterRng(null);
                        if (_best != null)
                        {
                            Invoke(new FormDelegate(()=>PopulatePictureBox(_best)));
                        }
                    }
                }
            }
            Interlocked.Exchange(ref _specimensEvaluated, e.SpecimensEvaluated);
        }

        private void PopulatePictureBox(IGeneticSpecimen best)
        {
            try
            {
                pictureBoxMain.Image = GeneticHelper.GetImage(best.GetEngine(), 1);
                textBoxBestDescription.Text = best.GetDescription();
                textBoxGeneration.Text = best.Generation.ToString();
                textBoxFitness.Text = best.Fitness.ToString("N0");
                textBoxOperations.Text = best.Operations.ToString("N0");
                textBoxTestsPassed.Text = best.TestsPassed.ToString("N0");
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
            PopulatePictureBox(_simulation.Best);                        
            dataGridViewAverageFitness.SuspendLayout();
            dataGridViewAverageFitness.Rows.Clear();
            for (int i=0; i < _simulation._allSpecimens.Count; i++)
            {
                dataGridViewAverageFitness.Rows.Add(i, _simulation._allSpecimens[i].Average(x => x.Fitness).ToString("N0"));
            }
            dataGridViewAverageFitness.ResumeLayout();            
            toolStripProgressBarMain.Style = ProgressBarStyle.Continuous;
            toolStripProgressBarMain.Value = 0;
            toolStripStatusLabel1.Text = "";
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
                PopulatePictureBox(_best);
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
            if (tabControl1.SelectedTab == tabPage2)
            {
                dataGridViewLog.SuspendLayout();
                dataGridViewLog.Rows.Clear();
                var logMessages = Logging.GetLogMessages();
                foreach(var logmessage in logMessages)
                {
                    dataGridViewLog.Rows.Add(logmessage.Id, logmessage.TimeStamp.ToShortTimeString(), logmessage.Message);
                }
                dataGridViewLog.ResumeLayout();
            }
        }

        private void buttonStartTesting_Click(object sender, EventArgs e)
        {
            _sourceRngTesting = new CancellationTokenSource();
            DynamicRandomEngine engine = null;
            try
            {
                engine = new DynamicRandomEngine(textBoxStateExpressionRngTesting.Text, textBoxOutputRngTesting.Text);
            }
            catch(Exception ex) 
            {
                MessageBox.Show($"Error compiling expression!  {ex}", "Compilation error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            engine.Seed(1);
            
            //EnableControls(true);
                        
            textBoxFitnessRngTesting.Text = null;
            textBoxTestsPassedRngTesting.Text = null;
            pictureBoxRngTesting.Image = null;
            
            progressBarRngTesting.Style = ProgressBarStyle.Marquee;
            //toolStripStatusLabel1.Text = "Running...";

            long maxFitness = (long)numericUpDownMaxFitnessRngTesting.Value;
            var thread = new Thread(()=>RunRngTest(engine, maxFitness));
            thread.IsBackground = true;
            thread.Start();                        
        }

        private void RunRngTest(IRandomEngine engine, long maxFitness)
        {
            RandomnessTest simulation = null;
            try
            {
                simulation = new RandomnessTest(engine, 1, maxFitness);
                simulation.Start();                
            }
            finally
            {
                Invoke(new FormDelegate(()=>RngTestingFinished(simulation, engine)));
            }
            

        }

        private void RngTestingFinished(RandomnessTest simulation, IRandomEngine engine)
        {
            textBoxFitnessRngTesting.Text = simulation.Iterations.ToString("N0");
            textBoxTestsPassedRngTesting.Text = simulation.TestsPassed.ToString();
            pictureBoxRngTesting.Image = GeneticHelper.GetImage(engine, 1);            
            progressBarRngTesting.Style =  ProgressBarStyle.Blocks;
            textBoxDescriptionRngTesting.Text = simulation.GetFailedTestsDescription();
        }                        
    }
}
