using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TclassLibrary;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Threading;


// Important: include the opencvsharp library
using OpenCvSharp;
using OpenCvSharp.Extensions;


namespace TclassLibrary
{
    public partial class WebCamera : Form
    {
        // Create class-level accesible variables
        VideoCapture capture  ;
        Mat frame;
        Bitmap image;
        private Thread camera;
        bool isCameraRunning = false;

        string cs = string.Empty;
        int ncompid = 0;
        string dloca = string.Empty;

        public WebCamera(string tcCos, int tnCompid, string tcLoca)
        {
            InitializeComponent();
            cs = tcCos;
            ncompid = tnCompid;
            dloca = tcLoca;
        }

        private void WebCamera_Load(object sender, EventArgs e)
        {

        }


        // Declare required methods
        private void CaptureCamera()
        {
            camera = new Thread(new ThreadStart(CaptureCameraCallback));
            camera.Start();
        }

        private void CaptureCameraCallback()
        {

            frame = new Mat();
            capture = new VideoCapture(0);
            capture.Open(0);

            if (capture.IsOpened())
            {
                while (isCameraRunning)
                {

                    capture.Read(frame);
                    image = BitmapConverter.ToBitmap(frame);
                    if (pictureBox1.Image != null)
                    {
                        pictureBox1.Image.Dispose();
                    }
                    pictureBox1.Image = image;
                }
            }
        }


        // When the user clicks on the start/stop button, start or release the camera and setup flags
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text.Equals("Start"))
            {
                CaptureCamera();
                button1.Text = "Stop";
                isCameraRunning = true;
            }
            else
            {
                capture.Release();
                button1.Text = "Start";
                isCameraRunning = false;
            }
        }

        // When the user clicks on take snapshot, the image that is displayed in the pictureBox will be saved in your computer
        private void button2_Click(object sender, EventArgs e)
        {
            if (isCameraRunning)
            {
                // Take snapshot of the current image generate by OpenCV in the Picture Box
                Bitmap snapshot = new Bitmap(pictureBox1.Image);
                pictureBox1.Image = snapshot;
                // Save in some directory
                // in this example, we'll generate a random filename e.g 47059681-95ed-4e95-9b50-320092a3d652.png
                // snapshot.Save(@"C:\Users\sdkca\Desktop\mysnapshot.png", ImageFormat.Png);
                //         snapshot.Save(string.Format(@"C:\Users\sdkca\Desktop\{0}.png", Guid.NewGuid()), ImageFormat.Png);
            }
            else
            {
                Console.WriteLine("Cannot take picture if the camera isn't capturing image!");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}



