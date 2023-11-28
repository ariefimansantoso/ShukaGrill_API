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
    public class BranchController : ApiController
    {
        ShukakuRepository _repo;

        public BranchController()
        {
            _repo = new ShukakuRepository();
        }

        [System.Web.Http.HttpGet]
        public IHttpActionResult Get()
        {
            var branches = _repo.GetBranches();
            List<BranchViewModel> branchesVM = new List<BranchViewModel>();

            foreach (var branch in branches)
            {
                BranchViewModel viewModel = new BranchViewModel();
                viewModel.ID = branch.ID;
                viewModel.BranchName = branch.BranchName;
                branchesVM.Add(viewModel);
            }

            return Json(branchesVM);
        }

    }
}
