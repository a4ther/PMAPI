using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace PM.API.Models.Logging
{
    public class RequestLog
    {
        [JsonProperty(Order = 3)]
        public dynamic Body { get; set; }

        [JsonProperty(Order = 1)]
        public string Route { get; set; }

        [JsonProperty(Order = 2)]
        public string QueryString { get; set; }
    }
}
