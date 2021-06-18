using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
   public class ReservationSqlDAO: IReservationDAO
    {
        private readonly string connectionString;

        public ReservationSqlDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }
    }
}
