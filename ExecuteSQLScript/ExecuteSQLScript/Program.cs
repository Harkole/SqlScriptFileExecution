using ExecuteSQLScript.Options;
using ExecuteSQLScript.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace ExecuteSQLScript
{
    class Program
    {
        /// <summary>
        /// Entry point to program
        /// </summary>
        static void Main()
        {

            try
            {
                // Step in to the Asyncronis program
                Task main = MainAsync();
                main.Wait();
            }
            catch (AggregateException aggEx)
            {
                foreach(Exception ex in aggEx.InnerExceptions)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ResetColor();
                }
            }
        }

        /// <summary>
        /// Asyncronis entry point to program
        /// </summary>
        /// <returns></returns>
        static async Task MainAsync()
        {
            SqlOptions sqlOptions = new SqlOptions();
            FileOptions fileOptions = new FileOptions();

            // Load the SQL Options from file
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            // Set the connection string
            IConfigurationSection sqlSection = configuration.GetSection(nameof(SqlOptions));
            sqlOptions.ConnectionString = sqlSection[nameof(SqlOptions.ConnectionString)];

            // Set the Continue on Error flag
            bool.TryParse(sqlSection[nameof(SqlOptions.StopOnError)], out bool stopOnError);
            sqlOptions.StopOnError = stopOnError;


            // Set the command file and options
            IConfigurationSection fileSection = configuration.GetSection(nameof(FileOptions));
            fileOptions.File = fileSection[nameof(FileOptions.File)];

            bool.TryParse(fileSection[nameof(FileOptions.SplitOnGo)], out bool splitOnGo);
            fileOptions.SplitOnGo = splitOnGo;

            // Load the script and execution services
            LoadScriptService scriptService = new LoadScriptService(fileOptions);
            ExecuteCommandService commandService = new ExecuteCommandService(sqlOptions);

            // Execute the commands
            foreach(string command in scriptService.GetSqlCommands())
            {
                if (!string.IsNullOrEmpty(command))
                {
                    Console.WriteLine();
                    Console.WriteLine(command);

                    await commandService.ExecuteCommand(command);
                }
            }
        }
    }
}
