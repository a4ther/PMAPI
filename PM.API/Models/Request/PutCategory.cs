namespace PM.API.Models.Request
{
    public class PutCategory
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int? ParentID { get; set; }
    }
}
