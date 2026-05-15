/* Aaron Keith Sanders
 * G00225605
 * UA Grantham
 * IS498: Senior Research Project
 * File: AccountCreation.xaml.cs
 * current date: 5 April 2026
*/

using System.Diagnostics;
using SQLite;

namespace SandersCapstoneProject;

public partial class AccountCreation : ContentPage
{
    private SQLiteConnection _mydb; // Connection to mydatabase.db
    public AccountCreation()
    {
        InitializeComponent();

        // Creating another fresh connection to the local database
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "mydatabase.db");
        _mydb = new SQLiteConnection(dbPath);

        // Ensure tables exist
        _mydb.CreateTable<EmployeeAccounts>();
        _mydb.CreateTable<InstitutionAccounts>();

        Debug.WriteLine($"Database initialized at: {dbPath}");
    }

    // Stores the account type based on the user selection (either employee or institution)
    private string selectedAccountType;

    // A method to handle when the account type is selected, based on the radio button from user input
    private void OnAccountTypeChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            var accountRadio = sender as RadioButton;
            selectedAccountType = accountRadio.Value.ToString();

            // A function that changes the UX front-end UI depending on the account type selected 
            UXChanger();

            // Displays in debug the selected account type
            System.Diagnostics.Debug.WriteLine($"Selected: {selectedAccountType}");
        }
    }

    private void UXChanger()
    {
        // Show/hide First Name field
        if (selectedAccountType == "Employee")
        {
            EmployeeSection.IsVisible = true;
            InstitutionSection.IsVisible = false;
        }
        else if (selectedAccountType == "Institution")
        {
            InstitutionSection.IsVisible = true;
            EmployeeSection.IsVisible = false;
        }
    }

    // Used for handling new employee account users
    [Obsolete]
    private async void OnCreateEmployeeAccountClicked(object sender, EventArgs e)
    {
        // Adds visual tactility to button presses
        if (sender is Button btn)
        {
            await btn.ScaleTo(0.8, 100);   // shrink
            await btn.ScaleTo(1, 100);     // bounce back
        }


        // Get input from UI
        string firstName = FirstNameEntry.Text;
        string lastName = LastNameEntry.Text;
        string city = CityEntry.Text;
        string state = StateEntry.Text;
        string zip = ZipEntry.Text;
        string universityName = UniversityEntry.Text;
        string degreeType = DegreeTypeEntry.Text;
        string degreeMajor = DegreeMajorEntry.Text;
        string degreeMinor = DegreeMinorEntry.Text;
        string phoneNumber = PhoneEntry.Text;
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;

        // Now set the password to the hashed password instead
        password = PasswordHelper.HashPassword(password);

        if (EmailOrPhoneExists(email, phoneNumber))
        {
            // Produce an alert message of any duplicates found in the database, then return
            await DisplayAlert("Duplicates Found!", "The phone number and/or email already exists, Do you already have an account?" +
                "If not try a different email or phone number please.", "Ok");
            return;
        }

        // Create new employee object
        var newEmployee = new EmployeeAccounts
        {
            FirstName = firstName,
            LastName = lastName,
            City = city,
            State = state,
            Zip = zip,
            UniversityName = universityName,
            DegreeType = degreeType,
            DegreeMajor = degreeMajor,
            DegreeMinor = degreeMinor,
            PhoneNumber = phoneNumber,
            Email = email,
            Password = password // should store the hashed password as byte[] array and convert to base 64 string
        };        

        // Insert into database
        _mydb.Insert(newEmployee);

        // After insert, SQLite automatically sets the Id
        Debug.WriteLine($"NEW USER CREATED:");
        Debug.WriteLine($"ID: {newEmployee.Id}");
        Debug.WriteLine($"First Name: {newEmployee.FirstName}");
        Debug.WriteLine($"Last Name: {newEmployee.LastName}");
        Debug.WriteLine($"City: {newEmployee.City}");
        Debug.WriteLine($"State: {newEmployee.State}");
        Debug.WriteLine($"Zip: {newEmployee.Zip}");
        Debug.WriteLine($"DegreeType: {newEmployee.DegreeType}");
        Debug.WriteLine($"DegreeMajor: {newEmployee.DegreeMajor}");
        Debug.WriteLine($"DegreeMinor: {newEmployee.DegreeMinor}");
        Debug.WriteLine($"PhoneNumber: {newEmployee.PhoneNumber}");
        Debug.WriteLine($"Email: {newEmployee.Email}");

        // Sends the user back to the home page after creating their account
        await DisplayAlert("Success", "Account created successfully!", "OK");
        await Navigation.PopToRootAsync();
    }

        // Used for handling new employee account users
    private async void OnCreateInstitutionAccountClicked(object sender, EventArgs e)
    {
        // Adds visual tactility to button presses
        if (sender is Button btn)
        {
            await btn.ScaleTo(0.8, 100);   // shrink
            await btn.ScaleTo(1, 100);     // bounce back
        }


        // Get input from UI
        string instName = InstitutionEntry.Text;
        string city = InstCityEntry.Text;
        string state = InstStateEntry.Text;
        string zip = InstZipEntry.Text;
        string phoneNumber = InstPhoneEntry.Text;
        string email = InstEmailEntry.Text;
        string password = InstPasswordEntry.Text;

        password = PasswordHelper.HashPassword(password);

        if (EmailOrPhoneExists(email, phoneNumber))
        {
            // Produce an alert message of any duplicates found in the database, then return
            await DisplayAlert("Duplicates Found!", "The phone number and/or email already exists, Do you already have an account?" +
                "If not try a different email or phone number please.", "Ok");
            return;
        }

        // Create new employee object
        var newInstitution = new InstitutionAccounts
        {
            InstName = instName,
            City = city,
            State = state,
            Zip = zip,
            PhoneNumber = phoneNumber,
            Email = email,
            Password = password
        };

        // Insert into database
        _mydb.Insert(newInstitution);

        // After insert, SQLite automatically sets the Id
        Debug.WriteLine($"NEW USER CREATED:");
        Debug.WriteLine($"ID: {newInstitution.Id}");
        Debug.WriteLine($"Institution Name: {newInstitution.InstName}");
        Debug.WriteLine($"City: {newInstitution.City}");
        Debug.WriteLine($"State: {newInstitution.State}");
        Debug.WriteLine($"Zip: {newInstitution.Zip}");
        Debug.WriteLine($"Phone Number: {newInstitution.PhoneNumber}");
        Debug.WriteLine($"Email: {newInstitution.Email}");
        PasswordHelper.VerifyPassword(password, newInstitution.Password);

        // Sends the user back to the home page after creating their account
        await DisplayAlert("Success", "Account created successfully!", "OK");
        await Navigation.PopToRootAsync();
    }

    // Shows the password at input, removing the mask, if you need to see what you're typing
    private void OnShowPasswordCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        PasswordEntry.IsPassword = !e.Value;
    }

    // A function the resets the data in the employee table, used for clearing data from mydatabase.db
    private async void OnResetTables(object sender, EventArgs e)
    {
        // Adds visual tactility to button presses
        if (sender is Button btn)
        {
            await btn.ScaleTo(0.8, 100);   // shrink
            await btn.ScaleTo(1, 100);     // bounce back
        }

        if (_mydb == null)
        {
            Debug.WriteLine("Database connection is NULL!");
            return;
        }

        // Delete all rows
        _mydb.DeleteAll<EmployeeAccounts>();
        _mydb.DeleteAll<InstitutionAccounts>();

        // Reset auto-increment counter
        _mydb.Execute("DELETE FROM sqlite_sequence WHERE name = 'EmployeeAccounts'");
        _mydb.Execute("DELETE FROM sqlite_sequence WHERE name = 'InstitutionAccounts'");

        await DisplayAlert("Alert: Success!", "All data in the tables have been reset", "OK");
        Debug.WriteLine("All data tables have been reset.");
    }

    // Method to check for any duplicate emails or phone numbers in the database
    private bool EmailOrPhoneExists(string email, string phone)
    {
        // Employee table is checked
        bool employeeExists = _mydb.Table<EmployeeAccounts>()
            .Any(e => e.Email == email || e.PhoneNumber == phone);

        // Institution table is checked
        bool institutionExists = _mydb.Table<InstitutionAccounts>()
            .Any(i => i.Email == email || i.PhoneNumber == phone);

        return employeeExists || institutionExists;
    }
}