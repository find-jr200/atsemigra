namespace atsemigra
{
    partial class Main
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnOutput = new System.Windows.Forms.Button();
            this.ColorPalette = new System.Windows.Forms.PictureBox();
            this.CurrentColor = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCjrOutput = new System.Windows.Forms.Button();
            this.btnBinOutput = new System.Windows.Forms.Button();
            this.trkRed = new System.Windows.Forms.TrackBar();
            this.trkGreen = new System.Windows.Forms.TrackBar();
            this.trkBlue = new System.Windows.Forms.TrackBar();
            this.lblR_Value = new System.Windows.Forms.Label();
            this.lblG_Value = new System.Windows.Forms.Label();
            this.lblB_Value = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnColorReset = new System.Windows.Forms.Button();
            this.chkGuideLine = new System.Windows.Forms.CheckBox();
            this.txtScript = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnTextEnable = new System.Windows.Forms.Button();
            this.btnTextWrite = new System.Windows.Forms.Button();
            this.btnTextCjrOutput = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnInitialize = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorPalette)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkRed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkGreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkBlue)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(10, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(512, 384);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // btnOutput
            // 
            this.btnOutput.Location = new System.Drawing.Point(516, 63);
            this.btnOutput.Name = "btnOutput";
            this.btnOutput.Size = new System.Drawing.Size(75, 23);
            this.btnOutput.TabIndex = 13;
            this.btnOutput.Text = "BASIC出力";
            this.btnOutput.UseVisualStyleBackColor = true;
            this.btnOutput.Click += new System.EventHandler(this.btnOutput_Click);
            // 
            // ColorPalette
            // 
            this.ColorPalette.Location = new System.Drawing.Point(555, 99);
            this.ColorPalette.Name = "ColorPalette";
            this.ColorPalette.Size = new System.Drawing.Size(32, 256);
            this.ColorPalette.TabIndex = 4;
            this.ColorPalette.TabStop = false;
            this.ColorPalette.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ColorPalette_MouseClick);
            // 
            // CurrentColor
            // 
            this.CurrentColor.Location = new System.Drawing.Point(555, 40);
            this.CurrentColor.Name = "CurrentColor";
            this.CurrentColor.Size = new System.Drawing.Size(32, 32);
            this.CurrentColor.TabIndex = 5;
            this.CurrentColor.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(541, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "現在の色";
            // 
            // btnCjrOutput
            // 
            this.btnCjrOutput.Location = new System.Drawing.Point(516, 34);
            this.btnCjrOutput.Name = "btnCjrOutput";
            this.btnCjrOutput.Size = new System.Drawing.Size(75, 23);
            this.btnCjrOutput.TabIndex = 12;
            this.btnCjrOutput.Text = "CJR出力";
            this.btnCjrOutput.UseVisualStyleBackColor = true;
            this.btnCjrOutput.Click += new System.EventHandler(this.btnCjrOutput_Click);
            // 
            // btnBinOutput
            // 
            this.btnBinOutput.Location = new System.Drawing.Point(516, 7);
            this.btnBinOutput.Name = "btnBinOutput";
            this.btnBinOutput.Size = new System.Drawing.Size(75, 23);
            this.btnBinOutput.TabIndex = 11;
            this.btnBinOutput.Text = "BIN出力";
            this.btnBinOutput.UseVisualStyleBackColor = true;
            this.btnBinOutput.Click += new System.EventHandler(this.btnBinOutput_Click);
            // 
            // trkRed
            // 
            this.trkRed.Location = new System.Drawing.Point(43, 3);
            this.trkRed.Maximum = 255;
            this.trkRed.Name = "trkRed";
            this.trkRed.Size = new System.Drawing.Size(104, 45);
            this.trkRed.TabIndex = 0;
            this.trkRed.Value = 128;
            this.trkRed.ValueChanged += new System.EventHandler(this.trkRed_ValueChanged);
            // 
            // trkGreen
            // 
            this.trkGreen.Location = new System.Drawing.Point(148, 3);
            this.trkGreen.Maximum = 255;
            this.trkGreen.Name = "trkGreen";
            this.trkGreen.Size = new System.Drawing.Size(104, 45);
            this.trkGreen.TabIndex = 3;
            this.trkGreen.Value = 128;
            this.trkGreen.ValueChanged += new System.EventHandler(this.trkGreen_ValueChanged);
            // 
            // trkBlue
            // 
            this.trkBlue.Location = new System.Drawing.Point(253, 3);
            this.trkBlue.Maximum = 255;
            this.trkBlue.Name = "trkBlue";
            this.trkBlue.Size = new System.Drawing.Size(104, 45);
            this.trkBlue.TabIndex = 6;
            this.trkBlue.Value = 128;
            this.trkBlue.ValueChanged += new System.EventHandler(this.trkBlue_ValueChanged);
            // 
            // lblR_Value
            // 
            this.lblR_Value.AutoSize = true;
            this.lblR_Value.Location = new System.Drawing.Point(84, 36);
            this.lblR_Value.Name = "lblR_Value";
            this.lblR_Value.Size = new System.Drawing.Size(23, 12);
            this.lblR_Value.TabIndex = 2;
            this.lblR_Value.Text = "128";
            // 
            // lblG_Value
            // 
            this.lblG_Value.AutoSize = true;
            this.lblG_Value.Location = new System.Drawing.Point(190, 36);
            this.lblG_Value.Name = "lblG_Value";
            this.lblG_Value.Size = new System.Drawing.Size(23, 12);
            this.lblG_Value.TabIndex = 5;
            this.lblG_Value.Text = "128";
            // 
            // lblB_Value
            // 
            this.lblB_Value.AutoSize = true;
            this.lblB_Value.Location = new System.Drawing.Point(295, 35);
            this.lblB_Value.Name = "lblB_Value";
            this.lblB_Value.Size = new System.Drawing.Size(23, 12);
            this.lblB_Value.TabIndex = 8;
            this.lblB_Value.Text = "128";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(63, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "R:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(169, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(15, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "G:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(274, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(15, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "B:";
            // 
            // btnColorReset
            // 
            this.btnColorReset.Location = new System.Drawing.Point(137, 58);
            this.btnColorReset.Name = "btnColorReset";
            this.btnColorReset.Size = new System.Drawing.Size(129, 23);
            this.btnColorReset.TabIndex = 9;
            this.btnColorReset.Text = "ファイル再読み込み";
            this.btnColorReset.UseVisualStyleBackColor = true;
            this.btnColorReset.Click += new System.EventHandler(this.btnColorReset_Click);
            // 
            // chkGuideLine
            // 
            this.chkGuideLine.AutoSize = true;
            this.chkGuideLine.Location = new System.Drawing.Point(376, 58);
            this.chkGuideLine.Name = "chkGuideLine";
            this.chkGuideLine.Size = new System.Drawing.Size(70, 16);
            this.chkGuideLine.TabIndex = 10;
            this.chkGuideLine.Text = "Grid Line";
            this.chkGuideLine.UseVisualStyleBackColor = true;
            this.chkGuideLine.CheckedChanged += new System.EventHandler(this.chkGuideLine_CheckedChanged);
            // 
            // txtScript
            // 
            this.txtScript.Enabled = false;
            this.txtScript.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtScript.ImeMode = System.Windows.Forms.ImeMode.On;
            this.txtScript.Location = new System.Drawing.Point(136, 9);
            this.txtScript.Multiline = true;
            this.txtScript.Name = "txtScript";
            this.txtScript.Size = new System.Drawing.Size(233, 51);
            this.txtScript.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnTextEnable);
            this.panel1.Controls.Add(this.btnTextWrite);
            this.panel1.Controls.Add(this.btnTextCjrOutput);
            this.panel1.Controls.Add(this.txtScript);
            this.panel1.Location = new System.Drawing.Point(13, 500);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(602, 68);
            this.panel1.TabIndex = 3;
            // 
            // btnTextEnable
            // 
            this.btnTextEnable.Location = new System.Drawing.Point(3, 11);
            this.btnTextEnable.Name = "btnTextEnable";
            this.btnTextEnable.Size = new System.Drawing.Size(103, 23);
            this.btnTextEnable.TabIndex = 0;
            this.btnTextEnable.Text = "あっと漢字有効化";
            this.btnTextEnable.UseVisualStyleBackColor = true;
            this.btnTextEnable.Click += new System.EventHandler(this.btnTextEnable_Click);
            // 
            // btnTextWrite
            // 
            this.btnTextWrite.Enabled = false;
            this.btnTextWrite.Location = new System.Drawing.Point(403, 11);
            this.btnTextWrite.Name = "btnTextWrite";
            this.btnTextWrite.Size = new System.Drawing.Size(75, 23);
            this.btnTextWrite.TabIndex = 2;
            this.btnTextWrite.Text = "書き込み";
            this.btnTextWrite.UseVisualStyleBackColor = true;
            this.btnTextWrite.Click += new System.EventHandler(this.btnTextWrite_Click);
            // 
            // btnTextCjrOutput
            // 
            this.btnTextCjrOutput.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnTextCjrOutput.Enabled = false;
            this.btnTextCjrOutput.Location = new System.Drawing.Point(515, 11);
            this.btnTextCjrOutput.Name = "btnTextCjrOutput";
            this.btnTextCjrOutput.Size = new System.Drawing.Size(75, 23);
            this.btnTextCjrOutput.TabIndex = 3;
            this.btnTextCjrOutput.Text = "CJR出力";
            this.btnTextCjrOutput.UseVisualStyleBackColor = true;
            this.btnTextCjrOutput.Click += new System.EventHandler(this.btnTextCjrOutput_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.lblR_Value);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.chkGuideLine);
            this.panel2.Controls.Add(this.lblG_Value);
            this.panel2.Controls.Add(this.btnBinOutput);
            this.panel2.Controls.Add(this.btnColorReset);
            this.panel2.Controls.Add(this.lblB_Value);
            this.panel2.Controls.Add(this.btnOutput);
            this.panel2.Controls.Add(this.btnCjrOutput);
            this.panel2.Controls.Add(this.trkGreen);
            this.panel2.Controls.Add(this.trkRed);
            this.panel2.Controls.Add(this.trkBlue);
            this.panel2.Location = new System.Drawing.Point(12, 403);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(603, 91);
            this.panel2.TabIndex = 2;
            // 
            // btnInitialize
            // 
            this.btnInitialize.Location = new System.Drawing.Point(533, 373);
            this.btnInitialize.Name = "btnInitialize";
            this.btnInitialize.Size = new System.Drawing.Size(75, 23);
            this.btnInitialize.TabIndex = 1;
            this.btnInitialize.Text = "初期化";
            this.btnInitialize.UseVisualStyleBackColor = true;
            this.btnInitialize.Click += new System.EventHandler(this.btnInitialize_Click);
            // 
            // Main
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 573);
            this.Controls.Add(this.btnInitialize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CurrentColor);
            this.Controls.Add(this.ColorPalette);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "あっとセミグラ";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Main_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Main_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorPalette)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkRed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkGreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkBlue)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnOutput;
        private System.Windows.Forms.PictureBox ColorPalette;
        private System.Windows.Forms.PictureBox CurrentColor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCjrOutput;
        private System.Windows.Forms.Button btnBinOutput;
        private System.Windows.Forms.TrackBar trkRed;
        private System.Windows.Forms.TrackBar trkGreen;
        private System.Windows.Forms.TrackBar trkBlue;
        private System.Windows.Forms.Label lblR_Value;
        private System.Windows.Forms.Label lblG_Value;
        private System.Windows.Forms.Label lblB_Value;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnColorReset;
        private System.Windows.Forms.CheckBox chkGuideLine;
        private System.Windows.Forms.TextBox txtScript;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnTextCjrOutput;
        private System.Windows.Forms.Button btnInitialize;
        private System.Windows.Forms.Button btnTextWrite;
        private System.Windows.Forms.Button btnTextEnable;
    }
}

