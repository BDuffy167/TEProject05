using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Transactions;

namespace ProjectOrganizerTests.DAL
{

    [TestClass]
    public abstract class CorporateDAOTestsBase
    {
        protected string ConnectionString { get; } = "Data Source=L-P137G001\\SQLEXPRESS;Initial Catalog=EmployeeDB;Integrated Security=True";

        /// <summary>
        /// Holds the newly generated city id.
        /// </summary>
        protected int NewDepartmenttId { get; private set; }

        /// <summary>
        /// The transaction for each test.
        /// </summary>
        private TransactionScope transaction;

        [TestInitialize]
        public void Setup()
        {
            // Begin the transaction
            transaction = new TransactionScope(); // Equivalent to BEGIN TRANSACTION

            // Get the SQL Script to run
            string sql = File.ReadAllText("setup.sql");

            // Execute the script
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (transaction != null)
            {
                transaction.Dispose();
            }
        }
        protected int GetRowCount(string table)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"SELECT COUNT(*) FROM {table}", conn);
                int count = Convert.ToInt32(cmd.ExecuteScalar());

                return count;
            }
        }
    }
}