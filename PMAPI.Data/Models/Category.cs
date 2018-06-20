using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMAPI.Data.Models
{
    [Table("Category")]
    public class Category : BaseEntity
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(25, ErrorMessage = "Name must be 25 characters or less")]
        public string Name { get; set; }

        public ICollection<Category> Categories { get; set; }

        public Category()
        {
            Categories = new Collection<Category>();
        }
    }
}
