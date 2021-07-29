using DDAC_Assignment_Mining_Commerce.Helper;
using DDAC_Assignment_Mining_Commerce.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos.Table;
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
    public class UserModel
    {
        public UserModel() { }

        public int ID { get; set; }

        [Display(Name ="Full Name")]
        [StringLength(70, MinimumLength = 6, ErrorMessage = "The {0} must be between {2}-{1} chars")]
        [Required]
        public string fullname { get; set; }

        [Display(Name ="Email")]
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [Display(Name = "Phone no.")]
        [Required(ErrorMessage = "Phone Number is Required")]
        [DataType(DataType.PhoneNumber)]
        public string phone { get; set; }

        [Display(Name ="Gender")]
        [StringLength(1,MinimumLength = 1)]
        [Required]
        public string gender {get;set;}

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage ="Date of Birth is Required")]
        public DateTime DOB { get; set; }

        [Display(Name ="Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Required]
        public string password { get; set; }

        [NotMapped]
        public string image_url = "/assets/default_profile.jpg";
        [NotMapped]
        public string roleTable = "userRole";


        public string getProfilePicName() {
            return "user_profile_" + this.ID+"_"+".jpg";
        }

        public void UploadProfilePicture(IFormFile image, BlobService _blob)
        {
            if (image != null)
            {
                try
                {
                    _blob.uploadImgToBlobContainer("profilepicture", this.getProfilePicName(), image);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Profile Picture Upload Failed+ex");
                    Debug.WriteLine(ex);
                }
            }
        }

        public string getProfilePicture(BlobService _blob) {
            return _blob.getBlobURLFromStorage("profilepicture", this.getProfilePicName(), this.image_url);
        }


        public async Task setUserRole(CosmosTableService _table, UserType userType) {
            CloudTable table = _table.getTable(this.roleTable);
            string type = userType == UserType.BUYER ? "B" : userType == UserType.SELLER ? "S" : "A";
            try
            {
                TableOperation insertUserRole = TableOperation.InsertOrReplace(new UserRole(this.ID.ToString(), type));
                TableResult result = await table.ExecuteAsync(insertUserRole);
                UserRole role = result.Result as UserRole;
            }
            catch (Exception ex) {
                Console.WriteLine("Error in inserting user");
                Console.WriteLine(ex.Message);
            }
            
        }

        public async Task<string> getUserRole(CosmosTableService _table) {
            try
            {
                CloudTable table = _table.getTable(this.roleTable);
                TableOperation retrieveUserRole = TableOperation.Retrieve<UserRole>("partitionKey", this.ID.ToString());
                TableResult result = await table.ExecuteAsync(retrieveUserRole);
                UserRole role = result.Result as UserRole;
                if (role != null)
                {
                    return role.userType;
                }
                else {
                    return "";
                }
                
            }
            catch (StorageException ex) {
                Console.WriteLine(ex.Message);
                return "";
            }
                        
        }
    }

    public enum UserType{ 
        [Display(Name ="Admin")]
        ADMIN,
        [Display(Name = "Seller")]
        SELLER,
        [Display(Name = "Buyer")]
        BUYER
    }

    public class UserRole:TableEntity {
        public UserRole() { }
        public UserRole(string id, string userType) {
            this.user_id = id;
            this.userType = userType;
            this.PartitionKey = "partitionKey";
            this.RowKey = id.ToString();
        }
        public string user_id { get; set; }
        public string userType { get; set; } = null;
    }
}
