using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Models
{
    public class DisplayProductModel
    {
        public int ID { get; set; }

        [Display(Name = "Product Name")]
        [Required]
        public string product_name{ get; set; }

        [Display(Name ="Product Description")]
        [Required]
        public string product_description{ get; set; }

        [Display(Name = "Product Image")]
        [Required]
        public string product_image { get; set; }

        [Display(Name = "Product Price")]
        [Required]
        public string product_price { get; set; }

    }
}
