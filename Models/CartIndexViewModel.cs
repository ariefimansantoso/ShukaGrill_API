using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShukakuApi.Models
{
    public class CartUpdateViewModel
    {
        public int MenuID { get; set; }
        public int BranchID { get; set; }
        public int PaxChildren { get; set; }
        public int Pax { get; set; }
        public int PaxSenior { get; set; }

    }
    public class GeneralPromoViewModel
    {
        public int ID
        {
            get; set;
        }

        public string PromoCode
        {
            get; set;
        }

        public System.DateTime PromoStartDate
        {
            get; set;
        }

        public System.DateTime PromoEndDate
        {
            get; set;
        }

        public bool IsDeleted
        {
            get; set;
        }

        public int DiscountValue
        {
            get; set;
        }

        public bool IsActive
        {
            get; set;
        }

        public bool IsPromo
        {
            get; set;
        }

        public string DiscountType
        {
            get; set;
        }

        public int BranchID
        {
            get; set;
        }

        public int Buy
        {
            get; set;
        }

        public int Get
        {
            get; set;
        }
        
        public string PromoTitle
        {
            get;set;
        }
    }

    public class CartIndexViewModel
    {
        public GeneralPromoViewModel CurrentPromo { get; set; }
        public MenuViewModel SelectedMenu { get; set; }
        public int PaxChildren { get; set; }
        public int Pax { get; set; }
        public int PaxSenior { get; set; }
        public int SelectedBranchId { get; set; }
        public int TotalPax { get; set; }
        public bool IsDiskon { get; set; }
        public decimal Price { get; set; }
        public decimal ChildrenPrice { get; set; }
        public decimal SeniorPrice { get; set; }
        public decimal PaxPrice { get; set; }
        public decimal PaxChildrenPrice { get; set; }
        public decimal PaxSeniorPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalPriceBeforeDiscount { get; set; }
        public DateTime TodayDate { get; set; }
        public string DiscountType { get; set; }
        public decimal Diskon { get; set; }
        public decimal TotalAfterDiskon { get; set; }
        public decimal PB1 { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal GrandTotal { get; set; }
        public string SelectedBranchName { get; set; }
        public int MenuID { get; set; }
        public int BranchID { get; set; }
        public string OrderForDate { get; set; }
        public string OrderEmail { get; set; }
    }
}