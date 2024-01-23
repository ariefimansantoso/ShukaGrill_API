using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;

namespace ShukakuApi.Models
{
    public class OrderViewModel
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        public int MenuID { get; set; }

        public int Pax { get; set; }

        public System.DateTime OrderDate { get; set; }

        public decimal SubTotal { get; set; }

        public bool OrderCompleted { get; set; }

        public string OrderStatus { get; set; }

        public System.DateTime OrderForDate { get; set; }

        public int BranchID { get; set; }

        public decimal OrderTotal { get; set; }

        public string MidtransOrderID { get; set; }

        public string MidtransTransactionStatus { get; set; }

        public int SeniorPax { get; set; }

        public int ChildrenPax { get; set; }

        public string MidtransToken { get; set; }

        public string MidtransUpdate { get; set; }

        public System.DateTime MidtransUpdateDateTime { get; set; }

        public decimal PB1 { get; set; }

        public decimal ServiceCharge { get; set; }

        public int DiscountApplied { get; set; }

        public string PromoCodeApplied { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal SubTotalAfterDiscount { get; set; }

        public int PromoID { get; set; }

        public int OldBranchID { get; set; }

        public System.DateTime ChangeBranchDateTime { get; set; }

        public string MenuName { get; set; }
        public string FullName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string BranchName { get; set; }
        public bool IsUpcoming { get; set; }
        public string MenuPhotoUrl { get; set; }
    }
}