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
    public class MenuController : ApiController
    {
        ShukakuRepository _repo;

        public MenuController()
        {
            _repo = new ShukakuRepository();
        }

        [System.Web.Http.HttpGet]
        public IHttpActionResult ActiveMenu()
        {
            var activeMenu = _repo.GetActiveMenu();

            return Json(activeMenu);
        }

        [System.Web.Http.HttpGet]
        public IHttpActionResult GetMenu(int id)
        {
            var activeMenu = _repo.GetMenuById(id);

            if(activeMenu == null)
            {
                return NotFound();
            }

            return Json(activeMenu);
        }

    }
}
