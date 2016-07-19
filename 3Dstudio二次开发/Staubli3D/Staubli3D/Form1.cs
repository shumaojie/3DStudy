using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Staubli3D
{
    public partial class Form1 : Form
    {
        STLClass stltest = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void BT_1OK_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            if (!Directory.Exists(TB_1SRS.Text))
            {
                MessageBox.Show("SRS目录不存在！");
                return;
            }
            if (comboBox1.Text == "")
            {
                MessageBox.Show("版本号不能为空！");
                return;
            }
            if (comboBox2.Text == "")
            {
                MessageBox.Show("机器人序号不能为空！");
                return;
            }
            PublicData.Data.RobotFile = TB_1SRS.Text.Trim() + comboBox1.Text.Trim() + @"\Robots\highres\" + comboBox2.Text.Trim() + @".act";
            if (!File.Exists(PublicData.Data.RobotFile))
            {
                MessageBox.Show("机器人文件不存在！");
                return;
            }


            if (!CheckSTlFile())
            {
                MessageBox.Show("STL文件不放置顺序不正确！");
                return;
            }
            if(!GetSTLFile())
            {
                MessageBox.Show("STL文件不存在！");
                return;
            }

            stltest = new STLClass();
            stltest.Srs = comboBox1.Text.Trim();
            stltest.RobotType = comboBox2.Text.Trim(); ;
            stltest.DirPath = TB_1SRS.Text.Trim();
            if (!Directory.Exists(stltest.DirPath + stltest.Srs + @"\Robots\"))
            {
                MessageBox.Show("安装目录不存在！");
                return;
            }
            if (CB_1MM.Checked)
            {
                stltest.kt = 1;
            }
            else
            {
                stltest.kt = 1000;
            }

            GP1_ALL.Enabled = false;
            GB_File.Enabled = false;
            PublicData.Data.ThreadStep = 0;
            ThreadPool.QueueUserWorkItem(new WaitCallback(STL), null);
            timer1.Enabled = true;

        }


        private void Config_Click(object sender, EventArgs e)
        {
            if (sender is PictureBox)
            {

                PictureBox pb = (PictureBox)sender;
                int index = int.Parse(pb.Tag.ToString());




            Form2 frm = new Form2(index);
            frm.ShowDialog();
            }


        }


        private bool CheckSTlFile()
        {
            PublicData.Data.STLCount = 0;
            if (textBox1.Text.Trim() == "")
            {

                PublicData.Data.STLCount = 0;
                return textBox2.Text.Trim() == "" && textBox3.Text.Trim() == ""
                     && textBox4.Text.Trim() == "" && textBox5.Text.Trim() == ""
                     && textBox6.Text.Trim() == "" && textBox7.Text.Trim() == ""
                     && textBox8.Text.Trim() == "" && textBox9.Text.Trim() == ""
                     && textBox10.Text.Trim() == "";

            }

            if (textBox2.Text.Trim() == "")
            {

                PublicData.Data.STLCount = 1;
                return textBox3.Text.Trim() == ""
                     && textBox4.Text.Trim() == "" && textBox5.Text.Trim() == ""
                     && textBox6.Text.Trim() == "" && textBox7.Text.Trim() == ""
                     && textBox8.Text.Trim() == "" && textBox9.Text.Trim() == ""
                     && textBox10.Text.Trim() == "";

            }

            if (textBox3.Text.Trim() == "")
            {

                PublicData.Data.STLCount = 2;
                return textBox4.Text.Trim() == "" && textBox5.Text.Trim() == ""
                     && textBox6.Text.Trim() == "" && textBox7.Text.Trim() == ""
                     && textBox8.Text.Trim() == "" && textBox9.Text.Trim() == ""
                     && textBox10.Text.Trim() == "";

            }

            if (textBox4.Text.Trim() == "")
            {

                PublicData.Data.STLCount = 3;
                return textBox5.Text.Trim() == ""
                     && textBox6.Text.Trim() == "" && textBox7.Text.Trim() == ""
                     && textBox8.Text.Trim() == "" && textBox9.Text.Trim() == ""
                     && textBox10.Text.Trim() == "";

            }

            if (textBox5.Text.Trim() == "")
            {

                PublicData.Data.STLCount = 4;
                return textBox6.Text.Trim() == "" && textBox7.Text.Trim() == ""
                     && textBox8.Text.Trim() == "" && textBox9.Text.Trim() == ""
                     && textBox10.Text.Trim() == "";

            }
 
            if (textBox6.Text.Trim() == "")
            {

                PublicData.Data.STLCount = 5;
                return textBox7.Text.Trim() == ""
                     && textBox8.Text.Trim() == "" && textBox9.Text.Trim() == ""
                     && textBox10.Text.Trim() == "";

            }

            if (textBox7.Text.Trim() == "")
            {

                PublicData.Data.STLCount = 6;
                return textBox8.Text.Trim() == "" && textBox9.Text.Trim() == ""
                     && textBox10.Text.Trim() == "";

            }

            if (textBox8.Text.Trim() == "")
            {

                PublicData.Data.STLCount = 7;
                return textBox9.Text.Trim() == ""
                     && textBox10.Text.Trim() == "";

            }

            if (textBox9.Text.Trim() == "")
            {

                PublicData.Data.STLCount = 8;
                return textBox10.Text.Trim() == "";

            }


            if (textBox10.Text.Trim() == "")
            {

                PublicData.Data.STLCount = 9;
                return true;

            }
            PublicData.Data.STLCount = 10;
            return true;

        }

        private bool GetSTLFile()
        {
            PublicData.Data.STLFile = new string[PublicData.Data.STLCount];
            if (PublicData.Data.STLCount > 0)
            {
                PublicData.Data.STLFile[0] = textBox1.Text.Trim();
                if (!File.Exists(PublicData.Data.STLFile[0]))
                    return false;

            }
            if (PublicData.Data.STLCount > 1)
            {
                PublicData.Data.STLFile[1] = textBox2.Text.Trim();
                if (!File.Exists(PublicData.Data.STLFile[1]))
                    return false;

            }
            if (PublicData.Data.STLCount > 2)
            {
                PublicData.Data.STLFile[2] = textBox3.Text.Trim();
                if (!File.Exists(PublicData.Data.STLFile[2]))
                    return false;

            }
            if (PublicData.Data.STLCount > 3)
            {
                PublicData.Data.STLFile[3] = textBox4.Text.Trim();
                if (!File.Exists(PublicData.Data.STLFile[3]))
                    return false;

            }
            if (PublicData.Data.STLCount > 4)
            {
                PublicData.Data.STLFile[4] = textBox5.Text.Trim();
                if (!File.Exists(PublicData.Data.STLFile[4]))
                    return false;

            }
            if (PublicData.Data.STLCount > 5)
            {
                PublicData.Data.STLFile[5] = textBox6.Text.Trim();
                if (!File.Exists(PublicData.Data.STLFile[5]))
                    return false;

            }
            if (PublicData.Data.STLCount > 6)
            {
                PublicData.Data.STLFile[6] = textBox7.Text.Trim();
                if (!File.Exists(PublicData.Data.STLFile[6]))
                    return false;

            }
            if (PublicData.Data.STLCount > 7)
            {
                PublicData.Data.STLFile[7] = textBox8.Text.Trim();
                if (!File.Exists(PublicData.Data.STLFile[7]))
                    return false;

            }
            if (PublicData.Data.STLCount > 8)
            {
                PublicData.Data.STLFile[8] = textBox9.Text.Trim();
                if (!File.Exists(PublicData.Data.STLFile[8]))
                    return false;

            }
            if (PublicData.Data.STLCount > 9)
            {
                PublicData.Data.STLFile[9] = textBox10.Text.Trim();
                if (!File.Exists(PublicData.Data.STLFile[9]))
                    return false;

            }
            return true;


        }


        private void STL(object target)
        {

            bool Res;
            if (!checkBox1.Checked)
            {
                if (PublicData.Data.STLCount == 0)
                {
                    Res = stltest.ActGroundFile();
                }
                else
                {
                    Res = stltest.ActGroundFile(PublicData.Data.STLFile, PublicData.Data.ColorType, PublicData.Data.OpaqueType, PublicData.Data.Transfer, PublicData.Data.MaterialType);
                }
            }
            else
            {
                if (PublicData.Data.STLCount == 0)
                {
                    Res = stltest.ActRobotFile(PublicData.Data.RobotFile);
                }
                else
                {
                    Res = stltest.ActRobotFile(PublicData.Data.RobotFile, PublicData.Data.STLFile, PublicData.Data.ColorType, PublicData.Data.OpaqueType, PublicData.Data.Transfer, PublicData.Data.MaterialType);
                }

            }
            if (Res)

                PublicData.Data.ThreadStep = 1;

            else
                PublicData.Data.ThreadStep = -1;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (PublicData.Data.ThreadStep == 0)
            {
                return;
            }

            if (PublicData.Data.ThreadStep == 1)
            {
                timer1.Enabled = false;
                MessageBox.Show("成功！请重启3D studio");
            }

            if (PublicData.Data.ThreadStep == -1)
            {
                timer1.Enabled = false;
                MessageBox.Show("失败！请检查输入参数");
            }
    
            GP1_ALL.Enabled = true;
            GB_File.Enabled = true;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(TB_1SRS.Text))
            {
                MessageBox.Show("SRS目录不存在！");
                return;
            }
            if (comboBox1.Text == "")
            {
                MessageBox.Show("版本号不能为空！");
                return;
            }
            if (comboBox2.Text == "")
            {
                MessageBox.Show("机器人序号不能为空！");
                return;
            }

            if (label11.BackColor.Name.ToString().ToUpper().IndexOf("FF") >= 0)
            {
                PublicData.Data.Color = "GREY";
            }
            else
            {
                PublicData.Data.Color = label11.BackColor.Name.ToString().ToUpper();
            }

            for (int i = 0; i < 9; i++)
            {

                PublicData.Data.Transfer[i] = new double[6];
                PublicData.Data.Transfer[i][0] = (double)NU0.Value;
                PublicData.Data.Transfer[i][1] = (double)NU1.Value;
                PublicData.Data.Transfer[i][2] = (double)NU2.Value;
                PublicData.Data.Transfer[i][3] = (double)NU3.Value;
                PublicData.Data.Transfer[i][4] = (double)NU4.Value;
                PublicData.Data.Transfer[i][5] = (double)NU5.Value;

                PublicData.Data.MaterialType[i] = CB_1M.Checked;
                PublicData.Data.OpaqueType[i] = CB_1O.Checked;
                PublicData.Data.ColorType[i] = PublicData.Data.Color.Trim().ToUpper();
                
            }


                MessageBox.Show("全局参数设置成功！");
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            cdg1.AllowFullOpen = true;

            if (cdg1.ShowDialog() == DialogResult.OK&&cdg1.Color.IsNamedColor)
            {
                label11.BackColor = cdg1.Color;
                PublicData.Data.Color =(-1*cdg1.Color.ToArgb()).ToString();

                if (PublicData.Data.Color == "GRAY")
                {
                    PublicData.Data.Color = "GREY";
                }
            }



        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (sender is PictureBox)
            {
                PictureBox PB = (PictureBox)sender;
                openFileDialog1.Filter = "STL文件|*.stl";
                openFileDialog1.InitialDirectory=@"C:\";

                if(openFileDialog1.ShowDialog()==DialogResult.OK)
                {
                    //MessageBox.Show("1");
                    if (PB.Tag.ToString() == "1")
                    {
                        textBox1.Text = openFileDialog1.FileName;
                        return;
                    }
                    if (PB.Tag.ToString() == "2")
                    {
                        textBox2.Text = openFileDialog1.FileName;
                        return;
                    }
                    if (PB.Tag.ToString() == "3")
                    {
                        textBox3.Text = openFileDialog1.FileName;
                        return;
                    }
                    if (PB.Tag.ToString() == "4")
                    {
                        textBox4.Text = openFileDialog1.FileName;
                        return;
                    }
                    if (PB.Tag.ToString() == "5")
                    {
                        textBox5.Text = openFileDialog1.FileName;
                        return;
                    }
                    if (PB.Tag.ToString() == "6")
                    {
                        textBox6.Text = openFileDialog1.FileName;
                        return;
                    }
                    if (PB.Tag.ToString() == "7")
                    {
                        textBox7.Text = openFileDialog1.FileName;
                        return;
                    }
                    if (PB.Tag.ToString() == "8")
                    {
                        textBox8.Text = openFileDialog1.FileName;
                        return;
                    }

                    if (PB.Tag.ToString() == "9")
                    {
                        textBox9.Text = openFileDialog1.FileName;
                        return;
                    }
                    if (PB.Tag.ToString() == "10")
                    {
                        textBox10.Text = openFileDialog1.FileName;
                        return;
                    }
                   
                }
                
            
            
            
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = @"C:\Program Files\";
            folderBrowserDialog1.ShowNewFolderButton = false;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                TB_1SRS.Text = folderBrowserDialog1.SelectedPath;
            
            }
            
        }


    }


    static public class PublicData
    {
        public struct Data
        {
            static public string Color="RED";

            static public int ThreadStep = 0;
            static public int STLCount = 0;

            static public string[] STLFile = null;
            static public string[] ColorType = new string[10] { "GREY", "GREY", "GREY", "GREY", "GREY", "GREY", "GREY", "GREY", "GREY", "GREY" };
            static public bool[] OpaqueType = new bool[10];
            static public double[][] Transfer = new double[10][] { new double[6], new double[6], new double[6], new double[6], new double[6], new double[6], new double[6], new double[6], new double[6], new double[6]};
            static public bool[] MaterialType = new bool[10];

            static public string RobotFile = "FileName";
        }



    }



    public class STLClass
    {


        public int FaceCount = 0;
        public string Srs = null;
        public int PointCount = 1;
        public double kt = 1;
        public string DirPath = null;
        public string RobotType =null;


        //读取文本

        private string[] RobotString;
        private string[] RobotStringEnd;
        private int PreLine;
        private string PreFlag = null;

        private int AftLine;
        private string AfterFlag = null;
        //
        private double[, ,] stldata = null;


        /// <summary>
        /// 小数点后面四位,3D studio默认是6位
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string ToStr(double data)
        {
            if (Math.Abs(data) < 0.0001)
            {
                return "0.0000";

            }
            data = Math.Round(data, 4);


            if (data == (double)((int)data))
            {

                return data.ToString() + ".0000";
            }

            return data.ToString();



        }
        private string ToStr6(double data)
        {
            if (Math.Abs(data) < 0.000001)
            {
                return "0.000000";

            }
            data = Math.Round(data, 6);


            if (data == (double)((int)data))
            {

                return data.ToString() + ".000000";
            }

            return data.ToString();
        }
        private string ToOpaque(bool Res)
        {
            if (Res)
                return "OPAQUE";
            else
                return "OPAQUE";

        }

        private string ToMaterial(bool Res)
        {
            if (Res)
                return "NORMAL";
            else
                return "METALLIC";

        }
        private void getSinCos(double x_angle, out double x_sin, out double x_cos)
        {
            double l_t, l_t2;

            // Too big angle involve numerical noise
            if (Math.Abs(x_angle) > Math.Pow(1, 10))
            {
                x_angle = x_angle % (2.0 * Math.PI);
            }
            l_t = Math.Tan(x_angle / 2.0);
            l_t2 = l_t * l_t;
            x_sin = 2.0 * l_t / (1.0 + l_t2);
            x_cos = (1.0 - l_t2) / (1.0 + l_t2);
        }
        private string ToTransfer(double[] transf)
        {
            if (transf == null || transf.Length != 6)
                return null;
            double MM = 1000;

            double x = transf[0] / MM;
            double y = transf[1] / MM;
            double z = transf[2] / MM;

            double rx = transf[3] * Math.PI / 180;
            double ry = transf[4] * Math.PI / 180;
            double rz = transf[5] * Math.PI / 180;



            double l_sinRx, l_sinRy, l_sinRz;
            double l_cosRx, l_cosRy, l_cosRz;

            getSinCos(rx, out l_sinRx, out l_cosRx);
            getSinCos(ry, out l_sinRy, out l_cosRy);
            getSinCos(rz, out l_sinRz, out l_cosRz);

         //   Frame x_fr = new Frame();
            double[] x_fr=new double[12];


            x_fr[0] = l_cosRz * l_cosRy;
            x_fr[1] = l_cosRz * l_sinRy * l_sinRx + l_sinRz * l_cosRx;
            x_fr[2] = -l_cosRz * l_sinRy * l_cosRx + l_sinRz * l_sinRx;

            x_fr[3] = -l_sinRz * l_cosRy;
            x_fr[4] = l_cosRz * l_cosRx - l_sinRz * l_sinRy * l_sinRx;
            x_fr[5] = l_cosRz * l_sinRx + l_sinRz * l_sinRy * l_cosRx;

            x_fr[6] = l_sinRy;
            x_fr[7] = -l_cosRy * l_sinRx;
            x_fr[8] = l_cosRy * l_cosRx;

            x_fr[9] = x;
            x_fr[10] = y;
            x_fr[11] = z;

            return "     Create_Transf(" + ToStr6(x_fr[0]) + "," + ToStr6(x_fr[3]) + "," + ToStr6(x_fr[6]) + "," + ToStr6(x_fr[9]) + "," + "\r\n" +
                   "                   " + ToStr6(x_fr[1]) + "," + ToStr6(x_fr[4]) + "," + ToStr6(x_fr[7]) + "," + ToStr6(x_fr[10]) + "," + "\r\n" +
                   "                   " + ToStr6(x_fr[2]) + "," + ToStr6(x_fr[5]) + "," + ToStr6(x_fr[8]) + "," + ToStr6(x_fr[11]) + "));";






        }
        private bool CheckData(int t)
        {
            try
            {
                if (Math.Abs(stldata[t, 0, 0] - stldata[t, 1, 0]) <= 0.0001 && Math.Abs(stldata[t, 0, 1] - stldata[t, 1, 1]) <= 0.0001 && Math.Abs(stldata[t, 0, 2] - stldata[t, 1, 2]) <= 0.0001)
                {
                    return false;
                }
                if (Math.Abs(stldata[t, 0, 0] - stldata[t, 2, 0]) <= 0.0001 && Math.Abs(stldata[t, 0, 1] - stldata[t, 2, 1]) <= 0.0001 && Math.Abs(stldata[t, 0, 2] - stldata[t, 2, 2]) <= 0.0001)
                {
                    return false;
                }


                double a = Math.Sqrt(Math.Pow(stldata[t, 0, 0] - stldata[t, 1, 0], 2) + Math.Pow(stldata[t, 0, 1] - stldata[t, 1, 1], 2) + Math.Pow(stldata[t, 0, 2] - stldata[t, 1, 2], 2));
                double b = Math.Sqrt(Math.Pow(stldata[t, 0, 0] - stldata[t, 2, 0], 2) + Math.Pow(stldata[t, 0, 1] - stldata[t, 2, 1], 2) + Math.Pow(stldata[t, 0, 2] - stldata[t, 2, 2], 2));
                double c = Math.Sqrt(Math.Pow(stldata[t, 2, 0] - stldata[t, 1, 0], 2) + Math.Pow(stldata[t, 2, 1] - stldata[t, 1, 1], 2) + Math.Pow(stldata[t, 2, 2] - stldata[t, 1, 2], 2));

                if (Math.Abs(a + b - c) <= 0.0001 || Math.Abs(a + c - b) <= 0.0001 || Math.Abs(c + b - a) <= 0.0001)
                {
                    return false;
                }


                return true;

            }
            catch
            {

                return false;
            }



        }
        private bool GetStrFlag(string Robot)
        {
            if (Robot == null)
            {
                return false;
            }
            //160L标志行
            if (Robot.Trim().ToUpper() == "rx160l".ToUpper())
            {
                PreFlag = "PO_J6_ = O0;";
                AfterFlag = @"O6,O0,REVOLUTE,";
                return true;
            }

            if (Robot.Trim().ToUpper() == "tx90".ToUpper())
            {
                PreFlag = "PO_3D_IGES_TX90_D30129300A_ENSEMBLE_33_ = O0;";
                AfterFlag = @"O0 = Assemble(""3D_IGES_TX90_D30129300A_ENSEMBLE_33"",O8,O0,";
                return true;
            }
            return false;

        }

        private bool GetSTLData(string FileName)
        {


            FileStream fs = null;
            StreamReader sr = null;
            try
            {
                if (FileName == null || !File.Exists(FileName))
                {
                    return false;
                }
                //一个面为三个点，
                //前三个为 X,Y,Z, 法线的x,y,z U,V
                //后面接着6个零
                double[, ,] Buff = new double[300000, 3, 9];
                FaceCount = 0;
                PointCount = 0;

                string StrLine = null;

                string[] SplitChar;

                fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                sr = new StreamReader(fs);  //使用StreamReader类来读取文件 
                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                while ((StrLine = sr.ReadLine()) != null)
                {
                    StrLine = StrLine.Trim();
                    if (StrLine.IndexOf("facet normal ") >= 0)
                    {
                        SplitChar = StrLine.Split(' ');
                        if (SplitChar.Length != 5)
                        {
                            return false;
                        }
                        for (int t = 0; t < 3; t++)
                        {
                            Buff[FaceCount, t, 3] = double.Parse(SplitChar[2]);
                            Buff[FaceCount, t, 4] = double.Parse(SplitChar[3]);
                            Buff[FaceCount, t, 5] = double.Parse(SplitChar[4]);
                        }
                        FaceCount++;

                    }


                    if (StrLine.IndexOf("vertex ") >= 0)
                    {
                        SplitChar = StrLine.Split(' ');
                        if (SplitChar.Length != 4)
                        {
                            return false;
                        }
                        if (PointCount >= 3)
                        {
                            PointCount = 0;

                        }
                        Buff[FaceCount - 1, PointCount, 0] = double.Parse(SplitChar[1]);
                        Buff[FaceCount - 1, PointCount, 1] = double.Parse(SplitChar[2]);
                        Buff[FaceCount - 1, PointCount, 2] = double.Parse(SplitChar[3]);
                        PointCount++;

                    }
                }

                //if (FaceCount == 0)
                //    return false;

                stldata = new double[FaceCount, 3, 9];

                for (int i = 0; i < FaceCount; i++)
                {

                    for (int j = 0; j < 3; j++)
                    {

                        for (int k = 0; k < 9; k++)
                        {

                            stldata[i, j, k] = Buff[i, j, k];

                        }

                    }

                }
                return true;


            }
            catch
            {


                return false;
            }

            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
                if (sr != null)
                {
                    sr.Close();
                }


            }


        }
        private bool GetRobotActData(string FileName, string RobotType)
        {

            FileStream fs = null;
            StreamReader sr = null;
            try
            {
                if (FileName == null || !File.Exists(FileName) || !GetStrFlag(RobotType))
                {
                    return false;
                }
                //一个面为三个点，
                //前三个为 X,Y,Z, 法线的x,y,z U,V

                RobotString = new string[100000];
                RobotStringEnd = new string[100000];
                PreLine = 0;
                AftLine = 0;
                bool Head = true;
                bool End = false;
                string StrLine = null;
                int i = 0;
                fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                sr = new StreamReader(fs);  //使用StreamReader类来读取文件 
                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                while ((StrLine = sr.ReadLine()) != null)
                {


                    if (StrLine.Trim().ToUpper().IndexOf(PreFlag.Trim().ToUpper()) >= 0)
                    {

                        PreLine = i;
                        RobotString[i] = StrLine;

                        Head = false;
                    }

                    if (StrLine.Trim().ToUpper().IndexOf(AfterFlag.Trim().ToUpper()) >= 0)
                    {
                        i = 0;
                        End = true;

                    }
                    if (Head)
                    {
                        RobotString[i] = StrLine;


                    }
                    if (End)
                    {
                        RobotStringEnd[i] = StrLine;


                    }
                    i++;

                }
                AftLine = i - 1;

                if (PreLine < 10 || AftLine < 10)
                {
                    return false;
                }
                else
                {
                    return true;
                }


            }
            catch
            {


                return false;
            }

            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
                if (sr != null)
                {
                    sr.Close();
                }


            }




        }

        private bool ActFileBody(string FileName, string ColorType, bool OpaqueType, double[] Transfer, bool MaterialType)
        {

            FileStream fs = null;

            try
            {
                if (FileName == null||!File.Exists(FileName)|| Transfer == null||Transfer.Length != 6||ColorType == null||stldata==null)
                {
                    return false;

                }
               // return false;
                fs = new FileStream(FileName, FileMode.Append, FileAccess.Write);
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                {

                    //命名
                    string FaceName = "F_0";
                    string ObjectName = "ObjectAdd";
                    int k = 0;
                  
                    for (int t = 0; t < FaceCount; t++)
                    {
                       
                        if (t % 3000 == 0)
                        {

                            if (((FaceCount - t) / 3000 > 0))
                            {
                                sw.WriteLine(ObjectName + "= Create_face_object(30);");
                            }

                            else
                            {
                                if ((FaceCount - t) % 100 > 0)
                                {
                                    sw.WriteLine(ObjectName + "= Create_face_object(" + ((FaceCount - t) / 100 + 1).ToString() + ");");
                                }
                                else
                                {
                                    sw.WriteLine(ObjectName + "= Create_face_object(" + ((FaceCount - t) / 100).ToString() + ");");

                                }
                            }

                            sw.WriteLine();
                        }

                        if (t % 100 == 0)
                        {
                            sw.WriteLine();
                            if ((FaceCount - t) > 100)
                            {

                                sw.WriteLine(FaceName + "= Create_Polyhedral_Face (300,100,0,/* points, triangles and contours */");
                            }
                            else
                            {

                                sw.WriteLine(FaceName + "= Create_Polyhedral_Face (" + ((FaceCount - t) * 3).ToString() + "," + ((FaceCount - t) * 1).ToString() + ",0,/* points, triangles and contours */");
                            }
                            sw.WriteLine("                              -10000.000000,11000.000000,-1000.000000,1000.000000, /* limits in u and v */");
                            sw.WriteLine("                             1,FALSE,FALSE,0); /* direction, closed in u and v, wire_frame mode */");
                            sw.WriteLine();
                        }


                        if (CheckData(t))
                        {
                            k = t;

                        }
                        sw.WriteLine("Add_Point_in_Face(" + FaceName + "," + ToStr(stldata[k, 0, 0] / kt) + "," + ToStr(stldata[k, 0, 1] / kt) + "," + ToStr(stldata[k, 0, 2] / kt) + ", /* point */");
                        sw.WriteLine("                   " + ToStr(stldata[k, 0, 6]) + "," + ToStr(stldata[k, 0, 7]) + "," + " /* u,v */");
                        sw.WriteLine("                   " + ToStr(stldata[k, 0, 3]) + "," + ToStr(stldata[k, 0, 4]) + "," + ToStr(stldata[k, 0, 5]) + " /* normal */);");


                        sw.WriteLine("Add_Point_in_Face(" + FaceName + "," + ToStr(stldata[k, 1, 0] / kt) + "," + ToStr(stldata[k, 1, 1] / kt) + "," + ToStr(stldata[k, 1, 2] / kt) + ", /* point */");
                        sw.WriteLine("                   " + ToStr(stldata[k, 1, 6]) + "," + ToStr(stldata[k, 1, 7]) + "," + " /* u,v */");
                        sw.WriteLine("                   " + ToStr(stldata[k, 1, 3]) + "," + ToStr(stldata[k, 1, 4]) + "," + ToStr(stldata[k, 1, 5]) + " /* normal */);");


                        sw.WriteLine("Add_Point_in_Face(" + FaceName + "," + ToStr(stldata[k, 2, 0] / kt) + "," + ToStr(stldata[k, 2, 1] / kt) + "," + ToStr(stldata[k, 2, 2] / kt) + ", /* point */");
                        sw.WriteLine("                   " + ToStr(stldata[k, 2, 6]) + "," + ToStr(stldata[k, 2, 7]) + "," + " /* u,v */");
                        sw.WriteLine("                   " + ToStr(stldata[k, 2, 3]) + "," + ToStr(stldata[k, 2, 4]) + "," + ToStr(stldata[k, 2, 5]) + " /* normal */);");

                        if (((t + 1) % 100 == 0 && t != 0) || (t == FaceCount - 1))
                        {
                            int face = 0;
                            sw.WriteLine();
                            if (t != (FaceCount - 1))
                            {
                                face = 100;

                            }
                            else
                            {

                                face = FaceCount % 100;
                            }

                            for (int j = 0; j < face; j++)
                            {
                                sw.WriteLine("Add_Triangle_In_Face(" + FaceName + "," + (j * 3 + 0).ToString() + "," + (j * 3 + 1).ToString() + "," + (j * 3 + 2).ToString() + "/* point indices */);");
                            }

                            sw.WriteLine();
                            sw.WriteLine("Face_Dvisions_in_u(" + FaceName + ",0);");
                            sw.WriteLine("Face_Dvisions_in_v(" + FaceName + ",0);");
                            sw.WriteLine("Add_face_in_object(" + ObjectName + "," + (t % 3000 / 100).ToString() + "," + FaceName + ");");
                            sw.WriteLine();
                        }


                        if (((t + 1) % 3000 == 0 && t != 0) || (t == FaceCount - 1))
                        {
                            sw.WriteLine();
                            sw.WriteLine(@"End_face_object(" + ObjectName + @",""SR6_stand_6_"");");
                            sw.WriteLine("Graphic(" + ObjectName + ",A_TYPE_PO,MODE,SHADING,COLOR," + ColorType.Trim().ToUpper() + ",TRANSPARENCY," + ToOpaque(OpaqueType) + ",MATERIAL," + ToMaterial(MaterialType) + ",BACK_FACE);");

                            sw.WriteLine();

                            //进行转换

                            sw.WriteLine(@"Buff= Assemble(""obj0"",Buff," + ObjectName + @",");
                            if (ToTransfer(Transfer) == null)
                            {
                                return false;
                            }
                            sw.Write(ToTransfer(Transfer));
                            sw.WriteLine();
                            sw.WriteLine();
                            sw.WriteLine();
                        }


                    }
                }
                return true;
            }

            catch
            {

                return false;

            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }


            }





        }

        private bool ActGroundHead(string FileName)
        {



            FileStream fs = null;

            try
            {
                if (FileName == null)
                {
                    return false;

                }


                fs = new FileStream(FileName, FileMode.Create, FileAccess.Write);
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                {

                    sw.WriteLine();
                    sw.WriteLine(@"Buff = Create_Parallelepiped(""para"",0.200000,0.200000,0.005000);");
                    sw.WriteLine(@"Graphic(Buff,A_TYPE_PO,MODE,SHADING,COLOR,WHITE,TRANSPARENCY,SEMI_OPAQUE,MATERIAL,NORMAL);");


                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.Close();
                    return true;
                }
            }

            catch
            {

                return false;

            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }


            }

        }

        private bool ActGroundEnd(string FileName)
        {




            FileStream fs = null;

            try
            {
                if (FileName == null || !File.Exists(FileName))
                {
                    return false;

                }


                fs = new FileStream(FileName, FileMode.Append, FileAccess.Write);
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                {

                    sw.WriteLine(@"Buff= Make_Obstacle(""ground"",Buff);");
                    sw.WriteLine("Base_Default_Dds(Buff,0.050000);");
                    sw.WriteLine("Place_In_Cell(Buff,");
                    sw.WriteLine(@" Create_Transf( 1.000000,0.000000,0.000000,0.000000,");
                    sw.WriteLine(@"                0.000000,1.000000,0.000000,0.000000,");
                    sw.WriteLine(@"                0.000000,0.000000,1.000000,0.000000));");
                    sw.WriteLine();


                    sw.Close();
                    return true;
                }
            }

            catch
            {

                return false;

            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }


            }











        }


        private bool ActRobotHead(string FileName)
        {
            FileStream fs = null;

            try
            {
                if (FileName == null)
                {
                    return false;

                }


                fs = new FileStream(FileName, FileMode.Create, FileAccess.Write);
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                {


                    for (int i = 0; i <= PreLine; i++)
                    {
                        sw.WriteLine(RobotString[i]);

                    }


                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine("Buff=O0;");
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.Close();
                    return true;
                }
            }

            catch
            {

                return false;

            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }


            }




        }

        private bool ActRobotEnd(string FileName)
        {
            FileStream fs = null;

            try
            {
                if (FileName == null || !File.Exists(FileName))
                {
                    return false;

                }


                fs = new FileStream(FileName, FileMode.Append, FileAccess.Write);
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                {
                    sw.WriteLine("O0=Buff;");
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();
                    for (int i = 0; i <= AftLine; i++)
                    {
                       
                            sw.WriteLine(RobotStringEnd[i]);
                   

                    }


                    
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.Close();
                    return true;
                }
            }

            catch
            {

                return false;

            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }


            }




        }







        //******************************************************************************************************************************
        //******************************************************************************************************************************
        public bool ActGroundFile(string[] STLFile, string[] ColorType, bool[] OpaqueType, double[][] Transfer, bool[] MaterialType)
        {
            if (STLFile == null || ColorType == null || OpaqueType == null || Transfer == null || MaterialType == null)
            {

                return false;
            }
            if (STLFile.Length >ColorType.Length || STLFile.Length > OpaqueType.Length || STLFile.Length > MaterialType.Length || STLFile.Length < 1)
            {
                return false;
            }

            string FileName = DirPath + Srs + @"\Robots\ground.act";

            if (!Directory.Exists(DirPath + Srs + @"\Robots\")||!File.Exists(FileName))
            {
                return false;
            }

            if (!ActGroundHead(FileName))
                return false;

            for (int i = 0; i < STLFile.Length; i++)
            {

                if (!GetSTLData(STLFile[i])|| !ActFileBody(FileName, ColorType[i], OpaqueType[i], Transfer[i], MaterialType[i]))
                {
                    return false;
                }

            }




            return ActGroundEnd(FileName);

        }

        public bool ActGroundFile()
        {
            if (!Directory.Exists(DirPath + Srs + @"\Robots"))
            {
                return false;
            }
            string FileName = DirPath + Srs + @"\Robots\ground.act";
            return ActGroundHead(FileName) && ActGroundEnd(FileName);

        }

        public bool ActRobotFile(string FileName, string[] STLFile, string[] ColorType, bool[] OpaqueType, double[][] Transfer, bool[] MaterialType)
        {
            if (!File.Exists(FileName) || STLFile == null || ColorType == null || OpaqueType == null || Transfer == null || MaterialType == null)
            {

                return false;
            }
  
            if (STLFile.Length >ColorType.Length || STLFile.Length > OpaqueType.Length || STLFile.Length >MaterialType.Length || STLFile.Length < 1)
            { 
                return false;
            }

            if (!GetRobotActData(FileName, RobotType)|| !ActRobotHead(FileName))
                return false;


            for (int i = 0; i < STLFile.Length; i++)
            {
                if (!GetSTLData(STLFile[i])|| !ActFileBody(FileName, ColorType[i], OpaqueType[i], Transfer[i], MaterialType[i]))
                {
                    return false;
                }

            }
            return ActRobotEnd(FileName);

        }

        public bool ActRobotFile(string FileName)
        {

            return File.Exists(FileName)&& GetRobotActData(FileName, RobotType)&&ActRobotHead(FileName)&&ActRobotEnd(FileName);

        }

    }



}
