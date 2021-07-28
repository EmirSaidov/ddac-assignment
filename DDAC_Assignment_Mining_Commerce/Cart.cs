using Newtonsoft.Json;

namespace CosmosGettingStartedTutorial
{
    public class Cart
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
