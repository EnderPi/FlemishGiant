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

        private delegate void SimFinished();

        public MainForm()
        {
            InitializeComponent();
            _simulation = new GeneticSimulationFormPoco();
        }

        private void buttonRunSimulation_Click(object sender, EventArgs e)
        {
            toolStripProgressBarMain.Style = ProgressBarStyle.Marquee;
            var thread = new Thread(RunSimulation);
            thread.IsBackground = true;
            thread.Start();
        }

        private void RunSimulation()
        {            
            try
            {
                _simulation.Run();
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
            toolStripProgressBarMain.Style = ProgressBarStyle.Continuous;
            toolStripProgressBarMain.Value = 0;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            var best = _simulation.Best;
            pictureBoxMain.Image = best.GetImage(trackBar1.Value);
        }
    }
}
