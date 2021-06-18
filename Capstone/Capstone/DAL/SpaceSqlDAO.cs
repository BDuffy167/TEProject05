using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
   public  class SpaceSqlDAO: ISpaceDAO
    {
        private readonly string connectionString;

        public SpaceSqlDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }
    }
}
