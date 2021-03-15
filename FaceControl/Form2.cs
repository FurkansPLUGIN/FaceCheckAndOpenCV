using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceControl
{
    public partial class Form2 : Form
    {
        Bitmap img1, img2;
        int count1 = 0, count2 = 0;
        bool flag = true;
        string fname1;
        string fname2;

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Title = "Images";
            openFileDialog1.Filter = "All Images|*.jpg; *.bmp; *.png";

            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName.ToString() != "")
            {
                fname1 = openFileDialog1.FileName.ToString();
                pictureBox1.Image = new Bitmap(openFileDialog1.OpenFile());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string img1_ref, img2_ref;
            img1 = new Bitmap(fname1);
            img2 = new Bitmap(fname2);
            if (img1.Width == img2.Width && img1.Height == img2.Height)
            {
                for (int i = 0; i < img1.Width; i++)
                {
                    for (int j = 0; j < img1.Height; j++)
                    {
                        img1_ref = img1.GetPixel(i, j).ToString();
                        img2_ref = img2.GetPixel(i, j).ToString();
                        if (img1_ref != img2_ref)
                        {
                            count2++;

                            flag = false;
                            break;
                        }
                        count1++;
                    }

                }

                if (flag == false)
                    label1.Text = "Aynı değil farklı piksel sayısı: " + count2 +
                        "Ayın piksel sayısı: " + count1;

                else
                    label1.Text = "Resimler ayın: ";
            }
            else
                label1.Text = "Resimler karıştıralamiyor";
        }

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog2.FileName = "";
            openFileDialog2.Title = "Images";
            openFileDialog2.Filter = "All Images|*.jpg; *.bmp; *.png";

            openFileDialog2.ShowDialog();
            if (openFileDialog2.FileName.ToString() != "")
            {
                fname2 = openFileDialog2.FileName.ToString();
                pictureBox2.Image = new Bitmap(openFileDialog2.OpenFile());
            }
        }
    }
}
