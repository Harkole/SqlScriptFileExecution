using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ExecuteSQLScript.Services
{
    class LoadScriptService
    {
        private readonly Options.FileOptions fileOptions;

        public LoadScriptService(Options.FileOptions fileOptions)
        {
            this.fileOptions = fileOptions ?? throw new ArgumentNullException(nameof(fileOptions));
        }

        /// <summary>
        /// Loads the file set in the appsettings.json file and returns the list of
        /// commands to execute, if the split command is set to false, then it will
        /// return a list of a single item
        /// </summary>
        /// <returns>The list of commands to execute</returns>
        public IEnumerable<string> GetSqlCommands()
        {
            List<string> commands = new List<string>();

            try
            {
                // Read the file contents
                string fileContents = File.ReadAllText(fileOptions.File);

                // Check to see if we are to split the commands
                if (fileOptions.SplitOnGo)
                {
                    // Loop over each split, adding the command to the list
                    foreach(string command in Regex.Split(fileContents, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled))
                    {
                        commands.Add(command);
                    }
                }
                else
                {
                    // Just add the entire script as a single command
                    commands.Add(fileContents);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occured reading the file:\n{ex.Message}");
            }

            // Return the commands
            return commands as IEnumerable<string>;
        }
    }
}
