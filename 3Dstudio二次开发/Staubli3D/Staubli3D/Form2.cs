using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Staubli3D
{
    public partial class Form2 : Form
    {
        int index = 0;
        public Form2()
        {
            InitializeComponent();
        }
        public Form2(int i)
        {
            InitializeComponent();
            index = i;
        }

        private void BT_1Cancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BT_1OK_Click(object sender, EventArgs e)
        {

            if (label11.BackColor.Name.ToString().ToUpper().IndexOf("FF") >= 0)
            {
                PublicData.Data.Color = "GREY";
            }
            else
            {
                PublicData.Data.Color = label11.BackColor.Name.ToString().ToUpper();
            }
            if (PublicData.Data.Color == "GRAY")
            {
                PublicData.Data.Color = "GREY";
            }
            // MessageBox.Show(label11.BackColor.Name.ToString());
            PublicData.Data.ColorType[index] = PublicData.Data.Color;
            int i = index;
            PublicData.Data.Transfer[i] = new double[6];
            PublicData.Data.Transfer[i][5] = (double)NU6.Value;
            PublicData.Data.Transfer[i][0] = (double)NU1.Value;
            PublicData.Data.Transfer[i][1] = (double)NU2.Value;
            PublicData.Data.Transfer[i][2] = (double)NU3.Value;
            PublicData.Data.Transfer[i][3] = (double)NU4.Value;
            PublicData.Data.Transfer[i][4] = (double)NU5.Value;

            PublicData.Data.MaterialType[i] = CB_1M.Checked;
            PublicData.Data.OpaqueType[i] = CB_1O.Checked;
            this.Close();

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            cdg1.AllowFullOpen = false;
            cdg1.ShowHelp = true;
            if (cdg1.ShowDialog() == DialogResult.OK)
            {

                label11.BackColor = cdg1.Color;
                if (label11.BackColor.Name.ToString().ToUpper().IndexOf("FF") >= 0)
                {
                    PublicData.Data.Color = "GREY";
                }
                else
                {
                    PublicData.Data.Color = label11.BackColor.Name.ToString().ToUpper();
                }
                // MessageBox.Show(label11.BackColor.Name.ToString());
                PublicData.Data.ColorType[index] = PublicData.Data.Color;
            }

        }
    }
}
