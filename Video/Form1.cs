using AForge.Video;
using AForge.Video.DirectShow;
using Accord.Video.FFMPEG;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using Accord.Imaging.Filters;
using Accord.Imaging.Textures;
using Accord;

namespace Video
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private SaveFileDialog saveAvi;
        private FilterInfoCollection VideoCaptureDevices;
        private VideoCaptureDevice FinalVideo;
        private VideoCaptureDeviceForm captureDevice = new VideoCaptureDeviceForm();
       // private VideoFileWriter writer = new VideoFileWriter();
        private string butStop = "";
        private static Bitmap image;

        private void Form1_Load(object sender, EventArgs e)
        {
            VideoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo device in VideoCaptureDevices)
            {
                comboBox1.Items.Add(device);
            }
            captureDevice = new VideoCaptureDeviceForm();
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FinalVideo = new VideoCaptureDevice(VideoCaptureDevices[comboBox1.SelectedIndex].MonikerString);
            FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);
            FinalVideo.Start();
        }

        private void FinalVideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (butStop == "Stop Record")
            {
                image = (Bitmap)eventArgs.Frame.Clone();
                pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();

                //FileWriter.WriteVideoFrame(image);
                //AVIwriter.AddFrame(video);
            }
            else
            {
                image = (Bitmap)eventArgs.Frame.Clone();
                pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (FinalVideo.IsRunning == true)
            {
                FinalVideo.Stop();
                pictureBox1.Image = null;
                pictureBox1.Invalidate();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
                Application.Exit(); 
        }

       
        private void button5_Click(object sender, EventArgs e)
        {
            image = (Bitmap)pictureBox1.Image;
            pictureBox2.Image = (Bitmap)pictureBox1.Image.Clone();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                saveAvi = new SaveFileDialog();
                saveAvi.Filter = "Bitmap Image (.bmp)|*.bmp|Gif Image (.gif)|*.gif|JPEG Image (.jpeg)|*.jpeg|Png Image (.png)|*.png|Tiff Image (.tiff)|*.tiff|Wmf Image (.wmf)|*.wmf";
                if (saveAvi.ShowDialog() == DialogResult.OK)
                {
                    pictureBox2.Image.Save(saveAvi.FileName);
                    MessageBox.Show("The photo has saved.", "Success", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("The photo couldn't save.", "Fail", MessageBoxButtons.OK);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = null;
            pictureBox2.Invalidate();
        }

        private void sbContrast_ValueChanged(object sender, EventArgs e)
        {
            Bitmap img = new Bitmap(image); 
            ContrastCorrection filter = new ContrastCorrection(sbContrast.Value);
            filter.ApplyInPlace(img);
            pictureBox2.Image = img;
        }

        private void sbGrayScale_ValueChanged(object sender, EventArgs e)
        {
            
            double cr = 0.00;
            double cg = 0.00;
            double cb = 0.00;
            double value = sbGrayScale.Value / 100f;
            cr = +(value * 0.2);
            cg = +(value * 0.7);
            cb = +(value * 0.7);
           
            Bitmap img = new Bitmap(image);
            Grayscale filter = new Grayscale(cr,cg,cb);
            Bitmap grayImage = filter.Apply(img);
            if (value == 0)
            {
                pictureBox2.Image = image;
            }
            else
            {
                pictureBox2.Image = grayImage;
            }

        }

        private void sbColorRemap_ValueChanged(object sender, EventArgs e)
        {
            Bitmap img = new Bitmap(image);
            LevelsLinear filter = new LevelsLinear();
            // set ranges
            filter.InRed = new IntRange(sbColorRemap.Value, 300);
            filter.InGreen = new IntRange(sbColorRemap.Value, 300);
            filter.InBlue = new IntRange(sbColorRemap.Value, 300);
            // apply the filter
            filter.ApplyInPlace(img);
            pictureBox2.Image = img;
        }
    }
}
