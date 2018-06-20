using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PMAPI.Domain.Models
{
    public class CategoryResponse : BaseResponse, IEquatable<CategoryResponse>
    {
        public string Name { get; set; }

        [JsonIgnore]
        public IEnumerable<CategoryResponse> Categories { get; set; }

        public bool ShouldSerializeName()
        {
            return !string.IsNullOrEmpty(Name);
        }

        public bool Equals(CategoryResponse other)
        {
            if (other == null)
            {
                return false;
            }

            if (this.ID != 0 && other.ID != 0)
            {
                return this.ID.Equals(other.ID);
            }

            return this.Name.Equals(other.Name);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var categoryObj = obj as CategoryResponse;
            if (categoryObj == null)
            {
                return false;
            }
            return Equals(categoryObj);
        }

        public override int GetHashCode()
        {
            if (this.ID != 0)
            {
                return this.ID.GetHashCode();
            }

            return this.Name.GetHashCode();
        }

        public static bool operator == (CategoryResponse category1, CategoryResponse category2)
        {
            if (Object.ReferenceEquals(category1, null))
            {
                return Object.ReferenceEquals(category2, null);
            }

            return category1.Equals(category2);
        }

        public static bool operator != (CategoryResponse category1, CategoryResponse category2)
        {
            return !(category1 == category2);
        }
    }
}
