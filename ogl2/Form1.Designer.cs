using System;

namespace ogl2
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.glControl1 = new OpenTK.GLControl();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.clearButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cullingCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cullingComboBox = new System.Windows.Forms.ComboBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.polygonMode2 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.polygonMode1 = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.alphaCheckBox = new System.Windows.Forms.CheckBox();
            this.alphaTrackBar = new System.Windows.Forms.TrackBar();
            this.label9 = new System.Windows.Forms.Label();
            this.alphaComboBox = new System.Windows.Forms.ComboBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.scissorReset = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.scissorCheckBox = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.blendDestination = new System.Windows.Forms.ComboBox();
            this.blendSource = new System.Windows.Forms.ComboBox();
            this.blendCheckBox = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.fractalSteps = new System.Windows.Forms.NumericUpDown();
            this.ChangeSeedButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.alphaTrackBar)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel6.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fractalSteps)).BeginInit();
            this.SuspendLayout();
            // 
            // glControl1
            // 
            this.glControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Location = new System.Drawing.Point(12, 12);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(591, 426);
            this.glControl1.TabIndex = 0;
            this.glControl1.VSync = false;
            this.glControl1.Load += new System.EventHandler(this.glControl1_Load);
            this.glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl1_Paint);
            this.glControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseDown);
            this.glControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseMove);
            this.glControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.glControl1_MouseUp);
            this.glControl1.Resize += new System.EventHandler(this.glControl1_Resize);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(6, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(149, 21);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.panel7);
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Controls.Add(this.panel2);
            this.flowLayoutPanel1.Controls.Add(this.panel3);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 6);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(175, 319);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.comboBox1);
            this.panel7.Controls.Add(this.clearButton);
            this.panel7.Location = new System.Drawing.Point(3, 3);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(169, 58);
            this.panel7.TabIndex = 8;
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(6, 30);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(149, 23);
            this.clearButton.TabIndex = 2;
            this.clearButton.Text = "Очистить";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.numericUpDown1);
            this.panel1.Location = new System.Drawing.Point(3, 67);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(169, 56);
            this.panel1.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Размер примитива";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(6, 25);
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(149, 20);
            this.numericUpDown1.TabIndex = 3;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cullingCheckBox);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.cullingComboBox);
            this.panel2.Location = new System.Drawing.Point(3, 129);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(169, 66);
            this.panel2.TabIndex = 6;
            // 
            // cullingCheckBox
            // 
            this.cullingCheckBox.AutoSize = true;
            this.cullingCheckBox.Location = new System.Drawing.Point(6, 16);
            this.cullingCheckBox.Name = "cullingCheckBox";
            this.cullingCheckBox.Size = new System.Drawing.Size(75, 17);
            this.cullingCheckBox.TabIndex = 5;
            this.cullingCheckBox.Text = "Включить";
            this.cullingCheckBox.UseVisualStyleBackColor = true;
            this.cullingCheckBox.CheckedChanged += new System.EventHandler(this.cullingCheckBox_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Culling";
            // 
            // cullingComboBox
            // 
            this.cullingComboBox.FormattingEnabled = true;
            this.cullingComboBox.Location = new System.Drawing.Point(6, 39);
            this.cullingComboBox.Name = "cullingComboBox";
            this.cullingComboBox.Size = new System.Drawing.Size(149, 21);
            this.cullingComboBox.TabIndex = 4;
            this.cullingComboBox.SelectedIndexChanged += new System.EventHandler(this.cullingComboBox_SelectedIndexChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.polygonMode2);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.polygonMode1);
            this.panel3.Location = new System.Drawing.Point(3, 201);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(169, 114);
            this.panel3.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "BACK";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "FRONT";
            // 
            // polygonMode2
            // 
            this.polygonMode2.FormattingEnabled = true;
            this.polygonMode2.Location = new System.Drawing.Point(6, 87);
            this.polygonMode2.Name = "polygonMode2";
            this.polygonMode2.Size = new System.Drawing.Size(149, 21);
            this.polygonMode2.TabIndex = 5;
            this.polygonMode2.SelectedIndexChanged += new System.EventHandler(this.polygonMode2_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "PolygonMode";
            // 
            // polygonMode1
            // 
            this.polygonMode1.FormattingEnabled = true;
            this.polygonMode1.Location = new System.Drawing.Point(6, 47);
            this.polygonMode1.Name = "polygonMode1";
            this.polygonMode1.Size = new System.Drawing.Size(149, 21);
            this.polygonMode1.TabIndex = 4;
            this.polygonMode1.SelectedIndexChanged += new System.EventHandler(this.polygonMode1_SelectedIndexChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(609, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(189, 426);
            this.tabControl1.TabIndex = 4;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage1.Controls.Add(this.flowLayoutPanel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(181, 400);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Лаб. 1";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage2.Controls.Add(this.flowLayoutPanel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(181, 400);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Лаб. 2";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.panel5);
            this.flowLayoutPanel2.Controls.Add(this.panel4);
            this.flowLayoutPanel2.Controls.Add(this.panel6);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 6);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(175, 388);
            this.flowLayoutPanel2.TabIndex = 8;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.alphaCheckBox);
            this.panel5.Controls.Add(this.alphaTrackBar);
            this.panel5.Controls.Add(this.label9);
            this.panel5.Controls.Add(this.alphaComboBox);
            this.panel5.Location = new System.Drawing.Point(3, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(169, 111);
            this.panel5.TabIndex = 8;
            // 
            // alphaCheckBox
            // 
            this.alphaCheckBox.AutoSize = true;
            this.alphaCheckBox.Location = new System.Drawing.Point(6, 18);
            this.alphaCheckBox.Name = "alphaCheckBox";
            this.alphaCheckBox.Size = new System.Drawing.Size(75, 17);
            this.alphaCheckBox.TabIndex = 6;
            this.alphaCheckBox.Text = "Включить";
            this.alphaCheckBox.UseVisualStyleBackColor = true;
            this.alphaCheckBox.CheckedChanged += new System.EventHandler(this.alphaCheckBox_CheckedChanged);
            // 
            // alphaTrackBar
            // 
            this.alphaTrackBar.Location = new System.Drawing.Point(0, 68);
            this.alphaTrackBar.Maximum = 100;
            this.alphaTrackBar.Name = "alphaTrackBar";
            this.alphaTrackBar.Size = new System.Drawing.Size(166, 45);
            this.alphaTrackBar.TabIndex = 5;
            this.alphaTrackBar.Scroll += new System.EventHandler(this.alphaTrackBar_Scroll);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(79, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Прозрачность";
            // 
            // alphaComboBox
            // 
            this.alphaComboBox.FormattingEnabled = true;
            this.alphaComboBox.Location = new System.Drawing.Point(6, 41);
            this.alphaComboBox.Name = "alphaComboBox";
            this.alphaComboBox.Size = new System.Drawing.Size(149, 21);
            this.alphaComboBox.TabIndex = 4;
            this.alphaComboBox.SelectedIndexChanged += new System.EventHandler(this.alphaComboBox_SelectedIndexChanged);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.scissorReset);
            this.panel4.Controls.Add(this.button1);
            this.panel4.Controls.Add(this.scissorCheckBox);
            this.panel4.Controls.Add(this.label6);
            this.panel4.Location = new System.Drawing.Point(3, 120);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(169, 108);
            this.panel4.TabIndex = 7;
            // 
            // scissorReset
            // 
            this.scissorReset.Location = new System.Drawing.Point(6, 68);
            this.scissorReset.Name = "scissorReset";
            this.scissorReset.Size = new System.Drawing.Size(160, 23);
            this.scissorReset.TabIndex = 7;
            this.scissorReset.Text = "Сбросить выделение";
            this.scissorReset.UseVisualStyleBackColor = true;
            this.scissorReset.Click += new System.EventHandler(this.scissorReset_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 39);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(160, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Выбрать область";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // scissorCheckBox
            // 
            this.scissorCheckBox.AutoSize = true;
            this.scissorCheckBox.Location = new System.Drawing.Point(6, 16);
            this.scissorCheckBox.Name = "scissorCheckBox";
            this.scissorCheckBox.Size = new System.Drawing.Size(75, 17);
            this.scissorCheckBox.TabIndex = 5;
            this.scissorCheckBox.Text = "Включить";
            this.scissorCheckBox.UseVisualStyleBackColor = true;
            this.scissorCheckBox.CheckedChanged += new System.EventHandler(this.scissorCheckBox_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Отсечение";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label10);
            this.panel6.Controls.Add(this.label8);
            this.panel6.Controls.Add(this.blendDestination);
            this.panel6.Controls.Add(this.blendSource);
            this.panel6.Controls.Add(this.blendCheckBox);
            this.panel6.Controls.Add(this.label7);
            this.panel6.Location = new System.Drawing.Point(3, 234);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(169, 123);
            this.panel6.TabIndex = 9;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(0, 83);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "Destination";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 36);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Source";
            // 
            // blendDestination
            // 
            this.blendDestination.FormattingEnabled = true;
            this.blendDestination.Location = new System.Drawing.Point(6, 99);
            this.blendDestination.Name = "blendDestination";
            this.blendDestination.Size = new System.Drawing.Size(149, 21);
            this.blendDestination.TabIndex = 7;
            this.blendDestination.SelectedIndexChanged += new System.EventHandler(this.blendDestination_SelectedIndexChanged);
            // 
            // blendSource
            // 
            this.blendSource.FormattingEnabled = true;
            this.blendSource.Location = new System.Drawing.Point(6, 52);
            this.blendSource.Name = "blendSource";
            this.blendSource.Size = new System.Drawing.Size(149, 21);
            this.blendSource.TabIndex = 6;
            this.blendSource.SelectedIndexChanged += new System.EventHandler(this.blendSource_SelectedIndexChanged);
            // 
            // blendCheckBox
            // 
            this.blendCheckBox.AutoSize = true;
            this.blendCheckBox.Location = new System.Drawing.Point(6, 16);
            this.blendCheckBox.Name = "blendCheckBox";
            this.blendCheckBox.Size = new System.Drawing.Size(75, 17);
            this.blendCheckBox.TabIndex = 5;
            this.blendCheckBox.Text = "Включить";
            this.blendCheckBox.UseVisualStyleBackColor = true;
            this.blendCheckBox.CheckedChanged += new System.EventHandler(this.blendCheckBox_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Смешение цветов";
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage3.Controls.Add(this.panel8);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(181, 400);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Лаб. 3";
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.ChangeSeedButton);
            this.panel8.Controls.Add(this.label11);
            this.panel8.Controls.Add(this.fractalSteps);
            this.panel8.Location = new System.Drawing.Point(6, 3);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(169, 83);
            this.panel8.TabIndex = 6;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(100, 13);
            this.label11.TabIndex = 4;
            this.label11.Text = "Количество шагов";
            // 
            // fractalSteps
            // 
            this.fractalSteps.Location = new System.Drawing.Point(6, 25);
            this.fractalSteps.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.fractalSteps.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.fractalSteps.Name = "fractalSteps";
            this.fractalSteps.Size = new System.Drawing.Size(149, 20);
            this.fractalSteps.TabIndex = 3;
            this.fractalSteps.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.fractalSteps.ValueChanged += new System.EventHandler(this.fractalSteps_ValueChanged);
            // 
            // ChangeSeedButton
            // 
            this.ChangeSeedButton.Location = new System.Drawing.Point(6, 51);
            this.ChangeSeedButton.Name = "ChangeSeedButton";
            this.ChangeSeedButton.Size = new System.Drawing.Size(149, 23);
            this.ChangeSeedButton.TabIndex = 5;
            this.ChangeSeedButton.Text = "Изменить зерно";
            this.ChangeSeedButton.UseVisualStyleBackColor = true;
            this.ChangeSeedButton.Click += new System.EventHandler(this.ChangeSeedButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.glControl1);
            this.Icon = global::ogl2.Properties.Resources.opengl_logo;
            this.Name = "Form1";
            this.Text = "Печеркин А. С. 0381 Лаб. 1-3";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.alphaTrackBar)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fractalSteps)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private OpenTK.GLControl glControl1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.ComboBox cullingComboBox;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox cullingCheckBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox polygonMode1;
        private System.Windows.Forms.ComboBox polygonMode2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.CheckBox scissorCheckBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button scissorReset;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox alphaComboBox;
        private System.Windows.Forms.TrackBar alphaTrackBar;
        private System.Windows.Forms.CheckBox alphaCheckBox;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.ComboBox blendSource;
        private System.Windows.Forms.CheckBox blendCheckBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox blendDestination;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown fractalSteps;
        private System.Windows.Forms.Button ChangeSeedButton;
    }
}

