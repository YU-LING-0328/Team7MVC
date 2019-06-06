using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Team7MVC.ViewModels;
using Dapper;
using Team7MVC.Models;

namespace Team7MVC.Repositories
{
    public class WineRepository
    {
        private static string connString;
        private readonly SqlConnection conn;

        int affectedRows = -1;

        public WineRepository()
        {
            if (string.IsNullOrEmpty(connString))
            {
                connString = ConfigurationManager.ConnectionStrings["WineDB"].ConnectionString;
            }

            conn = new SqlConnection(connString);
        }

        public List<Products> GetProducts()
        {
            List<Products> products;

            using (conn)
            {
                string sql = "select * from Products";

                products = conn.Query<Products>(sql).ToList();
            }

            return products;
        }

        public List<Products> Getproducts(int? Id)
        {
            List<Products> products;

            using (conn)
            {
                string sql = string.Empty;
                if (Id == 1)
                {
                    sql = @"select * from Products
                            where Year = (select top 1 Year from Products order by Year desc)";
                }
                else if (Id == 2)
                {
                    sql = @"with t1 (ProductId, Total)
                            as
                            (
	                            select top 6 ProductID, SUM(Quantity) as Total 
	                            from [Order Details]
	                            group by ProductID
	                            order by Total desc
                            )
                            select p.ProductID, p.ProductName, p.Origin, p.Year, p.Capacity,
		                    p.UnitPrice, p.Stock, p.Grade, p.Variety, p.Area, p.Picture, p.Introduction, p.CategoryID
                            from t1 as t
                            INNER JOIN Products as p on p.ProductID = t.ProductId";
                }
                else
                {
                    sql = @"select * from Products
                            where UnitPrice >= 10000
                            order by UnitPrice desc";
                }

                products = conn.Query<Products>(sql).ToList();

            }

            return products;
        }

        public List<Products> GetProducts(string SearchStr)
        {
            List<Products> products;

            using (conn)
            {
                string sql = @"select * from Products
                                where ProductName LIKE @ProductName";

                products = conn.Query<Products>(sql, new { ProductName = "%" + SearchStr + "%" }).ToList();
            }

            return products;
        }

        public List<Products> GetProducts(int? Year_s, int? Year_e)
        {
            List<Products> products;

            using (conn)
            {
                string sql = @"select * from Products
                                where Year between @Year_s and @Year_e";

                products = conn.Query<Products>(sql, new { Year_s, Year_e }).ToList();
            }

            return products;
        }

        public List<Products> GetProducts(int? Year_s)
        {
            List<Products> products;

            using (conn)
            {
                string sql = @"select * from Products
                                where Year >= @Year_s";

                products = conn.Query<Products>(sql, new { Year_s }).ToList();
            }

            return products;
        }

        public List<Products> GetProducts(decimal? Price_s)
        {
            List<Products> products;

            using (conn)
            {
                string sql = @"select * from Products
                                where UnitPrice >= @Price_s";

                products = conn.Query<Products>(sql, new { Price_s }).ToList();
            }

            return products;
        }

        public List<Products> GetProducts(decimal? Price_s, decimal? Price_e)
        {
            List<Products> products;

            using (conn)
            {
                string sql = @"select * from Products
                                where UnitPrice between @Price_s and @Price_e";

                products = conn.Query<Products>(sql, new { Price_s, Price_e }).ToList();
            }

            return products;
        }

        public List<Products> GetProducts(string[] Array, int temp)
        {
            List<Products> products = new List<Products>();
            List<Products> tempList;

            using (conn)
            {
                if (temp == 1)
                {
                    foreach (var item in Array)
                    {
                        string sql = @"select * from Products
                                where Origin = @Origin";

                        tempList = conn.Query<Products>(sql, new { Origin = item }).ToList();

                        foreach (var item_t in tempList)
                        {
                            products.Add(item_t);
                        }
                    }
                }
                else
                {
                    foreach (var item in Array)
                    {
                        string sql = @"select * from Products as p
                                        INNER JOIN Categories as c on c.CategoryID = p.CategoryID
                                        where CategoryName = @Category";

                        tempList = conn.Query<Products>(sql, new { Category = item }).ToList();

                        foreach (var item_t in tempList)
                        {
                            products.Add(item_t);
                        }
                    }
                }

            }

            return products;
        }

        public List<ShopListsViewModel> ShopList(string CustomerID)
        {
            List<ShopListsViewModel> shopLists;

            using (conn)
            {
                string sql = @"select p.Picture, p.ProductName, p.Year, p.Origin, sh.Price,
                                sh.Quantity, (sh.Price * sh.Quantity) as TotalCost
                                from ShopList as sh
                                INNER JOIN Products as p on p.ProductID = sh.ProductID
                                INNER JOIN Customers as c on c.CustomerID = sh.CustomerID
                                where c.Account = @CustomerID";
                shopLists = conn.Query<ShopListsViewModel>(sql, new { CustomerID }).ToList();
            }

            return shopLists;
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

        public void Payment(Orders orders, string Account)
        {
            List<ShopLists> shopLists;
            int OrderID = 0;
            using (conn)
            {
                string sql = @"insert into Orders (CustomerID, OrderDate, RequiredDate, ShipName,
					            ShipperID, ShipAddress, Freight, PayWay, PayDate)
                                values((select CustomerID from Customers where Account = @Account),
                                @OrderDate, @RequiredDate, @ShipName,@ShipperID, @ShipAddress,
                                @Freight, @PayWay, @PayDate)";
                conn.Execute(sql, new { Account, orders.OrderDate, orders.RequiredDate, orders.ShipName, orders.ShipperID, orders.ShipAddress, orders.Freight, orders.PayWay, orders.PayDate });

                sql = @"select OrderID
                        from Orders as o
                        INNER JOIN Customers as c on c.CustomerID = o.CustomerID
                        where Account = 'Account01'
                        order by OrderID Desc";

                OrderID = conn.QueryFirstOrDefault<int>(sql);

                sql = @"select sh.CustomerID, ProductID, Price, Quantity 
                        from ShopList as sh
                        INNER JOIN Customers as c on c.CustomerID = sh.CustomerID
                        where Account = @Account";
                shopLists = conn.Query<ShopLists>(sql, new { Account }).ToList();

                sql = @"insert into [Order Details] (OrderID, ProductID, UnitPrice, Quantity,
                        Discount)
                        values(@OrderID, @ProductID, @Price, @Quantity, 1)";
                foreach (var item in shopLists)
                {
                    conn.Execute(sql, new { OrderID, item.ProductID, item.Price, item.Quantity });
                }
            }
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