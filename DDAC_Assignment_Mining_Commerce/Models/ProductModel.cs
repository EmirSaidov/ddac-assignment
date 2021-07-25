﻿using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DDAC_Assignment_Mining_Commerce.Models
{
    public class ProductModel
    {
        public int ID { get; set; }

        public int sellerID { get; set; }
        public virtual SellerModel seller{ get; set; }

        [Display(Name = "Product Image")]
        public string imageUri { get; set; }

        [Display(Name ="Product Name")]
        [Required(ErrorMessage ="Product Name is Required")]
        public string productName { get; set; }

        [Display(Name ="Product Price")]
        [Required(ErrorMessage = "Product Price is Required")]
        public double productPrice{ get; set; }

        [Display(Name ="Product Quantity")]
        [Required(ErrorMessage = "Product Quantity is Required")]
        public int productQuantity{ get; set; }
    }
}