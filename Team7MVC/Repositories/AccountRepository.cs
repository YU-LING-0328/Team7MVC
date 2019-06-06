using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Team7MVC.Models;
using Team7MVC.ViewModels;

namespace Team7MVC.Repositories
{
    public class AccountRepository
    {
        private static string connString;
        private readonly SqlConnection conn;

        public AccountRepository()
        {
            if (string.IsNullOrEmpty(connString))
            {
                connString = ConfigurationManager.ConnectionStrings["WineDB"].ConnectionString;
            }

            conn = new SqlConnection(connString);
        }

        public void Register(RegisterViewModel customer)
        {
            using (conn)
            {
                string sql = @"insert into Customers (Account,Password,CustomerName,Gender,Birthday,Email,Address,Phone,VIP)
                                values(@Account,@Password,@CustomerName,@Gender,@Birthday,@Email,@Address,@Phone,0);";
                conn.Execute(sql, new { customer.Account, customer.Password, customer.CustomerName, customer.Gender, customer.Birthday, customer.Email, customer.Address, customer.Phone });
            }
        }

        public Customers Login(string account, string password)
        {
            Customers customer;
            using (conn)
            {
                string sql = "select Account, Password from Customers where Account = @account and Password = @password";
                customer = conn.QueryFirstOrDefault<Customers>(sql, new { account, password });
            }
            return customer;
        }
    }
}