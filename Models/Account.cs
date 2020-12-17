using System;
using System.ComponentModel.DataAnnotations;

namespace SpectrumTest.Models
{

    public class Account
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime ServiceDate { get; set; }
    }
}
