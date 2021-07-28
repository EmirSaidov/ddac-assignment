using Newtonsoft.Json;

namespace DDAC_Assignment_Mining_Commerce.Models
{
    public class CartModel
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "partitionKey")]
        public string PartitionKey { get; set; }
        public string BuyerID { get; set; }
        public string ProductID { get; set; }
        public bool IsRegistered { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

   

   
}
