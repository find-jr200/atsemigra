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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ColorPalette)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentColor)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(512, 384);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // btnOutput
            // 
            this.btnOutput.Location = new System.Drawing.Point(495, 402);
            this.btnOutput.Name = "btnOutput";
            this.btnOutput.Size = new System.Drawing.Size(79, 22);
            this.btnOutput.TabIndex = 3;
            this.btnOutput.Text = "BASIC出力";
            this.btnOutput.UseVisualStyleBackColor = true;
            this.btnOutput.Click += new System.EventHandler(this.btnOutput_Click);
            // 
            // ColorPalette
            // 
            this.ColorPalette.Location = new System.Drawing.Point(542, 99);
            this.ColorPalette.Name = "ColorPalette";
            this.ColorPalette.Size = new System.Drawing.Size(32, 256);
            this.ColorPalette.TabIndex = 4;
            this.ColorPalette.TabStop = false;
            this.ColorPalette.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ColorPalette_MouseClick);
            // 
            // CurrentColor
            // 
            this.CurrentColor.Location = new System.Drawing.Point(542, 40);
            this.CurrentColor.Name = "CurrentColor";
            this.CurrentColor.Size = new System.Drawing.Size(32, 32);
            this.CurrentColor.TabIndex = 5;
            this.CurrentColor.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(528, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "現在の色";
            // 
            // btnCjrOutput
            // 
            this.btnCjrOutput.Location = new System.Drawing.Point(414, 402);
            this.btnCjrOutput.Name = "btnCjrOutput";
            this.btnCjrOutput.Size = new System.Drawing.Size(75, 23);
            this.btnCjrOutput.TabIndex = 2;
            this.btnCjrOutput.Text = "CJR出力";
            this.btnCjrOutput.UseVisualStyleBackColor = true;
            this.btnCjrOutput.Click += new System.EventHandler(this.btnCjrOutput_Click);
            // 
            // btnBinOutput
            // 
            this.btnBinOutput.Location = new System.Drawing.Point(333, 402);
            this.btnBinOutput.Name = "btnBinOutput";
            this.btnBinOutput.Size = new System.Drawing.Size(75, 23);
            this.btnBinOutput.TabIndex = 1;
            this.btnBinOutput.Text = "BIN出力";
            this.btnBinOutput.UseVisualStyleBackColor = true;
            this.btnBinOutput.Click += new System.EventHandler(this.btnBinOutput_Click);
            // 
            // Main
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 432);
            this.Controls.Add(this.btnBinOutput);
            this.Controls.Add(this.btnCjrOutput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CurrentColor);
            this.Controls.Add(this.ColorPalette);
            this.Controls.Add(this.btnOutput);
            this.Controls.Add(this.pictureBox1);
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
    }
}

