using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

using libbook.Content;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Net;
using ZXing.Common;
using ZXing;
using Services;
using System.Web.UI;

namespace libbook.Controllers
{
  
    public class CameraController : Controller
    {

        Services.Book serv = new Services.Book();

        Result ressultss { get; set; }

        private readonly IHostingEnvironment _environment;
        // GET: Camera
        public static Microsoft.AspNetCore.Http.HttpRequest request;
        public CameraController(IHostingEnvironment environment)
        {
            _environment = environment;
        }
        public CameraController()
        {

        }

        public ActionResult Index(string result)
        {
          
            ViewBag.Result = result;
            return View();
        }


        public ActionResult CameraCapture()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Capture()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Capture(string filePath)
        {
            Bitmap image;

            image = Base64StringToBitmap(filePath);

            string res = ExtractQRCodeMessageFromImage(image);
            //GaussianBlur blur = new GaussianBlur(image);
            //image = blur.Process(2);
            //var options = new DecodingOptions { PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.QR_CODE }, TryHarder = true };
            //using (image)
            //{
            //    LuminanceSource source;
            //    source = new BitmapLuminanceSource(image);
            //    BinaryBitmap bitmap = new BinaryBitmap(new HybridBinarizer(source));

            //     result = new MultiFormatReader().decode(bitmap);

            //    var reader = new BarcodeReader(null, null, ls => new GlobalHistogramBinarizer(ls)) { AutoRotate = false, TryInverted = false, Options = options };
            //    var result1 = reader.Decode(image);

            //}

            //if (result == null)
            //    return new ContentResult() { Content = "<script language='javascript' type='text/javascript'>alert('Try Again!');</script>" };
          
            if(res!= "QRCode couldn't be decoded.")
            {
                var book = serv.FindByGuid(res);
                if (book != null)   
                {

                    return Json(new { redirectToUrl = Url.Action("LendBook", "LendingBook", new { bookId = book.Id }) });
                }
                else
                {
                    return Json(new { redirectToUrl = Url.Action("Index", "Camera", new { result = res }) });
                }

            }
            else
            {
                return Json(new { redirectToUrl = Url.Action("Index", "Camera", new { result = res }) });
            }


            //return RedirectToAction("Index", new { result =result});
           
        }


        private string ExtractQRCodeMessageFromImage(Bitmap bitmap)
        {
            string message="";
            try
            {
                BarcodeReader reader = new BarcodeReader
                    (null, newbitmap => new BitmapLuminanceSource(bitmap), luminance => new GlobalHistogramBinarizer(luminance));

                reader.AutoRotate = true;
                reader.TryInverted = true;
                reader.Options = new DecodingOptions { TryHarder = true };

                var result = reader.Decode(bitmap);

                if (result != null)
                {
                    message = result.Text;
                    return message;
                }
                else
                {
                    message = "QRCode couldn't be decoded.";
                    return message;
                }
            }
            catch (Exception ex)
            {
                message = "QRCode couldn't be detected.";
                return message;
            }
        }



        private Bitmap Base64StringToBitmap(string base64String)
        {
            

            Bitmap bmpReturn = null;

            byte[] buffer = Convert.FromBase64String(base64String);
            MemoryStream memoryStream = new MemoryStream(buffer);

            memoryStream.Position = 0;

            bmpReturn = (Bitmap)Bitmap.FromStream(memoryStream);

            memoryStream.Close();
            memoryStream = null;
            buffer = null;

            return bmpReturn;
        }


        private void StoreInFolder(IFormFile file, string fileName)
        {
            using (FileStream fs = System.IO.File.Create(fileName))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
        }



    }
}
