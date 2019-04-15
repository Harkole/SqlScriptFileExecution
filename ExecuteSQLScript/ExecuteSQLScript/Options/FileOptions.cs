namespace ExecuteSQLScript.Options
{
    /// <summary>
    /// Options for handling the file and where to locate it
    /// </summary>
    class FileOptions
    {
        /// <summary>
        /// The full file path, including the file and extension
        /// </summary>
        public string File { get; set; } = string.Empty;

        /// <summary>
        /// Flag for controlling if the system should split the script file in to
        /// smaller parts using the GO statements
        /// </summary>
        public bool SplitOnGo { get; set; } = true;
    }
}
