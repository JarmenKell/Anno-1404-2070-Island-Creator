using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Insel_Editor
{
    public partial class Form1 : Form
    {
        public static Encoding Iso88591;
        public static int size;
        public static StringBuilder isd;
        public static int mid;
        public static StringBuilder og;
        public static byte type;

        public Form1()
        {
            InitializeComponent();
            Iso88591 = Encoding.GetEncoding("ISO8859-1");
            this.Shown += new EventHandler(Form1_Shown);
        }

        void Form1_Shown(object sender, EventArgs e)
        {
            Graphics picture = this.CreateGraphics();
            picture.DrawLine(new Pen(Color.Black), 12, 32, 186, 32);
            picture.DrawLine(new Pen(Color.Black), 99, 32, 99, 90);
            tsize.Focus();
        }

        private void insel_Click(object sender, EventArgs e)
        {
            type = 1;
            size = int.Parse(tsize.Text) / 16 * 16;
            byte blocks = 0; //(byte)(size / 512);
            byte seasize = (byte)(15 + blocks * 16);
            og = new StringBuilder();
            mid = 1;
            
            isd = new StringBuilder("<Width>" + size + "</Width>\r\n<Height>" + size + "</Height>\r\n<Clime>0</Clime>\r\n<Difficulty>0</Difficulty>\r\n<UsedChunks><m_XSize>" + size / 16 + "</m_XSize>\r\n<m_YSize>" + size / 16 + "</m_YSize>\r\n<m_IntXSize>1</m_IntXSize>\r\n");
            isd.Append("<m_BitGrid>CDATA[" + CDATA(size / 4, 255) + "]</m_BitGrid>\r\n</UsedChunks>\r\n<BuildBlockerShapes></BuildBlockerShapes>\r\n<CamBlockerShapes></CamBlockerShapes>\r\n<SurfLines></SurfLines>\r\n<CoastBuildingLines><i><Points>");

            long[,] cblpos = new long[5, 2];
            cblpos[0, 0] = (seasize - 1) * 4096;
            cblpos[0, 1] = (seasize - 1) * 4096;
            cblpos[1, 0] = (size - seasize + 1) * 4096;
            cblpos[1, 1] = (seasize - 1) * 4096;
            cblpos[2, 0] = (size - seasize + 1) * 4096;
            cblpos[2, 1] = (size - seasize + 1) * 4096;
            cblpos[3, 0] = (seasize - 1) * 4096;
            cblpos[3, 1] = (size - seasize + 1) * 4096;
            cblpos[4, 0] = (seasize - 1) * 4096;
            cblpos[4, 1] = (seasize - 1) * 4096;
            byte[] coastbuildinglines = new byte[20];
            byte[] cblbyte = BitConverter.GetBytes(16);
            for (byte i = 0; i < 5; i++)
            {
                byte j;
                for (j = 0; j < 4; j++) coastbuildinglines[j] = cblbyte[j];
                byte[] x = BitConverter.GetBytes(cblpos[i, 0]);
                for (j = 0; j < 8; j++) coastbuildinglines[j + 4] = x[j];
                byte[] y = BitConverter.GetBytes(cblpos[i, 1]);
                for (j = 0; j < 8; j++) coastbuildinglines[j + 12] = y[j];
                isd.Append("<i>CDATA[" + Iso88591.GetString(coastbuildinglines) + "]</i>\r\n");
            }

            isd.Append("</Points><Directions><i>0</i>\r\n<i>368640</i>\r\n<i>737280</i>\r\n<i>1105920</i></Directions>\r\n</i></CoastBuildingLines>\r\n<Lakes></Lakes>\r\n<Rivers></Rivers>\r\n<RiverGrid><m_XSize>" + size + "</m_XSize>\r\n<m_YSize>" + size + "</m_YSize>\r\n<m_IntXSize>" + size / 32 + "</m_IntXSize>\r\n");
            isd.Append("<m_BitGrid>CDATA[" + CDATA(size * size / 8, 0) + "]</m_BitGrid>");
            isd.Append("\r\n</RiverGrid>\r\n<SeaLevel>0</SeaLevel>\r\n<Sandbanks></Sandbanks>\r\n<Fogbanks></Fogbanks>\r\n<TerrainNormalSplines></TerrainNormalSplines>\r\n<AnimalLayer><AnimalLayer><m_AnchorPoints></m_AnchorPoints>\r\n<m_Connections></m_Connections>\r\n</AnimalLayer>");
            for (byte i = 1; i < 10; i++) isd.Append("\r\n<AnimalLayer" + i + "><m_AnchorPoints></m_AnchorPoints>\r\n<m_Connections></m_Connections>\r\n</AnimalLayer" + i + ">");
            isd.Append("\r\n</AnimalLayer>\r\n<AIPoints></AIPoints>\r\n<ConstructionRecords></ConstructionRecords>\r\n<Volcanos></Volcanos>\r\n<SendExplorationMessage>1</SendExplorationMessage>\r\n<m_GOPManager><m_GRIDManager>");

            int hmsize = size * size * 2;
            byte[] hmap = new byte[hmsize + 4];
            byte[] hmbyte = BitConverter.GetBytes(hmsize);
            for (byte i = 0; i < 4; i++) hmap[i] = hmbyte[i];
            int h = 4;
            for (int i = 0; i < size; i++)
            {
                if (i < seasize | i >= size - seasize)
                {
                    for (int j = 0; j < size; j++)
                    {
                        hmap[h + 1] = 132;
                        h += 2;
                    }
                }
                else
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (j < seasize | j >= size - seasize)
                        {
                            hmap[h + 1] = 132;
                            h += 2;
                        }
                        else
                        {
                            hmap[h] = 204;
                            hmap[h + 1] = 2;
                            h += 2;
                        }
                    }
                }
            }

            isd.Append("<m_HeightMap_v2>CDATA[" + Iso88591.GetString(hmap) + "]</m_HeightMap_v2>\r\n");
            isd.Append("<m_PathBlockerLayer><BitGrid><m_XSize>" + size * 2 + "</m_XSize>\r\n<m_YSize>" + size * 2 + "</m_YSize>\r\n<m_IntXSize>" + size / 16 + "</m_IntXSize>\r\n");
            isd.Append("<m_BitGrid>CDATA[" + CDATA(size * size / 2, 0) + "]</m_BitGrid></BitGrid>\r\n</m_PathBlockerLayer>\r\n");
            isd.Append("<m_HeightPathBlockerLayer><BitGrid><m_XSize>" + size * 2 + "</m_XSize>\r\n<m_YSize>" + size * 2 + "</m_YSize>\r\n<m_IntXSize>" + size / 16 + "</m_IntXSize>\r\n");
            isd.Append("<m_BitGrid>CDATA[" + CDATA(size * size / 2, 0) + "]</m_BitGrid>\r\n</BitGrid>\r\n</m_HeightPathBlockerLayer>\r\n");
            isd.Append("<m_SubmarineBlockerLayer><SubmarineLevelHeight>-98304</SubmarineLevelHeight>\r\n<SubmarineIslandBlockerRadius>16384</SubmarineIslandBlockerRadius>\r\n<BitGrid><m_XSize>" + size + "</m_XSize>\r\n<m_YSize>" + size + "</m_YSize>\r\n<m_IntXSize>" + size / 32 + "</m_IntXSize>\r\n");
            isd.Append("<m_BitGrid>CDATA[" + CDATA(size * size / 8, 255) + "]</m_BitGrid>\r\n</BitGrid>\r\n</m_SubmarineBlockerLayer>\r\n</m_GRIDManager>\r\n");
            isd.Append("<m_StreetGrid>CDATA[" + CDATA(size * size, 0) + "]</m_StreetGrid>\r\n<Objects><Handle><Objects>\r\n");

            for (int i = 0; i < (size - seasize - seasize - 5) / 10; i++) isd.Append(handle(335, (seasize + 5) + i * 5, size - seasize - 6));
            for (int i = 0; i < (size - seasize - seasize - 5) / 10; i++) isd.Append(handle(350, size - ((seasize + 5) + i * 5), size - seasize - 6));
            for (int i = 0; i < (size - seasize - seasize + 5) / 10; i++) isd.Append(handle(347, seasize + i * 10, size - seasize + 9));

            isd.Append("</Objects>\r\n</Handle>\r\n<Feedback><Objects></Objects>\r\n</Feedback>\r\n<Simple><Objects></Objects>\r\n</Simple>\r\n<Nature><Objects>");

            byte[] tree = new byte[] { 0x00, 0x00, 0x00, 0x14, 0x31, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x6C, 0x89, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2D, 0x0B, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            byte[] tpos = BitConverter.GetBytes(size * 2048);
            for (byte i = 0; i < 4; i++) tree[i + 3] = tpos[i];
            for (byte i = 0; i < 4; i++) tree[i + 11] = tpos[i];
            isd.Append("<Object><m_ID>1992</m_ID><m_GUID>313</m_GUID><m_Variation>1</m_Variation><m_Position>CDATA[" + Iso88591.GetString(tree) + "]</m_Position><m_PlayerID>15</m_PlayerID><m_Direction>0</m_Direction><Nature><m_Scale>1.018424</m_Scale><m_Orientation>CDATA[" + Iso88591.GetString(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xB1, 0x63, 0x7F, 0xBF, 0x00, 0x00, 0x00, 0x00, 0x2D, 0x5D, 0x8D, 0x3D }) + "]</m_Orientation></Nature></Object>\r\n");
            
            isd.Append("</Objects>\r\n</Nature>\r\n<Grass><Objects></Objects>\r\n</Grass>\r\n</Objects>\r\n<SnapPartner></SnapPartner>\r\n<ObjectNames></ObjectNames>\r\n");
            isd.Append("<ObjectGroups>" + og.ToString() + "</ObjectGroups>\r\n</m_GOPManager>\r\n<Terrain><Version>3</>\r\n<TileCountX>" + size + "</>\r\n<TileCountZ>" + size + "</>\r\n<TileCount>0</>\r\n<ChunkMap><Width>" + size / 16 + "</>\r\n<Height>" + size / 16 + "</>\r\n");

            string tex1 = CDATA(289, 255);
            string tex2 = CDATA(289, 127);
            byte[] glevel = new byte[] { 51, 51, 51, 63 };
            
            Single[] csingle = new Single[17];
            csingle[0] = BitConverter.ToSingle(glevel, 0);
            csingle[1] = csingle[0] - (float)0.2;
            csingle[2] = csingle[0] - (float)0.4;
            csingle[3] = csingle[0] - (float)0.7;
            csingle[4] = csingle[0] - (float)1.2;
            csingle[5] = csingle[0] - (float)1.7;
            csingle[6] = csingle[0] - (float)2.4;
            csingle[7] = csingle[0] - (float)3.1;
            csingle[8] = csingle[0] - (float)4.0;
            csingle[9] = csingle[0] - (float)5.1;
            csingle[10] = csingle[0] - (float)6.3;
            csingle[11] = csingle[0] - (float)7.8;
            csingle[12] = csingle[0] - (float)9.5;
            csingle[13] = csingle[0] - (float)11.6;
            csingle[14] = csingle[0] - (float)14.2;
            csingle[15] = csingle[0] - (float)17.7;
            csingle[16] = csingle[0] - (float)24.7;
            
            byte[] cmap = new byte[68];
            h = 0;
            for (byte i = 0; i < 17; i++)
            {
                byte[] temp = BitConverter.GetBytes(csingle[i]);
                cmap[h] = temp[0];
                h++;
                cmap[h] = temp[1];
                h++;
                cmap[h] = temp[2];
                h++;
                cmap[h] = temp[3];
                h++;
            }
            byte[][] coast = heightmap(cmap);
            //byte[][] coast = heightmap(new byte[] { 0x33, 0x33, 0x33, 0x3F, 0x33, 0x33, 0x33, 0x3F, 0xC3, 0x53, 0x7B, 0x3E, 0x23, 0x50, 0x91, 0xBE, 0x0F, 0xAB, 0x18, 0xBF, 0x54, 0x5B, 0x82, 0xBF, 0x8E, 0x46, 0xB5, 0xBF, 0x93, 0x7B, 0xDD, 0xBF, 0x97, 0xB5, 0xEB, 0xBF, 0x64, 0x8F, 0xE6, 0xBF, 0x83, 0x82, 0xF1, 0xBF, 0x6B, 0x15, 0x21, 0xC0, 0xAE, 0xCA, 0x30, 0xC0, 0x0F, 0x52, 0x36, 0xC0, 0xE5, 0x14, 0x53, 0xC0, 0xF4, 0x96, 0x69, 0xC0, 0x7E, 0xA3, 0x7D, 0xC0 });

            byte[] grass = new byte[1160];
            grass[0] = 132;
            grass[1] = 4;
            h = 4;
            for (int i = 0; i < 289; i++)
            {
                for (byte j = 0; j < 4; j++)
                {
                    grass[h] = glevel[j];
                    h++;
                }
            }

            byte[] water = new byte[1160];
            water[0] = 132;
            water[1] = 4;
            h = 4;
            for (int i = 0; i < 289; i++)
            {
                water[h + 2] = 32;
                water[h + 3] = 194;
                h += 4;
            }

            for (byte i = 0; i < blocks; i++) for (int j = 0; j < (size / 16); j++) element(water, tex1, tex2, 79, 196608);
            for (byte i = 0; i < blocks; i++) element(water, tex1, tex2, 79, 196608);
            element(coast[0], tex1, tex2, 79, 196608);
            for (int i = 1 + blocks; i < (size / 16) - 1 - blocks; i++)
            {
                element(coast[1], tex1, tex2, 79, 196608);
            }
            element(coast[2], tex1, tex2, 79, 196608);
            for (byte i = 0; i < blocks; i++) element(water, tex1, tex2, 79, 196608);

            for (int i = 1 + blocks; i < (size / 16) - 1 - blocks; i++)
            {
                for (byte j = 0; j < blocks; j++) element(water, tex1, tex2, 79, 196608);
                element(coast[7], tex1, tex2, 79, 196608);
                for (int j = 1 + blocks; j < (size / 16) - 1 - blocks; j++)
                {
                    element(grass, tex1, tex2, 52, 131073);
                }
                element(coast[3], tex1, tex2, 79, 196608);
                for (byte j = 0; j < blocks; j++) element(water, tex1, tex2, 79, 196608);
            }

            for (byte i = 0; i < blocks; i++) element(water, tex1, tex2, 79, 196608);
            element(coast[6], tex1, tex2, 79, 196608);
            for (int i = 1 + blocks; i < (size / 16) - 1 - blocks; i++)
            {
                element(coast[5], tex1, tex2, 79, 196608);
            }
            element(coast[4], tex1, tex2, 79, 196608);
            for (byte i = 0; i < blocks; i++) element(water, tex1, tex2, 79, 196608);
            for (byte i = 0; i < blocks; i++) for (int j = 0; j < (size / 16); j++) element(water, tex1, tex2, 79, 196608);

            isd.Append("</>\r\n</>\r\n");
            sfd.ShowDialog();
            try
            {
                StreamWriter sw = new StreamWriter(sfd.FileName, false, Iso88591);
                sw.Write(isd.ToString());
                sw.Close();
            }
            catch { }
        }
        
        private void plateau_Click(object sender, EventArgs e)
        {
            type = 2;
            size = int.Parse(tsize.Text);
            byte blocks = (byte)(size / 512);
            byte seasize = (byte)(blocks * 16);
            og = new StringBuilder();
            mid = 1;


            isd = new StringBuilder("<Width>" + size + "</Width>\r\n<Height>" + size + "</Height>\r\n<Clime>1</Clime>\r\n<Difficulty>0</Difficulty>\r\n<UsedChunks><m_XSize>" + size / 16 + "</m_XSize>\r\n<m_YSize>" + size / 16 + "</m_YSize>\r\n<m_IntXSize>1</m_IntXSize>\r\n");
            isd.Append("<m_BitGrid>CDATA[" + CDATA(size / 4, 255) + "]</m_BitGrid>\r\n</UsedChunks>\r\n<BuildBlockerShapes></BuildBlockerShapes>\r\n<CamBlockerShapes></CamBlockerShapes>\r\n<SurfLines></SurfLines>\r\n<CoastBuildingLines>");
            isd.Append("</CoastBuildingLines>\r\n<Lakes></Lakes>\r\n<Rivers></Rivers>\r\n<RiverGrid><m_XSize>" + size + "</m_XSize>\r\n<m_YSize>" + size + "</m_YSize>\r\n<m_IntXSize>" + size / 32 + "</m_IntXSize>\r\n");
            isd.Append("<m_BitGrid>CDATA[" + CDATA(size * size / 8, 0) + "]</m_BitGrid>\r\n</RiverGrid>\r\n");
            isd.Append("<SeaLevel>-28</SeaLevel>\r\n<Sandbanks></Sandbanks>\r\n<Fogbanks></Fogbanks>\r\n<TerrainNormalSplines></TerrainNormalSplines>\r\n<AnimalLayer><AnimalLayer><m_AnchorPoints></m_AnchorPoints>\r\n<m_Connections></m_Connections>\r\n</AnimalLayer>");
            for (byte i = 1; i < 10; i++) isd.Append("\r\n<AnimalLayer" + i + "><m_AnchorPoints></m_AnchorPoints>\r\n<m_Connections></m_Connections>\r\n</AnimalLayer" + i + ">");
            isd.Append("\r\n</AnimalLayer>\r\n<AIPoints></AIPoints>\r\n<ConstructionRecords></ConstructionRecords>\r\n<Volcanos></Volcanos>\r\n<SendExplorationMessage>1</SendExplorationMessage>\r\n<m_GOPManager><m_GRIDManager>");

            int hmsize = size * size * 2;
            byte[] hmap = new byte[hmsize + 4];
            byte[] hmbyte = BitConverter.GetBytes(hmsize);
            for (byte i = 0; i < 4; i++) hmap[i] = hmbyte[i];
            int h = 4;
            for (int i = 0; i < size; i++)
            {
                if (i < seasize | i >= size - seasize)
                {
                    for (int j = 0; j < size; j++)
                    {
                        hmap[h + 1] = 132;
                        h += 2;
                    }
                }
                else
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (j < seasize | j >= size - seasize)
                        {
                            hmap[h + 1] = 132;
                            h += 2;
                        }
                        else
                        {
                            hmap[h + 1] = 148;
                            h += 2;
                        }
                    }
                }
            }

            isd.Append("<m_HeightMap_v2>CDATA[" + Iso88591.GetString(hmap) + "]</m_HeightMap_v2>\r\n");
            isd.Append("<m_PathBlockerLayer><BitGrid><m_XSize>" + size * 2 + "</m_XSize>\r\n<m_YSize>" + size * 2 + "</m_YSize>\r\n<m_IntXSize>" + size / 16 + "</m_IntXSize>\r\n");
            isd.Append("<m_BitGrid>CDATA[" + CDATA(size * size / 2, 0) + "]</m_BitGrid></BitGrid>\r\n</m_PathBlockerLayer>\r\n");
            isd.Append("<m_HeightPathBlockerLayer><BitGrid><m_XSize>" + size * 2 + "</m_XSize>\r\n<m_YSize>" + size * 2 + "</m_YSize>\r\n<m_IntXSize>" + size / 16 + "</m_IntXSize>\r\n");
            isd.Append("<m_BitGrid>CDATA[" + CDATA(size * size / 2, 0) + "]</m_BitGrid>\r\n</BitGrid>\r\n</m_HeightPathBlockerLayer>\r\n");
            isd.Append("<m_SubmarineBlockerLayer><SubmarineLevelHeight>-98304</SubmarineLevelHeight>\r\n<SubmarineIslandBlockerRadius>16384</SubmarineIslandBlockerRadius>\r\n<BitGrid><m_XSize>" + size + "</m_XSize>\r\n<m_YSize>" + size + "</m_YSize>\r\n<m_IntXSize>" + size / 32 + "</m_IntXSize>\r\n");
            isd.Append("<m_BitGrid>CDATA[" + CDATA(size * size / 8, 0) + "]</m_BitGrid>\r\n</BitGrid>\r\n</m_SubmarineBlockerLayer>\r\n</m_GRIDManager>\r\n");
            isd.Append("<m_StreetGrid>CDATA[" + CDATA(size * size, 0) + "]</m_StreetGrid>\r\n<Objects><Handle><Objects>\r\n");

            for (int i = 0; i < (size - seasize - seasize - 6) / 12; i++) isd.Append(handle(344, (seasize + 6) + i * 6, size - seasize - 3));
            for (int i = 0; i < (size - seasize - seasize - 9) / 18; i++) isd.Append(handle(349, size - ((seasize + 9) + i * 9), size - seasize - 5));
            for (int i = 0; i < (size - seasize - seasize - 21) / 21; i++) isd.Append(handle(340, (seasize + 21) + i * 21, size - seasize - 28));
            //isd.Append(handle(40000005, seasize + 21, size - seasize - 28));
            //isd.Append(handle(40000005, size - (seasize + 21), size - seasize - 28));

            isd.Append("</Objects>\r\n</Handle>\r\n<Feedback><Objects></Objects>\r\n</Feedback>\r\n<Simple><Objects></Objects>\r\n</Simple>\r\n<Nature><Objects></Objects>\r\n</Nature>\r\n<Grass><Objects></Objects>\r\n</Grass>\r\n</Objects>\r\n<SnapPartner></SnapPartner>\r\n<ObjectNames></ObjectNames>\r\n");
            isd.Append("<ObjectGroups>" + og.ToString() + "</ObjectGroups>\r\n</m_GOPManager>\r\n<Terrain><Version>3</>\r\n<TileCountX>" + size + "</>\r\n<TileCountZ>" + size + "</>\r\n<TileCount>0</>\r\n<ChunkMap><Width>" + size / 16 + "</>\r\n<Height>" + size / 16 + "</>\r\n");

            string tex = CDATA(289, 255);
            string tex2 = CDATA(289, 127);
            byte[] plateau = new byte[1160];
            plateau[0] = 132;
            plateau[1] = 4;
            h = 4;
            for (int i = 0; i < 289; i++)
            {
                plateau[h + 2] = 216;
                plateau[h + 3] = 193;
                h += 4;
            }

            byte[] water = new byte[1160];
            water[0] = 132;
            water[1] = 4;
            h = 4;
            for (int i = 0; i < 289; i++)
            {
                water[h + 2] = 32;
                water[h + 3] = 194;
                h += 4;
            }

            for (int i = 0; i < blocks; i++) for (int j = 0; j < (size / 16); j++) element(plateau, tex, tex2, 79, 196608);
            for (int i = blocks; i < (size / 16) - blocks; i++)
            {
                for (int j = 0; j < blocks; j++) element(plateau, tex, tex2, 79, 196608);
                for (int j = blocks; j < (size / 16) - blocks; j++) element(plateau, tex, tex, 78, 131072);
                for (int j = 0; j < blocks; j++) element(plateau, tex, tex2, 79, 196608);
            }
            for (int i = 0; i < blocks; i++) for (int j = 0; j < (size / 16); j++) element(plateau, tex, tex2, 79, 196608);

            isd.Append("</>\r\n</>\r\n");
            sfd.ShowDialog();
            try
            {
                StreamWriter sw = new StreamWriter(sfd.FileName, false, Iso88591);
                sw.Write(isd.ToString());
                sw.Close();
            }
            catch { }
        }

        private void o1404_Click(object sender, EventArgs e)
        {
            type = 3;
            byte clime = 0;
            if (rb2.Checked) clime = 1;
            size = int.Parse(tsize.Text);
            byte blocks = (byte)(size / 512);
            byte seasize = (byte)(15 + blocks * 16);
            og = new StringBuilder();
            mid = 1;

            isd = new StringBuilder("<Width>" + size + "</Width>\r\n<Height>" + size + "</Height>\r\n<Clime>" + clime + "</Clime>\r\n<Difficulty>0</Difficulty>\r\n<UsedChunks><m_XSize>" + size / 16 + "</m_XSize>\r\n<m_YSize>" + size / 16 + "</m_YSize>\r\n<m_IntXSize>1</m_IntXSize>\r\n");
            isd.Append("<m_BitGrid>CDATA[" + CDATA(size / 4, 255) + "]</m_BitGrid>\r\n</UsedChunks>\r\n<BuildBlockerShapes></BuildBlockerShapes>\r\n<DesertShapes></DesertShapes>\r\n<SurfLines></SurfLines>\r\n<CoastBuildingLines><i><Points>");

            long[,] cblpos = new long[9, 2];
            cblpos[0, 0] = (seasize - 1 + 16) * 4096;
            cblpos[0, 1] = (seasize - 1) * 4096;
            cblpos[1, 0] = (size - seasize + 1 - 16) * 4096;
            cblpos[1, 1] = (seasize - 1) * 4096;
            cblpos[2, 0] = (size - seasize + 1) * 4096;
            cblpos[2, 1] = (seasize - 1 + 16) * 4096;
            cblpos[3, 0] = (size - seasize + 1) * 4096;
            cblpos[3, 1] = (size - seasize + 1 - 16) * 4096;
            cblpos[4, 0] = (size - seasize + 1 - 16) * 4096;
            cblpos[4, 1] = (size - seasize + 1) * 4096;
            cblpos[5, 0] = (seasize - 1 + 16) * 4096;
            cblpos[5, 1] = (size - seasize + 1) * 4096;
            cblpos[6, 0] = (seasize - 1) * 4096;
            cblpos[6, 1] = (size - seasize + 1 - 16) * 4096;
            cblpos[7, 0] = (seasize - 1) * 4096;
            cblpos[7, 1] = (seasize - 1 + 16) * 4096;
            cblpos[8, 0] = (seasize - 1 + 16) * 4096;
            cblpos[8, 1] = (seasize - 1) * 4096;
            byte[] coastbuildinglines = new byte[20];
            byte[] cblbyte = BitConverter.GetBytes(16);
            for (byte i = 0; i < 9; i++)
            {
                byte j;
                for (j = 0; j < 4; j++) coastbuildinglines[j] = cblbyte[j];
                byte[] x = BitConverter.GetBytes(cblpos[i, 0]);
                for (j = 0; j < 8; j++) coastbuildinglines[j + 4] = x[j];
                byte[] y = BitConverter.GetBytes(cblpos[i, 1]);
                for (j = 0; j < 8; j++) coastbuildinglines[j + 12] = y[j];
                isd.Append("<i>CDATA[" + Iso88591.GetString(coastbuildinglines) + "]</i>\r\n");
            }

            isd.Append("</Points><Directions><i>0</i>\r\n<i>184320</i>\r\n<i>368640</i>\r\n<i>552960</i>\r\n<i>737280</i>\r\n<i>921600</i>\r\n<i>1105920</i>\r\n<i>1290420</i></Directions>\r\n</i></CoastBuildingLines>\r\n<Lakes></Lakes>\r\n<Sandbanks></Sandbanks>\r\n<Fogbanks></Fogbanks>\r\n<TerrainNormalSplines></TerrainNormalSplines>\r\n<NativeSlots></NativeSlots>\r\n<Rivers></Rivers>\r\n");
            isd.Append("<AIPoints></AIPoints>\r\n<ConstructionRecords></ConstructionRecords>\r\n<SendExplorationMessage>1</SendExplorationMessage>\r\n<m_GOPManager><m_GRIDManager>");

            int hmsize = size * size * 2;
            byte[] hmap = new byte[hmsize + 4];
            byte[] hmbyte = BitConverter.GetBytes(hmsize);
            for (byte i = 0; i < 4; i++) hmap[i] = hmbyte[i];
            int h = 4;
            for (int i = 0; i < size; i++)
            {
                if (i < seasize | i >= (size - seasize))
                {
                    for (int j = 0; j < size; j++)
                    {
                        hmap[h + 1] = 240;
                        h += 2;
                    }
                }
                else
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (j < seasize | j >= (size - seasize))
                        {
                            hmap[h + 1] = 240;
                            h += 2;
                        }
                        else
                        {
                            hmap[h] = 204;
                            hmap[h + 1] = 2;
                            h += 2;
                        }
                    }
                }
            }

            h = 4 + (size * seasize * 2) + (seasize * 2);
            for (sbyte i = 16; i > 0; i--)
            {
                for (byte j = 0; j < i; j++)
                {
                    hmap[h] = 0;
                    h++;
                    hmap[h] = 240;
                    h++;
                }

                h += (size * 2) - (i * 4) - (seasize * 4);

                for (byte j = 0; j < i; j++)
                {
                    hmap[h] = 0;
                    h++;
                    hmap[h] = 240;
                    h++;
                }

                h += seasize * 4;
            }

            h = 4 + hmsize - (size * (seasize + 16) * 2) + (seasize * 2);
            for (byte i = 1; i <= 16; i++)
            {
                for (byte j = 0; j < i; j++)
                {
                    hmap[h] = 0;
                    h++;
                    hmap[h] = 240;
                    h++;
                }

                h += (size * 2) - (i * 4) - (seasize * 4);

                for (byte j = 0; j < i; j++)
                {
                    hmap[h] = 0;
                    h++;
                    hmap[h] = 240;
                    h++;
                }

                h += seasize * 4;
            }

            isd.Append("<m_HeightMap>CDATA[" + Iso88591.GetString(hmap) + "]</m_HeightMap>\r\n<m_PathBlockerLayer><BitGrid><m_XSize>" + size * 2 + "</m_XSize>\r\n<m_YSize>" + size * 2 + "</m_YSize>\r\n<m_IntXSize>" + size / 16 + "</m_IntXSize>\r\n");
            isd.Append("<m_BitGrid>CDATA[" + CDATA(size * size / 2, 0) + "]</m_BitGrid></BitGrid>\r\n</m_PathBlockerLayer>\r\n</m_GRIDManager>\r\n<Streets></Streets>\r\n<Objects>\r\n<Simple><Objects></Objects>\r\n</Simple><Handle><Objects>\r\n");

            //600 Minen
            for (int i = 0; i < (size - seasize - seasize - 40) / 10; i++)
            {
                isd.Append(handle(600, (seasize + 24) + i * 10, seasize + 16));
                isd.Append(handle(600, (size - seasize - 16), (seasize + 24) + i * 10));
                isd.Append(handle(600, (seasize + 24) + i * 10, size - seasize - 16));
                isd.Append(handle(600, (seasize + 16), (seasize + 24) + i * 10));
            }
            //602 Steine
            for (int i = 0; i < (size - seasize - seasize - 60) / 8; i++)
            {
                isd.Append(handle(602, (seasize + 32) + i * 8, seasize + 32));
                //isd.Append(handle(602, (size - seasize - 32), (seasize + 32) + i * 8));
                //isd.Append(handle(602, (seasize + 32) + i * 8, size - seasize - 32));
                //isd.Append(handle(602, (seasize + 32), (seasize + 32) + i * 8));
            }
            //603 Flussbauplätze
            for (int i = 0; i < (size - seasize - seasize - 60) / 14; i++)
            {
                //isd.Append(handle(603, (seasize + 32) + i * 14, seasize + 32));
                isd.Append(handle(603, (size - seasize - 32), (seasize + 32) + i * 14));
                isd.Append(handle(603, (seasize + 32) + i * 14, size - seasize - 32));
                isd.Append(handle(603, (seasize + 32), (seasize + 32) + i * 14));
            }
            //601 Korallenriff
            for (int i = 0; i < (size - seasize - seasize) / 20; i++)
            {
                isd.Append(handle(601, seasize + 11 + i * 20, seasize - 9));
                isd.Append(handle(601, (size - seasize + 9), seasize + 11 + i * 20));
                isd.Append(handle(601, seasize + 11 + i * 20, size - seasize + 9));
                isd.Append(handle(601, (seasize - 9), seasize + 11 + i * 20));
            }

            isd.Append("</Objects>\r\n</Handle>\r\n<Nature><Objects></Objects>\r\n</Nature>\r\n<Grass><Objects></Objects>\r\n</Grass>\r\n</Objects>\r\n<SnapPartner></SnapPartner>\r\n<ObjectNames></ObjectNames>\r\n");
            isd.Append("<ObjectGroups>" + og + "</ObjectGroups>\r\n</m_GOPManager>\r\n<Terrain><Version>3</>\r\n<TileCountX>" + size + "</>\r\n<TileCountZ>" + size + "</>\r\n<TileCount>0</>\r\n<ChunkMap><Width>" + size / 16 + "</>\r\n<Height>" + size / 16 + "</>\r\n");

            string tex = CDATA(289, 255);
            //byte[][] coast = heightmap(new byte[] { 0x30, 0x33, 0x33, 0x3F, 0x30, 0x33, 0x33, 0x3F, 0x30, 0x33, 0x33, 0x3F, 0x30, 0x33, 0x33, 0x3F, 0x30, 0x33, 0x33, 0x3F, 0x30, 0x33, 0x33, 0x3F, 0x30, 0x33, 0x33, 0x3F, 0x95, 0x32, 0x33, 0x3F, 0xA4, 0xC0, 0x25, 0x3F, 0x19, 0x83, 0x03, 0x3E, 0xCE, 0xF6, 0x0A, 0xBF, 0xF0, 0xF2, 0x89, 0xBF, 0xE8, 0x76, 0xC1, 0xBF, 0x25, 0xA1, 0xF0, 0xBF, 0x0A, 0x90, 0x0A, 0xC0, 0x26, 0x2E, 0x14, 0xC0, 0xB4, 0x2B, 0x17, 0xC0 });
            byte[][] coast = heightmap(new byte[] { 0xA4, 0xC0, 0x25, 0x3F, 0x19, 0x83, 0x03, 0x3E, 0xCE, 0xF6, 0x0A, 0xBF, 0xF0, 0xF2, 0x89, 0xBF, 0xF0, 0xF2, 0x89, 0xBF, 0xE8, 0x76, 0xC1, 0xBF, 0xE8, 0x76, 0xC1, 0xBF, 0x25, 0xA1, 0xF0, 0xBF, 0x25, 0xA1, 0xF0, 0xBF, 0x0A, 0x90, 0x0A, 0xC0, 0x0A, 0x90, 0x0A, 0xC0, 0x26, 0x2E, 0x14, 0xC0, 0x26, 0x2E, 0x14, 0xC0, 0xB4, 0x2B, 0x17, 0xC0, 0xB4, 0x2B, 0x17, 0xC0, 0xB4, 0x2B, 0x17, 0xC0, 0xB4, 0x2B, 0x17, 0xC0 });

            byte[] glevel = new byte[] { 48, 51, 51, 63 };
            byte[] grass = new byte[1160];
            grass[0] = 132;
            grass[1] = 4;
            h = 4;
            for (int i = 0; i < 289; i++)
            {
                for (byte j = 0; j < 4; j++)
                {
                    grass[h] = glevel[j];
                    h++;
                }
            }

            byte[] water = new byte[1160];
            water[0] = 132;
            water[1] = 4;
            h = 4;
            for (int i = 0; i < 289; i++)
            {
                water[h + 2] = 128;
                water[h + 3] = 192;
                h += 4;
            }

            byte[] wlevel = new byte[] { 0, 0, 128, 192 };
            for (sbyte i = 17; i > 0; i--)
            {
                h = 4 + (17 - i) * 68;
                for (byte j = 0; j < i; j++)
                {
                    for (byte k = 0; k < 4; k++)
                    {
                        coast[0][h] = wlevel[k];
                        h++;
                    }
                }
            }
            for (sbyte i = 17; i > 0; i--)
            {
                h = 4 + (17 - i) * 72;
                for (byte j = 0; j < i; j++)
                {
                    for (byte k = 0; k < 4; k++)
                    {
                        coast[2][h] = wlevel[k];
                        h++;
                    }
                }
            }
            for (byte i = 1; i <= 17; i++)
            {
                h = 4 + (i - 1) * 68 + (17 - i) * 4;
                for (byte j = 0; j < i; j++)
                {
                    for (byte k = 0; k < 4; k++)
                    {
                        coast[4][h] = wlevel[k];
                        h++;
                    }
                }
            }
            for (byte i = 1; i <= 17; i++)
            {
                h = 4 + (i - 1) * 68;
                for (byte j = 0; j < i; j++)
                {
                    for (byte k = 0; k < 4; k++)
                    {
                        coast[6][h] = wlevel[k];
                        h++;
                    }
                }
            }

            for (byte i = 0; i < blocks; i++) for (int j = 0; j < (size / 16); j++) element(water, tex, tex, 10, 0);
            for (byte i = 0; i < blocks; i++) element(water, tex, tex, 10, 0);
            element(coast[0], tex, tex, 10, 0);
            for (int i = 1 + blocks; i < (size / 16) - 1 - blocks; i++)
            {
                element(coast[1], tex, tex, 10, 0);
            }
            element(coast[2], tex, tex, 10, 0);
            for (byte i = 0; i < blocks; i++) element(water, tex, tex, 10, 0);

            for (int i = 1 + blocks; i < (size / 16) - 1 - blocks; i++)
            {
                for (byte j = 0; j < blocks; j++) element(water, tex, tex, 10, 0);
                element(coast[7], tex, tex, 10, 0);
                for (int j = 1 + blocks; j < (size / 16) - 1 - blocks; j++)
                {
                    element(grass, tex, tex, 2, 0);
                }
                element(coast[3], tex, tex, 10, 0);
                for (byte j = 0; j < blocks; j++) element(water, tex, tex, 10, 0);
            }

            for (byte i = 0; i < blocks; i++) element(water, tex, tex, 10, 0);
            element(coast[6], tex, tex, 10, 0);
            for (int i = 1 + blocks; i < (size / 16) - 1 - blocks; i++)
            {
                element(coast[5], tex, tex, 10, 0);
            }
            element(coast[4], tex, tex, 10, 0);
            for (byte i = 0; i < blocks; i++) element(water, tex, tex, 10, 0);
            for (byte i = 0; i < blocks; i++) for (int j = 0; j < (size / 16); j++) element(water, tex, tex, 10, 0);

            isd.Append("</>\r\n</>\r\n");
            sfd.ShowDialog();
            try
            {
                StreamWriter sw = new StreamWriter(sfd.FileName, false, Iso88591);
                sw.Write(isd.ToString());
                sw.Close();
            }
            catch { }
        }

        //private void o1404_Click(object sender, EventArgs e)
        //{
        //    type = 3;
        //    byte clime = 0;
        //    if (rb2.Checked) clime = 1;
        //    size = int.Parse(tsize.Text);
        //    byte blocks = (byte)(size / 512);
        //    byte seasize = (byte)(15 + blocks * 16);
        //    og = new StringBuilder();
        //    mid = 1;

        //    isd = new StringBuilder("<Width>" + size + "</Width>\r\n<Height>" + size + "</Height>\r\n<Clime>" + clime + "</Clime>\r\n<Difficulty>0</Difficulty>\r\n<UsedChunks><m_XSize>" + size / 16 + "</m_XSize>\r\n<m_YSize>" + size / 16 + "</m_YSize>\r\n<m_IntXSize>1</m_IntXSize>\r\n");
        //    isd.Append("<m_BitGrid>CDATA[" + CDATA(size / 4, 255) + "]</m_BitGrid>\r\n</UsedChunks>\r\n<BuildBlockerShapes></BuildBlockerShapes>\r\n<DesertShapes></DesertShapes>\r\n<SurfLines></SurfLines>\r\n<CoastBuildingLines><i><Points>");

        //    long[,] cblpos = new long[5, 2];
        //    cblpos[0, 0] = (seasize - 1) * 4096;
        //    cblpos[0, 1] = (seasize - 1) * 4096;
        //    cblpos[1, 0] = (size - seasize + 1) * 4096;
        //    cblpos[1, 1] = (seasize - 1) * 4096;
        //    cblpos[2, 0] = (size - seasize + 1) * 4096;
        //    cblpos[2, 1] = (size - seasize + 1) * 4096;
        //    cblpos[3, 0] = (seasize - 1) * 4096;
        //    cblpos[3, 1] = (size - seasize + 1) * 4096;
        //    cblpos[4, 0] = (seasize - 1) * 4096;
        //    cblpos[4, 1] = (seasize - 1) * 4096;
        //    byte[] coastbuildinglines = new byte[20];
        //    byte[] cblbyte = BitConverter.GetBytes(16);
        //    for (byte i = 0; i < 5; i++)
        //    {
        //        byte j;
        //        for (j = 0; j < 4; j++) coastbuildinglines[j] = cblbyte[j];
        //        byte[] x = BitConverter.GetBytes(cblpos[i, 0]);
        //        for (j = 0; j < 8; j++) coastbuildinglines[j + 4] = x[j];
        //        byte[] y = BitConverter.GetBytes(cblpos[i, 1]);
        //        for (j = 0; j < 8; j++) coastbuildinglines[j + 12] = y[j];
        //        isd.Append("<i>CDATA[" + Iso88591.GetString(coastbuildinglines) + "]</i>\r\n");
        //    }

        //    isd.Append("</Points><Directions><i>0</i>\r\n<i>368640</i>\r\n<i>737280</i>\r\n<i>1105920</i></Directions>\r\n</i></CoastBuildingLines>\r\n<Lakes></Lakes>\r\n<Sandbanks></Sandbanks>\r\n<Fogbanks></Fogbanks>\r\n<TerrainNormalSplines></TerrainNormalSplines>\r\n<NativeSlots></NativeSlots>\r\n<Rivers></Rivers>\r\n");
        //    isd.Append("<AIPoints></AIPoints>\r\n<ConstructionRecords></ConstructionRecords>\r\n<SendExplorationMessage>1</SendExplorationMessage>\r\n<m_GOPManager><m_GRIDManager>");

        //    int hmsize = size * size * 2;
        //    byte[] hmap = new byte[hmsize + 4];
        //    byte[] hmbyte = BitConverter.GetBytes(hmsize);
        //    for (byte i = 0; i < 4; i++) hmap[i] = hmbyte[i];
        //    int h = 4;
        //    for (int i = 0; i < size; i++)
        //    {
        //        if (i < seasize | i >= (size - seasize))
        //        {
        //            for (int j = 0; j < size; j++)
        //            {
        //                hmap[h + 1] = 240;
        //                h += 2;
        //            }
        //        }
        //        else
        //        {
        //            for (int j = 0; j < size; j++)
        //            {
        //                if (j < seasize | j >= (size - seasize))
        //                {
        //                    hmap[h + 1] = 240;
        //                    h += 2;
        //                }
        //                else
        //                {
        //                    hmap[h] = 204;
        //                    hmap[h + 1] = 2;
        //                    h += 2;
        //                }
        //            }
        //        }
        //    }

        //    isd.Append("<m_HeightMap>CDATA[" + Iso88591.GetString(hmap) + "]</m_HeightMap>\r\n<m_PathBlockerLayer><BitGrid><m_XSize>" + size * 2 + "</m_XSize>\r\n<m_YSize>" + size * 2 + "</m_YSize>\r\n<m_IntXSize>" + size / 16 + "</m_IntXSize>\r\n");
        //    isd.Append("<m_BitGrid>CDATA[" + CDATA(size * size / 2, 0) + "]</m_BitGrid></BitGrid>\r\n</m_PathBlockerLayer>\r\n</m_GRIDManager>\r\n<Streets></Streets>\r\n<Objects>\r\n<Simple><Objects></Objects>\r\n</Simple><Handle><Objects>\r\n");

        //    for (int i = 0; i < (size - seasize - seasize - 5) / 10; i++) isd.Append(handle(600, (seasize + 5) + i * 5, size - seasize - 8));
        //    for (int i = 0; i < (size - seasize - seasize - 7) / 14; i++) isd.Append(handle(603, size - ((seasize + 7) + i * 7), size - seasize - 10));
        //    for (int i = 0; i < (size - seasize - seasize - 4) / 8; i++) isd.Append(handle(602, size - ((seasize + 4) + i * 4), size - seasize - 6));
        //    for (int i = 0; i < (size - seasize - seasize + 18) / 17; i++) isd.Append(handle(601, seasize + i * 17, size - seasize + 9));

        //    isd.Append("</Objects>\r\n</Handle>\r\n<Nature><Objects></Objects>\r\n</Nature>\r\n<Grass><Objects></Objects>\r\n</Grass>\r\n</Objects>\r\n<SnapPartner></SnapPartner>\r\n<ObjectNames></ObjectNames>\r\n");
        //    isd.Append("<ObjectGroups>" + og + "</ObjectGroups>\r\n</m_GOPManager>\r\n<Terrain><Version>3</>\r\n<TileCountX>" + size + "</>\r\n<TileCountZ>" + size + "</>\r\n<TileCount>0</>\r\n<ChunkMap><Width>" + size / 16 + "</>\r\n<Height>" + size / 16 + "</>\r\n");

        //    string tex = CDATA(289, 255);
        //    //byte[][] coast = heightmap(new byte[] { 0x30, 0x33, 0x33, 0x3F, 0x30, 0x33, 0x33, 0x3F, 0x30, 0x33, 0x33, 0x3F, 0x30, 0x33, 0x33, 0x3F, 0x30, 0x33, 0x33, 0x3F, 0x30, 0x33, 0x33, 0x3F, 0x30, 0x33, 0x33, 0x3F, 0x95, 0x32, 0x33, 0x3F, 0xA4, 0xC0, 0x25, 0x3F, 0x19, 0x83, 0x03, 0x3E, 0xCE, 0xF6, 0x0A, 0xBF, 0xF0, 0xF2, 0x89, 0xBF, 0xE8, 0x76, 0xC1, 0xBF, 0x25, 0xA1, 0xF0, 0xBF, 0x0A, 0x90, 0x0A, 0xC0, 0x26, 0x2E, 0x14, 0xC0, 0xB4, 0x2B, 0x17, 0xC0 });
        //    byte[][] coast = heightmap(new byte[] { 0xA4, 0xC0, 0x25, 0x3F, 0x19, 0x83, 0x03, 0x3E, 0xCE, 0xF6, 0x0A, 0xBF, 0xF0, 0xF2, 0x89, 0xBF, 0xF0, 0xF2, 0x89, 0xBF, 0xE8, 0x76, 0xC1, 0xBF, 0xE8, 0x76, 0xC1, 0xBF, 0x25, 0xA1, 0xF0, 0xBF, 0x25, 0xA1, 0xF0, 0xBF, 0x0A, 0x90, 0x0A, 0xC0, 0x0A, 0x90, 0x0A, 0xC0, 0x26, 0x2E, 0x14, 0xC0, 0x26, 0x2E, 0x14, 0xC0, 0xB4, 0x2B, 0x17, 0xC0, 0xB4, 0x2B, 0x17, 0xC0, 0xB4, 0x2B, 0x17, 0xC0, 0xB4, 0x2B, 0x17, 0xC0 });

        //    byte[] glevel = new byte[] { 48, 51, 51, 63 };
        //    byte[] grass = new byte[1160];
        //    grass[0] = 132;
        //    grass[1] = 4;
        //    h = 4;
        //    for (int i = 0; i < 289; i++)
        //    {
        //        for (byte j = 0; j < 4; j++)
        //        {
        //            grass[h] = glevel[j];
        //            h++;
        //        }
        //    }

        //    byte[] water = new byte[1160];
        //    water[0] = 132;
        //    water[1] = 4;
        //    h = 4;
        //    for (int i = 0; i < 289; i++)
        //    {
        //        water[h + 2] = 128;
        //        water[h + 3] = 192;
        //        h += 4;
        //    }


        //    for (byte i = 0; i < blocks; i++) for (int j = 0; j < (size / 16); j++) element(water, tex, tex, 10, 0);
        //    for (byte i = 0; i < blocks; i++) element(water, tex, tex, 10, 0);
        //    element(coast[0], tex, tex, 10, 0);
        //    for (int i = 1 + blocks; i < (size / 16) - 1 - blocks; i++)
        //    {
        //        element(coast[1], tex, tex, 10, 0);
        //    }
        //    element(coast[2], tex, tex, 10, 0);
        //    for (byte i = 0; i < blocks; i++) element(water, tex, tex, 10, 0);

        //    for (int i = 1 + blocks; i < (size / 16) - 1 - blocks; i++)
        //    {
        //        for (byte j = 0; j < blocks; j++) element(water, tex, tex, 10, 0);
        //        element(coast[7], tex, tex, 10, 0);
        //        for (int j = 1 + blocks; j < (size / 16) - 1 - blocks; j++)
        //        {
        //            element(grass, tex, tex, 2, 0);
        //        }
        //        element(coast[3], tex, tex, 10, 0);
        //        for (byte j = 0; j < blocks; j++) element(water, tex, tex, 10, 0);
        //    }

        //    for (byte i = 0; i < blocks; i++) element(water, tex, tex, 10, 0);
        //    element(coast[6], tex, tex, 10, 0);
        //    for (int i = 1 + blocks; i < (size / 16) - 1 - blocks; i++)
        //    {
        //        element(coast[5], tex, tex, 10, 0);
        //    }
        //    element(coast[4], tex, tex, 10, 0);
        //    for (byte i = 0; i < blocks; i++) element(water, tex, tex, 10, 0);
        //    for (byte i = 0; i < blocks; i++) for (int j = 0; j < (size / 16); j++) element(water, tex, tex, 10, 0);

        //    isd.Append("</>\r\n</>\r\n");
        //    sfd.ShowDialog();
        //    try
        //    {
        //        StreamWriter sw = new StreamWriter(sfd.FileName, false, Iso88591);
        //        sw.Write(isd.ToString());
        //        sw.Close();
        //    }
        //    catch { }
        //}

        public string CDATA(int dsize, byte dbyte)
        {
            byte[] data = new byte[dsize + 4];
            byte[] byted = BitConverter.GetBytes(dsize);
            for (byte i = 0; i < 4; i++) data[i] = byted[i];
            for (int i = 4; i < data.Length; i++) data[i] = dbyte;
            return Iso88591.GetString(data);
        }

        public string handle(int guid, Single x, Single y)
        {
            byte[] pdata = new byte[28];
            pdata[0] = 24;
            byte[] xbyte = BitConverter.GetBytes((long)(x * 4096));
            byte[] ybyte = BitConverter.GetBytes((long)(y * 4096));
            byte[] zbyte = null;
            for (byte i = 0; i < 8; i++) pdata[i + 4] = xbyte[i];
            for (byte i = 0; i < 8; i++) pdata[i + 12] = ybyte[i];
            xbyte = BitConverter.GetBytes(x);
            ybyte = BitConverter.GetBytes(y);
            byte[] rdata = new byte[82];
            StringBuilder obj = new StringBuilder();
            byte[] ogbyte;
            byte[] mbyte;

            switch (guid)
            {
                case 335:
                    rdata = new byte[] { 78, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 136, 0, 0, 0, 0, 1, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 48, 51, 63, 0, 0, 0, 0, 0, 0, 0, 0, 243, 4, 53, 63, 0, 0, 0, 0, 242, 4, 53, 63, 0, 0, 128, 63, 159, 158, 158, 61, 159, 158, 158, 62, 238, 236, 236, 62, 0, 0, 128, 4 };
                    zbyte = BitConverter.GetBytes((long)8256);
                    obj.Append(handle(342, x, y - 3));
                    break;

                case 342:
                    rdata = new byte[] { 78, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 136, 0, 0, 0, 0, 1, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 64, 0, 0, 0, 0, 0, 0, 0, 0, 249, 85, 48, 63, 0, 0, 0, 0, 173, 149, 57, 63, 0, 0, 128, 63, 159, 158, 158, 61, 159, 158, 158, 62, 238, 236, 236, 62, 0, 0, 128, 4 };
                    zbyte = BitConverter.GetBytes((long)2867);
                    ogbyte = new byte[12];
                    ogbyte[0] = 8;
                    mbyte = BitConverter.GetBytes(mid);
                    for (byte i = 0; i < 4; i++) ogbyte[i + 4] = mbyte[i];
                    mbyte = BitConverter.GetBytes(mid + 1);
                    for (byte i = 0; i < 4; i++) ogbyte[i + 8] = mbyte[i];
                    og.Append("<i>CDATA[" + Iso88591.GetString(ogbyte) + "]</i>");
                    break;

                case 347:
                    rdata = new byte[] { 78, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 0, 0, 0, 0, 1, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 48, 51, 63, 0, 0, 0, 0, 0, 0, 0, 0, 173, 252, 63, 191, 0, 0, 0, 0, 11, 253, 247, 190, 0, 0, 128, 63, 159, 158, 158, 61, 159, 158, 158, 62, 238, 236, 236, 62, 0, 0, 128, 4 };
                    zbyte = BitConverter.GetBytes((long)2867);
                    break;

                case 350:
                    rdata = new byte[] { 78, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 136, 0, 0, 0, 0, 1, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 48, 51, 63, 0, 0, 0, 0, 0, 0, 0, 128, 0, 0, 128, 191, 0, 0, 0, 0, 46, 189, 59, 179, 0, 0, 128, 63, 159, 158, 158, 61, 159, 158, 158, 62, 238, 236, 236, 62, 0, 0, 128, 4 };
                    zbyte = BitConverter.GetBytes((long)2867);
                    break;

                case 349:
                    rdata = new byte[] { 78, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 136, 0, 0, 0, 0, 1, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 216, 193, 0, 0, 0, 0, 0, 0, 0, 0, 243, 4, 53, 191, 0, 0, 0, 0, 242, 4, 53, 63, 0, 0, 128, 63, 159, 158, 158, 61, 159, 158, 158, 62, 238, 236, 236, 62, 0, 0, 128, 4 };
                    zbyte = BitConverter.GetBytes((long)-110592);
                    break;

                case 340:
                    rdata = new byte[] { 78, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 136, 0, 0, 0, 0, 1, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 216, 193, 0, 0, 0, 0, 0, 0, 0, 0, 81, 187, 45, 63, 0, 0, 0, 0, 84, 6, 53, 63, 0, 0, 128, 63, 159, 158, 158, 61, 159, 158, 158, 62, 238, 236, 236, 62, 0, 0, 128, 4 };
                    zbyte = BitConverter.GetBytes((long)-110592);
                    break;

                case 344:
                    rdata = new byte[] { 78, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 136, 0, 0, 0, 0, 1, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 216, 193, 0, 0, 0, 0, 0, 0, 0, 0, 243, 4, 53, 191, 0, 0, 0, 0, 242, 4, 53, 63, 0, 0, 128, 63, 159, 158, 158, 61, 159, 158, 158, 62, 238, 236, 236, 62, 0, 0, 128, 4 };
                    zbyte = BitConverter.GetBytes((long)-110592);
                    break;

                case 600:
                    rdata = new byte[] { 78, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 255, 255, 255, 255, 0, 0, 128, 63, 0, 128, 0, 0, 0, 255, 1, 0, 0, 0, 15, 0, 0, 0, 0, 0, 0, 0, 0, 140, 10, 64, 0, 0, 0, 0, 0, 0, 0, 0, 224, 108, 125, 63, 0, 0, 0, 0, 126, 221, 10, 63, 0, 0, 128, 63, 211, 210, 210, 62, 245, 244, 122, 62, 245, 244, 122, 62, 0, 0, 128, 63 };
                    zbyte = BitConverter.GetBytes((long)8867);
                    obj.Append(handle(609, x, y - 3));
                    break;

                case 609:
                    rdata = new byte[] { 78, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 255, 255, 255, 255, 0, 0, 128, 63, 0, 128, 0, 0, 0, 255, 1, 0, 0, 0, 16, 0, 16, 0, 0, 0, 0, 0, 0, 48, 51, 63, 0, 0, 0, 0, 0, 0, 0, 128, 0, 0, 128, 191, 0, 0, 0, 0, 0, 0, 0, 128, 0, 0, 128, 63, 211, 210, 210, 62, 245, 244, 122, 62, 245, 244, 122, 62, 0, 0, 128, 63 };
                    zbyte = BitConverter.GetBytes((long)2867);
                    ogbyte = new byte[12];
                    ogbyte[0] = 8;
                    mbyte = BitConverter.GetBytes(mid);
                    for (byte i = 0; i < 4; i++) ogbyte[i + 4] = mbyte[i];
                    mbyte = BitConverter.GetBytes(mid + 1);
                    for (byte i = 0; i < 4; i++) ogbyte[i + 8] = mbyte[i];
                    og.Append("<i>CDATA[" + Iso88591.GetString(ogbyte) + "]</i>");
                    break;

                case 602:
                    rdata = new byte[] { 78, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 255, 255, 255, 255, 0, 0, 128, 63, 0, 128, 0, 0, 0, 255, 1, 0, 0, 0, 42, 0, 0, 0, 0, 0, 0, 0, 0, 48, 51, 63, 0, 0, 0, 0, 0, 0, 0, 0, 20, 53, 240, 190, 0, 0, 0, 0, 226, 19, 98, 63, 0, 0, 128, 63, 211, 210, 210, 62, 245, 244, 122, 62, 245, 244, 122, 62, 0, 0, 128, 63 };
                    zbyte = BitConverter.GetBytes((long)2867);
                    break;

                case 603:
                    rdata = new byte[] { 78, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 255, 255, 255, 255, 0, 0, 128, 63, 0, 128, 0, 0, 0, 255, 1, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 0, 48, 51, 0, 0, 0, 0, 0, 0, 0, 0, 243, 4, 53, 63, 0, 0, 0, 0, 242, 4, 53, 63, 0, 0, 128, 63, 153, 152, 152, 51, 153, 152, 152, 52, 237, 236, 236, 62, 0, 0, 128, 63 };
                    zbyte = BitConverter.GetBytes((long)2864);
                    break;

                case 601:
                    rdata = new byte[] { 78, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 255, 255, 255, 255, 0, 0, 128, 63, 0, 128, 0, 0, 0, 255, 1, 0, 0, 0, 100, 0, 0, 0, 0, 0, 0, 0, 0, 64, 197, 190, 0, 0, 0, 0, 0, 0, 0, 0, 197, 51, 85, 191, 0, 0, 0, 0, 240, 180, 208, 63, 0, 0, 128, 63, 153, 152, 152, 51, 153, 152, 152, 52, 237, 236, 236, 62, 0, 0, 128, 63 };
                    zbyte = BitConverter.GetBytes((long)-1578);
                    break;

                //case 40000005:
                //    rdata = new byte[] { 0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x08 ,0x00 ,0x00 ,0x00 ,0x00 ,0x01 ,0x00 ,0x00 ,0x00 ,0x64 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0xF0 ,0x41 ,0xC0 ,0x22 ,0x10 ,0xC2 ,0x00 ,0x00 ,0x5C ,0x42 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x80 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x80 ,0x3F ,0x00 ,0x00 ,0x80 ,0x3F ,0x99 ,0x98 ,0x98 ,0x3D ,0x99 ,0x98 ,0x98 ,0x3E ,0xEE ,0xEC ,0xEC ,0x3E ,0x00 ,0x00 ,0x80 ,0x3F };
                //    zbyte = BitConverter.GetBytes((long)-147595);
                //    break;
            }

            for (byte i = 0; i < 8; i++) pdata[i + 20] = zbyte[i];
            for (byte i = 0; i < 4; i++) rdata[i + 34] = xbyte[i];
            for (byte i = 0; i < 4; i++) rdata[i + 42] = ybyte[i];
            if (type == 1 | type == 2)
            {
                obj.Append("<Object><m_ID>" + mid + "</m_ID>\r\n<m_GUID>" + guid + "</m_GUID>\r\n<m_Position>CDATA[" + Iso88591.GetString(pdata) + "]</m_Position>\r\n<m_PlayerID>15</m_PlayerID>\r\n<m_Direction>0</m_Direction>\r\n");
                obj.Append("<Blocking></Blocking>\r\n<PropertyMesh><m_RenderParameters>CDATA[" + Iso88591.GetString(rdata) + "]</m_RenderParameters>\r\n</PropertyMesh>\r\n</Object>");
            }
            else
            {
                obj.Append("<Object><m_ID>" + mid + "</m_ID>\r\n<m_GUID>" + guid + "</m_GUID>\r\n<m_Position>CDATA[" + Iso88591.GetString(pdata) + "]</m_Position>\r\n<m_PlayerID>15</m_PlayerID>\r\n<m_Orientation>0</m_Direction>\r\n");
                obj.Append("<PropertyMesh><m_RenderParameters>CDATA[" + Iso88591.GetString(rdata) + "]</m_RenderParameters>\r\n</PropertyMesh>\r\n<Blocking></Blocking>\r\n</Object>");
            }
            mid++;
            return obj.ToString();
        }
    
        public byte[][] heightmap(byte[] line)
        {
            byte[][] map = new byte[8][];
            for (byte i = 0; i < 8; i++)
            {
                map[i] = new byte[1160];
                map[i][0] = 132;
                map[i][1] = 4;
            }
            int h;
            byte g;
            byte angle = 0;

            h = 4;
            for (byte i = 0; i < 17; i++)
            {
                g = 64;
                for (byte j = 0; j < 17; j++)
                {
                    map[angle][h] = line[g];
                    h++;
                    map[angle][h] = line[g + 1];
                    h++;
                    map[angle][h] = line[g + 2];
                    h++;
                    map[angle][h] = line[g + 3];
                    h++;
                    if (j < i) g -= 4;
                }
            }
            angle++;

            h = 4;
            g = 64;
            for (byte i = 0; i < 17; i++)
            {
                for (byte j = 0; j < 17; j++)
                {
                    map[angle][h] = line[g];
                    h++;
                    map[angle][h] = line[g + 1];
                    h++;
                    map[angle][h] = line[g + 2];
                    h++;
                    map[angle][h] = line[g + 3];
                    h++;
                }
                g -= 4;
            }
            angle++;

            h = 4;
            for (byte i = 0; i < 17; i++)
            {
                g = (byte)(64 - i * 4);
                for (byte j = 0; j < 17; j++)
                {
                    map[angle][h] = line[g];
                    h++;
                    map[angle][h] = line[g + 1];
                    h++;
                    map[angle][h] = line[g + 2];
                    h++;
                    map[angle][h] = line[g + 3];
                    h++;
                    if (j >= 16 - i) g += 4;
                }
            }
            angle++;

            h = 4;
            for (byte i = 0; i < 17; i++)
            {
                g = 0;
                for (byte j = 0; j < 17; j++)
                {
                    map[angle][h] = line[g];
                    h++;
                    map[angle][h] = line[g + 1];
                    h++;
                    map[angle][h] = line[g + 2];
                    h++;
                    map[angle][h] = line[g + 3];
                    h++;
                    g += 4;
                }
            }
            angle++;

            h = 4;
            for (byte i = 0; i < 17; i++)
            {
                g = (byte)(i * 4);
                for (byte j = 0; j < 17; j++)
                {
                    map[angle][h] = line[g];
                    h++;
                    map[angle][h] = line[g + 1];
                    h++;
                    map[angle][h] = line[g + 2];
                    h++;
                    map[angle][h] = line[g + 3];
                    h++;
                    if (j >= i) g += 4;
                }
            }
            angle++;

            h = 4;
            g = 0;
            for (byte i = 0; i < 17; i++)
            {
                for (byte j = 0; j < 17; j++)
                {
                    map[angle][h] = line[g];
                    h++;
                    map[angle][h] = line[g + 1];
                    h++;
                    map[angle][h] = line[g + 2];
                    h++;
                    map[angle][h] = line[g + 3];
                    h++;
                }
                g += 4;
            }
            angle++;

            h = 4;
            for (byte i = 0; i < 17; i++)
            {
                g = 64;
                for (byte j = 0; j < 17; j++)
                {
                    map[angle][h] = line[g];
                    h++;
                    map[angle][h] = line[g + 1];
                    h++;
                    map[angle][h] = line[g + 2];
                    h++;
                    map[angle][h] = line[g + 3];
                    h++;
                    if (j < 16 - i) g -= 4;
                }
            }
            angle++;

            h = 4;
            for (byte i = 0; i < 17; i++)
            {
                g = 64;
                for (byte j = 0; j < 17; j++)
                {
                    map[angle][h] = line[g];
                    h++;
                    map[angle][h] = line[g + 1];
                    h++;
                    map[angle][h] = line[g + 2];
                    h++;
                    map[angle][h] = line[g + 3];
                    h++;
                    g -= 4;
                }
            }
            angle++;

            return map;
        }

        public void element(byte[] hmap, string tex1, string tex2, byte tindex, int flags)
        {
            isd.Append("<Element><VertexResolution>4</>\r\n");
            if (type == 3) isd.Append("<Flags>" + flags + "</>\r\n");
            isd.Append("<HeightMap><Width>17</>\r\n<Data>CDATA[" + Iso88591.GetString(hmap) + "]</>\r\n</>\r\n");
            if (type == 1 | type == 2) isd.Append("<Flags>" + flags + "</>\r\n");
            isd.Append("<TexIndexData><TextureIndex>0</>\r\n<AlphaMap><Width>17</>\r\n<Data>CDATA[" + tex1 + "]</>\r\n</>\r\n</>\r\n");
            isd.Append("<TexIndexData><TextureIndex>" + tindex + "</>\r\n<AlphaMap><Width>17</>\r\n<Data>CDATA[" + tex2 + "]</>\r\n</>\r\n</>\r\n</>\r\n");
        }
    }
}