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
        
        [Display(Name ="Store")]
        public string storeName { get; set; }
        [Display(Name ="Store Address")]
        public string store_address { get; set; }
        [Display(Name ="Store Contact")]
        [DataType(DataType.PhoneNumber)]
        public string store_contact { get; set; }
        public bool is_approved = false;
    }
}
