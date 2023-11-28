using ShukakuApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Helpers;
using System.Web.UI.WebControls;

namespace ShukakuApi.Repositories
{
    public class ShukakuRepository : IDisposable
    {
        private ShukakuDataContext _db;

        public ShukakuRepository()
        {
            _db = new ShukakuDataContext(ConfigurationManager.ConnectionStrings["ShukakuConnectionString"].ConnectionString);
        }

        public List<MenuViewModel> GetActiveMenu()
        {
            var list = new List<MenuViewModel>();
            var activeMenu = _db.Menus.Where(x => x.IsActive).ToList();

            foreach (var menu in activeMenu)
            {
                MenuViewModel menuViewModel = new MenuViewModel();

                menuViewModel.Id = menu.ID;
                menuViewModel.PriceChildren = menu.PriceChildren;
                menuViewModel.Price = menu.Price;
                menuViewModel.PriceSenior = menu.PriceSenior;
                menuViewModel.ImageUrl = menu.MenuPhotoPath;
                menuViewModel.MinPax = menu.MinPax;
                menuViewModel.SortingOrder = menu.SortingOrder;
                menuViewModel.Description = menu.MenuDescription;
                menuViewModel.Name = menu.MenuName;

                var ratingMenu = _db.MenuRatings.Where(x => x.MenuID == menu.ID).FirstOrDefault();
                if(ratingMenu != null)
                {
                    menuViewModel.Rating = ratingMenu.RatingValue;
                }
                else
                {
                    menuViewModel.Rating = 0;
                }

                list.Add(menuViewModel);
            }

            return list;
        }

        public MenuViewModel GetMenuById(int id)
        {
            var menu = _db.Menus.FirstOrDefault(x => x.ID == id);

            if (menu != null)
            {
                MenuViewModel menuViewModel = new MenuViewModel();

                menuViewModel.Id = menu.ID;
                menuViewModel.PriceChildren = menu.PriceChildren;
                menuViewModel.Price = menu.Price;
                menuViewModel.PriceSenior = menu.PriceSenior;
                menuViewModel.ImageUrl = menu.MenuPhotoPath;
                menuViewModel.MinPax = menu.MinPax;
                menuViewModel.SortingOrder = menu.SortingOrder;
                menuViewModel.Description = menu.MenuDescription;
                menuViewModel.Name = menu.MenuName;

                var ratingMenu = _db.MenuRatings.Where(x => x.MenuID == menu.ID).FirstOrDefault();
                if (ratingMenu != null)
                {
                    menuViewModel.Rating = ratingMenu.RatingValue;
                }
                else
                {
                    menuViewModel.Rating = 0;
                }

                return menuViewModel;
            }

            return null;
        }

        public ProfileViewModel GetUser(string email)
        {
            var profile = _db.Users.SingleOrDefault(x => x.Email == email);
            ProfileViewModel viewModel = new ProfileViewModel();

            if (profile != null)
            {
                viewModel.ID = profile.ID;
                viewModel.Email = email;
                viewModel.Address = profile.Address;
                viewModel.LastName = profile.LastName;
                viewModel.City = profile.City;
                viewModel.FullName = profile.FullName;
                viewModel.Kecamatan = profile.Kecamatan;
                viewModel.Kelurahan = profile.Kelurahan;
                viewModel.PostCode = profile.PostCode;
                viewModel.Province = profile.Province;

                return viewModel;
            }

            return null;
        }

        public List<Branch> GetBranches()
        {
            var branches = _db.Branches.Where(x => x.IsActive).ToList();

            return branches;
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_db != null)
                {
                    _db.Dispose();
                    _db = null;
                }
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ShukakuRepository()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}