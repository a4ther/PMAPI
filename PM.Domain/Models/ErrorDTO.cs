using Newtonsoft.Json;

namespace PM.Domain.Models
{
    public class ErrorDTO
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
