using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Models
{
    public class UserModel
    {
        public int ID { get; set; }

        [Display(Name ="Full Name")]
        public string fullname { get; set; }

        [Display(Name ="Email")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [Display(Name = "Phone no.")]
        [DataType(DataType.PhoneNumber)]
        public string phone { get; set; }

        [Display(Name ="Gender")]
        public char gender {get;set;}

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        [Display(Name ="Password")]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}
