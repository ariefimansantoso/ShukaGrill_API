using ShukakuApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace ShukakuApi.Controllers
{
    public class ProfileController : ApiController
    {
        ShukakuRepository _repo;

        public ProfileController()
        {
            _repo = new ShukakuRepository();
        }

        // GET api/<controller>/5
        public IHttpActionResult Get(string email)
        {
            var profile = _repo.GetUser(email);

            if(profile == null)
            {
                return Json(profile);
            }

            return Content(HttpStatusCode.BadRequest, "null");
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

    }
}