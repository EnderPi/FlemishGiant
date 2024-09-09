namespace RngGenetics
{
    partial class GeneticSimulationPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            checkedListBoxGeneticTests = new System.Windows.Forms.CheckedListBox();
            comboBoxGeneticFeistelType = new System.Windows.Forms.ComboBox();
            label26 = new System.Windows.Forms.Label();
            numericUpDownGeneticFeistelRounds = new System.Windows.Forms.NumericUpDown();
            label2 = new System.Windows.Forms.Label();
            numericUpDownInitialAdds = new System.Windows.Forms.NumericUpDown();
            checkBoxTestAsHash = new System.Windows.Forms.CheckBox();
            checkedListBoxOperations = new System.Windows.Forms.CheckedListBox();
            textBoxFailures = new System.Windows.Forms.TextBox();
            numericUpDownSelectionPressure = new System.Windows.Forms.NumericUpDown();
            label23 = new System.Windows.Forms.Label();
            buttonPushToTesting = new System.Windows.Forms.Button();
            label15 = new System.Windows.Forms.Label();
            label17 = new System.Windows.Forms.Label();
            buttonRunSimulation = new System.Windows.Forms.Button();
            label16 = new System.Windows.Forms.Label();
            pictureBoxMain = new System.Windows.Forms.PictureBox();
            comboBoxGeneticType = new System.Windows.Forms.ComboBox();
            textBoxBestDescription = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            textBoxStateOneFunction = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            label14 = new System.Windows.Forms.Label();
            numericUpDownSpecimensPerGeneration = new System.Windows.Forms.NumericUpDown();
            numericUpDownMaxFitness = new System.Windows.Forms.NumericUpDown();
            label4 = new System.Windows.Forms.Label();
            textBoxTestsPassed = new System.Windows.Forms.TextBox();
            numericUpDownConvergenceAge = new System.Windows.Forms.NumericUpDown();
            label13 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            textBoxOperations = new System.Windows.Forms.TextBox();
            numericUpDownMutationRate = new System.Windows.Forms.NumericUpDown();
            label12 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            textBoxCurrentGeneration = new System.Windows.Forms.TextBox();
            numericUpDownSpecimensPerTournament = new System.Windows.Forms.NumericUpDown();
            label11 = new System.Windows.Forms.Label();
            buttonStop = new System.Windows.Forms.Button();
            textBoxSpecimensEvaluated = new System.Windows.Forms.TextBox();
            textBoxGeneration = new System.Windows.Forms.TextBox();
            label10 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            textBoxFitness = new System.Windows.Forms.TextBox();
            dataGridViewAverageFitness = new System.Windows.Forms.DataGridView();
            ColumnGeneration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ColumnAverageFitness = new System.Windows.Forms.DataGridViewTextBoxColumn();
            label9 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            numericUpDownThreads = new System.Windows.Forms.NumericUpDown();
            timerUpdateUI = new System.Windows.Forms.Timer(components);
            timerUpdateVisual = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)numericUpDownGeneticFeistelRounds).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownInitialAdds).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownSelectionPressure).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxMain).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownSpecimensPerGeneration).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownMaxFitness).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownConvergenceAge).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownMutationRate).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownSpecimensPerTournament).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewAverageFitness).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownThreads).BeginInit();
            SuspendLayout();
            // 
            // checkedListBoxGeneticTests
            // 
            checkedListBoxGeneticTests.FormattingEnabled = true;
            checkedListBoxGeneticTests.Location = new System.Drawing.Point(382, 366);
            checkedListBoxGeneticTests.Name = "checkedListBoxGeneticTests";
            checkedListBoxGeneticTests.Size = new System.Drawing.Size(206, 148);
            checkedListBoxGeneticTests.TabIndex = 104;
            // 
            // comboBoxGeneticFeistelType
            // 
            comboBoxGeneticFeistelType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxGeneticFeistelType.FormattingEnabled = true;
            comboBoxGeneticFeistelType.Location = new System.Drawing.Point(322, 304);
            comboBoxGeneticFeistelType.Name = "comboBoxGeneticFeistelType";
            comboBoxGeneticFeistelType.Size = new System.Drawing.Size(121, 23);
            comboBoxGeneticFeistelType.TabIndex = 103;
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Location = new System.Drawing.Point(40, 307);
            label26.Name = "label26";
            label26.Size = new System.Drawing.Size(83, 15);
            label26.TabIndex = 102;
            label26.Text = "Feistel Rounds";
            // 
            // numericUpDownGeneticFeistelRounds
            // 
            numericUpDownGeneticFeistelRounds.Location = new System.Drawing.Point(200, 305);
            numericUpDownGeneticFeistelRounds.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numericUpDownGeneticFeistelRounds.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownGeneticFeistelRounds.Name = "numericUpDownGeneticFeistelRounds";
            numericUpDownGeneticFeistelRounds.Size = new System.Drawing.Size(75, 23);
            numericUpDownGeneticFeistelRounds.TabIndex = 101;
            numericUpDownGeneticFeistelRounds.Value = new decimal(new int[] { 6, 0, 0, 0 });
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(40, 277);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(104, 15);
            label2.TabIndex = 100;
            label2.Text = "Initial Node Count";
            // 
            // numericUpDownInitialAdds
            // 
            numericUpDownInitialAdds.Location = new System.Drawing.Point(199, 275);
            numericUpDownInitialAdds.Name = "numericUpDownInitialAdds";
            numericUpDownInitialAdds.Size = new System.Drawing.Size(76, 23);
            numericUpDownInitialAdds.TabIndex = 99;
            numericUpDownInitialAdds.Value = new decimal(new int[] { 2, 0, 0, 0 });
            // 
            // checkBoxTestAsHash
            // 
            checkBoxTestAsHash.AutoSize = true;
            checkBoxTestAsHash.Checked = true;
            checkBoxTestAsHash.CheckState = System.Windows.Forms.CheckState.Checked;
            checkBoxTestAsHash.Location = new System.Drawing.Point(71, 331);
            checkBoxTestAsHash.Name = "checkBoxTestAsHash";
            checkBoxTestAsHash.Size = new System.Drawing.Size(92, 19);
            checkBoxTestAsHash.TabIndex = 98;
            checkBoxTestAsHash.Text = "Test As Hash";
            checkBoxTestAsHash.UseVisualStyleBackColor = true;
            // 
            // checkedListBoxOperations
            // 
            checkedListBoxOperations.FormattingEnabled = true;
            checkedListBoxOperations.Items.AddRange(new object[] { "Add", "Subtract", "Multiply", "Divide", "Remainder", "Shift Right", "Shift Left", "Rotate Right", "Rotate Left", "And", "Or", "Xor", "Not", "Xor Shift Right", "Rotate-Multiply", "Loop" });
            checkedListBoxOperations.Location = new System.Drawing.Point(80, 366);
            checkedListBoxOperations.MultiColumn = true;
            checkedListBoxOperations.Name = "checkedListBoxOperations";
            checkedListBoxOperations.Size = new System.Drawing.Size(285, 148);
            checkedListBoxOperations.TabIndex = 97;
            // 
            // textBoxFailures
            // 
            textBoxFailures.Location = new System.Drawing.Point(1026, 400);
            textBoxFailures.Multiline = true;
            textBoxFailures.Name = "textBoxFailures";
            textBoxFailures.ReadOnly = true;
            textBoxFailures.Size = new System.Drawing.Size(222, 116);
            textBoxFailures.TabIndex = 96;
            // 
            // numericUpDownSelectionPressure
            // 
            numericUpDownSelectionPressure.DecimalPlaces = 3;
            numericUpDownSelectionPressure.Location = new System.Drawing.Point(245, 243);
            numericUpDownSelectionPressure.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownSelectionPressure.Name = "numericUpDownSelectionPressure";
            numericUpDownSelectionPressure.Size = new System.Drawing.Size(75, 23);
            numericUpDownSelectionPressure.TabIndex = 95;
            numericUpDownSelectionPressure.Value = new decimal(new int[] { 6, 0, 0, 65536 });
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Location = new System.Drawing.Point(38, 245);
            label23.Name = "label23";
            label23.Size = new System.Drawing.Size(179, 15);
            label23.TabIndex = 94;
            label23.Text = "Selection (0-1, 0.5 is no pressure)";
            // 
            // buttonPushToTesting
            // 
            buttonPushToTesting.Location = new System.Drawing.Point(523, 83);
            buttonPushToTesting.Name = "buttonPushToTesting";
            buttonPushToTesting.Size = new System.Drawing.Size(75, 58);
            buttonPushToTesting.TabIndex = 93;
            buttonPushToTesting.Text = "Push to Testing";
            buttonPushToTesting.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new System.Drawing.Point(38, 10);
            label15.Name = "label15";
            label15.Size = new System.Drawing.Size(105, 15);
            label15.TabIndex = 89;
            label15.Text = "Type of Simulation";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new System.Drawing.Point(695, 10);
            label17.Name = "label17";
            label17.Size = new System.Drawing.Size(109, 15);
            label17.TabIndex = 92;
            label17.Text = "Randomness Visual";
            // 
            // buttonRunSimulation
            // 
            buttonRunSimulation.Location = new System.Drawing.Point(523, 10);
            buttonRunSimulation.Name = "buttonRunSimulation";
            buttonRunSimulation.Size = new System.Drawing.Size(75, 23);
            buttonRunSimulation.TabIndex = 58;
            buttonRunSimulation.Text = "Go!";
            buttonRunSimulation.UseVisualStyleBackColor = true;
            buttonRunSimulation.Click += buttonRunSimulation_Click;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new System.Drawing.Point(38, 217);
            label16.Name = "label16";
            label16.Size = new System.Drawing.Size(116, 15);
            label16.TabIndex = 91;
            label16.Text = "State One Constraint";
            // 
            // pictureBoxMain
            // 
            pictureBoxMain.Location = new System.Drawing.Point(695, 42);
            pictureBoxMain.Name = "pictureBoxMain";
            pictureBoxMain.Size = new System.Drawing.Size(256, 256);
            pictureBoxMain.TabIndex = 59;
            pictureBoxMain.TabStop = false;
            // 
            // comboBoxGeneticType
            // 
            comboBoxGeneticType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxGeneticType.FormattingEnabled = true;
            comboBoxGeneticType.Location = new System.Drawing.Point(199, 7);
            comboBoxGeneticType.Name = "comboBoxGeneticType";
            comboBoxGeneticType.Size = new System.Drawing.Size(225, 23);
            comboBoxGeneticType.TabIndex = 90;
            comboBoxGeneticType.SelectedIndexChanged += ComboBoxGeneticType_SelectedIndexChanged;
            // 
            // textBoxBestDescription
            // 
            textBoxBestDescription.Location = new System.Drawing.Point(695, 304);
            textBoxBestDescription.Multiline = true;
            textBoxBestDescription.Name = "textBoxBestDescription";
            textBoxBestDescription.ReadOnly = true;
            textBoxBestDescription.Size = new System.Drawing.Size(256, 182);
            textBoxBestDescription.TabIndex = 60;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(622, 327);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(67, 15);
            label1.TabIndex = 61;
            label1.Text = "Description";
            // 
            // textBoxStateOneFunction
            // 
            textBoxStateOneFunction.Location = new System.Drawing.Point(200, 214);
            textBoxStateOneFunction.Name = "textBoxStateOneFunction";
            textBoxStateOneFunction.Size = new System.Drawing.Size(224, 23);
            textBoxStateOneFunction.TabIndex = 88;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(37, 39);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(145, 15);
            label3.TabIndex = 62;
            label3.Text = "Specimens Per Generation";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new System.Drawing.Point(37, 184);
            label14.Name = "label14";
            label14.Size = new System.Drawing.Size(69, 15);
            label14.TabIndex = 87;
            label14.Text = "Max Fitness";
            // 
            // numericUpDownSpecimensPerGeneration
            // 
            numericUpDownSpecimensPerGeneration.Location = new System.Drawing.Point(200, 37);
            numericUpDownSpecimensPerGeneration.Maximum = new decimal(new int[] { 65536, 0, 0, 0 });
            numericUpDownSpecimensPerGeneration.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownSpecimensPerGeneration.Name = "numericUpDownSpecimensPerGeneration";
            numericUpDownSpecimensPerGeneration.Size = new System.Drawing.Size(120, 23);
            numericUpDownSpecimensPerGeneration.TabIndex = 63;
            numericUpDownSpecimensPerGeneration.ThousandsSeparator = true;
            numericUpDownSpecimensPerGeneration.Value = new decimal(new int[] { 256, 0, 0, 0 });
            // 
            // numericUpDownMaxFitness
            // 
            numericUpDownMaxFitness.Location = new System.Drawing.Point(200, 182);
            numericUpDownMaxFitness.Maximum = new decimal(new int[] { 1410065408, 2, 0, 0 });
            numericUpDownMaxFitness.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownMaxFitness.Name = "numericUpDownMaxFitness";
            numericUpDownMaxFitness.Size = new System.Drawing.Size(120, 23);
            numericUpDownMaxFitness.TabIndex = 86;
            numericUpDownMaxFitness.ThousandsSeparator = true;
            numericUpDownMaxFitness.Value = new decimal(new int[] { 100000, 0, 0, 0 });
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(37, 68);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(101, 15);
            label4.TabIndex = 64;
            label4.Text = "Convergence Age";
            // 
            // textBoxTestsPassed
            // 
            textBoxTestsPassed.Location = new System.Drawing.Point(841, 501);
            textBoxTestsPassed.Name = "textBoxTestsPassed";
            textBoxTestsPassed.ReadOnly = true;
            textBoxTestsPassed.Size = new System.Drawing.Size(110, 23);
            textBoxTestsPassed.TabIndex = 85;
            // 
            // numericUpDownConvergenceAge
            // 
            numericUpDownConvergenceAge.Location = new System.Drawing.Point(200, 66);
            numericUpDownConvergenceAge.Maximum = new decimal(new int[] { 64, 0, 0, 0 });
            numericUpDownConvergenceAge.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownConvergenceAge.Name = "numericUpDownConvergenceAge";
            numericUpDownConvergenceAge.Size = new System.Drawing.Size(120, 23);
            numericUpDownConvergenceAge.TabIndex = 65;
            numericUpDownConvergenceAge.ThousandsSeparator = true;
            numericUpDownConvergenceAge.Value = new decimal(new int[] { 64, 0, 0, 0 });
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new System.Drawing.Point(764, 504);
            label13.Name = "label13";
            label13.Size = new System.Drawing.Size(71, 15);
            label13.TabIndex = 84;
            label13.Text = "Tests Passed";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(37, 97);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(82, 15);
            label5.TabIndex = 66;
            label5.Text = "Mutation Rate";
            // 
            // textBoxOperations
            // 
            textBoxOperations.Location = new System.Drawing.Point(883, 532);
            textBoxOperations.Name = "textBoxOperations";
            textBoxOperations.ReadOnly = true;
            textBoxOperations.Size = new System.Drawing.Size(68, 23);
            textBoxOperations.TabIndex = 83;
            // 
            // numericUpDownMutationRate
            // 
            numericUpDownMutationRate.DecimalPlaces = 3;
            numericUpDownMutationRate.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            numericUpDownMutationRate.Location = new System.Drawing.Point(200, 95);
            numericUpDownMutationRate.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownMutationRate.Name = "numericUpDownMutationRate";
            numericUpDownMutationRate.Size = new System.Drawing.Size(120, 23);
            numericUpDownMutationRate.TabIndex = 67;
            numericUpDownMutationRate.ThousandsSeparator = true;
            numericUpDownMutationRate.Value = new decimal(new int[] { 1, 0, 0, 65536 });
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new System.Drawing.Point(811, 539);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(65, 15);
            label12.TabIndex = 82;
            label12.Text = "Operations";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(37, 126);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(151, 15);
            label6.TabIndex = 68;
            label6.Text = "Specimens Per Tournament";
            // 
            // textBoxCurrentGeneration
            // 
            textBoxCurrentGeneration.Location = new System.Drawing.Point(1169, 363);
            textBoxCurrentGeneration.Name = "textBoxCurrentGeneration";
            textBoxCurrentGeneration.ReadOnly = true;
            textBoxCurrentGeneration.Size = new System.Drawing.Size(79, 23);
            textBoxCurrentGeneration.TabIndex = 81;
            // 
            // numericUpDownSpecimensPerTournament
            // 
            numericUpDownSpecimensPerTournament.Location = new System.Drawing.Point(200, 124);
            numericUpDownSpecimensPerTournament.Maximum = new decimal(new int[] { 65536, 0, 0, 0 });
            numericUpDownSpecimensPerTournament.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownSpecimensPerTournament.Name = "numericUpDownSpecimensPerTournament";
            numericUpDownSpecimensPerTournament.Size = new System.Drawing.Size(120, 23);
            numericUpDownSpecimensPerTournament.TabIndex = 69;
            numericUpDownSpecimensPerTournament.ThousandsSeparator = true;
            numericUpDownSpecimensPerTournament.Value = new decimal(new int[] { 2, 0, 0, 0 });
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new System.Drawing.Point(1026, 366);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(108, 15);
            label11.TabIndex = 80;
            label11.Text = "Current Generation";
            // 
            // buttonStop
            // 
            buttonStop.Enabled = false;
            buttonStop.Location = new System.Drawing.Point(523, 42);
            buttonStop.Name = "buttonStop";
            buttonStop.Size = new System.Drawing.Size(75, 23);
            buttonStop.TabIndex = 70;
            buttonStop.Text = "Stop!";
            buttonStop.UseVisualStyleBackColor = true;
            buttonStop.Click += buttonStop_Click;
            // 
            // textBoxSpecimensEvaluated
            // 
            textBoxSpecimensEvaluated.Location = new System.Drawing.Point(1169, 324);
            textBoxSpecimensEvaluated.Name = "textBoxSpecimensEvaluated";
            textBoxSpecimensEvaluated.ReadOnly = true;
            textBoxSpecimensEvaluated.Size = new System.Drawing.Size(79, 23);
            textBoxSpecimensEvaluated.TabIndex = 79;
            // 
            // textBoxGeneration
            // 
            textBoxGeneration.Location = new System.Drawing.Point(695, 502);
            textBoxGeneration.Name = "textBoxGeneration";
            textBoxGeneration.ReadOnly = true;
            textBoxGeneration.Size = new System.Drawing.Size(63, 23);
            textBoxGeneration.TabIndex = 71;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(1026, 327);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(118, 15);
            label10.TabIndex = 78;
            label10.Text = "Specimens Evaluated";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(613, 505);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(65, 15);
            label7.TabIndex = 72;
            label7.Text = "Generation";
            // 
            // textBoxFitness
            // 
            textBoxFitness.Location = new System.Drawing.Point(695, 531);
            textBoxFitness.Name = "textBoxFitness";
            textBoxFitness.ReadOnly = true;
            textBoxFitness.Size = new System.Drawing.Size(110, 23);
            textBoxFitness.TabIndex = 77;
            // 
            // dataGridViewAverageFitness
            // 
            dataGridViewAverageFitness.AllowUserToAddRows = false;
            dataGridViewAverageFitness.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dataGridViewAverageFitness.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewAverageFitness.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewAverageFitness.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { ColumnGeneration, ColumnAverageFitness });
            dataGridViewAverageFitness.Location = new System.Drawing.Point(1008, 42);
            dataGridViewAverageFitness.Name = "dataGridViewAverageFitness";
            dataGridViewAverageFitness.ReadOnly = true;
            dataGridViewAverageFitness.RowHeadersVisible = false;
            dataGridViewAverageFitness.Size = new System.Drawing.Size(240, 256);
            dataGridViewAverageFitness.TabIndex = 73;
            // 
            // ColumnGeneration
            // 
            ColumnGeneration.HeaderText = "Generation";
            ColumnGeneration.Name = "ColumnGeneration";
            ColumnGeneration.ReadOnly = true;
            // 
            // ColumnAverageFitness
            // 
            ColumnAverageFitness.HeaderText = "Average Fitness";
            ColumnAverageFitness.Name = "ColumnAverageFitness";
            ColumnAverageFitness.ReadOnly = true;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(633, 535);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(43, 15);
            label9.TabIndex = 76;
            label9.Text = "Fitness";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(37, 155);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(64, 15);
            label8.TabIndex = 74;
            label8.Text = "Paralellism";
            // 
            // numericUpDownThreads
            // 
            numericUpDownThreads.Location = new System.Drawing.Point(200, 153);
            numericUpDownThreads.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownThreads.Name = "numericUpDownThreads";
            numericUpDownThreads.Size = new System.Drawing.Size(120, 23);
            numericUpDownThreads.TabIndex = 75;
            numericUpDownThreads.Value = new decimal(new int[] { 4, 0, 0, 0 });
            // 
            // timerUpdateUI
            // 
            timerUpdateUI.Interval = 1000;
            timerUpdateUI.Tick += timerUpdateUI_Tick;
            // 
            // timerUpdateVisual
            // 
            timerUpdateVisual.Interval = 4000;
            timerUpdateVisual.Tick += TimerUpdateVisual_Tick;
            // 
            // GeneticSimulationPanel
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(checkedListBoxGeneticTests);
            Controls.Add(comboBoxGeneticFeistelType);
            Controls.Add(label26);
            Controls.Add(numericUpDownGeneticFeistelRounds);
            Controls.Add(label2);
            Controls.Add(numericUpDownInitialAdds);
            Controls.Add(checkBoxTestAsHash);
            Controls.Add(checkedListBoxOperations);
            Controls.Add(textBoxFailures);
            Controls.Add(numericUpDownSelectionPressure);
            Controls.Add(label23);
            Controls.Add(buttonPushToTesting);
            Controls.Add(label15);
            Controls.Add(label17);
            Controls.Add(buttonRunSimulation);
            Controls.Add(label16);
            Controls.Add(pictureBoxMain);
            Controls.Add(comboBoxGeneticType);
            Controls.Add(textBoxBestDescription);
            Controls.Add(label1);
            Controls.Add(textBoxStateOneFunction);
            Controls.Add(label3);
            Controls.Add(label14);
            Controls.Add(numericUpDownSpecimensPerGeneration);
            Controls.Add(numericUpDownMaxFitness);
            Controls.Add(label4);
            Controls.Add(textBoxTestsPassed);
            Controls.Add(numericUpDownConvergenceAge);
            Controls.Add(label13);
            Controls.Add(label5);
            Controls.Add(textBoxOperations);
            Controls.Add(numericUpDownMutationRate);
            Controls.Add(label12);
            Controls.Add(label6);
            Controls.Add(textBoxCurrentGeneration);
            Controls.Add(numericUpDownSpecimensPerTournament);
            Controls.Add(label11);
            Controls.Add(buttonStop);
            Controls.Add(textBoxSpecimensEvaluated);
            Controls.Add(textBoxGeneration);
            Controls.Add(label10);
            Controls.Add(label7);
            Controls.Add(textBoxFitness);
            Controls.Add(dataGridViewAverageFitness);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(numericUpDownThreads);
            Name = "GeneticSimulationPanel";
            Size = new System.Drawing.Size(1285, 563);
            ((System.ComponentModel.ISupportInitialize)numericUpDownGeneticFeistelRounds).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownInitialAdds).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownSelectionPressure).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxMain).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownSpecimensPerGeneration).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownMaxFitness).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownConvergenceAge).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownMutationRate).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownSpecimensPerTournament).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewAverageFitness).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownThreads).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBoxGeneticTests;
        private System.Windows.Forms.ComboBox comboBoxGeneticFeistelType;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.NumericUpDown numericUpDownGeneticFeistelRounds;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownInitialAdds;
        private System.Windows.Forms.CheckBox checkBoxTestAsHash;
        private System.Windows.Forms.CheckedListBox checkedListBoxOperations;
        private System.Windows.Forms.TextBox textBoxFailures;
        private System.Windows.Forms.NumericUpDown numericUpDownSelectionPressure;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Button buttonPushToTesting;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button buttonRunSimulation;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.PictureBox pictureBoxMain;
        private System.Windows.Forms.ComboBox comboBoxGeneticType;
        private System.Windows.Forms.TextBox textBoxBestDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxStateOneFunction;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown numericUpDownSpecimensPerGeneration;
        private System.Windows.Forms.NumericUpDown numericUpDownMaxFitness;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxTestsPassed;
        private System.Windows.Forms.NumericUpDown numericUpDownConvergenceAge;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxOperations;
        private System.Windows.Forms.NumericUpDown numericUpDownMutationRate;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxCurrentGeneration;
        private System.Windows.Forms.NumericUpDown numericUpDownSpecimensPerTournament;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.TextBox textBoxSpecimensEvaluated;
        private System.Windows.Forms.TextBox textBoxGeneration;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxFitness;
        private System.Windows.Forms.DataGridView dataGridViewAverageFitness;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnGeneration;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnAverageFitness;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numericUpDownThreads;
        private System.Windows.Forms.Timer timerUpdateUI;
        private System.Windows.Forms.Timer timerUpdateVisual;
    }
}
