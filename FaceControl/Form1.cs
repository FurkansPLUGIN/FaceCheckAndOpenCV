using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Face;
using Emgu.CV.CvEnum;
using System.IO;
using System.Threading;

namespace FaceControl
{
    public partial class Form1 : Form
    {
        #region Variables
        private Capture capture = null;
        private Image<Bgr, byte> currentFrame = null;
        Mat frame = new Mat();
        private bool yuzAlgilamaDurum = false;
        CascadeClassifier classifier = new CascadeClassifier(@"C:\haarcascades\haarcascade_frontalface_alt.xml");
        Image<Bgr, byte> yuzSonucu = null;
       static List<Image<Gray, Byte>> TrainedYuz = new List<Image<Gray, Byte>>();
       static List<int> kisiBilgileri = new List<int>();
        bool ImageKayıtDurumu = false;
       private static bool isTrained = false;
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            capture = new Capture();
            capture.ImageGrabbed += processFrame;
            capture.Start();
        }

        private void processFrame(object sender, EventArgs e)
        {
            capture.Retrieve(frame, 0);
            currentFrame = frame.ToImage<Bgr, byte>().Resize(pictureBox1.Width, pictureBox1.Height, Inter.Cubic);

            if (yuzAlgilamaDurum)
            {
                Mat grayImage = new Mat();
                CvInvoke.CvtColor(currentFrame, grayImage, ColorConversion.Bgr2Gray);
                CvInvoke.EqualizeHist(grayImage, grayImage);

                Rectangle[] faces = classifier.DetectMultiScale(grayImage, 1.1, 3, Size.Empty, Size.Empty);
                if (faces.Length > 0)
                {
                   foreach(var face in faces)
                   {
                        CvInvoke.Rectangle(currentFrame, face, new Bgr(Color.Red).MCvScalar, 2);

                        Image<Bgr, byte> imageSonuc = currentFrame.Convert<Bgr, byte>();
                        imageSonuc.ROI = face;
                        pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                        pictureBox2.Image = imageSonuc.Bitmap;
                        if (ImageKayıtDurumu)
                        {
                            string path = Directory.GetCurrentDirectory() + @"\TrainedImages";
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            Task.Factory.StartNew(() => {
                                for (int i = 0; i < 10; i++)
                                {
                                    //resize the image then saving it
                                    imageSonuc.Resize(200, 200, Inter.Cubic).Save(path + @"\" + textBox1.Text + "_" + DateTime.Now.ToString("dd-mm-yyyy-hh-mm-ss") + ".jpg");
                                    Thread.Sleep(1000);
                                }
                            });
                        }
                        ImageKayıtDurumu = false;
                        //if (button3.InvokeRequired)
                        //{
                        //    button3.Invoke(new ThreadStart(delegate {
                        //        button3.Enabled = true;
                        //    }));
                        //}
                    }

                }
            }

            pictureBox1.Image = currentFrame.Bitmap;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            yuzAlgilamaDurum = true;


        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            button6.Enabled = true;
            ImageKayıtDurumu = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button6.Enabled = false;
            button3.Enabled = true;
            ImageKayıtDurumu = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            TrainImagesFromDir();
        }

        private static bool TrainImagesFromDir()
        {
            int resimsay = 0;
            double Threshold = -1;
            kisiBilgileri.Clear();
            TrainedYuz.Clear();
            try
            {
                string path = Directory.GetCurrentDirectory() + @"\TrainedImages";
                string[] files = Directory.GetFiles(path, "*.jpg", SearchOption.AllDirectories);
                foreach(var file in files)
                {
                    Image<Gray, Byte> trainedImage = new Image<Gray, byte>(file);
                    TrainedYuz.Add(trainedImage);
                    kisiBilgileri.Add(resimsay);
                    resimsay++;
                }
                EigenFaceRecognizer recognizer = new EigenFaceRecognizer(resimsay,Threshold);
                recognizer.Train(TrainedYuz.ToArray(), kisiBilgileri.ToArray());
               return isTrained = true;
            }
            catch(Exception ex)
            {
                return isTrained = false;
                MessageBox.Show("Yüz sırası oluşturulamadı", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            frm2.ShowDialog();
        }
    }
}
