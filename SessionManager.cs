/*
 * Aaron Keith Sanders
 * 
 * G00225605
 * 
 * UA Grantham
 * 
 * IS498: Senior Research Project
 * 
 * Robert Chubbuck
 * 
 * File: SessionManager.cs
 * 
 * Desc: This file contains the session manager, which will persist
 * data, such as being logged in/logged out, account user type
 * and the display name of the current user logged in. This used
 * Microsoft.Maui.Storage.Preferences to store the key value pairs
 * in a store outside of the running application
 * 
 * Date: 25 April 2026
 */
using System;
using System.Collections.Generic;
using System.Text;
// Microsoft.Maui.Storage is included to store key value pairs
// for the entire application, Preferences is used to store
// key value pairs outside of the running application to allow
// save state persistence
using Microsoft.Maui.Storage; 

namespace SandersCapstoneProject
{
    public static class SessionManager
    {
        // set is private within the scope of the SessionManger
        public static AuthState CurrentState { get; private set; } = AuthState.LoggedOut;

        public static AccountType CurrentAccountType { get; private set; } = AccountType.None;

        public static string DisplayName { get; private set; }
        public static string PhoneNumber { get; private set; }
        public static string Address { get; private set; }
        public static string Email { get; private set; }
        public static string DegreeType { get; private set; }
        public static string DegreeMajor { get; private set; }
        public static string DegreeMinor { get; private set; }
        public static bool IsPersistentLogin { get; private set; }

        public static void Login(string displayName, AccountType accountType,
            string phoneNumber, string address, string email,
            string degreeType, string degreeMajor, string degreeMinor,
            bool rememberMe)
        {
            // Sets the key value pairs from within the app
            DisplayName = displayName;
            CurrentAccountType = accountType;
            PhoneNumber = phoneNumber;
            Address = address;
            Email = email;
            DegreeType = degreeType;
            DegreeMajor = degreeMajor;
            DegreeMinor = degreeMinor;
            CurrentState = AuthState.LoggedIn;
            IsPersistentLogin = rememberMe;

            // When calling this, just insert true for the argument
            if (rememberMe)
            {
                // The key value pairs that are stored outside the app
                // , upon calling the Login() method
                Preferences.Set("IsLoggedIn", true);
                Preferences.Set("DisplayName", displayName);
                Preferences.Set("AccountType", accountType.ToString());
                Preferences.Set("PhoneNumber", phoneNumber);
                Preferences.Set("Address", address);
                Preferences.Set("Email", email);
                Preferences.Set("DegreeType", degreeType);
                Preferences.Set("DegreeMajor", degreeMajor);
                Preferences.Set("DegreeMinor", degreeMinor);
            }
        }
        
        public static void Logout()
        {
            // Sets the key value pairs from within the app
            DisplayName = null;
            CurrentAccountType = AccountType.None;
            PhoneNumber = null;
            Address = null;
            Email = null;
            DegreeType = null;
            DegreeMajor = null;
            DegreeMinor = null;
            CurrentState = AuthState.LoggedOut;
            IsPersistentLogin = false;

            Preferences.Clear(); // clears the key value pairs
        }

        public static void RestoreSession()
        {
            if (Preferences.Get("IsLoggedIn", false))
            {
                // retrieves the key value pairs
                DisplayName = Preferences.Get("DisplayName", "");
                CurrentAccountType = Enum.Parse<AccountType>(
                    Preferences.Get("AccountType", "None"));
                PhoneNumber = Preferences.Get("PhoneNumber", "");
                Address = Preferences.Get("Address", "");
                Email = Preferences.Get("Email", "");
                DegreeType = Preferences.Get("DegreeType", "");
                DegreeMajor = Preferences.Get("DegreeMajor", "");
                DegreeMinor = Preferences.Get("DegreeMinor", "");
                CurrentState = AuthState.LoggedIn; // logs the user back in
                IsPersistentLogin = true; // recalls the stored key value pairs
            }
        }
    }
}

