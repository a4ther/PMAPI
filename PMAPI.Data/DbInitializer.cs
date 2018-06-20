using System;
using System.Collections.ObjectModel;
using System.Linq;
using PMAPI.Data.Contexts;
using PMAPI.Data.Models;

namespace PMAPI.Data
{
    public static class DbInitializer
    {
        public static void Initialize(DataContext context)
        {
            context.Database.EnsureCreated();

            if (context.Categories.Any())
            {
                return;
            }

            context.AddRange(GetDefaultCategories());
            context.SaveChanges();
        }

        private static Category[] GetDefaultCategories()
        {
            var dateAdded = DateTime.Now;
            return new Category[]
            {
                NewCategory("Gifts & Donations", new string[] { }),
                NewCategory("Food & Beverage", new string[] { "Café", "Junk", "Healthy", "Restaurants" }),
                NewCategory("Bills & Utilities", new string[] { "Phone", "Internet", "Home" }),
                NewCategory("Transportation", new string[] { "Taxi", "Bus", "Ride", "Parking Fees" }),
                NewCategory("Shopping", new string[] { "Accesories", "Clothing", "Electronics", "Footwear" }),
                NewCategory("Entertainment", new string[] { "Games" }),
                NewCategory("Health & Fitness", new string[] { "Sports", "Personal Care", "Pharmacy", "Doctor" }),
                NewCategory("Travel", new string[] { }),
                NewCategory("Education", new string[] { "Books" }),
                NewCategory("Family", new string[] { "Pets" }),
                NewCategory("Fees & Charges", new string[] { }),
                NewCategory("Others", new string[] { }),
                NewCategory("Gifts", new string[] { }),
                NewCategory("Salary", new string[] { }),
                NewCategory("Selling", new string[] { }),
                NewCategory("Loan", new string[] { })
            }; 
        }

        private static Category NewCategory(string name, string[] categories)
        {
            var categoriesCollection = new Collection<Category>();
            foreach (var subCategory in categories)
            {
                categoriesCollection.Add(NewCategory(subCategory, new string[] { }));
            }

            return new Category 
            {
                Categories = categoriesCollection,
                DateAdded = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
                Name = name
            }; 
        }
    }
}
