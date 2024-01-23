using ShukakuApi.Helpers;
using ShukakuApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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

        public List<GeneralPromoViewModel> GetActivePromoDiscounts()
        {
            var today = DateTime.Today;

            var activePromoDiscount = _db.GeneralPromos.Where(x => x.IsActive &&
                x.PromoStartDate <= today && today <= x.PromoEndDate).ToList();

            List<GeneralPromoViewModel> generalPromoViewModels = new List<GeneralPromoViewModel>();

            foreach(var model in activePromoDiscount)
            {
                GeneralPromoViewModel generalPromoViewModel = new GeneralPromoViewModel();
                generalPromoViewModel.BranchID = model.BranchID;
                generalPromoViewModel.Buy = model.Buy;
                generalPromoViewModel.DiscountType = model.DiscountType;
                generalPromoViewModel.DiscountValue = model.DiscountValue;
                generalPromoViewModel.Get = model.Get;
                generalPromoViewModel.ID = model.ID;
                generalPromoViewModel.IsActive = model.IsActive;
                generalPromoViewModel.IsDeleted = model.IsDeleted;
                generalPromoViewModel.IsPromo = model.IsPromo;
                generalPromoViewModel.PromoCode = model.PromoCode;
                generalPromoViewModel.PromoEndDate = model.PromoEndDate;
                generalPromoViewModel.PromoStartDate = model.PromoStartDate;
                generalPromoViewModel.PromoTitle = model.PromoTitle;

                generalPromoViewModels.Add(generalPromoViewModel);
            }

            return generalPromoViewModels;
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
                viewModel.PhoneNumber = profile.PhoneNumber;
                viewModel.HashedPassword = profile.PasswordHashed;

                return viewModel;
            }

            return null;
        }

        public bool UpdateUser(ProfileViewModel updatedUser)
        {
            try
            {
                var profile = _db.Users.FirstOrDefault(x => x.Email == updatedUser.Email);

                if (profile == null)
                {
                    profile = new User();
                    profile.Kelurahan = "";
                    profile.Kecamatan = "";
                    profile.Modified = DateTime.Now;
                    profile.PhoneNumberValidated = true;
                    profile.Created = DateTime.Now;
                    profile.PostCode = "";
                    profile.Province = "";
                    profile.Address = updatedUser.Address;
                    profile.FullName = updatedUser.FullName;
                    profile.LastName = updatedUser.LastName;
                    profile.PhoneNumber = updatedUser.PhoneNumber;
                    profile.Email = updatedUser.Email;
                    profile.City = "";

                    // user is trying to change password
                    if (updatedUser.PasswordNew != "")
                    {
                        if (updatedUser.PasswordNew != updatedUser.PasswordConfirmation)
                        {
                            return false;
                        }

                        profile.PasswordHashed = Helper.CreateHashedPassword(updatedUser.PasswordNew);
                    }

                    _db.Users.InsertOnSubmit(profile);
                    _db.SubmitChanges();
                }
                else
                {
                    if (!Helper.CompareHashedPassword(profile.PasswordHashed, updatedUser.Password))
                    {
                        return false;
                    }

                    profile.Address = updatedUser.Address;
                    profile.FullName = updatedUser.FullName;
                    profile.LastName = updatedUser.LastName;
                    profile.Kelurahan = "";
                    profile.Kecamatan = "";
                    profile.Modified = DateTime.Now;
                    profile.PhoneNumberValidated = true;
                    profile.PostCode = "";
                    profile.Province = "";
                    profile.PhoneNumber = updatedUser.PhoneNumber;
                    profile.City = "";

                    // user is trying to change password
                    if (updatedUser.PasswordNew != "")
                    {
                        if (updatedUser.PasswordNew != updatedUser.PasswordConfirmation)
                        {
                            return false;
                        }

                        if (!Helper.CompareHashedPassword(profile.PasswordHashed, updatedUser.Password))
                        {
                            return false;
                        }

                        profile.PasswordHashed = Helper.CreateHashedPassword(updatedUser.PasswordNew);
                    }

                    _db.SubmitChanges();
                }

                return true;
            }
            catch(Exception ex)
            {
                string FilePath = HttpContext.Current.Server.MapPath("~/logs/Log.txt");
                string FileContent = ex.Message + "\r\n" + ex.StackTrace;
                File.WriteAllText(FilePath, FileContent);

                return false;
            }
        }

        public List<Branch> GetBranches()
        {
            var branches = _db.Branches.Where(x => x.IsActive).ToList();

            return branches;
        }

        public void InsertNewOrder(Order newOrder)
        {
            _db.Orders.InsertOnSubmit(newOrder);
            _db.SubmitChanges();
        }

        public List<OrderViewModel> GetOrders(string email)
        {
            var orders = _db.Orders.Where(x => x.User.Email == email && (
                x.MidtransTransactionStatus.ToLower() == "settlement" || x.MidtransTransactionStatus.ToLower() == "capture")).ToList();

            List<OrderViewModel> orderViewModels = new List<OrderViewModel>();

            foreach (var order in orders)
            {
                OrderViewModel orderViewModel = new OrderViewModel();
                orderViewModel.Email = email;
                orderViewModel.BranchID = order.BranchID;
                orderViewModel.BranchName = order.Branch.BranchName;
                orderViewModel.ChangeBranchDateTime = order.ChangeBranchDateTime;
                orderViewModel.ChildrenPax = order.ChildrenPax;
                orderViewModel.DiscountAmount = order.DiscountAmount;
                orderViewModel.DiscountApplied = order.DiscountApplied;
                orderViewModel.FullName = order.User.FullName;
                orderViewModel.ID = order.ID;
                orderViewModel.LastName = order.User.LastName;
                orderViewModel.MenuName = order.Menu.MenuName;
                orderViewModel.MidtransOrderID = order.MidtransOrderID;
                orderViewModel.MidtransToken = order.MidtransToken;
                orderViewModel.MidtransTransactionStatus = order.MidtransTransactionStatus;
                orderViewModel.MidtransUpdate = order.MidtransUpdate;
                orderViewModel.MidtransUpdateDateTime = order.MidtransUpdateDateTime;
                orderViewModel.OldBranchID = order.OldBranchID;
                orderViewModel.OrderCompleted = order.OrderCompleted;
                orderViewModel.OrderDate = order.OrderDate;
                orderViewModel.OrderForDate = order.OrderForDate;
                orderViewModel.OrderStatus = order.OrderStatus;
                orderViewModel.OrderTotal = order.OrderTotal;
                orderViewModel.Pax = order.Pax;
                orderViewModel.PB1 = order.PB1;
                orderViewModel.PromoCodeApplied = order.PromoCodeApplied;
                orderViewModel.PromoID = order.PromoID;
                orderViewModel.SeniorPax = order.SeniorPax;
                orderViewModel.ServiceCharge = order.ServiceCharge;
                orderViewModel.SubTotal = order.SubTotal;
                orderViewModel.SubTotalAfterDiscount = order.SubTotalAfterDiscount;
                orderViewModel.UserID = order.UserID;
                orderViewModel.MenuPhotoUrl = order.Menu.MenuPhotoPath;
                
                if(DateTime.Now.Date >= order.OrderForDate.Date)
                {
                    orderViewModel.IsUpcoming = false;
                }
                else
                {
                    orderViewModel.IsUpcoming = true;
                }

                orderViewModels.Add(orderViewModel);
            }

            return orderViewModels;
        }

        public List<OrderViewModel> GetOrdersByBranchID(int id)
        {
            var orders = _db.Orders.Where(x => x.BranchID == id && (
                x.MidtransTransactionStatus.ToLower() == "settlement" || x.MidtransTransactionStatus.ToLower() == "capture")).OrderByDescending(x => x.ID).ToList();

            List<OrderViewModel> orderViewModels = new List<OrderViewModel>();

            foreach (var order in orders)
            {
                OrderViewModel orderViewModel = new OrderViewModel();
                orderViewModel.Email = order.User.Email;
                orderViewModel.BranchID = order.BranchID;
                orderViewModel.BranchName = order.Branch.BranchName;
                orderViewModel.ChangeBranchDateTime = order.ChangeBranchDateTime;
                orderViewModel.ChildrenPax = order.ChildrenPax;
                orderViewModel.DiscountAmount = order.DiscountAmount;
                orderViewModel.DiscountApplied = order.DiscountApplied;
                orderViewModel.FullName = order.User.FullName;
                orderViewModel.ID = order.ID;
                orderViewModel.LastName = order.User.LastName;
                orderViewModel.MenuName = order.Menu.MenuName;
                orderViewModel.MidtransOrderID = order.MidtransOrderID;
                orderViewModel.MidtransToken = order.MidtransToken;
                orderViewModel.MidtransTransactionStatus = order.MidtransTransactionStatus;
                orderViewModel.MidtransUpdate = order.MidtransUpdate;
                orderViewModel.MidtransUpdateDateTime = order.MidtransUpdateDateTime;
                orderViewModel.OldBranchID = order.OldBranchID;
                orderViewModel.OrderCompleted = order.OrderCompleted;
                orderViewModel.OrderDate = order.OrderDate;
                orderViewModel.OrderForDate = order.OrderForDate;
                orderViewModel.OrderStatus = order.OrderStatus;
                orderViewModel.OrderTotal = order.OrderTotal;
                orderViewModel.Pax = order.Pax;
                orderViewModel.PB1 = order.PB1;
                orderViewModel.PromoCodeApplied = order.PromoCodeApplied;
                orderViewModel.PromoID = order.PromoID;
                orderViewModel.SeniorPax = order.SeniorPax;
                orderViewModel.ServiceCharge = order.ServiceCharge;
                orderViewModel.SubTotal = order.SubTotal;
                orderViewModel.SubTotalAfterDiscount = order.SubTotalAfterDiscount;
                orderViewModel.UserID = order.UserID;
                orderViewModel.MenuPhotoUrl = order.Menu.MenuPhotoPath;

                if (DateTime.Now.Date >= order.OrderForDate.Date)
                {
                    orderViewModel.IsUpcoming = false;
                }
                else
                {
                    orderViewModel.IsUpcoming = true;
                }

                orderViewModels.Add(orderViewModel);
            }

            return orderViewModels;
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