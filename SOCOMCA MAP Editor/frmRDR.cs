using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SOCOMCA_MAP_Editor
{
    public partial class frmRDR : Form
    {
        private List<string> strList = new List<string>();

        private struct Vertex
        {
            public Int16 Red;
            public Int16 Green;
            public Int16 Blue;
            public Int16 Alpha;
            public Int16 X;
            public Int16 Y;
            public Int16 Z;
            public Int16 i;
        }
        private struct Triangle
        {
            public Int16 Vertex1;
            public Int16 Vertex2;
            public Int16 Vertex3;
            public Int16 Unused4;
        }

        public frmRDR()
        {
            InitializeComponent();
        }
        private void frmRDR_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dlgRet = new DialogResult();

            dlgRet = openFileDialog1.ShowDialog();
            if (dlgRet.ToString() == "CANCEL") { return; }

            string fName = openFileDialog1.FileName;

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

            int i; int sCount; int i2;
            string tmpStr;
            sCount = 0;

            for (i = 0x90; i < 0x395c4; i++)
            {
                tmpStr = "[" + i.ToString("X8") + "] " + sCount.ToString("X4") + ": ";
                while (fData[i] != 0)
                {
                    tmpStr += System.Text.Encoding.UTF8.GetString(fData, i, 1);
                    i++;
                }
                if (tmpStr != "") { strList.Add(tmpStr); sCount++; }
                
            }

            string tmpID; string tmpEntry; string[] spc; string[] spc2;

            for (i = 0x395d0; i < 0x44a60; i += 8)
            {
                tmpID = fData[i + 3].ToString("X2") + fData[i + 2].ToString("X2") + fData[i + 1].ToString("X2") + fData[i].ToString("X2");
                tmpEntry = fData[i + 7].ToString("X2") + fData[i + 6].ToString("X2") + fData[i + 5].ToString("X2") + fData[i + 4].ToString("X2");
                for (i2 = 0; i2 < strList.Count; i2++)
                {
                    spc = strList.ElementAt<string>(i2).Split(']');
                    spc2 = spc[0].Split('[');
                    //MessageBox.Show(spc2[1] + ":" + tmpEntry);
                    if (spc2[1] == tmpEntry)
                    {
                        tmpStr = strList.ElementAt<string>(i2).Substring(0, 10) + " " + tmpID + " " + strList.ElementAt<string>(i2).Substring(11, strList.ElementAt<string>(i2).Length - 11);
                        strList.RemoveAt(i2);
                        strList.Insert(i2, tmpStr);
                        break;
                    }
                }
            }

            tmpStr = "";
            listBox1.Items.Clear();
            for (i = 0; i < strList.Count; i++)
            {
                listBox1.Items.Add(strList.ElementAt<string>(i).ToString());
                tmpStr += strList.ElementAt<string>(i).ToString() + "\r\n";
            }

            textBox1.Text = tmpStr;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dlgRet = new DialogResult();

            dlgRet = openFileDialog1.ShowDialog();
            if (dlgRet.ToString() == "CANCEL") { return; }

            string fName = openFileDialog1.FileName;

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

            int i;
            int addr = 0x013dd810; int mAddr;

            int vertices = addr + 0x14;
            int points = addr + 0x34;

            vertices = (fData[vertices + 3] * 0x1000000) + (fData[vertices + 2] * 0x10000) + (fData[vertices + 1] * 0x100) + fData[vertices];
            points = (fData[points + 3] * 0x1000000) + (fData[points + 2] * 0x10000) + (fData[points + 1] * 0x100) + fData[points];

            int vertexCount = (fData[vertices + 0x0f] * 0x100) + fData[vertices + 0x0e];
            Vertex[] VX = new Vertex[vertexCount];

            mAddr = vertices + 0x18;
            for (i = 0; i < vertexCount; i++)
            {
                VX[i].X = BitConverter.ToInt16(fData, mAddr); mAddr += 2;
                VX[i].Y = BitConverter.ToInt16(fData, mAddr); mAddr += 2;
                VX[i].Z = BitConverter.ToInt16(fData, mAddr); mAddr += 2;
                VX[i].i = BitConverter.ToInt16(fData, mAddr); mAddr += 2;

                VX[i].Red = BitConverter.ToInt16(fData, mAddr); mAddr += 2;
                VX[i].Green = BitConverter.ToInt16(fData, mAddr); mAddr += 2;
                VX[i].Blue = BitConverter.ToInt16(fData, mAddr); mAddr += 2;
                VX[i].Alpha = BitConverter.ToInt16(fData, mAddr); mAddr += 2;
            }

            int pointsCount = fData[points + 0x6] * 3;
            Triangle[] TR = new Triangle[pointsCount];

            mAddr = points + 0x8;
            for (i = 0; i < pointsCount / 3; i++)
            {
                TR[i].Vertex1 = fData[mAddr];
                TR[i].Vertex2 = fData[mAddr + 1];
                TR[i].Vertex3 = fData[mAddr + 2];
                TR[i].Unused4 = fData[mAddr + 3];
                mAddr += 4;
            }

            string output = "int points_count = " + pointsCount.ToString() + ";\r\n\r\nint points[" + pointsCount.ToString() + "] = {\r\n";
            int DS = 256;
            for (i = 0; i < pointsCount / 3; i++)
            {
                output += "\t" + (TR[i].Vertex1 / 3).ToString() + ", " + (TR[i].Vertex2 / 3).ToString() + ", " + (TR[i].Vertex3 / 3).ToString() + ",\r\n";
            }
            output += "};\r\n\r\nint vertex_count = " + vertexCount.ToString() + ";\r\n\r\nVECTOR vertices[" + vertexCount.ToString() + "] = {\r\n";
            for (i = 0; i < vertexCount; i++)
            {
                output += "{\t" + (VX[i].X / DS).ToString() + ".00f,\t" + (VX[i].Y / DS).ToString() + ".00f,\t" + (VX[i].Z / DS).ToString() + ".00f,\t1.0f\t},\r\n";
            }
            output += "};\r\n\r\nVECTOR colours[" + vertexCount.ToString() + "] = {\r\n";
            for (i = 0; i < vertexCount; i++)
            {
                output += "{\t" + (VX[i].Red / 128).ToString() + ".00f,\t" + (VX[i].Green / 128).ToString() + ".00f,\t" + (VX[i].Blue / 128).ToString() + ".00f,\t1.0f\t},\r\n";
            }
            output += "};";


            textBox1.Text = output;
        }
    }
}
