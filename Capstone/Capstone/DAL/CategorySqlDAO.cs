using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
   public class CategorySqlDAO: ICategoryDAO
    {
        private readonly string connectionString;

        public CategorySqlDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        //public IList<Category> GetVenueCategories(Venue venue)
        //{

        //}
    }
}
