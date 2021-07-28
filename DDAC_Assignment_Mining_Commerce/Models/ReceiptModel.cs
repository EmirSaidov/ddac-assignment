using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Models
{
    public class ReceiptModel
    {
        public int ID { get; set; }

        [Display(Name = "Company Name")]
        [Required]
        public string company_name { get; set; }

        [Display(Name = "Product Name")]
        [Required]
        public string receipt_product_name{ get; set; }

        [Display(Name = "Product Price")]
        [Required]
        public string receipt_product_price { get; set; }

        [Display(Name = "Total Price")]
        [Required]
        public string total_price { get; set; }

        [Display(Name = "Date")]
        [Required]
        public string date { get; set; }
    }
}
