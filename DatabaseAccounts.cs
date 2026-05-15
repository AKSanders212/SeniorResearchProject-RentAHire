/*
 * GUID: G00225605
 * Aaron Keith Sanders
 * UA Grantham
 * IS498-Senior Research Project
 * Robert Chubbuck
 * Capstone Project
 * File: EmployeeAccounts.cs
 * Date: 17 March 2026
 */ 
using System;
using System.Collections.Generic;
using System.Text;
// Import the SQLite library (Installation was SQLite-net-pcl NuGet package)
using SQLite;

namespace SandersCapstoneProject
{
    public class EmployeeAccounts
    {
        // This is the primary key for the EmployeeAccounts table
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        // The other columns for the table are the employee's information (the primary key will match
        // the employee account when retrieved). Passwords for this project will be hashed after
        // user input before being stored in the database for best cybersecurity practices.
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip {  get; set; }
        public string UniversityName { get; set; }
        public string DegreeType { get; set; }
        public string DegreeMajor { get; set; }        
        public string DegreeMinor { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // this will be hashed before storage

    }
    public class InstitutionAccounts
    {
        // This is the primary key for the InstitutionAccounts table
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }       
        public string InstName { get; set; } // InstitutionName
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string BusinessType { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // this will be hashed before storage

    }
}
