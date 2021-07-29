using DDAC_Assignment_Mining_Commerce.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
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

        [Display(Name ="Product Name")]
        [Required(ErrorMessage ="Product Name is Required")]
        public string productName { get; set; }

        [Display(Name ="Product Price")]
        [Required(ErrorMessage = "Product Price is Required")]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter valid number")]
        public double productPrice{ get; set; }

        [Display(Name ="Product Mass")]
        [Required(ErrorMessage = "Product Mass is Required")]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a valid number")]
        public double productMass{ get; set; }

        [Display(Name = "Product Description")]
        [DataType(DataType.MultilineText)]
        public string productDescription { get; set; } = "";

        [NotMapped]
        public string default_image_URL = "/assets/default_img.jpg";

        public string getProductPicName()
        {
            return "product_picture_" + this.ID + "_" + ".jpg";
        }

        public void UploadProfilePicture(IFormFile image, BlobService _blob)
        {
            if (image != null)
            {
                try
                {
                    _blob.uploadImgToBlobContainer("product", this.getProductPicName(), image);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Product Picture Upload Failed+ex");
                    Debug.WriteLine(ex);
                }
            }
        }

        public string getProductPicture(BlobService _blob)
        {
            return _blob.getBlobURLFromStorage("product", this.getProductPicName(), this.default_image_URL);
        }
    }
}
