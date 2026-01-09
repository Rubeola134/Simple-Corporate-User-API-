using System;
using System.Data;
using ConsoleApp1.Data;
using ConsoleApp1.Models;
using Dapper;
namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DataContextDapper dapper = new DataContextDapper();
            DataContextEF entityFramework = new DataContextEF();

            string sqlCommand = "SELECT GETDATE()";

            DateTime rightNow = dapper.LoadDataSignle<DateTime>(sqlCommand);
            Console.WriteLine(rightNow);

            Computer myComputer = new()
            {
                Motherboard = "ASUS ROG STRIX B550-F",
                HasWifi = true,
                HasLTE = false,
                ReleaseDate = new DateTime(2021, 5, 15),
                Price = 1299.99m,
                VideoCard = "NVIDIA GeForce RTX 3070"
            };

            entityFramework.Add(myComputer);
            entityFramework.SaveChanges();

            string sql = @"INSERT INTO TutorialAppSchema.Computer
(Motherboard, CPUCores, HasWifi, HasLTE, ReleaseDate, Price, VideoCard)
VALUES (@Motherboard, @CPUCores, @HasWifi, @HasLTE, @ReleaseDate, @Price, @VideoCard)";


            Console.WriteLine(sql);
            int result = dapper.ExecuteSql(sql, myComputer);

            Console.WriteLine($"{result} row(s) inserted.");

            string sqlSelect = @"SELECT * FROM TutorialAppSchema.Computer";

            IEnumerable<Computer> computers = dapper.LoadData<Computer>(sqlSelect);

            foreach (Computer singleComputer in computers)
            {
                Console.WriteLine($"Motherboard: {singleComputer.Motherboard}, Price: {singleComputer.Price}");
            }

             IEnumerable<Computer>? computersEF = entityFramework.Computers?.ToList<Computer>();

             if(computersEF != null)
             {
            foreach (Computer singleComputer in computersEF)
            {
                Console.WriteLine($"Motherboard: {singleComputer.Motherboard}, Prices: {singleComputer.Price}");
            }
             }

            // Console.WriteLine(myComputer.Motherboard);
            // Console.WriteLine(myComputer.HasWifi);
        }
    }
}