using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace StaubliRobot
{
    public class Read3DXML
    {
        /// <summary>
        /// xml数据
        /// </summary>
        double[,] pointdata;

        int PointCount = 0;

        int[,] tridata;

        int FaceCount = 0;

        double[,] normaldata;

        int NormalCount = 0;

        double[,] Newnormal;


        private bool GetXmlData(string XMLFile, string ActFile)
        {
            try
            {
                if (XMLFile == null || !File.Exists(XMLFile) || ActFile == null || !File.Exists(ActFile))
                {
                    return false;
                }


                string StrBuff;
                string[] SplitChar = null;
                //3.3 XML节点
                XmlNodeList l_Nodelist = null;
                //3.4XML属性
                XmlAttribute l_val = null;
                //3.5 搜索字符串
                string l_search = null;
                int[] ObjectId;
                //总共有多少个体
                //加载XML文档
                XmlDocument l_doc = new XmlDocument();
                l_doc.Load(XMLFile);
                //建立搜索管理器
                XmlNamespaceManager l_nsMgr = new XmlNamespaceManager(l_doc.NameTable);
                l_nsMgr.AddNamespace("dtx", l_doc.DocumentElement.NamespaceURI);


                //1.构造搜索字符串   *******     
                l_search = @"/dtx:Model_3dxml/dtx:GeometricRepresentationSet/dtx:Representation";
                l_Nodelist = l_doc.DocumentElement.SelectNodes(l_search, l_nsMgr);
                if (l_Nodelist != null && l_Nodelist.Count >= 1)
                {
                    ObjectId = new int[l_Nodelist.Count];
                    for (int k = 0; k < l_Nodelist.Count; k++)
                    {

                        l_val = l_Nodelist[k].Attributes["id"];
                        if (l_val != null)
                        {
                            ObjectId[k] = int.Parse(l_val.Value.ToString());
                        }

                        else
                        {

                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }




                //**************************************************************
                for (int ob = 0; ob < ObjectId.Length; ob++)
                {
                    l_search = @"/dtx:Model_3dxml/dtx:GeometricRepresentationSet/dtx:Representation[@id='" + ObjectId[ob].ToString() + @"']/dtx:AssociatedXML/dtx:Rep";

                    l_Nodelist = l_doc.DocumentElement.SelectNodes(l_search, l_nsMgr);

                    if (l_Nodelist != null && l_Nodelist.Count >= 1)
                    {

                        for (int k = 0; k < l_Nodelist.Count; k++)
                        {

                            StrBuff = l_Nodelist[k].ChildNodes[0].ChildNodes[0].InnerText;
                            if (StrBuff == null)
                            {
                                return false;
                            }

                            SplitChar = StrBuff.Split(new char[] { ' ', ',' });
                            if (SplitChar == null || SplitChar.Length % 3 != 0 || SplitChar.Length < 3)
                            {
                                return false;
                            }
                            PointCount = SplitChar.Length / 3;
                            pointdata = new double[PointCount, 3];
                            for (int i = 0; i < SplitChar.Length; i++)
                            {
                                pointdata[i / 3, i % 3] = double.Parse(SplitChar[i]);


                            }

                            //**************************************************************
                            StrBuff = l_Nodelist[k].ChildNodes[0].ChildNodes[1].InnerText;
                            if (StrBuff == null)
                            {
                                return false;
                            }
                            SplitChar = StrBuff.Split(new char[] { ' ', ',' });
                            if (SplitChar == null || SplitChar.Length % 3 != 0 || SplitChar.Length < 3)
                            {
                                return false;
                            }
                            NormalCount = SplitChar.Length / 3;
                            normaldata = new double[NormalCount, 3];
                            for (int i = 0; i < SplitChar.Length; i++)
                            {
                                normaldata[i / 3, i % 3] = double.Parse(SplitChar[i]);


                            }

                            //**************************************************************
                            l_val = l_Nodelist[k].ChildNodes[1].ChildNodes[0].Attributes["triangles"];
                            StrBuff = (l_val != null) ? (l_val.Value).ToString().Trim() : null;
                            if (StrBuff == null)
                            {
                                return false;
                            }
                            SplitChar = StrBuff.Split(' ');
                            if (SplitChar == null || SplitChar.Length % 3 != 0 || SplitChar.Length < 3)
                            {
                                MessageBox.Show(StrBuff);
                                return false;
                            }
                            FaceCount = SplitChar.Length / 3;
                            tridata = new int[FaceCount, 3];

                            for (int i = 0; i < SplitChar.Length; i++)
                            {
                                tridata[i / 3, i % 3] = int.Parse(SplitChar[i]);


                            }
                            //**************************************************************

                            if (!GetNewNormal()||!SaveGroundObject(ActFile))
                            {

                                return false;
                            }



                        }

                    }
                    else
                    {

                        return false;

                    }
                }




                return true;
            }
            catch 
            {
                return false;

            }







        }

        private string ToStr(double data)
        {
            if (Math.Abs(data) < 0.00001)
            {
                return "0.00000";

            }

            if (data == (double)((int)data))
            {

                return data.ToString() + ".00000";
            }

            return data.ToString();


        }


        private bool GetNewNormal()
        {
            try
            {

                Newnormal = new double[FaceCount, 3];
                for (int i = 0; i < FaceCount; i++)
                {
                double[] O = new double[3];
                double[] A = new double[3];
                double[] B = new double[3];
                    O[0] = pointdata[tridata[i, 0], 0];
                    O[1] = pointdata[tridata[i, 0], 1];
                    O[2] = pointdata[tridata[i, 0], 2];

                    A[0] = pointdata[tridata[i, 1], 0];
                    A[1] = pointdata[tridata[i, 1], 1];
                    A[2] = pointdata[tridata[i, 1], 2];

                    B[0] = pointdata[tridata[i, 2], 0];
                    B[1] = pointdata[tridata[i, 2], 0];
                    B[2] = pointdata[tridata[i, 2], 0];


                    double[] OA=new double[]{O[0]-A[0],O[1]-A[1],O[2]-A[2]};
                    double[] OB = new double[] { O[0] - B[0], O[1] - B[1], O[2] - B[2] };

                    double C0 = OA[1] * OB[2] - OA[2] * OB[1];
                    double C1 = OA[2] * OB[0] - OA[0] * OB[2];
                    double C2 = OA[0] * OB[1] - OA[1] * OB[0];

                    double T = Math.Sqrt(C0 * C0 + C1 * C1 + C2 * C2);
                    Newnormal[i, 0] = C0 / T;
                    Newnormal[i, 1] = C1 / T;
                    Newnormal[i, 2] = C2 / T;


                }
                return true;
            }
            catch
            {
                return false;
            }




        }


        private bool SaveGroundObject(string FileName)
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

                    sw.WriteLine(@"ObjectAdd = Create_Polyedron(""obj_add""," + FaceCount.ToString() + "," + PointCount.ToString() + "," + FaceCount.ToString() + ");");
                    sw.WriteLine();
                    for (int i = 0; i < PointCount; i++)
                    {

                        sw.WriteLine("Add_Point_Poly(ObjectAdd," + ToStr(pointdata[i, 0] / 1000) + "," + ToStr(pointdata[i, 1] / 1000) + "," + ToStr(pointdata[i, 2] / 1000) + ");");

                    }
                    sw.WriteLine();
                    for (int i = 0; i < FaceCount; i++)
                    {

                        sw.WriteLine("Add_Vector_Poly(ObjectAdd," + ToStr(Newnormal[i, 0]) + "," + ToStr(Newnormal[i, 1]) + "," + ToStr(Newnormal[i, 2]) + ");");

                    }
                    sw.WriteLine();

                    for (int i = 0; i < FaceCount; i++)
                    {

                        sw.WriteLine("Add_Face_Poly(ObjectAdd,3," + tridata[i, 0].ToString() + "," + i.ToString() + ","
                                                                  + tridata[i, 1].ToString() + "," + i.ToString() + ","
                                                                  + tridata[i, 2].ToString() + "," + i.ToString() + ");");

                    }
                    sw.WriteLine();
                    sw.Write(@"End_Polyhedron(ObjectAdd);
Graphic(ObjectAdd,A_TYPE_PO,MODE,SHADING,COLOR,RED,TRANSPARENCY,OPAQUE,MATERIAL,NORMAL);"
);
                    sw.WriteLine();
                    sw.Write(@"Buff = Assemble(""obj___"",Buff,ObjectAdd,
              Create_Transf( 1.000000,0.000000,0.000000,2.400000,
                             0.000000,1.000000,0.000000,0.000000,
                             0.000000,0.000000,1.000000,0.000000));");
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();
                    sw.WriteLine();

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

        private bool SaveGroundStart(string FileName)
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
              Create_Transf( 1.000000,0.000000,0.000000,0.410000,
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

        private bool SaveGroundEnd(string FileName)
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
Place_In_Cell(Buff,
                        Create_Transf( 1.000000,0.000000,0.000000,0.00000,
                                       0.000000,1.000000,0.000000,-0.000000,
                                       0.000000,0.000000,1.000000,-0.000000));");


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







        public bool TransXML2Act(string XMLFile, string ActFile)
        {



            if (XMLFile == null || ActFile == null || !File.Exists(XMLFile))
            {
                return false;

            }

            try
            {
                if (!SaveGroundStart(ActFile) || !GetXmlData(XMLFile, ActFile) || !SaveGroundEnd(ActFile))
                {
                    //   if (!SaveGroundStart(ActFile) ||  !SaveGroundEnd(ActFile))
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








        }

        public bool TransXML2Act(string XMLFile)
        {

            return TransXML2Act(XMLFile, @"C:\Program Files\Staubli\SRS 7.3\Robots\ground.act");

        }

        public bool TransXML2Act()
        {
            return TransXML2Act(@"C:\Documents and Settings\yye\Desktop\RX90B-HB-WS.3dxml.xml");

        }
    }
}
