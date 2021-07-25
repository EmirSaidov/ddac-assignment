using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Models
{
    public class SellerModel
    {
        public int ID { get; set; }

        public virtual UserModel user { get; set; }

        [Display(Name = "Store Name")]
        [Required(ErrorMessage = "Store Name is Required")]
        public string storeName { get; set; }
        [Display(Name = "Store Address")]
        [Required(ErrorMessage = "Store Address is Required")]
        public string store_address { get; set; }
        [Display(Name = "Store Contact")]
        [Required(ErrorMessage = "Store Contact is Required")]
        [DataType(DataType.PhoneNumber)]
        public string store_contact { get; set; }
        [Display(Name = "Approved Status")]
        public bool is_approved { get; set; } = false; 
    }
}
