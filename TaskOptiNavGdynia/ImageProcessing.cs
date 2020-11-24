using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TaskOptiNavGdynia
{
    class ImageProcessing
    {
        static BitmapSource parentBms;  //all clones produced from this bms
        static public int dataLength;   //size of rgb data associated with parent bitmapSource
        static private int stride;
        static private int pixelWidth;
        static private int pixelHeight;
        static private double dpiX;
        static private double dpiY;
        static private PixelFormat format;

        static public void Init(BitmapSource imageBms)
        {
            parentBms = imageBms;   //save the passed bms of parent
                                    //calc its image stride
            stride = imageBms.PixelWidth * ((imageBms.Format.BitsPerPixel + 7) / 8);
            //calc its image data length
            dataLength = stride * imageBms.PixelHeight;
            //save orignal bms data needed to clone a BitmapSource using new passed data
            pixelWidth = imageBms.PixelWidth;
            pixelHeight = imageBms.PixelHeight;
            dpiX = imageBms.DpiX;
            dpiY = imageBms.DpiY;
            format = imageBms.Format;
        } //Init()


        static public BitmapSource CloneBms(byte[] newRgbData)
        {
            BitmapSource childBms = BitmapSource.Create(pixelWidth, pixelHeight, dpiX, dpiY, format, null, newRgbData, stride);
            return childBms;
        } // CloneBms


        static public byte[] ToMainColors()
        {
            byte[] rgb = new byte[dataLength];
            parentBms.CopyPixels(rgb, stride, 0);
            return rgb;
        }
    }
}
