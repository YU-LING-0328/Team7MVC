using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using Team7MVC.Models;
using Team7MVC.ViewModels;

namespace Team7MVC.Repositories
{
    public class AdminRepository
    {
        private static string connString;
        private SqlConnection conn;

        public AdminRepository()
        {
            if (string.IsNullOrEmpty(connString))
            {
                connString = ConfigurationManager.ConnectionStrings["WineDB"].ConnectionString;
            }

            conn = new SqlConnection(connString);
        }

        #region Product

        public List<Products> GetAllProducts()
        {
            List<Products> products;
            using (conn)
            {
                string sql = "select * from Products";
                products = conn.Query<Products>(sql).ToList();
            }

            return products;
        }

        public List<Products> GetAllProducts(string SortBy)
        {
            List<Products> products;
            using (conn)
            {
                string sql = $"select * from Products order by {SortBy}";
                products = conn.Query<Products>(sql).ToList();
            }

            return products;
        }

        public Products GetProduct(int id)
        {
            Products product;

            using (conn)
            {
                string sql = "select * from Products where ProductID = @ProductId";
                product = conn.QueryFirstOrDefault<Products>(sql, new { ProductId = id });
            }

            return product;
        }

        public void CreateProduct(Products product)
        {
            using (conn)
            {
                string sql = @"insert into Products (ProductName,Origin,Year,Capacity,UnitPrice,Stock,Grade,Variety,Area,Picture,Introduction,CategoryID)
                                values(@ProductName,@Origin,@Year,@Capacity,@UnitPrice,@Stock,@Grade,@Variety,@Area,@Picture,@Introduction,@CategoryID);";
                conn.Execute(sql, new { product.ProductName, product.Origin, product.Year, product.Capacity, product.UnitPrice, product.Stock, product.Grade, product.Variety, product.Area, product.Picture, product.Introduction, product.CategoryID });
            }
        }

        public void UpdateProduct(Products product)
        {
            using (conn)
            {
                string sql = @"update Products
                                set ProductName = @ProductName,
                                Origin = @Origin,
                                Year = @Year,
                                Capacity = @Capacity,
                                UnitPrice = @UnitPrice,
                                Stock = @Stock,
                                Grade = @Grade,
                                Variety = @Variety,
                                Area = @Area,
                                Picture = @Picture,
                                Introduction = @Introduction,
                                CategoryID = @CategoryID
                                where ProductID = @ProductID";
                conn.Execute(sql, new { product.ProductName, product.Origin, product.Year, product.Capacity, product.UnitPrice, product.Stock, product.Grade, product.Variety, product.Area, product.Picture, product.Introduction, product.CategoryID, product.ProductID });
            }
        }

        public void DeleteProduct(int Id)
        {
            using (conn)
            {
                string sql = @"delete from Products
                                where ProductID = @ProductId";
                conn.Execute(sql, new { ProductId = Id });
            }
        }
        #endregion

        #region Order
        public List<Orders> GetAllOrders()
        {
            List<Orders> orders;
            using (conn)
            {
                string sql = "select * from Orders";
                orders = conn.Query<Orders>(sql).ToList();
            }

            return orders;
        }

        #endregion

        #region Customer

        public List<AdminCustomersViewModel> GetAllCustomers()
        {
            List<AdminCustomersViewModel> customers;
            using (conn)
            {
                string sql = @"select c.CustomerID, c.Account, c.CustomerName, c.Gender, c.Email,
                                c.Address, c.Phone,
                                SUM(ISNULL(od.UnitPrice * od.Quantity,0)) as TotalCost
                                from Customers as c
                                LEFT OUTER JOIN Orders as o on o.CustomerID = c.CustomerID
                                LEFT OUTER JOIN [Order Details] as od on od.OrderID = o.OrderID
                                group by c.CustomerID, c.Account, c.CustomerName, c.Gender, c.Email,
                                c.Address, c.Phone";
                customers = conn.Query<AdminCustomersViewModel>(sql).ToList();
            }

            return customers;
        }

        #endregion

        #region Message
        public List<Messages> GetAllMessages()
        {
            List<Messages> messages;
            using (conn)
            {
                string sql = "select * from Messages";
                messages = conn.Query<Messages>(sql).ToList();
            }

            return messages;
        }

        #endregion

        #region DashBoard資料

        public MonthSaleViewModel GetMonthSale()
        {
            MonthSaleViewModel monthSaleViewModels = new MonthSaleViewModel();

            using (conn = new SqlConnection(connString))
            {
                decimal[] amount = new decimal[12];
                string sql = @"select ISNULL(SUM(od.Quantity * od.UnitPrice * od.Discount),0) as Amount from Orders as o
                            INNER JOIN [Order Details] as od on od.OrderID = o.OrderID
                            where MONTH(OrderDate) = @Month";

                for (int i = 1; i <= 12; i++)
                {
                    amount[i - 1] = conn.QueryFirstOrDefault<decimal>(sql, new { Month = i });
                }
                monthSaleViewModels.Amount = amount;
                monthSaleViewModels.Month = new string[] { "一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月" };
            }

            return monthSaleViewModels;
        }

        public decimal GetYearSale()
        {
            decimal result = 0;
            using (conn = new SqlConnection(connString))
            {
                string sql = @"select ISNULL(SUM(od.Quantity * od.UnitPrice * od.Discount),0) as Amount from Orders as o
                                INNER JOIN [Order Details] as od on od.OrderID = o.OrderID
                                where Year(OrderDate) = @year";

                result = conn.QueryFirstOrDefault<decimal>(sql, new { Year = DateTime.Now.Year });
            }

            return result;
        }

        public int GetCustomer_Count()
        {
            int result = 0;
            using (conn = new SqlConnection(connString))
            {
                string sql = "select COUNT(*) from Customers";

                result = conn.QueryFirstOrDefault<int>(sql);
            }

            return result;
        }

        public int GetOrder_Count()
        {
            int result = 0;
            using (conn = new SqlConnection(connString))
            {
                string sql = @"select COUNT(*) from Orders
                                where MONTH(OrderDate) = @Month";

                result = conn.QueryFirstOrDefault<int>(sql, new { Month = DateTime.Now.Month });
            }

            return result; 
        }

        public HotOrigin GetOrigin_Count()
        {
            HotOrigin hotOrigin;
            using (conn = new SqlConnection(connString))
            {
                string sql = @"select top 1 p.Origin, SUM(od.Quantity) as OriginsTotal from [Order Details] as od
                                INNER JOIN Products as p on p.ProductID = od.ProductID
                                group by p.Origin
                                order by OriginsTotal desc";

                hotOrigin = conn.QueryFirstOrDefault<HotOrigin>(sql);
            }

            return hotOrigin;
        }

        #endregion

    }
}