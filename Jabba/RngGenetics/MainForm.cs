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
    public partial class MainForm : Form
    {
        private GeneticSimulationFormPoco _simulation;

        private CancellationTokenSource _source;

        private delegate void SimFinished();

        public MainForm()
        {
            InitializeComponent();
            toolStripStatusLabel1.Text = "";
            _simulation = new GeneticSimulationFormPoco();
        }

        private void buttonRunSimulation_Click(object sender, EventArgs e)
        {
            _source = new CancellationTokenSource();
            BindFormToModel();
            EnableControls(true);            
            toolStripProgressBarMain.Style = ProgressBarStyle.Marquee;
            toolStripStatusLabel1.Text = "Running...";
            var thread = new Thread(RunSimulation);
            thread.IsBackground = true;
            thread.Start();
        }

        private void BindFormToModel()
        {
            _simulation.SpecimensPerGeneration = (int)numericUpDownSpecimensPerGeneration.Value;
            _simulation.NumberOfGenerationsForConvergence = (int)numericUpDownConvergenceAge.Value;
            _simulation.MutationChance = (double)numericUpDownMutationRate.Value;
            _simulation.SpecimensPerTournament = (int)numericUpDownSpecimensPerTournament.Value;
            _simulation.Threads = (int)numericUpDownThreads.Value;
        }

        private void RunSimulation()
        {            
            try
            {
                _simulation.Run(_source.Token);
            }
            finally
            {
                Invoke(new SimFinished(SimulationFinished));
            }
        }

        private void SimulationFinished()
        {
            var best = _simulation.Best;            
            pictureBoxMain.Image = best.GetImage(1);
            best.NameConstants();
            textBoxState.Text = best.GetStateFunctionPretty();
            textBoxOutput.Text = best.GetOutputFunctionPretty();
            textBoxFitness.Text = best.Fitness.ToString("N0");
            dataGridViewAverageFitness.SuspendLayout();
            dataGridViewAverageFitness.Rows.Clear();
            for (int i=0; i < _simulation._allSpecimens.Count; i++)
            {
                dataGridViewAverageFitness.Rows.Add(i, _simulation._allSpecimens[i].Average(x => x.Fitness).ToString("N0"));
            }
            dataGridViewAverageFitness.ResumeLayout();

            textBoxGeneration.Text = best.Generation.ToString();
            toolStripProgressBarMain.Style = ProgressBarStyle.Continuous;
            toolStripProgressBarMain.Value = 0;
            toolStripStatusLabel1.Text = "";
            EnableControls(false);
            _source.Dispose();
        }

        private void EnableControls(bool isRunning)
        {
            buttonRunSimulation.Enabled = !isRunning;
            numericUpDownSpecimensPerTournament.Enabled = !isRunning;
            numericUpDownSpecimensPerGeneration.Enabled = !isRunning;
            numericUpDownMutationRate.Enabled = !isRunning;
            numericUpDownConvergenceAge.Enabled = !isRunning;
            numericUpDownThreads.Enabled = !isRunning;
            buttonStop.Enabled = isRunning;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            _source.Cancel();
        }
    }
}
