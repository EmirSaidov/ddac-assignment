using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Models
{
    public class PayModel
    {
        public int ID { get; set; }

        [Display(Name = "Full Name")]
        [StringLength(70, MinimumLength = 6, ErrorMessage = "The {0} must be between {2}-{1} chars")]
        [Required]
        public string fullname { get; set; }

        [Display(Name = "Phone number")]
        [Required(ErrorMessage = "Phone Number is Required")]
        [DataType(DataType.PhoneNumber)]
        public string phone { get; set; }

        [Display(Name = "Card Number")]
        [Required(ErrorMessage = "Card Number is Required")]
        [DataType(DataType.CreditCard)]
        public string card { get; set; }

        [Display(Name = "CVC Number")]
        [Required(ErrorMessage = "CVC Number is Required")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "The {0} must be 3 numbers")]
        public string cvc { get; set; }

        [Display(Name = "Expiration Date")]
        [Required(ErrorMessage = "Expiration Date is Required")]
        public string ex_date { get; set; }
    }
}
