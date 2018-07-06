using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PM.Data.Models
{
    [Table("Category")]
    public class Category : BaseEntity
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(25, ErrorMessage = "Name must be 25 characters or less")]
        public string Name { get; set; }

        [ForeignKey("Parent")]
        public int? ParentID { get; set; }

        public virtual Category Parent { get; set; }

        public virtual ICollection<Category> Subcategories { get; set; }
    }
}
