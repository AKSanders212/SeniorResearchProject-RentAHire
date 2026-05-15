/*
 * GUID: G00225605
 * Aaron Keith Sanders
 * UA Grantham
 * IS498-Senior Research Project
 * Robert Chubbuck
 * Capstone Project
 * File: MyDatabase.cs
 * Date: 17 March 2026
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks; // For asynchronous table creation and access in realtime
using System.Diagnostics; // For printing log messages in debug mode
using SQLite; // for my database

namespace SandersCapstoneProject
{
    public class MyDatabase
    {
        private readonly SQLiteAsyncConnection _mydatabase; 

        public MyDatabase()
        {
             string mydbpath = Path.Combine(FileSystem.AppDataDirectory, "mydatabase.db");
            _mydatabase = new SQLiteAsyncConnection(mydbpath); // links the local database to the app
            Debug.WriteLine($"Database Path: {mydbpath}"); // display the path to the db in Debug (for development purposes)

            // In Debug mode, provides a helper method to check if the tables have been created
#if DEBUG
            InitTables();
#else
    _mydatabase.CreateTableAsync<EmployeeAccounts>().Wait();
    _mydatabase.CreateTableAsync<InstitutionAccounts>().Wait();
#endif
        }

        // Initializes the data tables for my database for the user accounts for debug mode
        // For release mode the tables are created without checking for success
        private async void InitTables()
        {
            // Creating my data tables for the EmployeeAccounts and the InstitutionAccounts
            await _mydatabase.CreateTableAsync<EmployeeAccounts>();
            await _mydatabase.CreateTableAsync<InstitutionAccounts>();

            if (await TableExistsAsync("EmployeeAccounts"))
            {
                Debug.WriteLine("EmployeeAccounts table created successfully.");
            }

            if (await TableExistsAsync("InstitutionAccounts"))
            {
                Debug.WriteLine("InstitutionAccounts table created successfully.");
            }
        }

        // Helper method that provides a bool type to check if the tables have been created
        // This is for Debug mode
        public async Task<bool> TableExistsAsync(string tableName)
        {
            // This SQL statement counts all of the existing tables created, and if they exist, returns the result count
            var result = await _mydatabase.ExecuteScalarAsync<int>(
                $"SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='{tableName}'");

            return result > 0;
        }

        // Adds and returns the newly created Employee account
        public Task<int> AddEmployeeAsync(EmployeeAccounts employee)
        {
            return _mydatabase.InsertAsync(employee);
        }

        // Adds and returns the newly created Institution account
        public Task<int> AddInstitutionAsync(InstitutionAccounts institution)
        {
            return _mydatabase.InsertAsync(institution);
        }

        public Task<List<EmployeeAccounts>> GetEmployeesAsync()
        {
            // Returns the data from the EmployeeAccounts table in the db as an async list
            return _mydatabase.Table<EmployeeAccounts>().ToListAsync();
        }

        public Task<List<InstitutionAccounts>> GetInstitutionsAsync()
        {
            // Returns the data from the InstitutionAccounts table in the db as an async list
            return _mydatabase.Table<InstitutionAccounts>().ToListAsync();
        }
    }
}
