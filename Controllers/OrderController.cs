using Newtonsoft.Json;
using ShukakuApi.Models;
using ShukakuApi.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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

        [System.Web.Http.HttpPost]
        public IHttpActionResult Get(CartUpdateViewModel cartViewModel)
        {
            CartIndexViewModel viewModel = new CartIndexViewModel();
            viewModel.Pax = cartViewModel.Pax;
            viewModel.PaxChildren = cartViewModel.PaxChildren;
            viewModel.PaxSenior = cartViewModel.PaxSenior;
            viewModel.BranchID = cartViewModel.BranchID;
            viewModel.MenuID = cartViewModel.MenuID;

            var today = DateTime.Today;

            var selectedMenu = _repo.GetMenuById(cartViewModel.MenuID);

            var activePromoDiscount = _repo.GetActivePromoDiscounts();

            var promoBranch = activePromoDiscount.Where(x => x.BranchID == cartViewModel.BranchID).FirstOrDefault();

            if (promoBranch != null)
            {
                viewModel.CurrentPromo = promoBranch;
                viewModel.IsDiskon = true;
            }
            else
            {
                var currentPromo = activePromoDiscount.FirstOrDefault(x => x.BranchID <= 0);
                if (currentPromo != null)
                {
                    viewModel.CurrentPromo = currentPromo;
                    viewModel.IsDiskon = true;
                }
                else
                {
                    viewModel.IsDiskon = false;
                }
            }

            viewModel.TotalPax = cartViewModel.Pax + cartViewModel.PaxChildren + cartViewModel.PaxSenior;
            viewModel.TodayDate = today;
            viewModel.Price = selectedMenu.Price;
            viewModel.ChildrenPrice = selectedMenu.PriceChildren;
            viewModel.SeniorPrice = selectedMenu.PriceSenior;
            viewModel.PaxPrice = cartViewModel.Pax * selectedMenu.Price;
            viewModel.PaxChildrenPrice = cartViewModel.PaxChildren * selectedMenu.PriceChildren;
            viewModel.PaxSeniorPrice = cartViewModel.PaxSenior * selectedMenu.PriceSenior;
            viewModel.TotalPrice = viewModel.PaxPrice + viewModel.PaxChildrenPrice + viewModel.PaxSeniorPrice;
            viewModel.TotalPriceBeforeDiscount = viewModel.TotalPrice;
            viewModel.Diskon = 0;
            viewModel.TotalAfterDiskon = 0;

            if (viewModel.IsDiskon)
            {
                if (viewModel.CurrentPromo.DiscountType == "PERCENTAGE")
                {
                    viewModel.DiscountType = "Diskon " + viewModel.CurrentPromo.DiscountValue + "%";
                    viewModel.Diskon = viewModel.TotalPrice * ((decimal)viewModel.CurrentPromo.DiscountValue / (decimal)100);
                }
                else if (viewModel.CurrentPromo.DiscountType == "PERSON")
                {
                    viewModel.DiscountType = "Diskon Buy " + viewModel.CurrentPromo.Buy + " get " + viewModel.CurrentPromo.Get;
                    var countGet = viewModel.CurrentPromo.Get;
                    for (int i = countGet; i <= viewModel.TotalPax; i += countGet)
                    {
                        if (cartViewModel.PaxChildren > 0)
                        {
                            viewModel.Diskon += selectedMenu.PriceChildren;
                        }
                        else if (cartViewModel.PaxSenior > 0)
                        {
                            viewModel.Diskon += selectedMenu.PriceSenior;
                        }
                        else
                        {
                            viewModel.Diskon += selectedMenu.Price;
                        }
                    }
                }
                else if (viewModel.CurrentPromo.DiscountType == "VOUCHER")
                {
                    viewModel.DiscountType = "Diskon Voucher";
                    viewModel.Diskon = viewModel.TotalPrice * ((decimal)viewModel.CurrentPromo.DiscountValue / (decimal)100);
                }

                viewModel.TotalAfterDiskon = viewModel.TotalPrice - viewModel.Diskon;
                viewModel.TotalPrice = viewModel.TotalPrice - viewModel.Diskon;
            }

            viewModel.PB1 = viewModel.TotalPrice * (decimal)0.1;
            var totalPb1 = viewModel.TotalPrice + viewModel.PB1;
            viewModel.ServiceCharge = totalPb1 * (decimal)0.05;
            viewModel.GrandTotal = (decimal)(viewModel.TotalPrice + viewModel.PB1 + viewModel.ServiceCharge);

            return Json(viewModel);
        }

        [System.Web.Http.HttpPost]
        public IHttpActionResult Update(CartUpdateViewModel cartViewModel)
        {
            CartIndexViewModel viewModel = new CartIndexViewModel();
            viewModel.Pax = cartViewModel.Pax;
            viewModel.PaxChildren = cartViewModel.PaxChildren;
            viewModel.PaxSenior = cartViewModel.PaxSenior;
            viewModel.BranchID = cartViewModel.BranchID;
            viewModel.MenuID = cartViewModel.MenuID;

            var today = DateTime.Today;

            var selectedMenu = _repo.GetMenuById(cartViewModel.MenuID);

            var activePromoDiscount = _repo.GetActivePromoDiscounts();

            var promoBranch = activePromoDiscount.Where(x => x.BranchID == cartViewModel.BranchID).FirstOrDefault();

            if (promoBranch != null)
            {
                viewModel.CurrentPromo = promoBranch;
                viewModel.IsDiskon = true;
            }
            else
            {
                var currentPromo = activePromoDiscount.FirstOrDefault(x => x.BranchID <= 0);
                if (currentPromo != null)
                {
                    viewModel.CurrentPromo = currentPromo;
                    viewModel.IsDiskon = true;
                }
                else
                {
                    viewModel.IsDiskon = false;
                }
            }

            viewModel.TotalPax = cartViewModel.Pax + cartViewModel.PaxChildren + cartViewModel.PaxSenior;
            viewModel.TodayDate = today;
            viewModel.Price = selectedMenu.Price;
            viewModel.ChildrenPrice = selectedMenu.PriceChildren;
            viewModel.SeniorPrice = selectedMenu.PriceSenior;
            viewModel.PaxPrice = cartViewModel.Pax * selectedMenu.Price;
            viewModel.PaxChildrenPrice = cartViewModel.PaxChildren * selectedMenu.PriceChildren;
            viewModel.PaxSeniorPrice = cartViewModel.PaxSenior * selectedMenu.PriceSenior;
            viewModel.TotalPrice = viewModel.PaxPrice + viewModel.PaxChildrenPrice + viewModel.PaxSeniorPrice;
            viewModel.TotalPriceBeforeDiscount = viewModel.TotalPrice;
            viewModel.Diskon = 0;
            viewModel.TotalAfterDiskon = 0;

            if (viewModel.IsDiskon)
            {
                if (viewModel.CurrentPromo.DiscountType == "PERCENTAGE")
                {
                    viewModel.DiscountType = "Diskon " + viewModel.CurrentPromo.DiscountValue + "%";
                    viewModel.Diskon = viewModel.TotalPrice * ((decimal)viewModel.CurrentPromo.DiscountValue / (decimal)100);
                }
                else if (viewModel.CurrentPromo.DiscountType == "PERSON")
                {
                    viewModel.DiscountType = "Diskon Buy " + viewModel.CurrentPromo.Buy + " get " + viewModel.CurrentPromo.Get;
                    var countGet = viewModel.CurrentPromo.Get;
                    for (int i = countGet; i <= viewModel.TotalPax; i += countGet)
                    {
                        if (cartViewModel.PaxChildren > 0)
                        {
                            viewModel.Diskon += selectedMenu.PriceChildren;
                        }
                        else if (cartViewModel.PaxSenior > 0)
                        {
                            viewModel.Diskon += selectedMenu.PriceSenior;
                        }
                        else
                        {
                            viewModel.Diskon += selectedMenu.Price;
                        }
                    }
                }
                else if (viewModel.CurrentPromo.DiscountType == "VOUCHER")
                {
                    viewModel.DiscountType = "Diskon Voucher";
                    viewModel.Diskon = viewModel.TotalPrice * ((decimal)viewModel.CurrentPromo.DiscountValue / (decimal)100);
                }

                viewModel.TotalAfterDiskon = viewModel.TotalPrice - viewModel.Diskon;
                viewModel.TotalPrice = viewModel.TotalPrice - viewModel.Diskon;
            }

            viewModel.PB1 = viewModel.TotalPrice * (decimal)0.1;
            var totalPb1 = viewModel.TotalPrice + viewModel.PB1;
            viewModel.ServiceCharge = totalPb1 * (decimal)0.05;
            viewModel.GrandTotal = (decimal)(viewModel.TotalPrice + viewModel.PB1 + viewModel.ServiceCharge);

            return Json(viewModel);
        }


        [System.Web.Http.HttpPost]
        public IHttpActionResult Checkout(CartIndexViewModel viewModel)
        {
            try
            {
                var today = DateTime.Today;
                var currentUser = _repo.GetUser(viewModel.OrderEmail);

                if (currentUser == null)
                {
                    // redirect to non existense user, ask user to create profile
                    return Json(new
                    {
                        User = "Maaf, kamu belum punya profil, mohon lengkapi profil kamu dulu di halaman Profil ya"
                    });
                }

                if (string.IsNullOrEmpty(currentUser.Email) || string.IsNullOrEmpty(currentUser.FullName) || string.IsNullOrEmpty(currentUser.LastName) || string.IsNullOrEmpty(currentUser.PhoneNumber))
                {
                    return Json(new
                    {
                        User = "Maaf, profil kamu belum lengkap, mohon lengkapi profil kamu dulu di halaman Profil ya"
                    });
                }

                var dateSplitted = viewModel.OrderForDate.Split(new char[] { '-' });
                var orderForDateObj = new DateTime(Convert.ToInt32(dateSplitted[0]), Convert.ToInt32(dateSplitted[1]), Convert.ToInt32(dateSplitted[2]), Convert.ToInt32(dateSplitted[3]), Convert.ToInt32(dateSplitted[4]), 0);

                var activePromoDiscount = _repo.GetActivePromoDiscounts();

                var thePromo = activePromoDiscount.Where(x => x.BranchID == viewModel.BranchID).FirstOrDefault();

                var isDiskon = false;

                if (thePromo != null)
                {
                    isDiskon = true;
                }
                else
                {
                    thePromo = activePromoDiscount.FirstOrDefault(x => x.BranchID <= 0);
                    if (thePromo != null)
                    {
                        isDiskon = true;
                    }
                    else
                    {
                        isDiskon = false;
                    }
                }

                var orderIDMidtrans = "SHUKAKU-" + DateTime.Now.ToString("ddMMyyyyhhmmss") + Guid.NewGuid().ToString().ToUpper().Substring(0, 4);

                var isTest = Convert.ToBoolean(ConfigurationManager.AppSettings["IsTest"]);

                if (isTest)
                {
                    orderIDMidtrans = orderIDMidtrans + "-TEST";
                }

                var midtransMerchantID = ConfigurationManager.AppSettings["MidtransMerchantID"];
                var midtransClientKey = ConfigurationManager.AppSettings["MidtransClientKey"];
                var midtransServerKey = ConfigurationManager.AppSettings["MidtransServerKey"];
                var midtransEndpoint = ConfigurationManager.AppSettings["MidtransEndpoint"];

                var selectedMenu = _repo.GetMenuById(viewModel.MenuID);

                var paxPrice = viewModel.Pax * selectedMenu.Price;
                var paxChildrenPrice = viewModel.PaxChildren * selectedMenu.PriceChildren;
                var paxSeniorPrice = viewModel.PaxSenior * selectedMenu.PriceSenior;

                int orderTotalInt = Convert.ToInt32(paxPrice + paxChildrenPrice + paxSeniorPrice);
                decimal totalOrder = Convert.ToDecimal(orderTotalInt);
                int orderSubTotal = orderTotalInt;
                decimal discount = 0;
                int discountApplied = 0;
                int promoID = 0;

                decimal subTotalAfterDiscount = 0;

                var minPax = selectedMenu.MinPax;
                var totalPax = viewModel.Pax + viewModel.PaxChildren + viewModel.PaxSenior;

                if (totalPax < minPax)
                {
                    return Json(new
                    {
                        User = "Minimum jumlah pengunjung adalah " + minPax
                    });
                }

                if (isDiskon)
                {
                    if (thePromo.DiscountType == "PERCENTAGE")
                    {
                        discount = totalOrder * ((decimal)thePromo.DiscountValue / (decimal)100);
                    }
                    else if (thePromo.DiscountType == "PERSON")
                    {
                        var countGet = thePromo.Get;
                        for (int i = countGet; i <= totalPax; i += countGet)
                        {
                            if (viewModel.PaxChildren > 0)
                            {
                                discount += selectedMenu.PriceChildren;
                            }
                            else if (viewModel.PaxSenior > 0)
                            {
                                discount += selectedMenu.PriceSenior;
                            }
                            else
                            {
                                discount += selectedMenu.Price;
                            }
                        }
                    }
                    else if (thePromo.DiscountType == "VOUCHER")
                    {
                        discount = totalOrder * ((decimal)thePromo.DiscountValue / (decimal)100);
                    }

                    subTotalAfterDiscount = totalOrder - discount;
                    orderTotalInt = Convert.ToInt32(subTotalAfterDiscount);
                    promoID = thePromo.ID;
                }

                var pb1 = Convert.ToDecimal(orderTotalInt) * (decimal)0.1;
                var orderPlusPb1 = Convert.ToDecimal(orderTotalInt) + pb1;
                var serviceCharge = orderPlusPb1 * (decimal)0.05;
                var grandTotal = Convert.ToInt32(serviceCharge + orderPlusPb1);

                if (viewModel.MenuID == 1)
                {
                    if (orderForDateObj.Hour >= 17)
                    {
                        return Json(new
                        {
                            User = "Paket Eksis hanya bisa reservasi hingga pukul 17:00 setiap hari."
                        });
                    }
                }

                var newOrder = new Order
                {
                    BranchID = viewModel.BranchID,
                    ChildrenPax = viewModel.PaxChildren,
                    MenuID = viewModel.MenuID,
                    MidtransOrderID = orderIDMidtrans,
                    OrderDate = DateTime.Now,
                    OrderCompleted = false,
                    OrderStatus = "NEW",
                    OrderTotal = grandTotal,
                    Pax = viewModel.Pax,
                    SeniorPax = viewModel.PaxSenior,
                    SubTotal = orderSubTotal,
                    UserID = currentUser.ID,
                    OrderForDate = orderForDateObj,
                    MidtransTransactionStatus = "pending",
                    MidtransToken = "",
                    MidtransUpdate = "",
                    MidtransUpdateDateTime = new DateTime(1990, 1, 1),
                    PB1 = pb1,
                    ServiceCharge = serviceCharge,
                    DiscountApplied = discountApplied,
                    DiscountAmount = discount,
                    PromoCodeApplied = "",
                    SubTotalAfterDiscount = subTotalAfterDiscount,
                    ChangeBranchDateTime = new DateTime(1990, 1, 1),
                    PromoID = promoID
                };

                byte[] bytesAuthString;

                bytesAuthString = Encoding.ASCII.GetBytes(midtransServerKey + ":");
                var base64 = Convert.ToBase64String(bytesAuthString);

                var httpRequest = (HttpWebRequest)WebRequest.Create(midtransEndpoint);
                httpRequest.Method = "POST";

                httpRequest.Accept = "application/json";
                httpRequest.ContentType = "application/json";
                httpRequest.Headers["Authorization"] = "Basic " + base64;

                //var theData = string.Format(format: "{ \"transaction_details\": { \"order_id\": \"{0}\", \"gross_amount\": {1} }, \"credit_card\": { \"secure\" : true } }", arg0: orderIDMidtrans, arg1: orderTotalInt);
                MidtransData midtransData = new MidtransData();
                midtransData.transaction_details = new transaction_details
                {
                    order_id = orderIDMidtrans,
                    gross_amount = grandTotal
                };
                midtransData.credit_card = new credit_card
                {
                    secure = true
                };
                midtransData.customer_details = new customer_details
                {
                    first_name = currentUser.FullName,
                    last_name = currentUser.LastName,
                    email = currentUser.Email,
                    phone = currentUser.PhoneNumber
                };

                var theData = Newtonsoft.Json.JsonConvert.SerializeObject(midtransData, Formatting.Indented);

                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    streamWriter.Write(theData);
                }

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                string midtransResponse = "";
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    midtransResponse = streamReader.ReadToEnd();
                }

                MidtransResponse resp = System.Text.Json.JsonSerializer.Deserialize<MidtransResponse>(midtransResponse);

                newOrder.MidtransToken = resp.token;
                _repo.InsertNewOrder(newOrder);

                // delete existing cart on checkout
                //_db.Carts.DeleteOnSubmit(existingCart);
                //_db.SubmitChanges();

                return Json(new
                {
                    Url = resp.redirect_url
                });
            }
            catch(Exception ex)
            {
                string FilePath = HttpContext.Current.Server.MapPath("~/logs/Error-Checkout.txt");
                string FileContent = ex.Message + "\r\n" + ex.StackTrace;
                File.WriteAllText(FilePath, FileContent);

                return Json(new
                {
                    Error = "Mohon maaf telah terjadi kesalahan. Mohon hubungi admin di halaman kontak untuk lebih lanjut"
                });
            }
        }

        [System.Web.Http.HttpGet]
        public IHttpActionResult Orders(string email)
        {
            try
            {
                var orders = _repo.GetOrders(email);

                return Json(orders);
            }
            catch
            {
                return Json(new
                {
                    Message = "Error"
                });
            }
        }
    }
}
