using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;


namespace StaubliRobot
{
    /// <summary>
    /// 把每一个三角网作为一个实体来处理，缺点文件大，读取不容易
    /// </summary>
    public class StaubliSTL
    {
        private double[, ,] stldata = null;
        public int FaceCount = 0;
        public string Srs = null;
        public int PointCount = 1;
        public double kt = 1;

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


                double a = Math.Sqrt(Math.Pow(stldata[t, 0, 0] - stldata[t, 1, 0], 2)+ Math.Pow(stldata[t, 0, 1] - stldata[t, 1, 1], 2)+ Math.Pow(stldata[t, 0, 2] - stldata[t, 1, 2], 2));
                double b = Math.Sqrt(Math.Pow(stldata[t, 0, 0] - stldata[t, 2, 0], 2)+ Math.Pow(stldata[t, 0, 1] - stldata[t, 2, 1], 2)+ Math.Pow(stldata[t, 0, 2] - stldata[t, 2, 2], 2));
                double c = Math.Sqrt(Math.Pow(stldata[t, 2, 0] - stldata[t, 1, 0], 2)+ Math.Pow(stldata[t, 2, 1] - stldata[t, 1, 1], 2)+ Math.Pow(stldata[t, 2, 2] - stldata[t, 1, 2], 2));

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
                double[, ,] Buff = new double[1000000, 3, 9];
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
     
        private bool ActFileBody(string FileName)
        {

            FileStream fs = null;

            try
            {
                if (FileName == null||!File.Exists(FileName))
                {
                    return false;

                }


                fs = new FileStream(FileName, FileMode.Append, FileAccess.Write);
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                {

                    //命名
                    string FaceName = "F_0";
                    string ObjectName;
                  

                    for (int k = 0; k < FaceCount; k++)
                    {

                        if (CheckData(k) == false)
                        {
                            continue;
                        }
                        ObjectName = "Object" + k.ToString();

                        sw.WriteLine(ObjectName + "= Create_face_object(1);");

                        sw.WriteLine();
                        sw.WriteLine(FaceName + "= Create_Polyhedral_Face (3,1,0,/* points, triangles and contours */");
                        sw.WriteLine("                              -10000.000000,11000.000000,-1000.000000,1000.000000, /* limits in u and v */");
                        sw.WriteLine("                             1,FALSE,FALSE,0); /* direction, closed in u and v, wire_frame mode */");


                        sw.WriteLine();
                        sw.WriteLine("Add_Point_in_Face(" + FaceName + "," + ToStr(stldata[k, 0, 0] / kt) + "," + ToStr(stldata[k, 0, 1] / kt) + "," + ToStr(stldata[k, 0, 2] / kt) + ", /* point */");
                        sw.WriteLine("                   " + ToStr(stldata[k, 0, 6]) + "," + ToStr(stldata[k, 0, 7]) + "," + " /* u,v */");
                        sw.WriteLine("                   " + ToStr(stldata[k, 0, 3]) + "," + ToStr(stldata[k, 0, 4]) + "," + ToStr(stldata[k, 0, 5]) + " /* normal */);");


                        sw.WriteLine("Add_Point_in_Face(" + FaceName + "," + ToStr(stldata[k, 1, 0] / kt) + "," + ToStr(stldata[k, 1, 1] / kt) + "," + ToStr(stldata[k, 1, 2] / kt) + ", /* point */");
                        sw.WriteLine("                   " + ToStr(stldata[k, 1, 6]) + "," + ToStr(stldata[k, 1, 7]) + "," + " /* u,v */");
                        sw.WriteLine("                   " + ToStr(stldata[k, 1, 3]) + "," + ToStr(stldata[k, 1, 4]) + "," + ToStr(stldata[k, 1, 5]) + " /* normal */);");


                        sw.WriteLine("Add_Point_in_Face(" + FaceName + "," + ToStr(stldata[k, 2, 0] / kt) + "," + ToStr(stldata[k, 2, 1] / kt) + "," + ToStr(stldata[k, 2, 2] / kt) + ", /* point */");
                        sw.WriteLine("                   " + ToStr(stldata[k, 2, 6]) + "," + ToStr(stldata[k, 2, 7]) + "," + " /* u,v */");
                        sw.WriteLine("                   " + ToStr(stldata[k, 2, 3]) + "," + ToStr(stldata[k, 2, 4]) + "," + ToStr(stldata[k, 2, 5]) + " /* normal */);");



                        sw.WriteLine();

                        sw.WriteLine("Add_Triangle_In_Face(F_0,0,1,2/* point indices */);");

                        sw.WriteLine();
                        sw.WriteLine("Face_Dvisions_in_u(" + FaceName + ",0);");
                        sw.WriteLine("Face_Dvisions_in_v(" + FaceName + ",0);");
                        sw.WriteLine("Add_face_in_object(" + ObjectName + ",0," + FaceName + ");");

                        sw.WriteLine();
                        sw.WriteLine(@"End_face_object(" + ObjectName + @",""SR6_stand_6_"");");
                        sw.WriteLine("Graphic(" + ObjectName + ",A_TYPE_PO,MODE,SHADING,COLOR,17,TRANSPARENCY,OPAQUE,MATERIAL,NORMAL,BACK_FACE);");

                        sw.WriteLine();





//进行转换

                        sw.Write(@"
Buff= Assemble(""obj0"",Buff," + ObjectName + @",
              Create_Transf( 1.000000,0.000000,0.000000,0.000000,
                             0.000000,1.000000,0.000000,0.031500,
                             0.000000,0.000000,1.000000,0.000000));
"
                               );

                        sw.WriteLine();
                        sw.WriteLine();
                        sw.WriteLine();
                    }


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

        private bool ActFileHead(string FileName)
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

                    sw.Write(@"O0 = Create_Parallelepiped(""para14"",0.200000,0.200000,0.005000);
Graphic(O0,A_TYPE_PO,MODE,SHADING,COLOR,WHITE,TRANSPARENCY,SEMI_OPAQUE,MATERIAL,NORMAL);



Buff = Create_Parallelepiped(""para"",0.200000,0.200000,0.005000);
Graphic(Buff,A_TYPE_PO,MODE,SHADING,COLOR,WHITE,TRANSPARENCY,SEMI_OPAQUE,MATERIAL,NORMAL);






Buff = Assemble(""obj___"",Buff,O0,
              Create_Transf( 1.000000,0.000000,0.000000,0.040000,
                             0.000000,1.000000,0.000000,0.000000,
                             0.000000,0.000000,1.000000,0.000000));");


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


        private bool ActFileEnd(string FileName)
        {




            FileStream fs = null;

            try
            {
                if (FileName == null||!File.Exists(FileName))
                {
                    return false;

                }


                fs = new FileStream(FileName, FileMode.Append, FileAccess.Write);
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                {

                    sw.Write(@"
Buff= Make_Obstacle(""ground"",Buff);

Base_Default_Dds(Buff,0.050000);

Place_In_Cell(Buff,
                        Create_Transf( 1.000000,0.000000,0.000000,0.000000,
                                       0.000000,1.000000,0.000000,0.000000,
                                       0.000000,0.000000,1.000000,0.000000));");


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


        public bool ActFile(string FileName,string [] STLFile)
        {

            if (!ActFileHead(FileName))
                return false;

            for (int i = 0; i < STLFile.Length; i++)
            {

                if (!File.Exists(STLFile[i]) || !GetSTLData(STLFile[i]) || !ActFileBody(FileName))
                {
                    return false;
                }
            
            }




            if (!ActFileEnd(FileName))
                return false;

            return true;
        }

        public bool ActFile(string[] STLFile)
        {

            return ActFile(@"C:\Program Files\Staubli\" + Srs + @"\Robots\ground.act",STLFile);
        
        }

        public bool ActFile(string STLFile)
        {
            if (!Directory.Exists(@"C:\Program Files\Staubli\" + Srs + @"\Robots"))
            {
                return false;
            }

            string[] data = new string[1];
            data[0] = STLFile;

            return ActFile(@"C:\Program Files\Staubli\" + Srs + @"\Robots\ground.act", data);

        }
    }










    public class StaubliSTLM
    {
        private double[, ,] stldata = null;
        public int FaceCount = 0;
        public string Srs = null;
        public int PointCount = 1;
        public double kt = 1;
        int para = 300;

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
                double[, ,] Buff = new double[1000000, 3, 9];
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

        private bool ActFileBody(string FileName)
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
                                    sw.WriteLine(ObjectName + "= Create_face_object(" + ((FaceCount - t) / 100+1).ToString() + ");");
                                }
                                else
                                {
                                    sw.WriteLine(ObjectName + "= Create_face_object(" + ((FaceCount - t) / 100).ToString() + ");");
                                
                                }
                            }

                            sw.WriteLine();
                        }

                        if (t %100 == 0)
                        {
                            sw.WriteLine();
                            if ((FaceCount - t) > 100)
                            {

                                sw.WriteLine(FaceName + "= Create_Polyhedral_Face (300,100,0,/* points, triangles and contours */");
                               // sw.WriteLine();
                            }
                            else
                            {

                                sw.WriteLine(FaceName + "= Create_Polyhedral_Face (" + ((FaceCount - t) * 3).ToString()+","+((FaceCount - t) * 1).ToString() + ",0,/* points, triangles and contours */");
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



                        //t=99
                        if (((t+1) % 100 == 0 && t != 0) || (t == FaceCount - 1))
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
                                sw.WriteLine("Add_Triangle_In_Face(" + FaceName + ","+(j*3+0).ToString()+","+(j*3+1).ToString()+","+(j*3+2).ToString()+"/* point indices */);");
                            }

                            sw.WriteLine();
                            sw.WriteLine("Face_Dvisions_in_u(" + FaceName + ",0);");
                            sw.WriteLine("Face_Dvisions_in_v(" + FaceName + ",0);");
                            sw.WriteLine("Add_face_in_object(" + ObjectName + ","+(t%3000/100).ToString()+","+ FaceName + ");");
                            sw.WriteLine();
                        }


                        if (((t+1) % 3000 == 0 && t != 0)||(t==FaceCount-1))
                        {
                            sw.WriteLine();
                            sw.WriteLine(@"End_face_object(" + ObjectName + @",""SR6_stand_6_"");");
                            sw.WriteLine("Graphic(" + ObjectName + ",A_TYPE_PO,MODE,SHADING,COLOR,GREY,TRANSPARENCY,OPAQUE,MATERIAL,NORMAL,BACK_FACE);");

                            sw.WriteLine();






                            //进行转换

                            sw.Write(@"
Buff= Assemble(""obj0"",Buff," + ObjectName + @",
              Create_Transf( 0.000000,0.000000,1.000000,0.000000,
                             1.000000,0.000000,0.000000,0.000000,
                             0.000000,1.000000,0.000000,0.000000));
"
                                   );

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

        private bool ActFileHead(string FileName)
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

                    sw.Write(@"O0 = Create_Parallelepiped(""para14"",0.200000,0.200000,0.005000);
Graphic(O0,A_TYPE_PO,MODE,SHADING,COLOR,WHITE,TRANSPARENCY,SEMI_OPAQUE,MATERIAL,NORMAL);



Buff = Create_Parallelepiped(""para"",0.200000,0.200000,0.005000);
Graphic(Buff,A_TYPE_PO,MODE,SHADING,COLOR,WHITE,TRANSPARENCY,SEMI_OPAQUE,MATERIAL,NORMAL);






Buff = Assemble(""obj___"",Buff,O0,
              Create_Transf( 1.000000,0.000000,0.000000,0.040000,
                             0.000000,1.000000,0.000000,0.000000,
                             0.000000,0.000000,1.000000,0.000000));");


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


        private bool ActFileEnd(string FileName)
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

                    sw.Write(@"
Buff= Make_Obstacle(""ground"",Buff);

Base_Default_Dds(Buff,0.050000);

Place_In_Cell(Buff,
                        Create_Transf( 1.000000,0.000000,0.000000,0.000000,
                                       0.000000,1.000000,0.000000,0.000000,
                                       0.000000,0.000000,1.000000,0.000000));");


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


        public bool ActFile(string FileName, string[] STLFile)
        {

            if (!ActFileHead(FileName))
                return false;

            for (int i = 0; i < STLFile.Length; i++)
            {

                if (!File.Exists(STLFile[i]) || !GetSTLData(STLFile[i]) || !ActFileBody(FileName))
                {
                    return false;
                }

            }




            if (!ActFileEnd(FileName))
                return false;

            return true;
        }

        public bool ActFile(string[] STLFile)
        {

            return ActFile(@"C:\Program Files\Staubli\" + Srs + @"\Robots\ground.act", STLFile);

        }

        public bool ActFile(string STLFile)
        {
            if (!Directory.Exists(@"C:\Program Files\Staubli\" + Srs + @"\Robots"))
            {
                return false;
            }

            string[] data = new string[1];
            data[0] = STLFile;

            return ActFile(@"C:\Program Files\Staubli\" + Srs + @"\Robots\ground.act", data);

        }
    }














}

































//O0 = Create_face_object(70);




//F_0 = Create_Polyhedral_Face (3,1,1, /* points, triangles and contours */
//                              -220.000000,110.000000,-86.000000,86.000000, /* limits in u and v */
//                              1,FALSE,FALSE,0); /* direction, closed in u and v, wire_frame mode */

//Add_Point_in_Face(F_0,0.068586,0.000000,-0.086000, /* point */
//        68.585700,86.000000, /* u,v */
//        0.000000,1.000000,0.000000 /* normal */);
//Add_Point_in_Face(F_0,-0.205000,0.000000,-0.086000, /* point */
//        -205.000000,86.000000, /* u,v */
//        0.000000,1.000000,0.000000 /* normal */);
//Add_Point_in_Face(F_0,-0.220000,0.000000,-0.071000, /* point */
//        -220.000000,71.000000, /* u,v */
//        0.000000,1.000000,0.000000 /* normal */);
//Add_Triangle_In_Face(F_0,0,1,2 /* point indices */);
//Add_Contour_In_Face(F_0,1 /* edges number */,
//        3,0,0.000000,1,0.000000,2,0.00000);

//Face_Dvisions_in_u(F_0,0);
//Face_Dvisions_in_v(F_0,0);
//Add_face_in_object(O0,0,F_0);





//End_face_object(O0,"SR6_stand_6_");
//Graphic(O0,A_TYPE_PO,MODE,SHADING,COLOR,17,TRANSPARENCY,OPAQUE,MATERIAL,NORMAL,BACK_FACE);