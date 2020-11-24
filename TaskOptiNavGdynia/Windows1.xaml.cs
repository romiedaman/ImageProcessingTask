using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TaskOptiNavGdynia
{
    /// <summary>
    /// Interaction logic for Windows1.xaml
    /// </summary>
    public partial class Windows1 : Page
    {

        // when a button is clicked, rgbIMage is updated with a one of the sources(Bms) below
        // The Bms's below serve as a cache for the button filters.  They are null till computed.
        private BitmapSource rgbBms;
        private BitmapSource blueBms;
        private BitmapSource greenBms;
        private BitmapSource redBms;

        public enum RGB { Blue, Green, Red };

        //only rgbValues is guaranteed to always be available

        //The arrays contain the original data from the last picture loaded
        private byte[] rgbValues;  //storage for actual RGB  data (value range 0-255)   

        public Windows1()
        {
            InitializeComponent();
        }

        private void RGB_Click(object sender, RoutedEventArgs e)
        {
            tb1.Text += "RGB clicked\n";
            tb1.Text += String.Format("PixelHeight: {0}\nPixelWidth: {1}\n",
                ((BitmapSource)this.rgbImage.Source).PixelHeight,
                ((BitmapSource)this.rgbImage.Source).PixelWidth);
            this.rgbImage.Source = rgbBms;
        }


        private void Red_Click(object sender, RoutedEventArgs e)
        {
            tb1.Text += "Red clicked\n";
            if (redBms == null)
            {
                byte[] newBytes = ImageProcessing.ToMainColors();
                for (int i = 0; i < ImageProcessing.dataLength; i += 4)
                {
                    newBytes[i + (int)RGB.Blue] = 0;    //blue
                    newBytes[i + (int)RGB.Green] = 0;   //green;
                }
                redBms = ImageProcessing.CloneBms(newBytes);
            }
            this.rgbImage.Source = redBms;
        }


        private void Blue_Click(object sender, RoutedEventArgs e)
        {
            tb1.Text += "Blue clicked\n";
            if (blueBms == null)
            {
                byte[] newBytes = ImageProcessing.ToMainColors();
                for (int i = 0; i < ImageProcessing.dataLength; i += 4)
                {
                    newBytes[i + (int)RGB.Green] = 0;   //green;
                    newBytes[i + (int)RGB.Red] = 0; //red
                }
                blueBms = ImageProcessing.CloneBms(newBytes);
            }
            this.rgbImage.Source = blueBms;
        }


        private void Green_Click(object sender, RoutedEventArgs e)
        {
            tb1.Text += "Green clicked\n";
            if (greenBms == null)
            {
                byte[] newBytes = ImageProcessing.ToMainColors();
                for (int i = 0; i < ImageProcessing.dataLength; i += 4)
                {
                    newBytes[i + (int)RGB.Blue] = 0;    //blue;
                    newBytes[i + (int)RGB.Red] = 0; //red
                }
                greenBms = ImageProcessing.CloneBms(newBytes);
            }
            this.rgbImage.Source = greenBms;
        }


        private void Uncache()
        {
            blueBms = greenBms = redBms = null;
        }


        private void FileOpen_Click(object sender, RoutedEventArgs e)
        {
           
            OpenFileDialog oDialog = new OpenFileDialog();
            oDialog.Title = "Select a JPEG/PNG/BMP file to open";
            oDialog.Filter = "Image| *.Jpg; *.png; *.bmp;";

            BitmapSource resultImage = null;
            if ((bool)oDialog.ShowDialog())
            {
                using (Stream imSource = new FileStream(oDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var decoder = new JpegBitmapDecoder(imSource, BitmapCreateOptions.None, BitmapCacheOption.Default);
                    resultImage = decoder.Frames[0];
                    ImageProcessing.Init(resultImage);
                    this.rgbImage.Source = rgbBms = resultImage;
                    rgbValues = ImageProcessing.ToMainColors();
                    Uncache(); //uncache everything
                    tb1.Text += "New JPEG/PNG/BMP file loaded.\n";
                }
            }
        } // FileOpen_Click()


        private void FileSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sDialog = new SaveFileDialog();
            sDialog.Title = "Save display as a  JPEG/PNG/BMP file";
            sDialog.Filter = "Image| *.Jpg; *.png; *.bmp;";
            sDialog.ShowDialog();
            FileStream stream = new FileStream(sDialog.FileName, FileMode.Create);
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.QualityLevel = 70;
            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)this.rgbImage.Source));
            encoder.Save(stream);
            stream.Close();
        } //FileSave_Click()


        void OnLoad(object sender, RoutedEventArgs e)
        {
            tb1.Text = "Image Inited.\n";
            rgbBms = (BitmapSource)this.rgbImage.Source;
            ImageProcessing.Init(rgbBms);
            rgbValues = ImageProcessing.ToMainColors();
        }
    }
}
