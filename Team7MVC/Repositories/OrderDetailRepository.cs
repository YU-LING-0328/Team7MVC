using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Team7MVC.ViewModels;

namespace Team7MVC.Repositories
{
    public class OrderDetailRepository
    {
        private static string connString;
        private readonly SqlConnection conn;

        public OrderDetailRepository()
        {
            if (string.IsNullOrEmpty(connString))
            {
                connString = ConfigurationManager.ConnectionStrings["WineDB"].ConnectionString;
            }
            conn = new SqlConnection(connString);
        }
        public int GetCustomerID(string Account)
        {
            int CustomerId;

            using (conn)
            {
                string sql = @"select CustomerID 
                                from Customers
                                where Account = @Account";

                CustomerId = conn.QueryFirstOrDefault<int>(sql, new { Account });
            }

            return CustomerId;
        }

        public OrderDetailViewModel OrderDetailList(string Account)
        {
            OrderDetailViewModel orderDetailList = new OrderDetailViewModel();

            using (conn)
            {

                string sql = @"select  c.CustomerName, c.City,c.Email,c.Address,c.Phone,
                            o.ShipName,o.ShipPhone,o.PayWay,o.ShipCity,o.ShipAddress,o.Freight 
                            from Orders as o
                            INNER JOIN [Order Details] as od on od.OrderID = o.OrderID
                            INNER JOIN Products as p on p.ProductID = od.ProductID
                            INNER JOIN Customers as c on c.CustomerID = o.CustomerID
                            where o.OrderID = 
                            (
	                            select top 1 OrderID from Orders as o
	                            INNER JOIN Customers as c on c.CustomerID = o.CustomerID
	                            where Account = @Account
	                            order by OrderDate desc
                            )";

                orderDetailList.customerDetails = conn.QueryFirstOrDefault<CustomerDetail>(sql, new { Account });
                sql = @"select  od.Quantity,od.UnitPrice,
                        p.ProductName, p.Picture 
                        from Orders as o
                        INNER JOIN [Order Details] as od on od.OrderID = o.OrderID
                        INNER JOIN Products as p on p.ProductID = od.ProductID
                        INNER JOIN Customers as c on c.CustomerID = o.CustomerID
                        where o.OrderID = 
                        (
	                        select top 1 OrderID from Orders as o
	                        INNER JOIN Customers as c on c.CustomerID = o.CustomerID
	                        where Account = @Account
	                        order by OrderDate desc
                        )  ";

                orderDetailList.productDetails = conn.Query<ProductDetail>(sql, new { Account }).ToList();
            }
            return orderDetailList;
        }

    }
}