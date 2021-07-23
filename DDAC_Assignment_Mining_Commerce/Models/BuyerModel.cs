using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Models
{
    public class BuyerModel
    {
        public int ID { get; set; }

        public virtual UserModel user { get; set; }

        [Display(Name = "Address")]
        public string address { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string shipping_address { get; set; }
    }
}
