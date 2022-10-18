using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;


namespace atsemigra
{
    public partial class Main : Form
    {
        private const int VIEW_SIZE_W = 512;
        private const int VIEW_SIZE_H = 384;
        private const int DEFAULT_VAL = 128;

        private Bitmap viewImage, originalImage, currenColorImage, paletteImage;
        private Color[] DigitalColor;
        private int color = 7;
        private int lastX, lastY;
        private string[] fileName;
        private Size originalSize, viewSize;
        private bool bDrawLine = false;

        private string head = @"10 CLS
20 FOR I=$C100 TO $C3FF
30 READ T$
40 POKE I,VAL(""$""+T$)
50 READ A$
60 POKE I+$400,VAL(""$""+A$)
70 NEXT I
80 GOTO 80
";

        public Main()
        {
            InitializeComponent();

            DigitalColor = new Color[8];

            DigitalColor[0] = Color.FromArgb(0, 0, 0); // Black;
            DigitalColor[1] = Color.FromArgb(0, 0, 255); // Blue;
            DigitalColor[2] = Color.FromArgb(255, 0, 0); // Red;
            DigitalColor[3] = Color.FromArgb(255, 0, 255); // Magenta;
            DigitalColor[4] = Color.FromArgb(0, 255, 255); // SkyBlue;
            DigitalColor[5] = Color.FromArgb(0, 255, 0); // YellowGreen;
            DigitalColor[6] = Color.FromArgb(255, 255, 0); // Yellow;
            DigitalColor[7] = Color.FromArgb(255, 255, 255); // White;

            currenColorImage = new Bitmap(32, 32);
            paletteImage = new Bitmap(32, 256);
            viewImage = new Bitmap(VIEW_SIZE_W, VIEW_SIZE_H);

            viewSize.Width = VIEW_SIZE_W;
            viewSize.Height = VIEW_SIZE_H;

            originalSize.Width = 64;
            originalSize.Height = 48;

            this.CurrentColor.Image = currenColorImage;
            this.ColorPalette.Image = paletteImage;

            using (Graphics g = Graphics.FromImage(currenColorImage))
            {
                g.FillRectangle(new SolidBrush(DigitalColor[color]), 0, 0, 32, 32);
            }

            using (Graphics g = Graphics.FromImage(paletteImage))
            {
                for (int i = 0; i < 8; i++)
                    g.FillRectangle(new SolidBrush(DigitalColor[i]), 0, i * 31, 32, 32);
            }
        }


        private void Main_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Move;
            } else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void Main_DragDrop(object sender, DragEventArgs e)
        {
            fileName = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            SetOriginalBitmap();
        }


        private void ColorPalette_MouseClick(object sender, MouseEventArgs e)
        {
            color = (int)(e.Y / 32);
            using (Graphics g = Graphics.FromImage(currenColorImage))
            {
                g.FillRectangle(new SolidBrush(DigitalColor[color]), 0, 0, 32, 32);
            }
            this.CurrentColor.Refresh();
        }



        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (viewImage == null || originalImage == null || e.Button != MouseButtons.Left)
                return;

            int x = (int)(e.X / 8);
            int y = (int)(e.Y / 8);
            lastX = x;
            lastY = y;

            using (Graphics g = Graphics.FromImage(viewImage))
            {
                g.FillRectangle(new SolidBrush(DigitalColor[color]), x * 8, y * 8, 8, 8);
            }
            originalImage.SetPixel(x, y, DigitalColor[color]);

            DrawViewImage();
        }


        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.X >= 0 && e.Y >= 0 && e.X < viewImage.Width && e.Y < viewImage.Height)
            {
                if (viewImage == null || originalImage == null)
                    return;

                int x = (int)(e.X / 8);
                int y = (int)(e.Y / 8);


                using (Graphics g = Graphics.FromImage(viewImage))
                {
                    g.FillRectangle(new SolidBrush(DigitalColor[color]), x * 8, y * 8, 8, 8);
                }
                originalImage.SetPixel(x, y, DigitalColor[color]);

                DrawViewImage();
            }
        }


        private void trkRed_ValueChanged(object sender, EventArgs e)
        {
            SetOriginalBitmap();
            this.lblR_Value.Text = this.trkRed.Value.ToString();
        }

        private void trkGreen_ValueChanged(object sender, EventArgs e)
        {
            SetOriginalBitmap();
            this.lblG_Value.Text = this.trkGreen.Value.ToString();
        }

        private void trkBlue_ValueChanged(object sender, EventArgs e)
        {
            SetOriginalBitmap();
            this.lblB_Value.Text = this.trkBlue.Value.ToString();
        }






        /// <summary>
        /// BINファイル出力
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBinOutput_Click(object sender, EventArgs e)
        {
            byte[] tvram, avram;

            if (originalImage == null) return;

            SetVramArray(out tvram, out avram);

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.InitialDirectory = (System.Environment.GetFolderPath(Environment.SpecialFolder.Personal));
            dlg.Filter = "BINファイル(*.bin)|*.bin|すべてのファイル(*.*)|*.*";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (BinaryWriter writer = new BinaryWriter(new FileStream(dlg.FileName, FileMode.Create, FileAccess.Write)))
                    {
                        writer.Write(tvram);
                        writer.Write(avram);
                    }
                }
                catch
                {
                    MessageBox.Show("書き込みに失敗しました");
                }
            }
        }

        private void btnColorReset_Click(object sender, EventArgs e)
        {
            trkRed.Value = DEFAULT_VAL;
            lblR_Value.Text = trkRed.Value.ToString();

            trkGreen.Value = DEFAULT_VAL;
            lblG_Value.Text = trkGreen.Value.ToString();

            trkBlue.Value = DEFAULT_VAL;
            lblB_Value.Text = trkBlue.Value.ToString();

            SetOriginalBitmap();
        }

        private void chkGuideLine_CheckedChanged(object sender, EventArgs e)
        {
            bDrawLine = ((CheckBox)sender).Checked;
            if (originalImage != null)
                DrawViewImage();
        }


        /// <summary>
        /// CJRファイル出力
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCjrOutput_Click(object sender, EventArgs e)
        {
            byte[] tvram, avram;

            if (originalImage == null) return;

            SetVramArray(out tvram, out avram);

            var cjrfile = new CjrFormat();
            cjrfile.AddBinData(tvram, "GRAPH", 0xc100, false);
            cjrfile.AddBinData(avram, "GRAPH", 0xc500, false, true);
            cjrfile.CloseCjrData();
            cjrfile.SetBaudRate(CjrFormat.BaudRate.High);

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.InitialDirectory = (System.Environment.GetFolderPath(Environment.SpecialFolder.Personal));
            dlg.Filter = "CJRファイル(*.cjr)|*.cjr|すべてのファイル(*.*)|*.*";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (BinaryWriter writer = new BinaryWriter(new FileStream(dlg.FileName, FileMode.Create, FileAccess.Write)))
                    {
                        writer.Write(cjrfile.GetCjrData());
                    }
                }
                catch
                {
                    MessageBox.Show("書き込みに失敗しました");
                }
            }
        }


        /// <summary>
        /// Quick Type用テキスト出力
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOutput_Click(object sender, EventArgs e)
        {
            byte[] tvram, avram;

            if (originalImage == null) return;

            StringBuilder buff = new StringBuilder();
            int rownum = 1000;
            int count = 0;

            buff.Append(head);
            SetVramArray(out tvram, out avram);

            for (int i = 0; i < tvram.Length; ++i)
            {
                if (count == 0)
                {
                    buff.Append(rownum.ToString() + " DATA ");
                    rownum += 10;
                }

                count += 2;
                buff.Append(tvram[i].ToString("X"));
                buff.Append(",");
                buff.Append(avram[i].ToString("X"));

                if (count < 16)
                {
                    buff.Append(",");
                }
                else
                {
                    buff.Append("\r\n");
                    count = 0;
                }
            }

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.InitialDirectory = (System.Environment.GetFolderPath(Environment.SpecialFolder.Personal));
            dlg.Filter = "テキストファイル(*.txt)|*.txt|すべてのファイル(*.*)|*.*";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");

                try
                {
                    using (StreamWriter writer = new StreamWriter(dlg.FileName, true, sjisEnc))
                    {
                        writer.Write(buff.ToString());
                    }
                }
                catch
                {
                    MessageBox.Show("書き込みに失敗しました");
                }
            }
        }



        void SetVramArray(out byte[] tvram, out byte[] avram)
        {
            var tvlist = new List<byte>();
            var avlist = new List<byte>();

            for (int y = 0; y < 48; y += 2)
            {
                for (int x = 0; x < 64; x += 2)
                {
                    StringBuilder sbt = new StringBuilder();
                    StringBuilder sba = new StringBuilder();

                    sbt.Append(0);
                    sbt.Append(0);
                    Color c = originalImage.GetPixel(x + 1, y);
                    int r = c.R == 255 ? 1 : 0;
                    int g = c.G == 255 ? 1 : 0;
                    int b = c.B == 255 ? 1 : 0;
                    sbt.Append(g);
                    sbt.Append(r);
                    sbt.Append(b);

                    c = originalImage.GetPixel(x, y);
                    r = c.R == 255 ? 1 : 0;
                    g = c.G == 255 ? 1 : 0;
                    b = c.B == 255 ? 1 : 0;
                    sbt.Append(g);
                    sbt.Append(r);
                    sbt.Append(b);


                    sba.Append(1);
                    sba.Append(0);
                    c = originalImage.GetPixel(x + 1, y + 1);
                    r = c.R == 255 ? 1 : 0;
                    g = c.G == 255 ? 1 : 0;
                    b = c.B == 255 ? 1 : 0;
                    sba.Append(g);
                    sba.Append(r);
                    sba.Append(b);

                    c = originalImage.GetPixel(x, y + 1);
                    r = c.R == 255 ? 1 : 0;
                    g = c.G == 255 ? 1 : 0;
                    b = c.B == 255 ? 1 : 0;
                    sba.Append(g);
                    sba.Append(r);
                    sba.Append(b);

                    int t = Convert.ToInt32(sbt.ToString(), 2);
                    int a = Convert.ToInt32(sba.ToString(), 2);

                    tvlist.Add((byte)t);
                    avlist.Add((byte)a);
                }
            }
            tvram = tvlist.ToArray();
            avram = avlist.ToArray();
        }




        /// <summary>
        /// 画像ファイルの読み込み
        /// </summary>
        private void SetOriginalBitmap()
        {
            if (fileName != null && (fileName.Length == 1))
            {
                try
                {
                    originalImage = new Bitmap(new Bitmap(fileName[0]), originalSize);
                    viewImage = new Bitmap(viewSize.Width, viewSize.Height);

                    for (int x = 0; x < originalSize.Width; ++x)
                    {
                        for (int y = 0; y < originalSize.Height; ++y)
                        {
                            Color c = originalImage.GetPixel(x, y);
                            byte r = c.R, g = c.G, b = c.B;
                            if (r < this.trkRed.Value)
                            {
                                r = 0;
                            }
                            else
                            {
                                r = 255;
                            }

                            g = c.G;
                            if (g < this.trkGreen.Value)
                            {
                                g = 0;
                            }
                            else
                            {
                                g = 255;
                            }

                            b = c.B;
                            if (b < this.trkBlue.Value)
                            {
                                b = 0;
                            }
                            else
                            {
                                b = 255;
                            }
                            originalImage.SetPixel(x, y, Color.FromArgb(r, g, b));
                        }
                    }
                }
                catch
                {
                    System.Media.SystemSounds.Beep.Play();
                    return;
                }
                DrawViewImage();
            }
            else
            {
                System.Media.SystemSounds.Beep.Play();
            }
        }


        /// <summary>
        /// viewイメージ描画
        /// </summary>
        void DrawViewImage()
        {
            using (Graphics gr = Graphics.FromImage(viewImage))
            {
                var pen = new Pen(new SolidBrush(Color.Red), 1);
                for (int x = 0; x < originalSize.Width; ++x)
                {
                    for (int y = 0; y < originalSize.Height; ++y)
                    {
                        Color c = originalImage.GetPixel(x, y);
                        SolidBrush br = new SolidBrush(c);
                        gr.FillRectangle(br, x * 8, y * 8, 8, 8);
                        if (bDrawLine && (y % 2 == 0))
                            gr.DrawLine(pen, new Point(0, y * 8), new Point(viewSize.Width * 8, y * 8));
                    }
                    if (bDrawLine && x % 2 == 0)
                        gr.DrawLine(pen, new Point(x * 8, 0), new Point(x * 8, viewSize.Height * 8));
                }

            }
            this.pictureBox1.Image = viewImage;
        }
    }
}
