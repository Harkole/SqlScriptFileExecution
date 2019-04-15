# SqlScriptFileExecution
Uses C# to execute a SQL file (e.g. CreateTables.sql) that can contain multiple "GO" statements

## Use
- Update the appsettings.json file with the location of the SQL script to read in and execute
- Set the options as required (Continue on Error, split the file up etc [Note that spliting the file is recommended])
- Execute through `dotnet ExecuteSqlScript.dll`

## Contributions
Feel free to extend this out to handle different scenarios
