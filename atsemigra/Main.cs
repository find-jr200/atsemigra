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
        Bitmap viewImage = new Bitmap(512, 384);
        Bitmap originalImage, currenColorImage, paletteImage;
        Color[] DigitalColor = new Color[8];
        int color = 7;
        int lastX, lastY;

        string head = @"10 CLS
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
            string[] fileName = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if (fileName.Length == 1)
            {
                string ext = System.IO.Path.GetExtension(fileName[0]);
                try
                {
                    Bitmap tmp = new Bitmap(fileName[0]);
                    originalImage = new Bitmap(tmp, 64, 48);
                    for (int x = 0; x < 64; ++x)
                    {
                        for (int y = 0; y < 48; ++y)
                        {
                            Color c = originalImage.GetPixel(x, y);
                            byte r = c.R, g = c.G, b = c.B;
                            if (r < 128)
                            {
                                r = 0;
                            } else
                            {
                                r = 255;
                            }

                            g = c.G;
                            if (g < 128)
                            {
                                g = 0;
                            }
                            else
                            {
                                g = 255;
                            }

                            b = c.B;
                            if (b < 128)
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

                using (Graphics gr = Graphics.FromImage(viewImage))
                {
                    for (int x = 0; x < 64; ++x)
                    {
                        for (int y = 0; y < 48; ++y)
                        {
                            Color c = originalImage.GetPixel(x, y);
                            SolidBrush br = new SolidBrush(c);
                            gr.FillRectangle(br, x * 8, y * 8, 8, 8);
                        }
                    }

                }
                this.pictureBox1.Image = viewImage;
            } else
            {
                System.Media.SystemSounds.Beep.Play();
            }
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
            this.pictureBox1.Refresh();

            originalImage.SetPixel(x, y, DigitalColor[color]);
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
                this.pictureBox1.Refresh();

                originalImage.SetPixel(x, y, DigitalColor[color]);
            }
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
                    using (FileStream stream = new FileStream(dlg.FileName, FileMode.Create, FileAccess.Write))
                    using (BinaryWriter writer = new BinaryWriter(stream))
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
                    using (FileStream stream = new FileStream(dlg.FileName, FileMode.Create, FileAccess.Write))
                    using (BinaryWriter writer = new BinaryWriter(stream))
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
    }
}
