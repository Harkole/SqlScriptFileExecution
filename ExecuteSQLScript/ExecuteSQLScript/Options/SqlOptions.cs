namespace ExecuteSQLScript.Options
{
    /// <summary>
    /// Options for using SQL
    /// </summary>
    class SqlOptions
    {
        /// <summary>
        /// The connection string to use when executing SQL commands
        /// </summary>
        public string ConnectionString { get; set; } = string.Empty;

        /// <summary>
        /// If set to true will cause the application to stop executing commands
        /// immediately once an error is thrown
        /// </summary>
        public bool StopOnError { get; set; } = false;
    }
}
