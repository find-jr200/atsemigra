using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace atsemigra
{
    public partial class Main : Form
    {
        private const int VIEW_SIZE_W = 512;
        private const int VIEW_SIZE_H = 384;

        private const int TEXT_SIZE_W = 256;
        private const int TEXT_SIZE_H = 32;

        private const int ORG_SIZE_W = 64;
        private const int ORG_SIZE_H = 48;

        private const int DEFAULT_VAL = 128;

        private Bitmap viewImage, originalImage, currenColorImage, paletteImage, textImage;
        private Color[] DigitalColor;
        private int color = 7;
        private int lastX, lastY;
        private string[] fileName;
        private Size originalSize, viewSize, textSize;
        private bool bDrawLine = false, bTextEnable = false;
        private readonly Dictionary<int, byte[]> kanjidata;
        private byte[] charData;

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

            var formatter = new BinaryFormatter();
            try
            {
                using (var fileStr = new System.IO.FileStream("kanji.dat", System.IO.FileMode.Open))
                {
                    kanjidata = formatter.Deserialize(fileStr) as Dictionary<int, byte[]>;
                }
            }
            catch
            {
                MessageBox.Show("漢字データをロードできません。あっと漢字機能は使用できません。", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnTextEnable.Enabled = false;
                this.btnTextWrite.Enabled = false;
                this.btnTextCjrOutput.Enabled = false;
            }

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

            textSize.Width = 0;
            textSize.Height = 0;

            originalSize.Width = ORG_SIZE_W;
            originalSize.Height = ORG_SIZE_H;

            this.CurrentColor.Image = currenColorImage;
            this.ColorPalette.Image = paletteImage;

            using (var g = Graphics.FromImage(currenColorImage))
            {
                using (var br = new SolidBrush(DigitalColor[color]))
                    g.FillRectangle(br, 0, 0, 32, 32);
            }

            using (var g = Graphics.FromImage(paletteImage))
            {
                for (int i = 0; i < 8; ++i)
                    using (var br = new SolidBrush(DigitalColor[i]))
                        g.FillRectangle(br, 0, i * 31, 32, 32);
            }
            charData = new byte[128 * 8];
            ClearOrigimalImage(0);
        }


        private void Main_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void Main_DragDrop(object sender, DragEventArgs e)
        {
            this.trkBlue.Value = DEFAULT_VAL;
            this.trkGreen.Value = DEFAULT_VAL;
            this.trkRed.Value = DEFAULT_VAL;

            fileName = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            SetOriginalBitmap();
        }


        private void ColorPalette_MouseClick(object sender, MouseEventArgs e)
        {
            color = (int)(e.Y / 32);
            using (Graphics g = Graphics.FromImage(currenColorImage))
            {
                using (var br = new SolidBrush(DigitalColor[color]))
                    g.FillRectangle(br, 0, 0, 32, 32);
            }
            this.CurrentColor.Refresh();
        }



        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (viewImage == null || originalImage == null || e.Button != MouseButtons.Left)
                return;

            if (e.X < 0 || e.Y < 0 || e.X >= viewImage.Width || e.Y >= viewImage.Height - (textSize.Height * 2))
            {
                lastX = e.X;
                lastY = e.Y;
                return;
            }

            int x = (int)(e.X / 8);
            int y = (int)(e.Y / 8);

            originalImage.SetPixel(x, y, DigitalColor[color]);
            lastX = x;
            lastY = y;
            DrawViewImage();
        }


        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.X >= 0 && e.Y >= 0 && e.X < viewImage.Width && e.Y < viewImage.Height - textSize.Height)
            {
                if (viewImage == null || originalImage == null)
                    return;

                int x = (int)(e.X / 8);
                int y = (int)(e.Y / 8);

                using (var g = Graphics.FromImage(originalImage))
                {
                    using (var pen = new Pen(DigitalColor[color]))
                    g.DrawLine(pen, lastX, lastY, x, y);
                }
                lastX = x;
                lastY = y;

                DrawViewImage();
            }
        }



        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            DrawViewImage();
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
                    MessageBox.Show("書き込みに失敗しました", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnColorReset_Click(object sender, EventArgs e)
        {
            trkRed.Value = DEFAULT_VAL;
            trkGreen.Value = DEFAULT_VAL;
            trkBlue.Value = DEFAULT_VAL;

            SetOriginalBitmap();
        }

        private void btnTextCjrOutput_Click(object sender, EventArgs e)
        {
            byte[] tvram, avram;

            SetVramArray(out tvram, out avram, false);
            var cjrfile = new CjrFormat();
            cjrfile.AddBinData(charData, "GRAPH", 0xd400, false);
            cjrfile.AddBinData(tvram, "GRAPH", 0xc100, false, true);
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
                    using (var writer = new BinaryWriter(new FileStream(dlg.FileName, FileMode.Create, FileAccess.Write)))
                    {
                        writer.Write(cjrfile.GetCjrData());
                    }
                }
                catch
                {
                    MessageBox.Show("書き込みに失敗しました", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void chkGuideLine_CheckedChanged(object sender, EventArgs e)
        {
            bDrawLine = ((CheckBox)sender).Checked;
            if (originalImage != null)
                DrawViewImage();
        }

        private void btnInitialize_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show($"カラー {color} で初期化します。よろしいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;

            ClearOrigimalImage(color);
        }

        private void btnTextWrite_Click(object sender, EventArgs e)
        {
            string text = this.txtScript.Text.Trim();
            if (text.Length == 0)
                return;

            Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
            byte[] bytes = sjisEnc.GetBytes(text);

            using (var g = Graphics.FromImage(textImage))
            {
                using (var br = new SolidBrush(Color.Black))
                    g.FillRectangle(br, 0, 0, textSize.Width, textSize.Height);
            }

            for (int i = 0; i < 128 * 8; ++i)
                charData[i] = 0;

            int gx = 0, gy = 0;
            bool bLineFull = false;

            for (int i = 0; i < bytes.Length; i += 2)
            {
                byte b = bytes[i];
                int code = (b << 8) + bytes[i + 1];

                if (code == 0xd0a && !bLineFull)
                {
                    gy += 16;
                    if (gy >= 32) break;
                    gx = 0;
                    bLineFull = false;
                    continue;
                }

                byte[] kanji;
                try
                {
                    kanji = kanjidata[code];
                }
                catch
                {
                    continue;
                }

                for (int y = 0; y < 16; ++y)
                {
                    for (int x = 0; x < 16; ++x)
                    {
                        byte line = kanji[(int)(x / 8) + y * 2];
                        int mask = 0x80 >> (x >= 8 ? x - 8 : x);
                        int color;
                        if ((line & mask) != 0)
                            color = 7;
                        else
                            color = 0;

                        textImage.SetPixel(gx + x, gy + y, DigitalColor[color]);
                    }
                }

                int offset = ((int)(gy / 16) * 16) * 32 + (int)(gx / 16) * 16;

                for (int k = 0; k < 16; k += 2)
                {
                    charData[offset + k / 2] = kanji[k];
                    charData[offset + k / 2 + 8] = kanji[k + 1];

                    charData[offset + k / 2 + 256] = kanji[k + 16];
                    charData[offset + k / 2 + 8 + 256] = kanji[k + 17];
                }

                gx += 16;
                if (gx >= 256)
                {
                    bLineFull = true;
                    gx = 0;
                    gy += 16;
                    if (gy >= 32) break;
                }

            }
            DrawViewImage();
        }

        private void btnTextEnable_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show($"カラー {color} で初期化します。よろしいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;

            bTextEnable = !bTextEnable;

            if (bTextEnable)
            {
                textSize.Width = TEXT_SIZE_W;
                textSize.Height = TEXT_SIZE_H;

                originalSize.Width = ORG_SIZE_W;
                originalSize.Height = ORG_SIZE_H - 8;

                this.txtScript.Enabled = true;
                this.btnTextCjrOutput.Enabled = true;
                this.btnTextWrite.Enabled = true;

                this.btnBinOutput.Enabled = false;
                this.btnCjrOutput.Enabled = false;
                this.btnOutput.Enabled = false;

                textImage = new Bitmap(textSize.Width, textSize.Height);
                this.btnTextEnable.Text = "あっと漢字無効化";
            }
            else
            {
                textSize.Width = 0;
                textSize.Height = 0;

                originalSize.Width = ORG_SIZE_W;
                originalSize.Height = ORG_SIZE_H;

                this.txtScript.Enabled = false;
                this.btnTextCjrOutput.Enabled = false;
                this.btnTextWrite.Enabled = false;

                this.btnBinOutput.Enabled = true;
                this.btnCjrOutput.Enabled = true;
                this.btnOutput.Enabled = true;

                textImage = null;
                this.btnTextEnable.Text = "あっと漢字有効化";

            }
            ClearOrigimalImage(color);
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
                    MessageBox.Show("書き込みに失敗しました", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show("書き込みに失敗しました", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        /// <summary>
        /// OriginalImageビットマップデータをもとにJR-200のTVRAM, AVRAMのデータを作成
        /// テキスト領域有効の場合はフォントデータも追加
        /// </summary>
        /// <param name="tvram"></param>
        /// <param name="avram"></param>
        /// <param name="bImageOnly">テキスト領域向こうの場合true</param>
        void SetVramArray(out byte[] tvram, out byte[] avram, bool bImageOnly = true)
        {
            var tvlist = new List<byte>();
            var avlist = new List<byte>();

            for (int y = 0; y < originalSize.Height; y += 2)
            {
                for (int x = 0; x < originalSize.Width; x += 2)
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

            if (!bImageOnly)
            {
                for (int i = 0x80; i <= 0xff; ++i)
                {
                    tvlist.Add((byte)i);
                    avlist.Add(7);
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
            using (var gr = Graphics.FromImage(viewImage))
            {
                if (viewImage == null || originalImage == null) return;

                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                using (var brkBrush = new SolidBrush(Color.Black))
                    gr.FillRectangle(brkBrush, new Rectangle(0, 0, viewSize.Width, viewSize.Height));

                if (bTextEnable)
                    gr.DrawImage(textImage, 0, (192 - 32) * 2, viewSize.Width, textSize.Height * 2);

                using (var magPen = new Pen(DigitalColor[3])) {
                    for (int x = 0; x < originalSize.Width; ++x)
                    {
                        for (int y = 0; y < originalSize.Height; ++y)
                        {
                            Color c = originalImage.GetPixel(x, y);
                            using (var br = new SolidBrush(c))
                            {
                                if (bDrawLine)
                                {
                                    if (c == DigitalColor[0])
                                        gr.DrawRectangle(magPen, x * 8, y * 8, 8, 8);
                                    else
                                        gr.FillRectangle(br, x * 8, y * 8, 7, 7);
                                }
                                else
                                {
                                    gr.FillRectangle(br, x * 8, y * 8, 8, 8);
                                }
                            }
                        }
                    }
                }
            }
            this.pictureBox1.Image = viewImage;
        }

        /// <summary>
        /// OrigimalImageビットマップ初期化
        /// ドラッグドロップされたファイル名もnullに
        /// </summary>
        /// <param name="color">初期化する色</param>
        void ClearOrigimalImage(int color)
        {
            fileName = null;
            originalImage = new Bitmap(originalSize.Width, originalSize.Height);

            using (Graphics gr = Graphics.FromImage(originalImage))
            {
                if (viewImage == null || originalImage == null) return;
                using (var br = new SolidBrush(DigitalColor[color])) {
                    gr.FillRectangle(br, 0, 0, originalSize.Width, originalSize.Height);
                };
            }
            DrawViewImage();
        }
    }
}
