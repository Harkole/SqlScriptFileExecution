using ExecuteSQLScript.Options;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ExecuteSQLScript.Services
{
    class ExecuteCommandService
    {
        private readonly SqlOptions sqlOptions;
        private DbProviderFactory dbProvider = SqlClientFactory.Instance;

        public ExecuteCommandService(SqlOptions sqlOptions)
        {
            this.sqlOptions = sqlOptions ?? throw new ArgumentNullException(nameof(sqlOptions));
        }

        /// <summary>
        /// Takes a SQL command and attempts to execute it against the supplied
        /// connection string, any errors are sent to the console, future commands
        /// will only stop being executed if the StopOnError flag is set
        /// </summary>
        /// <param name="command">The SQL command to execute</param>
        /// <exception cref="Exception">Thrown to ensure the applciation stops if a command errors</exception>
        public async Task ExecuteCommand(string command)
        {
            if (string.IsNullOrEmpty(command))
            {
                throw new ArgumentNullException(nameof(command));
            }

            try
            {
                using (DbConnection conn = dbProvider.CreateConnection())
                {
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        // Base line the command
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = command;

                        // Open and Execute
                        conn.ConnectionString = sqlOptions.ConnectionString;
                        await conn.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (DbException dbEx)
            {
                // Output the error in red, resetting the console colour once finished
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(dbEx.Message);
                Console.ResetColor();

                // Force the application to throw the error if we're flagged to stop on error
                if (sqlOptions.StopOnError)
                {
                    throw new Exception($"Stopping execution due to errors:\n{dbEx.Message}");
                }
            }
        }
    }
}
