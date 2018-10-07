using System;
using System.IO;
using System.Reflection;
using Dapper;
using DbUp;
using Microsoft.Extensions.Configuration;

namespace ToBeRenamed.Database
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            if (!string.IsNullOrEmpty(args[0]) && args[0].Equals("delete"))
            {
                var connFactory = new PostgresSqlConnectionFactory(configuration);

                const string dropDatabases = @"
                    DROP DATABASE IF EXISTS ""Plum"";
                    DROP DATABASE IF EXISTS ""TestPlum"";";

                using (var cnn = connFactory.GetSqlConnection())
                {
                    // Insert new user, then get the user id
                    cnn.Execute(dropDatabases);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Databases successfully dropped!");
                    Console.ResetColor();
                }
            }


            var connectionString = configuration["ConnectionStrings:DefaultConnection"];
            var testConnectionString = configuration["ConnectionStrings:TestConnection"];

            if (string.IsNullOrEmpty(connectionString))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Invalid default connection string provided");
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                return -1;
            }

            if (string.IsNullOrEmpty(testConnectionString))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Invalid test connection string provided");
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                return -1;
            }

            EnsureDatabase.For.PostgresqlDatabase(connectionString);
            EnsureDatabase.For.PostgresqlDatabase(testConnectionString);
            
            var upgrader = DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

            var result = upgrader.PerformUpgrade();
            
            
            var testUpgrader = DeployChanges.To
                .PostgresqlDatabase(testConnectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

            var testResult = testUpgrader.PerformUpgrade();
            
            if (!result.Successful/*!testResult.Successful*/)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                //Console.WriteLine(testResult.Error);
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                return -1;
            }

            if (!testResult.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(testResult.Error);
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();

#if DEBUG
            Console.ReadLine();
#endif

            return 0;
        }
    }
}
