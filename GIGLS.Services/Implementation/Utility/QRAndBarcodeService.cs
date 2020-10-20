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
using GIGLS.Core.DTO.Shipments;
using GIGLS.Services.Implementation.Shipments;

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

        public async Task<PreShipmentMobileThirdPartyDTO> AddImage(string waybill)
        {
            var result = new PreShipmentMobileThirdPartyDTO();
            //generate the qrcode and barcode.
            var qrCodePath = await ConverWaybillToQRCodeImage(waybill);
            var barCodePath = await ConverWaybillToBarCodeImage(waybill);
            //get gig image
            string folderPath = HostingEnvironment.MapPath("~/Images/");
            var gigImgPath = folderPath + "\\GIGLogisticsLogo.png";
            var imageByte = await MergeImages(qrCodePath, barCodePath, gigImgPath, waybill);
            File.Delete(qrCodePath);
            File.Delete(barCodePath);
            var waybillImageString = Convert.ToBase64String(imageByte);
            result.WaybillImage = waybillImageString;
            result.WaybillImageFormat = "PNG";

            //upload the image to azure blob
            var filename = $"{waybill}AI.png";
            var blobname = await AzureBlobServiceUtil.UploadAsync(imageByte, filename);
            result.ImagePath = blobname;

            return result;
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
           
            //add text to image
            var result = await AddTextToImage(waybill, folderPath);
            return result;
        }


        public async Task<byte[]> AddTextToImage(string waybill,string imgPath)
        {
            PointF textLocation = new PointF(200f, 520f);

            Bitmap bitmap = (Bitmap)Image.FromFile(imgPath);//load the image file

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                using (Font arialFont = new Font("ArialBlack", 34))
                {
                    graphics.DrawString(waybill.ToUpper(), arialFont,System.Drawing.Brushes.Black, textLocation);
                }
            }
            string folderPath = HostingEnvironment.MapPath("~/Images/");
            folderPath = $"{folderPath}\\{waybill}AI.png";

            bitmap.Save(folderPath);//save the image file
            bitmap.Dispose();
            File.Delete(imgPath);
            byte[] binaryResult = null;
            using (var ms = new MemoryStream())
            {
                Bitmap bmpm = new Bitmap(folderPath);
                bmpm.Save(ms, ImageFormat.Png);
                binaryResult = ms.ToArray();
                bmpm.Dispose();
                File.Delete(folderPath);

            }
           
            return binaryResult;
        }


    }
}
