using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Team7MVC.Models;

namespace Team7MVC.Repositories
{
    public class MessageRepository
    {
        private static string connString;
        private readonly SqlConnection conn;

        int affectedRows = -1;

        public MessageRepository()
        {
            if (string.IsNullOrEmpty(connString))
            {
                connString = ConfigurationManager.ConnectionStrings["WineDB"].ConnectionString;
            }

            conn = new SqlConnection(connString);
        }

        public int CreateMessages(Messages mess)
        {
            //List<Questions> questions;
            using (conn)
            {
                string sql = @"INSERT INTO Messages(Name, Email, Phone, QuestionCategory, Comments, Datetime) VALUES (@name, @email, @phone, @questionCategory, @comments, @dateTime)";
                affectedRows = conn.Execute(sql, new { mess.Name, mess.Email, mess.Phone, mess.QuestionCategory, mess.Comments, mess.Datetime });
            }

            return affectedRows;
        }
    }
}