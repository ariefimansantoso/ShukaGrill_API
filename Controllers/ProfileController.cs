using ShukakuApi.Helpers;
using ShukakuApi.Models;
using ShukakuApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
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

            if(profile != null)
            {
                return Json(profile);
            }

            return Json(new
            {
                User = "null"
            });
        }

        public IHttpActionResult GetProfileUsingPassword(string email, string password)
        {
            var profile = _repo.GetUser(email);

            if (profile != null)
            {
                if (Helper.CompareHashedPassword(profile.HashedPassword, password))
                {
                    return Json(profile);
                }
            }

            return Json(new
            {
                User = "null"
            });
        }

        // POST api/<controller>
        [HttpPost]
        public IHttpActionResult Post(ProfileViewModel viewModel)
        {            
            var result = _repo.UpdateUser(viewModel);

            if (result == false)
            {
                return Json(new
                {
                    User = "null"
                });
            }

            return Json(new
            {
                User = "updated"
            });
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

    }
}