using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShukakuApi.Models
{
    public class ProfileViewModel
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Kelurahan { get; set; }
        public string Kecamatan { get; set; }
        public string Province { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string PasswordNew { get; set; }
        public string PasswordConfirmation { get; set; }
        public string HashedPassword { get; set; }
    }
}