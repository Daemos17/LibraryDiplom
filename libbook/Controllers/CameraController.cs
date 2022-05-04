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
using System.Web.UI;

namespace libbook.Controllers
{
    public class CameraController : Controller
    {

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

        [HttpGet]
        public ActionResult Capture()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Capture(string filePath)
        {
            Bitmap image ;
            Result result;
            string imagePath = "C:/users/daniil/downloads/" + filePath;
            image = (Bitmap)Bitmap.FromFile("storage/emulated/0/Download" + filePath);
            GaussianBlur blur = new GaussianBlur(image);
            image = blur.Process(2);
            var options = new DecodingOptions { PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.QR_CODE }, TryHarder = true };
            using (image)
            {
                LuminanceSource source;
                source = new BitmapLuminanceSource(image);
                BinaryBitmap bitmap = new BinaryBitmap(new HybridBinarizer(source));

                 result = new MultiFormatReader().decode(bitmap);


                
                var reader = new BarcodeReader(null, null, ls => new GlobalHistogramBinarizer(ls)) { AutoRotate = false, TryInverted = false, Options = options };
                var result1 = reader.Decode(image);

               
            }

        

            //if (result == null)
            //    return new ContentResult() { Content = "<script language='javascript' type='text/javascript'>alert('Try Again!');</script>" };
          
            
            return Json(new { redirectToUrl = Url.Action("Index", "Camera",new { result=result}) });

            //return RedirectToAction("Index", new { result =result});
           
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
