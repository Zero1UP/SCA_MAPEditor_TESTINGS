using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SOCOMCA_MAP_Editor
{
    class CA_Tiff
    {
        public struct Tiff_Header
        {
            /*
                Main Header
                30000009
                xxxxxxxx -> sub data 1
                01000101
                6c088000
                30000007
                xxxxxxxx -> sub data 2
                11000000
                50000007
                00000000
                00000000
                00000000
                00706358 // allocator pointer
                xxxxxxxx -> sub data 3
                00000000
                00000000
                0000xxxx // Index Array Size
                0000xxxx // uknown variable
                12000908 // uknown value
                xxxxxxxx -> texture "type" ?
                xxxxxxxx // uknown value 0xc109d6b7
                00000000
                xxxxxxxx -> Index Array
                xxxxxxxx -> Color Pallette
                13200013 // uknown value
                hhhhxxxx // h = height, x = uknown
                0081wwww // w = width
                0000beef
                00000000 
            */
            public int Line1; // 30000009

            public int SubHeader1_Address;
            public int Line3;
            public int Line4;
            public int Line5;
            public int SubHeader2_Address;
            public int Line7;
            public int Line8;
            public int Line9;
            public int Line10;
            public int Line11;
            public int Line12; // allocator pointer
            public int SubHeader3_Address;
            public int Line14;
            public int Line15;
            public int IndexSize;
            public int Line17;
            public int Line18;
            public int SubHeader4_Address;
            public int Line20;
            public int Line21;
            public int IndexArray_Address;
            public int ColorPallette_Address;
            public int Line24;

            public Int16 Line25_FirstHalf;
            public Int16 Height;

            public Int16 Width;
            public Int16 Line26_SecondHalf;

            public int Line27_BEEF;
            public int Line28;
        }
        public struct Tiff_SubHeader1
        {
            /*
                Sub Data 1
                00000000
                00000000
                00000000
                00000002
            */
            public int Line1;
            public int Line2;
            public int Line3;
            public int Line4;
        }
        public struct Tiff_SubHeader2
        {
            /*
                Sub Data 2
                00008006
                10000000
                0000000e
                xxxxxxxx -> Texture Header
                00000044
                00000000
                00000042
                00000000
                00000060
                00000000
                00000014
                00000000
            */
            public int Line1;
            public int Line2;
            public int Line3;
            public int HeaderMemAddress;
            public int Line5;
            public int Line6;
            public int Line7;
            public int Line8;
            public int Line9;
            public int Line10;
            public int Line11;
            public int Line12;
        }
        public struct Tiff_SubHeader3
        {
            /*
                Sub Data 3
                dd30b854 // ?
                20170805 // ?
                00000006
                00000000
                00053000
                00000000
                00000047
                00000000
                00000000
                00000000
                00000008
                00000000
                00000000
                00000000
                0000004a
                00000000
                15000000
                00000000
                00000000
                00000000
            */
            public int Line1;
            public int Line2;
            public int Line3;
            public int Line4;
            public int Line5;
            public int Line6;
            public int Line7;
            public int Line8;
            public int Line9;
            public int Line10;
            public int Line11;
            public int Line12;
            public int Line13;
            public int Line14;
            public int Line15;
            public int Line16;
            public int Line17;
            public int Line18;
            public int Line19;
            public int Line20;

        }
        public struct Tiff_SubHeader4
        {
            public int Line1;
            public int Line2;
            public int Line3;
        }
        
        public struct Tiff_ColorPallette
        {
            public int Line1; // 00003840 ? diff each texture
            public Int16 PalletteSize;
            public Int16 Line2_SecondHalf;
            public int Line3; // Point to SubHeader4
            public int Line4; // f80934f2 ? diff each texture
            public int Line5; // 00000001
            public int PalletteAddress;
            public int Line7;
            public int Line8;

            public Int16[] Colors16;
            public Int32[] Colors32;

            public int BitSize;
        }

        public Tiff_Header Header;
        public Tiff_SubHeader1 SubHeader1;
        public Tiff_SubHeader2 SubHeader2;
        public Tiff_SubHeader3 SubHeader3;
        public Tiff_SubHeader4 SubHeader4;

        public byte[] IndexArray;
        public Tiff_ColorPallette ColorPallette;
        public bool isUpperRaw = false;
        public bool splitQuads = false;

        public Bitmap Image;
        
        public Bitmap RenderBitmap()
        {
            int x; int y; int i; int biggest;
            
            Bitmap BMP = new Bitmap(Header.Width, Header.Height);
            Graphics Gfx = Graphics.FromImage(BMP);
            Brush tBrush;
            Gfx.Clear(Color.FromArgb(0));

            int tType = Header.Line24 & 0xFF; // Normal 8 bit index or condensed 4 bit index (Tiny)
            
            biggest = 0;
            x = 0; y = 0;
            for (i = 0; i < Header.IndexSize; i++)
            {
                if (y >= Header.Height) { y = 0; x++; }

                if (isUpperRaw)
                {
                    int a = IndexArray[i + 3];
                    if (a >= 128) { a = 255; }

                    tBrush = new SolidBrush(Color.FromArgb(a, IndexArray[i + 0], IndexArray[i + 1], IndexArray[i + 2]));
                    Gfx.FillRectangle(tBrush, x, y, 1, 1);
                    tBrush.Dispose();
                    i += 3;
                }
                else
                {
                    if (IndexArray[i] > biggest) { biggest = IndexArray[i]; }
                    if (ColorPallette.BitSize == 1)
                    {
                        if (tType == 0x13)
                        {
                            if (splitQuads)
                            {
                                int b1 = IndexArray[i];
                                b1 = ((b1 / 8) * 8) + b1;

                                tBrush = BrushFromInt16(ColorPallette.Colors16[b1]);
                                Gfx.FillRectangle(tBrush, x, y, 1, 1);
                                tBrush.Dispose();
                            }
                            else
                            {
                                tBrush = BrushFromInt16(ColorPallette.Colors16[IndexArray[i]]);
                                Gfx.FillRectangle(tBrush, x, y, 1, 1);
                                tBrush.Dispose();
                            }
                        }
                        if (tType == 0x14)
                        {
                            int b1; int b2;
                            b1 = IndexArray[i] & 0xF;
                            b2 = (IndexArray[i] / 0x10) & 0xF;

                            b1 = ((b1 / 8) * 8) + b1;
                            b2 = ((b2 / 8) * 8) + b2;

                            //if (b1 > 7) { b1 += 8; }
                            //if (b2 > 7) { b2 += 8; }

                            tBrush = BrushFromInt16(ColorPallette.Colors16[b1]);
                            Gfx.FillRectangle(tBrush, x, y, 1, 1);
                            tBrush.Dispose();
                            y++;
                            tBrush = BrushFromInt16(ColorPallette.Colors16[b2]);
                            Gfx.FillRectangle(tBrush, x, y, 1, 1);
                            tBrush.Dispose();
                        }
                    }
                    if (ColorPallette.BitSize == 2)
                    {
                        tBrush = BrushFromInt32(ColorPallette.Colors32[IndexArray[i]]);
                        Gfx.FillRectangle(tBrush, x, y, 1, 1);
                        tBrush.Dispose();
                    }
                }

                y++;
            }
            //MessageBox.Show(biggest.ToString("X2"));
            
            return BMP;
        }
        private SolidBrush BrushFromInt16(Int16 Col16)
        {
            int R; int G; int B; int A;

            R = ((Col16 & 0x1F) * 255) / 31;
            G = (((Col16 & 0x3E0) / 0x20) * 255) / 31;
            B = (((Col16 & 0x7C00) / 0x400) * 255) / 31;
            A = (Col16 & 0x8000) / 0x8000;
            if (A == 1) { A = 255; }

            return new SolidBrush(Color.FromArgb(A, R, G, B));
        }
        private SolidBrush BrushFromInt32(Int32 Col32)
        {
            int R; int G; int B; int A;
            byte[] bts = BitConverter.GetBytes(Col32);

            R = bts[0];
            G = bts[1];
            B = bts[2];
            A = bts[3];
            A = (A * 255) / 128;

            //if (A >= 128) { A = 255; }
            //MessageBox.Show(R.ToString("X2") + " " + G.ToString("X2") + " " + B.ToString("X2") + "; " + A.ToString("X2"));
            return new SolidBrush(Color.FromArgb(A, R, G, B));
        }

        public int ExtractTIFF(string fName, int tiffAddr)
        {
            int i;

            byte[] fData;
            if (System.IO.File.Exists(fName) == false) { return -1; }
            try { fData = System.IO.File.ReadAllBytes(fName); } catch { return -2; }
            if (fData.Length < tiffAddr) { return -3; }
            if (fData.Length < (tiffAddr + 0x100)) { return -3; }

            //-------------------------------------------------------------------------------------- Extract Header Data
            Header.Line1 = BitConverter.ToInt32(fData, tiffAddr + 0x00);
            Header.SubHeader1_Address = BitConverter.ToInt32(fData, tiffAddr + 0x04);
            Header.Line3 = BitConverter.ToInt32(fData, tiffAddr + 0x08);
            Header.Line4 = BitConverter.ToInt32(fData, tiffAddr + 0x0c);
            Header.Line5 = BitConverter.ToInt32(fData, tiffAddr + 0x10);
            Header.SubHeader2_Address = BitConverter.ToInt32(fData, tiffAddr + 0x14);
            Header.Line7 = BitConverter.ToInt32(fData, tiffAddr + 0x18);
            Header.Line8 = BitConverter.ToInt32(fData, tiffAddr + 0x1c);
            Header.Line9 = BitConverter.ToInt32(fData, tiffAddr + 0x20);
            Header.Line10 = BitConverter.ToInt32(fData, tiffAddr + 0x24);
            Header.Line11 = BitConverter.ToInt32(fData, tiffAddr + 0x28);
            Header.Line12 = BitConverter.ToInt32(fData, tiffAddr + 0x2c);
            Header.SubHeader3_Address = BitConverter.ToInt32(fData, tiffAddr + 0x30);
            Header.Line14 = BitConverter.ToInt32(fData, tiffAddr + 0x34);
            Header.Line15 = BitConverter.ToInt32(fData, tiffAddr + 0x38);
            Header.IndexSize = BitConverter.ToInt32(fData, tiffAddr + 0x3c);
            Header.Line17 = BitConverter.ToInt32(fData, tiffAddr + 0x40);
            Header.Line18 = BitConverter.ToInt32(fData, tiffAddr + 0x44);
            Header.SubHeader4_Address = BitConverter.ToInt32(fData, tiffAddr + 0x48);
            Header.Line20 = BitConverter.ToInt32(fData, tiffAddr + 0x4c);
            Header.Line21 = BitConverter.ToInt32(fData, tiffAddr + 0x50);
            Header.IndexArray_Address = BitConverter.ToInt32(fData, tiffAddr + 0x54);
            Header.ColorPallette_Address = BitConverter.ToInt32(fData, tiffAddr + 0x58);
            Header.Line24 = BitConverter.ToInt32(fData, tiffAddr + 0x5c);
            Header.Line25_FirstHalf = BitConverter.ToInt16(fData, tiffAddr + 0x60);
            Header.Height = BitConverter.ToInt16(fData, tiffAddr + 0x62);
            Header.Width = BitConverter.ToInt16(fData, tiffAddr + 0x64);
            Header.Line26_SecondHalf = BitConverter.ToInt16(fData, tiffAddr + 0x66);
            Header.Line27_BEEF = BitConverter.ToInt32(fData, tiffAddr + 0x68);
            Header.Line28 = BitConverter.ToInt32(fData, tiffAddr + 0x6c);
            if (Header.Line1 != 0x30000009) { return -4; }
            if (Header.IndexArray_Address <= 0) { return -5; }

            //-------------------------------------------------------------------------------------- Extract Sub Header 1
            if (Header.SubHeader1_Address > 0)
            {
                if (fData.Length < (Header.SubHeader1_Address + 0x10)) { return -3; }
                SubHeader1.Line1 = BitConverter.ToInt32(fData, Header.SubHeader1_Address + 0x00);
                SubHeader1.Line2 = BitConverter.ToInt32(fData, Header.SubHeader1_Address + 0x04);
                SubHeader1.Line3 = BitConverter.ToInt32(fData, Header.SubHeader1_Address + 0x08);
                SubHeader1.Line4 = BitConverter.ToInt32(fData, Header.SubHeader1_Address + 0x0c);
            }

            //-------------------------------------------------------------------------------------- Extract Sub Header 2
            if (Header.SubHeader2_Address > 0)
            {
                if (fData.Length < (Header.SubHeader2_Address + 0x30)) { return -3; }
                SubHeader2.Line1 = BitConverter.ToInt32(fData, Header.SubHeader2_Address + 0x00);
                SubHeader2.Line2 = BitConverter.ToInt32(fData, Header.SubHeader2_Address + 0x04);
                SubHeader2.Line3 = BitConverter.ToInt32(fData, Header.SubHeader2_Address + 0x08);
                SubHeader2.HeaderMemAddress = BitConverter.ToInt32(fData, Header.SubHeader2_Address + 0x0c);
                SubHeader2.Line5 = BitConverter.ToInt32(fData, Header.SubHeader2_Address + 0x10);
                SubHeader2.Line6 = BitConverter.ToInt32(fData, Header.SubHeader2_Address + 0x14);
                SubHeader2.Line7 = BitConverter.ToInt32(fData, Header.SubHeader2_Address + 0x18);
                SubHeader2.Line8 = BitConverter.ToInt32(fData, Header.SubHeader2_Address + 0x1c);
                SubHeader2.Line9 = BitConverter.ToInt32(fData, Header.SubHeader2_Address + 0x20);
                SubHeader2.Line10 = BitConverter.ToInt32(fData, Header.SubHeader2_Address + 0x24);
                SubHeader2.Line11 = BitConverter.ToInt32(fData, Header.SubHeader2_Address + 0x28);
                SubHeader2.Line12 = BitConverter.ToInt32(fData, Header.SubHeader2_Address + 0x2c);
            }

            //-------------------------------------------------------------------------------------- Extract Sub Header 3
            if (Header.SubHeader3_Address > 0)
            {
                if (fData.Length < (Header.SubHeader3_Address + 0x50)) { return -3; }
                SubHeader3.Line1 = BitConverter.ToInt32(fData, Header.SubHeader3_Address + 0x00);
                SubHeader3.Line2 = BitConverter.ToInt32(fData, Header.SubHeader3_Address + 0x04);
                SubHeader3.Line3 = BitConverter.ToInt32(fData, Header.SubHeader3_Address + 0x08);
                SubHeader3.Line4 = BitConverter.ToInt32(fData, Header.SubHeader3_Address + 0x0c);
                SubHeader3.Line5 = BitConverter.ToInt32(fData, Header.SubHeader3_Address + 0x10);
                SubHeader3.Line6 = BitConverter.ToInt32(fData, Header.SubHeader3_Address + 0x14);
                SubHeader3.Line7 = BitConverter.ToInt32(fData, Header.SubHeader3_Address + 0x18);
                SubHeader3.Line8 = BitConverter.ToInt32(fData, Header.SubHeader3_Address + 0x1c);
                SubHeader3.Line9 = BitConverter.ToInt32(fData, Header.SubHeader3_Address + 0x20);
                SubHeader3.Line10 = BitConverter.ToInt32(fData, Header.SubHeader3_Address + 0x24);
                SubHeader3.Line11 = BitConverter.ToInt32(fData, Header.SubHeader3_Address + 0x28);
                SubHeader3.Line12 = BitConverter.ToInt32(fData, Header.SubHeader3_Address + 0x2c);
                SubHeader3.Line13 = BitConverter.ToInt32(fData, Header.SubHeader3_Address + 0x30);
                SubHeader3.Line14 = BitConverter.ToInt32(fData, Header.SubHeader3_Address + 0x34);
                SubHeader3.Line15 = BitConverter.ToInt32(fData, Header.SubHeader3_Address + 0x38);
                SubHeader3.Line16 = BitConverter.ToInt32(fData, Header.SubHeader3_Address + 0x3c);
                SubHeader3.Line17 = BitConverter.ToInt32(fData, Header.SubHeader3_Address + 0x40);
                SubHeader3.Line18 = BitConverter.ToInt32(fData, Header.SubHeader3_Address + 0x44);
                SubHeader3.Line19 = BitConverter.ToInt32(fData, Header.SubHeader3_Address + 0x48);
                SubHeader3.Line20 = BitConverter.ToInt32(fData, Header.SubHeader3_Address + 0x4c);
            }

            //-------------------------------------------------------------------------------------- Extract Sub Header 4
            if (Header.SubHeader4_Address > 0)
            {
                if (fData.Length < (Header.SubHeader4_Address + 0x0c)) { return -3; }
                SubHeader4.Line1 = BitConverter.ToInt32(fData, Header.SubHeader4_Address + 0x00);
                SubHeader4.Line2 = BitConverter.ToInt32(fData, Header.SubHeader4_Address + 0x04);
                SubHeader4.Line3 = BitConverter.ToInt32(fData, Header.SubHeader4_Address + 0x08);
            }

            //-------------------------------------------------------------------------------------- Extract Index Array
            IndexArray = new byte[Header.IndexSize];
            int tmpLargest = 0;
            for (i = 0; i < Header.IndexSize; i++)
            {
                IndexArray[i] = fData[Header.IndexArray_Address + i];
                if (IndexArray[i] > tmpLargest) { tmpLargest = IndexArray[i]; }
            }
            if (tmpLargest <= 0xF) { splitQuads = true; }

            //-------------------------------------------------------------------------------------- Extract Color Pallette
            if (Header.ColorPallette_Address > 0)
            {
                if (fData.Length < (Header.ColorPallette_Address + 0x20)) { return -3; }

                isUpperRaw = false;
                ColorPallette.Line1 = BitConverter.ToInt32(fData, Header.ColorPallette_Address + 0x00);
                ColorPallette.PalletteSize = BitConverter.ToInt16(fData, Header.ColorPallette_Address + 0x04);
                ColorPallette.Line2_SecondHalf = BitConverter.ToInt16(fData, Header.ColorPallette_Address + 0x06);
                ColorPallette.Line3 = BitConverter.ToInt32(fData, Header.ColorPallette_Address + 0x08);
                ColorPallette.Line4 = BitConverter.ToInt32(fData, Header.ColorPallette_Address + 0x0c);
                ColorPallette.Line5 = BitConverter.ToInt32(fData, Header.ColorPallette_Address + 0x10);
                ColorPallette.PalletteAddress = BitConverter.ToInt32(fData, Header.ColorPallette_Address + 0x14);
                ColorPallette.Line7 = BitConverter.ToInt32(fData, Header.ColorPallette_Address + 0x18);
                ColorPallette.Line8 = BitConverter.ToInt32(fData, Header.ColorPallette_Address + 0x1c);

                ColorPallette.BitSize = 0;
                if (ColorPallette.PalletteSize == 0x0200)
                {
                    ColorPallette.BitSize = 1;
                    ColorPallette.Colors16 = new Int16[256];

                    for (i = 0; i < 256; i++)
                    {
                        ColorPallette.Colors16[i] = BitConverter.ToInt16(fData, ColorPallette.PalletteAddress + (i * 2));
                    }
                }

                if (ColorPallette.PalletteSize == 0x0400)
                {
                    ColorPallette.BitSize = 2;
                    ColorPallette.Colors32 = new Int32[256];

                    for (i = 0; i < 256; i++)
                    {
                        ColorPallette.Colors32[i] = BitConverter.ToInt32(fData, ColorPallette.PalletteAddress + (i * 4));
                    }
                }
            }
            else
            {
                isUpperRaw = true;  
            }

            //-------------------------------------------------------------------------------------- Pre-Render Image
            Image = RenderBitmap();

            return 1;
        }
    }
}
