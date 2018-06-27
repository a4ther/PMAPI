using System;
namespace PM.API.Models.Logging
{
    public class ResponseLog
    {
        public dynamic Body { get; set; }
        public int StatusCode { get; set; }
    }
}
