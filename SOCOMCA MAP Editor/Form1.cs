using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using D3D = Microsoft.DirectX.Direct3D;

namespace SOCOMCA_MAP_Editor
{
    public partial class Form1 : Form
    {
        private struct zdb_CDIPoly
        {
            public long Pointer_CDIPOLY;
            public long Header_Line_1;
            public long Header_Line_2;
            public long Header_Line_3;
            public long Header_Line_4;

            public long Pointer_Vertices;
            public long AlignmentPadding;

            public long vertexCount;
            public zdb_CDIPoly_Vertex[] Vertices;
        }
        private struct zdb_CDIPoly_Vertex
        {
            public Single X;
            public Single Y;
            public Single Z;
            public long Unknown4;
        }
        private List<zdb_CDIPoly> cdiPoly = new List<zdb_CDIPoly>();

        private double ZoomLevel = 1;
        private double ShiftXAmmount = 0;
        private double ShiftYAmmount = 0;

        private CA_Archive MyArchive = new CA_Archive();
        private int MapPreviewIndex = -1; private int MapPreviewCount = 0; private int MapPreviewFileIndex = -1;

        CA_Tiff myTiff = new CA_Tiff();

        private Device device; private bool deviceOn = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void PreviewRAWImage(byte[] imgBytes)
        {
            int i; int RGBCol;
            Bitmap BMP = new Bitmap(256, 256);
            Graphics Gfx = Graphics.FromImage(BMP); //pictureBox1.CreateGraphics();
            Brush tBrush;

            int x = 0;
            int y = 0;

            Gfx.Clear(Color.FromArgb(0));
            for (i = 0; i < imgBytes.Length; i += 4)
            {
                byte r = imgBytes[i];
                byte g = imgBytes[i + 1];
                byte b = imgBytes[i + 2];
                byte a = imgBytes[i + 3];

                RGBCol = 0;

                string HexVal = "FF" + r.ToString("X2") + g.ToString("X2") + b.ToString("X2");
                RGBCol = int.Parse(HexVal, System.Globalization.NumberStyles.HexNumber);

                tBrush = new SolidBrush(Color.FromArgb(RGBCol));
                Gfx.FillRectangle(tBrush, x, y, 1, 1);
                tBrush.Dispose();

                x++;
                if (x >= 256)
                {
                    x = 0;
                    y++;
                }
            }

            pictureBox1.Width = 256;
            pictureBox1.Height = 256;
            pictureBox1.Image = BMP;
            Gfx.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dlgRet;

            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "*.ZDB; *.ZAR|*.zdb;*.zar";
            dlgRet = openFileDialog1.ShowDialog();
            if (dlgRet.ToString() == "Cancel") { return; }

            int filesLoaded = MyArchive.OpenArchive(openFileDialog1.FileName);
            if (filesLoaded <= 0)
            {
                MessageBox.Show("Not a valid archive");
                return;
            }

            string[] dList = MyArchive.GetDIRListing("/RUN/RAW/MAPS").Split('\n');
            byte[] tBytes;
            MapPreviewCount = dList.Count() - 3;
            lblPreview.Text = "Preview (0/" + MapPreviewCount.ToString() + ")";

            this.Text = "[TEST] SCA Map Editor - " + openFileDialog1.FileName;
            int i = 0;
            for (i = 0; i < dList.Count(); i++)
            {
                string[] spc = dList[i].Split(' ');
                if (spc[0] == "[FILE]")
                {
                    MapPreviewIndex = i - 1;
                    MapPreviewFileIndex = int.Parse(spc[1]);
                    MyArchive.GetFileBytes(MapPreviewFileIndex, out tBytes);
                    PreviewRAWImage(tBytes);

                    lblPreview.Text = "Preview (" + MapPreviewIndex.ToString() + "/" + MapPreviewCount.ToString() + ")";
                    return;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dlgRet;
            string fName;
            int i; int RGBCol;
            Bitmap BMP = new Bitmap(256, 256);
            Graphics Gfx = Graphics.FromImage(BMP); //pictureBox1.CreateGraphics();
            Brush tBrush;

            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "*.RAW|*.raw";
            dlgRet = openFileDialog1.ShowDialog();
            if (dlgRet.ToString() == "Cancel") { return; }
            fName = openFileDialog1.FileName;

            byte[] rgba;
            if (System.IO.File.Exists(fName) == false) { return; }

            try
            {
                rgba = System.IO.File.ReadAllBytes(fName);
            }
            catch
            {
                return;
            }

            int x = 0;
            int y = 0;

            Gfx.Clear(Color.FromArgb(0));
            for (i = 0; i < rgba.Length; i += 4)
            {
                byte r = rgba[i];
                byte g = rgba[i + 1];
                byte b = rgba[i + 2];
                byte a = rgba[i + 3];

                RGBCol = 0;

                string HexVal = "FF" + r.ToString("X2") + g.ToString("X2") + b.ToString("X2");
                RGBCol = int.Parse(HexVal, System.Globalization.NumberStyles.HexNumber);

                //MessageBox.Show(HexVal + "\n" + r.ToString() + "\n" + g.ToString() + " " + b.ToString() + " " + a.ToString() + "\n" + RGBCol.ToString());

                tBrush = new SolidBrush(Color.FromArgb(RGBCol));
                Gfx.FillRectangle(tBrush, x, y, 1, 1);
                tBrush.Dispose();

                x++;
                if (x >= 256)
                {
                    x = 0;
                    y++;
                }
            }

            pictureBox1.Image = BMP;
            Gfx.Dispose();
            //BMP.Dispose();
            MessageBox.Show("Done.");
        }

        private void button3_Click(object sender, EventArgs e)
        {

            DialogResult dlgRet;
            string fName;
            int i; int tiffAddr; int tmpAddr; int mapSize; int mapAddr; int rawAddr; int texWidth; int texHeight; int colAddr;

            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "*.*|*.*";
            dlgRet = openFileDialog1.ShowDialog();
            if (dlgRet.ToString() == "Cancel") { return; }
            fName = openFileDialog1.FileName;

            int ret = myTiff.ExtractTIFF(fName, int.Parse(txtAddr.Text, System.Globalization.NumberStyles.HexNumber));

            //MessageBox.Show(myTiff.ColorPallette.BitSize.ToString());
            //Bitmap tBMP = new Bitmap(myTiff.Header.Width, myTiff.Header.Height);
            //Graphics tGfx = Graphics.FromImage(tBMP);
            
            pictureBox1.Width = myTiff.Header.Width;
            pictureBox1.Height = myTiff.Header.Height;
            pictureBox1.Image = myTiff.Image;
            //tGfx.Dispose();

            return;
            byte[] fData;
            if (System.IO.File.Exists(fName) == false) { return; }

            try
            {
                fData = System.IO.File.ReadAllBytes(fName);
            }
            catch
            {
                return;
            }

            tiffAddr = int.Parse(txtAddr.Text, System.Globalization.NumberStyles.HexNumber);

            // Map Size
            tmpAddr = tiffAddr + 0x3c;
            mapSize = (fData[tmpAddr + 3] * 0x1000000) + (fData[tmpAddr + 2] * 0x10000) + (fData[tmpAddr + 1] * 0x100) + fData[tmpAddr + 0];

            // Map Address
            tmpAddr = tiffAddr + 0x54;
            mapAddr = (fData[tmpAddr + 3] * 0x1000000) + (fData[tmpAddr + 2] * 0x10000) + (fData[tmpAddr + 1] * 0x100) + fData[tmpAddr + 0];

            // RAW Address
            tmpAddr = tiffAddr + 0x58;
            rawAddr = (fData[tmpAddr + 3] * 0x1000000) + (fData[tmpAddr + 2] * 0x10000) + (fData[tmpAddr + 1] * 0x100) + fData[tmpAddr + 0];

            // Texture Height
            tmpAddr = tiffAddr + 0x62;
            texHeight = (fData[tmpAddr + 1] * 0x100) + fData[tmpAddr + 0];

            // Texture Width
            tmpAddr = tiffAddr + 0x64;
            texWidth = (fData[tmpAddr + 1] * 0x100) + fData[tmpAddr + 0];
            
            // Color Pallette
            tmpAddr = rawAddr + 0x14;
            colAddr = (fData[tmpAddr + 3] * 0x1000000) + (fData[tmpAddr + 2] * 0x10000) + (fData[tmpAddr + 1] * 0x100) + fData[tmpAddr + 0];

            MessageBox.Show("Size: " + mapSize.ToString("X8") + "\n" +
                            "Map: " + mapAddr.ToString("X8") + "\n" +
                            "RAW: " + rawAddr.ToString("X8") + "\n" +
                            "COL: " + colAddr.ToString("X8") + "\n" +
                            "Width: " + texWidth.ToString() + "\n" +
                            "Height: " + texHeight.ToString());


            pictureBox1.Width = texWidth;
            pictureBox1.Height = texHeight;

            Bitmap BMP = new Bitmap(texWidth, texHeight);
            //Graphics Gfx = pictureBox1.CreateGraphics();
            Graphics Gfx = Graphics.FromImage(BMP);
            Brush tBrush;
            Gfx.Clear(Color.FromArgb(0));

            int x; int y; int colR; int colG; int colB; int colA; int tmpCol; int tColAddr; int rgbaCol;
            i = 0;
            for (x = 0; x < texWidth; x++) 
            {
                for (y = 0; y < texHeight; y++)
                {
                    int tmp = fData[mapAddr + i];
                    tmp /= 8;
                    tmp *= 16;
                    int tmp2 = fData[mapAddr + i];
                    tmp += (tmp2 * 2);

                    tColAddr = colAddr + tmp; //colAddr + (fData[mapAddr + i] * 2);
                    tmpCol = (fData[tColAddr + 1] * 0x100) + fData[tColAddr + 0];

                    colR = (tmpCol & 0x1F);
                    colG = ((tmpCol & 0x3E0) / 0x20);
                    colB = ((tmpCol & 0x7C00) / 0x400);
                    colA = (tmpCol & 0x8000) / 0x8000;
                    if (colA == 1) { colA = 255; }

                    colR = (colR * 255) / 31;
                    colG = (colG * 255) / 31;
                    colB = (colB * 255) / 31;

                    rgbaCol = (colA * 0x1000000) + (colR * 0x10000) + (colG * 0x100) + colB;

                    if (x == 85 && y == 118)
                    {
                        MessageBox.Show(tColAddr.ToString("X8") + " " + tmpCol.ToString("X4") + "\n" + 
                                         rgbaCol.ToString("X8") + "\n" +
                                         x.ToString() + ", " + y.ToString() + " -> " + ((y * texWidth) + x).ToString() + "\n" +
                                         (mapAddr + i).ToString("X8") + " " + fData[mapAddr + i].ToString("X2"));
                    }
                    

                    tBrush = new SolidBrush(Color.FromArgb(colA, colR, colG, colB));
                    Gfx.FillRectangle(tBrush, x, y, 1, 1);
                    tBrush.Dispose();

                    i++;
                }
            }

            pictureBox1.Image = BMP;
            Gfx.Dispose();
            //BMP.Dispose();
            MessageBox.Show("Done.");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MapPreviewCount <= 0) { return; }
            if (MapPreviewIndex < 0) { return; }


            string[] dList = MyArchive.GetDIRListing("/RUN/RAW/MAPS").Split('\n');
            byte[] tBytes;


            if (MapPreviewIndex >= MapPreviewCount) { MapPreviewIndex = 0; }
            MapPreviewIndex++;

            int i = MapPreviewIndex + 1;
            string[] spc = dList[i].Split(' ');
            MapPreviewFileIndex = int.Parse(spc[1]);
            MyArchive.GetFileBytes(MapPreviewFileIndex, out tBytes);
            PreviewRAWImage(tBytes);

            lblPreview.Text = "Preview (" + MapPreviewIndex.ToString() + "/" + MapPreviewCount.ToString() + ")";
            /*
            int i = 0; bool isNext = false;
            for (i = 0; i < dList.Count(); i++)
            {
                string[] spc = dList[i].Split(' ');
                if (spc[0] == "[FILE]")
                {
                    if (isNext)
                    {
                        MapPreviewIndex = i - 1;
                        MapPreviewFileIndex = int.Parse(spc[1]);
                        MyArchive.GetFileBytes(MapPreviewFileIndex, out tBytes);
                        PreviewRAWImage(tBytes);

                        lblPreview.Text = "Preview (" + MapPreviewIndex.ToString() + "/" + MapPreviewCount.ToString() + ")";
                        return;
                    }

                    if (int.Parse(spc[1]) == MapPreviewFileIndex) { isNext = true; }
                }
            }
            */

        }

        private void button5_Click(object sender, EventArgs e)
        {
            frmRDR RDROpener = new frmRDR();
            RDROpener.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult dlgRet;
            string fName;

            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "*.RAW; *.BIN|*.raw;*.bin|*.* Any|*.*";
            dlgRet = openFileDialog1.ShowDialog();
            if (dlgRet.ToString() == "Cancel") { return; }
            fName = openFileDialog1.FileName;

            byte[] fData;
            if (System.IO.File.Exists(fName) == false) { return; }

            try
            {
                fData = System.IO.File.ReadAllBytes(fName);
            }
            catch
            {
                return;
            }

            int i; int i2; long Mem32; int polyCount = 0;
            zdb_CDIPoly tmpPoly = new zdb_CDIPoly();
            cdiPoly.Clear();
            ZoomLevel = 1;
            ShiftXAmmount = 0;
            ShiftYAmmount = 0;

            for (i = 0; i < fData.Count(); i += 4)
            {
                Mem32 = (fData[i + 3] * 0x1000000) + (fData[i + 2] * 0x10000) + (fData[i + 1] * 0x100) + fData[i];
                if (Mem32 == 0x00843264)//0x006e5880)
                {
                    tmpPoly.Pointer_CDIPOLY = Mem32;
                    i += 4;
                    tmpPoly.Header_Line_1 = (fData[i + 3] * 0x1000000) + (fData[i + 2] * 0x10000) + (fData[i + 1] * 0x100) + fData[i];
                    i += 4;
                    tmpPoly.Header_Line_2 = (fData[i + 3] * 0x1000000) + (fData[i + 2] * 0x10000) + (fData[i + 1] * 0x100) + fData[i];
                    i += 4;
                    tmpPoly.Header_Line_3 = (fData[i + 3] * 0x1000000) + (fData[i + 2] * 0x10000) + (fData[i + 1] * 0x100) + fData[i];
                    tmpPoly.vertexCount = fData[i] / 4;
                    i += 4;
                    tmpPoly.Header_Line_4 = (fData[i + 3] * 0x1000000) + (fData[i + 2] * 0x10000) + (fData[i + 1] * 0x100) + fData[i];
                    i += 4;
                    tmpPoly.Pointer_Vertices = (fData[i + 3] * 0x1000000) + (fData[i + 2] * 0x10000) + (fData[i + 1] * 0x100) + fData[i];

                    if (tmpPoly.Pointer_Vertices > 0)
                    {
                        tmpPoly.AlignmentPadding = tmpPoly.Pointer_Vertices - i;

                        i += (int)tmpPoly.AlignmentPadding;

                        tmpPoly.Vertices = new zdb_CDIPoly_Vertex[tmpPoly.vertexCount];
                        for (i2 = 0; i2 < tmpPoly.vertexCount; i2++)
                        {
                            Mem32 = (fData[i + 3] * 0x1000000) + (fData[i + 2] * 0x10000) + (fData[i + 1] * 0x100) + fData[i]; i += 4;
                            tmpPoly.Vertices[i2].X = BitConverter.ToSingle(BitConverter.GetBytes(Mem32), 0);
                            Mem32 = (fData[i + 3] * 0x1000000) + (fData[i + 2] * 0x10000) + (fData[i + 1] * 0x100) + fData[i]; i += 4;
                            tmpPoly.Vertices[i2].Y = BitConverter.ToSingle(BitConverter.GetBytes(Mem32), 0);
                            Mem32 = (fData[i + 3] * 0x1000000) + (fData[i + 2] * 0x10000) + (fData[i + 1] * 0x100) + fData[i]; i += 4;
                            tmpPoly.Vertices[i2].Z = BitConverter.ToSingle(BitConverter.GetBytes(Mem32), 0);
                            Mem32 = (fData[i + 3] * 0x1000000) + (fData[i + 2] * 0x10000) + (fData[i + 1] * 0x100) + fData[i]; i += 4;
                            tmpPoly.Vertices[i2].Unknown4 = Mem32;
                        }

                        cdiPoly.Add(tmpPoly);
                        polyCount++;
                    }
                }
            }

            Single smallestX = 1000000; Single largestX = -1000000;
            Single smallestY = 1000000; Single largestY = -1000000;
            Single smallestZ = 1000000; Single largestZ = -1000000;
            for (i = 0; i < cdiPoly.Count; i++)
            {
                if (cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[0].X < smallestX) { smallestX = cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[0].X; }
                if (cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[0].Y < smallestY) { smallestY = cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[0].Y; }
                if (cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[0].Z < smallestZ) { smallestZ = cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[0].Z; }

                if (cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[0].X > largestX) { largestX = cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[0].X; }
                if (cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[0].Y > largestY) { largestY = cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[0].Y; }
                if (cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[0].Z > largestZ) { largestZ = cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[0].Z; }
            }

            MessageBox.Show(polyCount.ToString() + " Total CDIPoly"
                            + "\nSmallest X: " + smallestX.ToString()
                            + "\nSmallest Y: " + smallestY.ToString()
                            + "\nSmallest Z: " + smallestZ.ToString()
                            + "\nLargest X: " + largestX.ToString()
                            + "\nLargest Y: " + largestY.ToString()
                            + "\nLargest Z: " + largestZ.ToString());


        }

        private void button7_Click(object sender, EventArgs e)
        {
            int i; int i2;

            Bitmap BMP = new Bitmap(800, 600);
            Graphics Gfx = Graphics.FromImage(BMP);
            Brush tBrush;
            Gfx.Clear(Color.FromArgb(0));

            for (i = 0; i < cdiPoly.Count; i++)
            {
                /*
                for (i2 = 0; i2 < cdiPoly.ElementAt<zdb_CDIPoly>(i).vertexCount; i2++)
                {
                    cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2].X /= 1;
                    cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2].Y /= 1;
                    cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2].Z /= 1;

                    cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2].X += 200;
                    cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2].Y += 200;
                    cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2].Z += 0;

                }

                RotateCDIPoly(cdiPoly.ElementAt<zdb_CDIPoly>(i), Math.PI / 4, Math.Atan(Math.Sqrt(2)));
                */
                for (i2 = 0; i2 < cdiPoly.ElementAt<zdb_CDIPoly>(i).vertexCount; i2++)
                {
                    double tempX1 = System.Convert.ToDouble(cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2].X);
                    double tempY1 = System.Convert.ToDouble(cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2].Y);


                    double xy1x = (tempX1 * ZoomLevel) + ShiftXAmmount;
                    double xy1y = (tempY1 * ZoomLevel) + ShiftYAmmount;
                    double xy2x = 0;
                    double xy2y = 0;

                    if ((i2 + 1) < cdiPoly.ElementAt<zdb_CDIPoly>(i).vertexCount)
                    {
                        double tempX2 = System.Convert.ToDouble(cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2 + 1].X);
                        double tempY2 = System.Convert.ToDouble(cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2 + 1].Y);

                        xy2x = (tempX2 * ZoomLevel) + ShiftXAmmount;
                        xy2y = (tempY2 * ZoomLevel) + ShiftYAmmount;
                    }
                    else
                    {
                        double tempX2 = System.Convert.ToDouble(cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[0].X);
                        double tempY2 = System.Convert.ToDouble(cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[0].Y);

                        xy2x = (tempX2 * ZoomLevel) + ShiftXAmmount;
                        xy2y = (tempY2 * ZoomLevel) + ShiftYAmmount;
                        
                    }
                    Gfx.DrawLine(Pens.Black, System.Convert.ToSingle(xy1x), System.Convert.ToSingle(xy1y),
                                            System.Convert.ToSingle(xy2x), System.Convert.ToSingle(xy2y));

                    //Gfx.DrawLine(Pens.Black, (int)Math.Round(xy1x), (int)Math.Round(xy1y), (int)Math.Round(xy2x), (int)Math.Round(xy2y));
                }
            }

            pictureBox1.Image = BMP;
            Gfx.Dispose();
            pictureBox1.Refresh();

            /*
             * 
            var g = args.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.Clear(Color.White);

            g.TranslateTransform(Width / 2, Height / 2);

            foreach (var edge in edges)
            {
                double[] xy1 = nodes[edge[0]];
                double[] xy2 = nodes[edge[1]];
                g.DrawLine(Pens.Black, (int)Math.Round(xy1[0]), (int)Math.Round(xy1[1]),
                        (int)Math.Round(xy2[0]), (int)Math.Round(xy2[1]));
            }
            */
        }
        private void RotateCDIPoly(zdb_CDIPoly cPoly, double angleX, double angleY)
        {
            double sinX = Math.Sin(angleX);
            double cosX = Math.Cos(angleX);

            double sinY = Math.Sin(angleY);
            double cosY = Math.Cos(angleY);

            int i;
            for (i = 0; i < cPoly.vertexCount; i++)
            {
                double x = System.Convert.ToDouble(cPoly.Vertices[i].X);
                double y = System.Convert.ToDouble(cPoly.Vertices[i].Y);
                double z = System.Convert.ToDouble(cPoly.Vertices[i].Z);

                cPoly.Vertices[i].X = System.Convert.ToSingle((x * cosX - z * sinX));
                cPoly.Vertices[i].Z = System.Convert.ToSingle((z * cosX + x * sinX));

                z = System.Convert.ToDouble(cPoly.Vertices[i].Z);

                cPoly.Vertices[i].Y = System.Convert.ToSingle((y * cosY - z * sinY));
                cPoly.Vertices[i].Z = System.Convert.ToSingle((z * cosY + y * sinY));
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            /*
            int i; int i2;
            for (i = 0; i < cdiPoly.Count; i++)
            {

                for (i2 = 0; i2 < cdiPoly.ElementAt<zdb_CDIPoly>(i).vertexCount; i2++)
                {
                    cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2].X *= 1.01f;
                    cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2].Y *= 1.01f;
                    cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2].Z *= 1.01f;

                    //cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2].X += 200;
                    //cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2].Y += 200;
                    //cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2].Z += 0;

                }
            }
            */
            ZoomLevel += 0.01f;
            button7_Click(sender, e);
        }
        private void button8_Click(object sender, EventArgs e)
        {
            /*
            int i; int i2;
            for (i = 0; i < cdiPoly.Count; i++)
            {

                for (i2 = 0; i2 < cdiPoly.ElementAt<zdb_CDIPoly>(i).vertexCount; i2++)
                {
                    cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2].X /= 1.01f;
                    cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2].Y /= 1.01f;
                    cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2].Z /= 1.01f;

                    //cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2].X += 200;
                    //cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2].Y += 200;
                    //cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2].Z += 0;

                }
            }
            */
            ZoomLevel -= 0.01f;
            button7_Click(sender, e);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            int i;
            for (i = 0; i < cdiPoly.Count; i++)
            {
                RotateCDIPoly(cdiPoly.ElementAt<zdb_CDIPoly>(i), .02, 0);
            }
            button7_Click(sender, e);
        }
        private void button11_Click(object sender, EventArgs e)
        {
            int i;
            for (i = 0; i < cdiPoly.Count; i++)
            {
                RotateCDIPoly(cdiPoly.ElementAt<zdb_CDIPoly>(i), 0, 0.02);
            }
            button7_Click(sender, e);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            /*
            int i; int i2;
            for (i = 0; i < cdiPoly.Count; i++)
            {

                for (i2 = 0; i2 < cdiPoly.ElementAt<zdb_CDIPoly>(i).vertexCount; i2++)
                {
                    cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2].X -= 50;
                    cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2].Y -= 0;
                    cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2].Z -= 0;

                }
            }
            */
            ShiftXAmmount -= 50;
            button7_Click(sender, e);
        }
        private void button12_Click(object sender, EventArgs e)
        {
            /*
            int i; int i2;
            for (i = 0; i < cdiPoly.Count; i++)
            {

                for (i2 = 0; i2 < cdiPoly.ElementAt<zdb_CDIPoly>(i).vertexCount; i2++)
                {
                    cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2].X += 50;
                    cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2].Y += 0;
                    cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2].Z += 0;

                }
            }
            */
            ShiftXAmmount += 50;
            button7_Click(sender, e);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            /*
            int i; int i2;
            for (i = 0; i < cdiPoly.Count; i++)
            {

                for (i2 = 0; i2 < cdiPoly.ElementAt<zdb_CDIPoly>(i).vertexCount; i2++)
                {
                    cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2].X -= 0;
                    cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2].Y -= 10;
                    cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2].Z -= 0;

                }
            }
            */
            ShiftYAmmount -= 10;
            button7_Click(sender, e);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            /*
            int i; int i2;
            for (i = 0; i < cdiPoly.Count; i++)
            {

                for (i2 = 0; i2 < cdiPoly.ElementAt<zdb_CDIPoly>(i).vertexCount; i2++)
                {
                    cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2].X += 0;
                    cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2].Y += 10;
                    cdiPoly.ElementAt<zdb_CDIPoly>(i).Vertices[i2].Z += 0;

                }
            }
            */
            ShiftYAmmount += 10;
            button7_Click(sender, e);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MouseEventArgs mE = (MouseEventArgs)e;

            if (pictureBox1.Image == null) { return; }
            Bitmap BMP = new Bitmap(pictureBox1.Image);
            lblPreview.Text = "(" + mE.X.ToString() + ", " + mE.Y.ToString() + "): " + BMP.GetPixel(mE.X, mE.Y).ToString() + "\n" +
                              "Scale Width: " + (4096 / pictureBox1.Image.Width).ToString() + "; Scale Height: " + (4096 / pictureBox1.Image.Height).ToString() + "\n" +
                              "(" + (mE.X * (4096 / pictureBox1.Image.Width)).ToString() + ", " + (mE.Y * (4096 / pictureBox1.Image.Height)).ToString() + ")" + "\n" +
                              "(-" + (4096 - (mE.X * (4096 / pictureBox1.Image.Width))).ToString() + ", " + (mE.Y * (4096 / pictureBox1.Image.Height)).ToString() + ")";
            BMP.Dispose();
        }










        private float angle = 0f;
        private CustomVertex.PositionColored[] vertices;
        myownvertexformat[] verticesT = new myownvertexformat[3];
        System.IO.Stream txTest = new System.IO.MemoryStream();
        private VertexBuffer vb;
        private VertexDeclaration vd;
        private Effect effect;

        private Texture testTexture;

        private Matrix matView;
        private Matrix matProjection;

        struct myownvertexformat
        {
            public Vector3 Pos;
            public Vector2 TexCoord;

            public myownvertexformat(Vector3 _Pos, float texx, float texy)
            {
                Pos = _Pos;
                TexCoord.X = texx;
                TexCoord.Y = texy;
            }
        }

        private void AllocateResources()
        {
            vb = new VertexBuffer(typeof(myownvertexformat), 3, device, Usage.WriteOnly, VertexFormats.Position | VertexFormats.Normal | VertexFormats.Texture0, Pool.Managed);
            //effect = D3D.Effect.FromFile(device, @"../../OurHLSLFile.fx", null, null, ShaderFlags.None, null);
        }
        private void button16_Click(object sender, EventArgs e)
        {
            // Initialize
            pictureBox1.Width = 640; pictureBox1.Height = 480;
            
            PresentParameters presentParams = new PresentParameters();
            presentParams.Windowed = true;
            presentParams.SwapEffect = SwapEffect.Discard;
            device = new Device(0, DeviceType.Hardware, pictureBox1, CreateFlags.SoftwareVertexProcessing, presentParams);
            AllocateResources();


            device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4, pictureBox1.Width / pictureBox1.Height, 1f, 50f);
            device.Transform.View = Matrix.LookAtLH(new Vector3(0, 0, -30), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            device.RenderState.Lighting = false;
            device.RenderState.CullMode = Cull.None;

            vertices = new CustomVertex.PositionColored[3];
            vertices[0].Position = new Vector3(0f, 0f, 0f);
            vertices[0].Color = Color.Red.ToArgb();
            vertices[1].Position = new Vector3(10f, 0f, 0f);
            vertices[1].Color = Color.Green.ToArgb();
            vertices[2].Position = new Vector3(5f, 10f, 0f);
            vertices[2].Color = Color.Yellow.ToArgb();


            verticesT[0] = new myownvertexformat(new Vector3(2, -2, -2), 0.0f, 0.0f);
            verticesT[1] = new myownvertexformat(new Vector3(0, 2, 0), 0.125f, 1.0f);
            verticesT[2] = new myownvertexformat(new Vector3(-2, -2, 2), 0.25f, 0.0f);

            vb.SetData(verticesT, 0, LockFlags.None);

            VertexElement[] velements = new VertexElement[]
            {
                 new VertexElement(0, 0, DeclarationType.Float3,
                                         DeclarationMethod.Default,
                                         DeclarationUsage.Position, 0),

                 new VertexElement(0, 12, DeclarationType.Float2,
                                          DeclarationMethod.Default,
                                          DeclarationUsage.TextureCoordinate, 0),

                 VertexElement.VertexDeclarationEnd
            };
            vd = new VertexDeclaration(device, velements);

            myTiff.Image.Save("C:\\ISO\\Dumps\\CA\\Texture.bmp");
            testTexture = TextureLoader.FromFile(device, "C:\\ISO\\Dumps\\CA\\Texture.bmp");

            dxTimer.Enabled = true;
            
        }

        private void dxTimer_Tick(object sender, EventArgs e)
        {
            device.Clear(ClearFlags.Target, Color.Black, 1.0f, 0); // Clear the window to white
            device.BeginScene();

            device.Transform.World = Matrix.Translation(-5, -10 * 1 / 3, 0) * Matrix.RotationAxis(new Vector3(angle * 4, angle * 2, angle * 3), angle);
            // Rendering is done here

            //device.VertexFormat = VertexFormats.Position | VertexFormats.Diffuse;
            //device.DrawUserPrimitives(PrimitiveType.TriangleList, 1, vertices);

            device.SetStreamSource(0, vb, 0);
            device.VertexDeclaration = vd;
            //effect.Technique = "Simplest";
            //effect.SetValue("xViewProjection", matView * matProjection);
            //effect.SetValue("xColoredTexture", testTexture);
            //int numpasses = effect.Begin(0);
            //for (int i = 0; i < numpasses; i++)
            //{
             //   effect.BeginPass(i);
                device.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
             //   effect.EndPass();
            //}
            //effect.End();

            device.EndScene();
            device.Present();

            angle += 0.01f;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            frmModels caModels = new frmModels();
            caModels.Show();
        }
    }
}
