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
        public ActionResult OpenCapture(int idSt)
        {
            ViewBag.IdStudent = idSt;
            return View("Capture");
        }


        [HttpPost]
        public ActionResult Capture(string filePath, int idSt)
        {
            Bitmap image;
            image = Base64StringToBitmap(filePath);
            string res = ExtractQRCodeMessageFromImage(image);
            
            if(res!= "QRCode couldn't be decoded.")
            {
                var book = serv.FindByGuid(res);
                if (book != null)   
                {
                    return Json(new { redirectToUrl = 
                        Url.Action("LendBook", "LendingBook", new {idStudent = idSt, idBook = book.GUID }) });
                }
                else
                {
                    return Json(new { redirectToUrl = Url.Action("Index", "OpenCamera", new { idSt = idSt }) });
                }
            }
            else
            {
                return Json(new { redirectToUrl = Url.Action("Index", "LendingBook", new {idStudent = idSt}) });
            }
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
