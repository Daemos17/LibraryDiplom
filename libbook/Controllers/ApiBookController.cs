using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using datamodel;
using Services;

namespace libbook.Controllers
{
    public class ApiBookController : ApiController
    {


        Entities2 db = new Entities2();
           

        //GET  api/books
        [HttpGet]
        public IEnumerable<datamodel.Book> Get()
        {


            return db.Books.AsEnumerable();
               

            
        }




    }
}
