using EnderPi.Cryptography;
using EnderPi.Genetics;
using EnderPi.Random;
using EnderPi.Random.Test;
using EnderPi.SystemE;
using RngGenetics.Data;
using RngGenetics.Helper;
using RngGenetics.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace RngGenetics
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// A delegate just used to marshal events to the main form UI thread.
        /// </summary>
        private delegate void FormDelegate();

        /// <summary>
        /// Constructor, basic setup.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            toolStripStatusLabel1.Text = "";
        }

        /// <summary>
        /// Populates the log table.
        /// </summary>
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

        /// <summary>
        /// Refreshes the logs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlMain.SelectedTab == tabPageLogging)
            {
                PopulateLogs();
            }
            if (tabControlMain.SelectedTab == tabPageSBoxes)
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

        private void buttondobBug_Click(object sender, EventArgs e)
        {            
            using (var context2 = new EFCoreTestContext())
            {                
                var date = context2.EncryptedDates.First();                
            }
        }
    }
}
