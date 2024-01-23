using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShukakuApi.Models
{
    public class MidtransData
    {
        public transaction_details transaction_details { get; set; }
        public credit_card credit_card { get; set; }
        public customer_details customer_details { get; set; }
    }

    public class transaction_details
    {
        public string order_id { get; set; }
        public int gross_amount { get; set; }
    }

    public class credit_card
    {
        public bool secure { get; set; }
    }

    public class customer_details
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }

    public class MidtransResponse
    {
        public string token { get; set; }
        public string redirect_url { get; set; }
    }
}