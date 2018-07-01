using Newtonsoft.Json;

namespace PM.Domain.Models
{
    public class BaseDTO
    {
        [JsonProperty(Order = 1)]
        public int ID { get; set; }

        public bool ShouldSerializeID()
        {
            return ID != 0;
        }
    }
}
