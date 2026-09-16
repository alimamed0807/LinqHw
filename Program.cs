using Dapper;
using Microsoft.Data.SqlClient;
using System.Runtime.ConstrainedExecution;

namespace CarGallery
{
    internal class Program
    {
        static string connectionString =
    "Server=localhost;Database=CarGalleryDB;Trusted_Connection=True;TrustServerCertificate=True;";
        static void Main(string[] args)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var car = new Car
                {
                    Brand = "BMW",
                    Model = "M5",
                    Year = 2025,
                    Price = 95000,
                    Color = "Black",
                    IsNew = true
                };

                var insertSql = @"INSERT INTO Cars
                                  (Brand, Model, Year, Price, Color, IsNew)
                                  VALUES
                                  (@Brand, @Model, @Year, @Price, @Color, @IsNew)";

                connection.Execute(insertSql, car);

                var updateSql = @"UPDATE Cars
                                  SET Price = @Price,
                                      Color = @Color
                                  WHERE Id = @Id";

                connection.Execute(updateSql, new
                {
                    Id = 1,
                    Price = 90000,
                    Color = "White"
                });

                var deleteSql = @"DELETE FROM Cars
                  WHERE Id = @Id";

                connection.Execute(deleteSql, new
                {
                    Id = 4
                });
                var selectSql = @"SELECT * FROM Cars";

                var cars = connection.Query<Car>(selectSql);

                foreach (var carItem in cars)
                {
                    Console.WriteLine(carItem);
                }

                var priceSql = @"SELECT * FROM Cars
                                 WHERE Price > @Price";

                var expensiveCars = connection.Query<Car>(
                    priceSql,
                    new { Price = 50000 });

                foreach (var carItem in expensiveCars)
                {
                    Console.WriteLine(carItem);
                }

                var searchSql = @"SELECT * FROM Cars
                                  WHERE Brand = @Brand
                                  AND Model = @Model";

                var searchedCars = connection.Query<Car>(
                    searchSql,
                    new
                    {
                        Brand = "BMW",
                        Model = "M5"
                    });

                foreach (var carItem in searchedCars)
                {
                    Console.WriteLine(carItem);
                }

                var newCarsSql = @"SELECT * FROM Cars
                                   WHERE IsNew = @IsNew";

                var newCars = connection.Query<Car>(
                    newCarsSql,
                    new { IsNew = true });

                foreach (var carItem in newCars)
                {
                    Console.WriteLine(carItem);
                }

                var groupSql = @"SELECT * FROM Cars
                                 ORDER BY Brand";

                var groupedCars = connection.Query<Car>(groupSql);

                var groups = groupedCars.GroupBy(x => x.Brand);

                foreach (var group in groups)
                {
                    Console.WriteLine($"Brand: {group.Key}");

                    foreach (var carItem in group)
                    {
                        Console.WriteLine(carItem);
                    }

                    Console.WriteLine("--------------------------------");
                }

                var saleSql = @"SELECT
                                S.Id,
                                S.CarId,
                                S.CustomerId,
                                S.SaleDate,
                                S.SalePrice,
                                C.Id,
                                C.Brand,
                                C.Model,
                                C.Year,
                                C.Price,
                                C.Color,
                                C.IsNew,
                                CU.Id,
                                CU.FullName,
                                CU.Phone,
                                CU.Email
                                FROM Sales AS S
                                INNER JOIN Cars AS C
                                ON C.Id = S.CarId
                                INNER JOIN Customers AS CU
                                ON CU.Id = S.CustomerId";

                var sales = connection.Query<Sale, Car, Customer, Sale>(
                    saleSql,
                    (sale, carItem, customer) =>
                    {
                        sale.Car = carItem;
                        sale.Customer = customer;
                        return sale;
                    },
                    splitOn: "Id,Id"
                );

                foreach (var sale in sales)
                {
                    Console.WriteLine($"Sale ID: {sale.Id}");
                    Console.WriteLine($"Car: {sale.Car.Brand} {sale.Car.Model}");
                    Console.WriteLine($"Customer: {sale.Customer.FullName}");
                    Console.WriteLine($"Sale Price: {sale.SalePrice}");
                    Console.WriteLine($"Sale Date: {sale.SaleDate}");
                    Console.WriteLine("--------------------------------");
                }
            }
        }
    }
}