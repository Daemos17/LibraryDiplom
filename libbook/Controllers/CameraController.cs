using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace libbook.Controllers
{
    public class CameraController : Controller
    {

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
        [HttpGet]
        public ActionResult Capture()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Capture(string name)
        {

            

            IFormFileCollection files =HttpContext.Request.Files.T;

           


            if (files != null)
            {
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        // Getting Filename  
                        var fileName = file.FileName;
                        // Unique filename "Guid"  
                        var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                        // Getting Extension  
                        var fileExtension = Path.GetExtension(fileName);
                        // Concating filename + fileExtension (unique filename)  
                        var newFileName = string.Concat(myUniqueFileName, fileExtension);
                        //  Generating Path to store photo   
                        var filepath = Path.Combine(_environment.WebRootPath, "CameraPhotos") + $@"\{newFileName}";

                        if (!string.IsNullOrEmpty(filepath))
                        {
                            // Storing Image in Folder  
                            StoreInFolder(file, filepath);
                        }

                        var imageBytes = System.IO.File.ReadAllBytes(filepath);
                        if (imageBytes != null)
                        {
                            // Storing Image in Folder  

                        }

                    }
                }
                return Json(true);
            }
            else
            {
                return Json(false);
            }


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
