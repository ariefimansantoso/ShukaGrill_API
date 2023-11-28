using ShukakuApi.Models;
using ShukakuApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace ShukakuApi.Controllers
{    
    public class OrderController : ApiController
    {
        ShukakuRepository _repo;

        public OrderController()
        {
            _repo = new ShukakuRepository();
        }


    }
}
