using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShukakuApi.Models
{
    public class MenuViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public decimal PriceChildren { get; set; }
        public decimal PriceSenior { get; set; }
        public decimal Rating { get; set; }
        public int MinPax { get; set; }
        public int SortingOrder { get; set; }

    }
}