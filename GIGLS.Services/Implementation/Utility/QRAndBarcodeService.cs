using GIGLS.Core.IServices.Utility;
using System;
using System.Threading.Tasks;
using GIGLS.Core;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using QRCoder;
using BarcodeLib;
using System.Web.Hosting;

namespace GIGLS.Services.Implementation.Utility
{
    public class QRAndBarcodeService : IQRAndBarcodeService
    {
        private readonly IUnitOfWork _uow;

        public QRAndBarcodeService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<string> ConverWaybillToQRCodeImage(string waybill)
        {
            try
            {
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(waybill, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                Image qrCodeImage = qrCode.GetGraphic(20);
                var imgPath = String.Empty;
                string folderPath = HostingEnvironment.MapPath("~/Images/");
                using (var ms = new MemoryStream())
                {
                    Bitmap bmp1 = new Bitmap(qrCodeImage);
                    bmp1.Save(ms, ImageFormat.Png);
                    Image img = Image.FromStream(ms);
                    img.Save($"{folderPath}\\{waybill}QC.png", ImageFormat.Png);
                    imgPath = $"{folderPath}\\{waybill}QC.png";
                }

                return imgPath;
            }
            catch
            {
                throw;
            }
        }


        public async Task<string> ConverWaybillToBarCodeImage(string waybill)
        {
            try
            {
                Barcode barcodeAPI = new Barcode();
                int imageWidth = 290;
                int imageHeight = 120;
                System.Drawing.Color foreColor = System.Drawing.Color.Black;
                System.Drawing.Color backColor = System.Drawing.Color.Transparent;
                // Generate the barcode with your settings
                Image barcodeImage = barcodeAPI.Encode(TYPE.CODE128, waybill);
                var imgPath = String.Empty;
                string folderPath = HostingEnvironment.MapPath("~/Images/");
                using (var ms = new MemoryStream())
                {
                    Bitmap bmp1 = new Bitmap(barcodeImage);
                    bmp1.Save(ms, ImageFormat.Png);
                    Image img = Image.FromStream(ms);
                    img.Save($"{folderPath}\\{waybill}BC.png", ImageFormat.Png);
                    imgPath = $"{folderPath}\\{waybill}BC.png";
                }
                return imgPath;

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<byte[]> MergeImages(string path1, string path2, string path3, string waybill)
        {
            // Loads the images to tile 
            BitmapFrame frame1 = BitmapDecoder.Create(new Uri(path1), BitmapCreateOptions.None, BitmapCacheOption.OnLoad).Frames.First();
            BitmapFrame frame2 = BitmapDecoder.Create(new Uri(path2), BitmapCreateOptions.None, BitmapCacheOption.OnLoad).Frames.First();
            BitmapFrame frame3 = BitmapDecoder.Create(new Uri(path3), BitmapCreateOptions.None, BitmapCacheOption.OnLoad).Frames.First();

            // initialize image size
            int imageWidth = frame1.PixelWidth;
            int imageHeight = frame1.PixelHeight;
            int gigImgWidth = frame1.PixelWidth + frame1.PixelWidth;
            int gigImgHeight = frame1.PixelHeight / 2;
            int bcImgsp = frame1.PixelWidth / imageHeight;
            string folderPath = HostingEnvironment.MapPath("~/Images/");
            folderPath = $"{folderPath}\\{waybill}MI.png";

            // Draws the images into a DrawingVisual component
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawImage(frame1, new Rect(0, 0, imageWidth, imageHeight));
                drawingContext.DrawImage(frame2, new Rect(imageWidth, 0, imageWidth, imageHeight));
                drawingContext.DrawImage(frame3, new Rect(0, imageHeight, gigImgWidth, gigImgHeight));
            }

            // Converts visual into a BitmapSource
            RenderTargetBitmap bmp = new RenderTargetBitmap(imageWidth * 2, imageHeight * 2, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);

            //Add bitmapSource to the frames of the encoder
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            // Saves image
            using (Stream stream = File.Create(folderPath))
                encoder.Save(stream);

            byte[] binaryResult = null;
            using (var ms = new MemoryStream())
            {
                Bitmap bmpm = new Bitmap(folderPath);
                bmpm.Save(ms, ImageFormat.Png);
                binaryResult = ms.ToArray();
            }
            return binaryResult;
        }

    }
}
