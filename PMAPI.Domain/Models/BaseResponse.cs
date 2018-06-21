using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PM.Domain.Models
{
    public class BaseResponse
    {
        [Required]
        public int ID { get; set; }

        [JsonIgnore]
        public DateTime DateAdded { get; set; }

        [JsonIgnore]
        public DateTime DateModified { get; set; }

        public bool ShouldSerializeID()
        {
            return ID != 0;
        }
    }
}
