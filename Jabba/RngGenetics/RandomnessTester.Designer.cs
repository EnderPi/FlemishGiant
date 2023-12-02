
namespace RngGenetics
{
    partial class RandomnessTester
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
            buttonAddTestingTask = new System.Windows.Forms.Button();
            button1 = new System.Windows.Forms.Button();
            checkedListBoxRngTestingTests = new System.Windows.Forms.CheckedListBox();
            labelFeistelRounds = new System.Windows.Forms.Label();
            numericUpDownFeistelRounds = new System.Windows.Forms.NumericUpDown();
            comboBoxKeyType = new System.Windows.Forms.ComboBox();
            numericUpDownSeed = new System.Windows.Forms.NumericUpDown();
            label18 = new System.Windows.Forms.Label();
            checkBoxRngTestAsHash = new System.Windows.Forms.CheckBox();
            label24 = new System.Windows.Forms.Label();
            comboBoxRngTestingType = new System.Windows.Forms.ComboBox();
            progressBarRngTesting = new System.Windows.Forms.ProgressBar();
            label20 = new System.Windows.Forms.Label();
            pictureBoxRngTesting = new System.Windows.Forms.PictureBox();
            textBoxDescriptionRngTesting = new System.Windows.Forms.TextBox();
            label21 = new System.Windows.Forms.Label();
            textBoxTestsPassedRngTesting = new System.Windows.Forms.TextBox();
            label22 = new System.Windows.Forms.Label();
            textBoxFitnessRngTesting = new System.Windows.Forms.TextBox();
            label25 = new System.Windows.Forms.Label();
            buttonStopTesting = new System.Windows.Forms.Button();
            buttonStartTesting = new System.Windows.Forms.Button();
            numericUpDownMaxFitnessRngTesting = new System.Windows.Forms.NumericUpDown();
            label19 = new System.Windows.Forms.Label();
            textBoxOutputRngTesting = new System.Windows.Forms.TextBox();
            textBoxStateExpressionRngTesting = new System.Windows.Forms.TextBox();
            labelFieldTwo = new System.Windows.Forms.Label();
            labelFieldOne = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)numericUpDownFeistelRounds).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownSeed).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxRngTesting).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownMaxFitnessRngTesting).BeginInit();
            SuspendLayout();
            // 
            // buttonAddTestingTask
            // 
            buttonAddTestingTask.Location = new System.Drawing.Point(98, 353);
            buttonAddTestingTask.Name = "buttonAddTestingTask";
            buttonAddTestingTask.Size = new System.Drawing.Size(118, 23);
            buttonAddTestingTask.TabIndex = 94;
            buttonAddTestingTask.Text = "Add To Task List";
            buttonAddTestingTask.UseVisualStyleBackColor = true;
            buttonAddTestingTask.Click += buttonAddTestingTask_Click;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(998, 382);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(75, 23);
            button1.TabIndex = 93;
            button1.Text = "Iterate";
            button1.UseVisualStyleBackColor = true;
            // 
            // checkedListBoxRngTestingTests
            // 
            checkedListBoxRngTestingTests.FormattingEnabled = true;
            checkedListBoxRngTestingTests.Location = new System.Drawing.Point(148, 454);
            checkedListBoxRngTestingTests.Name = "checkedListBoxRngTestingTests";
            checkedListBoxRngTestingTests.Size = new System.Drawing.Size(366, 112);
            checkedListBoxRngTestingTests.TabIndex = 92;
            // 
            // labelFeistelRounds
            // 
            labelFeistelRounds.AutoSize = true;
            labelFeistelRounds.Location = new System.Drawing.Point(21, 322);
            labelFeistelRounds.Name = "labelFeistelRounds";
            labelFeistelRounds.Size = new System.Drawing.Size(83, 15);
            labelFeistelRounds.TabIndex = 91;
            labelFeistelRounds.Text = "Feistel Rounds";
            // 
            // numericUpDownFeistelRounds
            // 
            numericUpDownFeistelRounds.Location = new System.Drawing.Point(139, 317);
            numericUpDownFeistelRounds.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numericUpDownFeistelRounds.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownFeistelRounds.Name = "numericUpDownFeistelRounds";
            numericUpDownFeistelRounds.Size = new System.Drawing.Size(77, 23);
            numericUpDownFeistelRounds.TabIndex = 90;
            numericUpDownFeistelRounds.Value = new decimal(new int[] { 4, 0, 0, 0 });
            // 
            // comboBoxKeyType
            // 
            comboBoxKeyType.FormattingEnabled = true;
            comboBoxKeyType.Location = new System.Drawing.Point(363, 24);
            comboBoxKeyType.Name = "comboBoxKeyType";
            comboBoxKeyType.Size = new System.Drawing.Size(121, 23);
            comboBoxKeyType.TabIndex = 89;
            // 
            // numericUpDownSeed
            // 
            numericUpDownSeed.Location = new System.Drawing.Point(330, 285);
            numericUpDownSeed.Maximum = new decimal(new int[] { -1, -1, 0, 0 });
            numericUpDownSeed.Name = "numericUpDownSeed";
            numericUpDownSeed.Size = new System.Drawing.Size(156, 23);
            numericUpDownSeed.TabIndex = 88;
            numericUpDownSeed.ThousandsSeparator = true;
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new System.Drawing.Point(291, 285);
            label18.Name = "label18";
            label18.Size = new System.Drawing.Size(32, 15);
            label18.TabIndex = 87;
            label18.Text = "Seed";
            // 
            // checkBoxRngTestAsHash
            // 
            checkBoxRngTestAsHash.AutoSize = true;
            checkBoxRngTestAsHash.Location = new System.Drawing.Point(348, 321);
            checkBoxRngTestAsHash.Name = "checkBoxRngTestAsHash";
            checkBoxRngTestAsHash.Size = new System.Drawing.Size(90, 19);
            checkBoxRngTestAsHash.TabIndex = 86;
            checkBoxRngTestAsHash.Text = "Test as Hash";
            checkBoxRngTestAsHash.UseVisualStyleBackColor = true;
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.Location = new System.Drawing.Point(23, 24);
            label24.Name = "label24";
            label24.Size = new System.Drawing.Size(86, 15);
            label24.TabIndex = 85;
            label24.Text = "Specimen Type";
            // 
            // comboBoxRngTestingType
            // 
            comboBoxRngTestingType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxRngTestingType.FormattingEnabled = true;
            comboBoxRngTestingType.Location = new System.Drawing.Point(139, 24);
            comboBoxRngTestingType.Name = "comboBoxRngTestingType";
            comboBoxRngTestingType.Size = new System.Drawing.Size(183, 23);
            comboBoxRngTestingType.TabIndex = 84;
            comboBoxRngTestingType.SelectedIndexChanged += comboBoxRngTestingType_SelectedIndexChanged;
            // 
            // progressBarRngTesting
            // 
            progressBarRngTesting.Location = new System.Drawing.Point(148, 402);
            progressBarRngTesting.Name = "progressBarRngTesting";
            progressBarRngTesting.Size = new System.Drawing.Size(174, 23);
            progressBarRngTesting.TabIndex = 83;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new System.Drawing.Point(571, 24);
            label20.Name = "label20";
            label20.Size = new System.Drawing.Size(109, 15);
            label20.TabIndex = 82;
            label20.Text = "Randomness Visual";
            // 
            // pictureBoxRngTesting
            // 
            pictureBoxRngTesting.Location = new System.Drawing.Point(571, 56);
            pictureBoxRngTesting.Name = "pictureBoxRngTesting";
            pictureBoxRngTesting.Size = new System.Drawing.Size(256, 256);
            pictureBoxRngTesting.TabIndex = 75;
            pictureBoxRngTesting.TabStop = false;
            // 
            // textBoxDescriptionRngTesting
            // 
            textBoxDescriptionRngTesting.Location = new System.Drawing.Point(864, 56);
            textBoxDescriptionRngTesting.Multiline = true;
            textBoxDescriptionRngTesting.Name = "textBoxDescriptionRngTesting";
            textBoxDescriptionRngTesting.ReadOnly = true;
            textBoxDescriptionRngTesting.Size = new System.Drawing.Size(390, 256);
            textBoxDescriptionRngTesting.TabIndex = 76;
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new System.Drawing.Point(864, 26);
            label21.Name = "label21";
            label21.Size = new System.Drawing.Size(80, 15);
            label21.TabIndex = 77;
            label21.Text = "Failure Details";
            // 
            // textBoxTestsPassedRngTesting
            // 
            textBoxTestsPassedRngTesting.Location = new System.Drawing.Point(717, 353);
            textBoxTestsPassedRngTesting.Name = "textBoxTestsPassedRngTesting";
            textBoxTestsPassedRngTesting.ReadOnly = true;
            textBoxTestsPassedRngTesting.Size = new System.Drawing.Size(110, 23);
            textBoxTestsPassedRngTesting.TabIndex = 81;
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new System.Drawing.Point(627, 353);
            label22.Name = "label22";
            label22.Size = new System.Drawing.Size(71, 15);
            label22.TabIndex = 80;
            label22.Text = "Tests Passed";
            // 
            // textBoxFitnessRngTesting
            // 
            textBoxFitnessRngTesting.Location = new System.Drawing.Point(717, 321);
            textBoxFitnessRngTesting.Name = "textBoxFitnessRngTesting";
            textBoxFitnessRngTesting.ReadOnly = true;
            textBoxFitnessRngTesting.Size = new System.Drawing.Size(110, 23);
            textBoxFitnessRngTesting.TabIndex = 79;
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Location = new System.Drawing.Point(655, 325);
            label25.Name = "label25";
            label25.Size = new System.Drawing.Size(43, 15);
            label25.TabIndex = 78;
            label25.Text = "Fitness";
            // 
            // buttonStopTesting
            // 
            buttonStopTesting.Location = new System.Drawing.Point(389, 353);
            buttonStopTesting.Name = "buttonStopTesting";
            buttonStopTesting.Size = new System.Drawing.Size(134, 23);
            buttonStopTesting.TabIndex = 74;
            buttonStopTesting.Text = "Stop";
            buttonStopTesting.UseVisualStyleBackColor = true;
            buttonStopTesting.Click += buttonStopTesting_Click;
            // 
            // buttonStartTesting
            // 
            buttonStartTesting.Location = new System.Drawing.Point(235, 353);
            buttonStartTesting.Name = "buttonStartTesting";
            buttonStartTesting.Size = new System.Drawing.Size(134, 23);
            buttonStartTesting.TabIndex = 73;
            buttonStartTesting.Text = "Start Testing";
            buttonStartTesting.UseVisualStyleBackColor = true;
            buttonStartTesting.Click += buttonStartTesting_Click;
            // 
            // numericUpDownMaxFitnessRngTesting
            // 
            numericUpDownMaxFitnessRngTesting.Location = new System.Drawing.Point(141, 283);
            numericUpDownMaxFitnessRngTesting.Maximum = new decimal(new int[] { -727379968, 232, 0, 0 });
            numericUpDownMaxFitnessRngTesting.Minimum = new decimal(new int[] { 100, 0, 0, 0 });
            numericUpDownMaxFitnessRngTesting.Name = "numericUpDownMaxFitnessRngTesting";
            numericUpDownMaxFitnessRngTesting.Size = new System.Drawing.Size(144, 23);
            numericUpDownMaxFitnessRngTesting.TabIndex = 72;
            numericUpDownMaxFitnessRngTesting.ThousandsSeparator = true;
            numericUpDownMaxFitnessRngTesting.Value = new decimal(new int[] { 100000000, 0, 0, 0 });
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new System.Drawing.Point(14, 285);
            label19.Name = "label19";
            label19.Size = new System.Drawing.Size(69, 15);
            label19.TabIndex = 71;
            label19.Text = "Max Fitness";
            // 
            // textBoxOutputRngTesting
            // 
            textBoxOutputRngTesting.Location = new System.Drawing.Point(141, 214);
            textBoxOutputRngTesting.Multiline = true;
            textBoxOutputRngTesting.Name = "textBoxOutputRngTesting";
            textBoxOutputRngTesting.Size = new System.Drawing.Size(345, 63);
            textBoxOutputRngTesting.TabIndex = 70;
            // 
            // textBoxStateExpressionRngTesting
            // 
            textBoxStateExpressionRngTesting.Location = new System.Drawing.Point(139, 59);
            textBoxStateExpressionRngTesting.Multiline = true;
            textBoxStateExpressionRngTesting.Name = "textBoxStateExpressionRngTesting";
            textBoxStateExpressionRngTesting.Size = new System.Drawing.Size(345, 127);
            textBoxStateExpressionRngTesting.TabIndex = 69;
            // 
            // labelFieldTwo
            // 
            labelFieldTwo.AutoSize = true;
            labelFieldTwo.Location = new System.Drawing.Point(12, 214);
            labelFieldTwo.Name = "labelFieldTwo";
            labelFieldTwo.Size = new System.Drawing.Size(104, 15);
            labelFieldTwo.TabIndex = 68;
            labelFieldTwo.Text = "Output Expression";
            // 
            // labelFieldOne
            // 
            labelFieldOne.AutoSize = true;
            labelFieldOne.Location = new System.Drawing.Point(12, 64);
            labelFieldOne.Name = "labelFieldOne";
            labelFieldOne.Size = new System.Drawing.Size(92, 15);
            labelFieldOne.TabIndex = 67;
            labelFieldOne.Text = "State Expression";
            // 
            // RandomnessTester
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(buttonAddTestingTask);
            Controls.Add(button1);
            Controls.Add(checkedListBoxRngTestingTests);
            Controls.Add(labelFeistelRounds);
            Controls.Add(numericUpDownFeistelRounds);
            Controls.Add(comboBoxKeyType);
            Controls.Add(numericUpDownSeed);
            Controls.Add(label18);
            Controls.Add(checkBoxRngTestAsHash);
            Controls.Add(label24);
            Controls.Add(comboBoxRngTestingType);
            Controls.Add(progressBarRngTesting);
            Controls.Add(label20);
            Controls.Add(pictureBoxRngTesting);
            Controls.Add(textBoxDescriptionRngTesting);
            Controls.Add(label21);
            Controls.Add(textBoxTestsPassedRngTesting);
            Controls.Add(label22);
            Controls.Add(textBoxFitnessRngTesting);
            Controls.Add(label25);
            Controls.Add(buttonStopTesting);
            Controls.Add(buttonStartTesting);
            Controls.Add(numericUpDownMaxFitnessRngTesting);
            Controls.Add(label19);
            Controls.Add(textBoxOutputRngTesting);
            Controls.Add(textBoxStateExpressionRngTesting);
            Controls.Add(labelFieldTwo);
            Controls.Add(labelFieldOne);
            Name = "RandomnessTester";
            Size = new System.Drawing.Size(1271, 636);
            ((System.ComponentModel.ISupportInitialize)numericUpDownFeistelRounds).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownSeed).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxRngTesting).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownMaxFitnessRngTesting).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button buttonAddTestingTask;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckedListBox checkedListBoxRngTestingTests;
        private System.Windows.Forms.Label labelFeistelRounds;
        private System.Windows.Forms.NumericUpDown numericUpDownFeistelRounds;
        private System.Windows.Forms.ComboBox comboBoxKeyType;
        private System.Windows.Forms.NumericUpDown numericUpDownSeed;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.CheckBox checkBoxRngTestAsHash;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.ComboBox comboBoxRngTestingType;
        private System.Windows.Forms.ProgressBar progressBarRngTesting;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.PictureBox pictureBoxRngTesting;
        private System.Windows.Forms.TextBox textBoxDescriptionRngTesting;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox textBoxTestsPassedRngTesting;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox textBoxFitnessRngTesting;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Button buttonStopTesting;
        private System.Windows.Forms.Button buttonStartTesting;
        private System.Windows.Forms.NumericUpDown numericUpDownMaxFitnessRngTesting;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox textBoxOutputRngTesting;
        private System.Windows.Forms.TextBox textBoxStateExpressionRngTesting;
        private System.Windows.Forms.Label labelFieldTwo;
        private System.Windows.Forms.Label labelFieldOne;
    }
}
