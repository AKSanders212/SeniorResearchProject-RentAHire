/* Aaron Keith Sanders
     UA Grantham
     IS498: Senior Research Project
     Senior Research Capstone Project
     Robert Chubbuck
     File: MainPage.xaml.cs
     28 March 2026
*/

// REGISTER NEW PAGES @ APPSHELL.XAML.CS - THEN CALL THEM (await Shell.Current.GoToAsync(nameof(NAME OF PAGE))
//using Android.Telephony;
using SQLite;
using System.Diagnostics;

namespace SandersCapstoneProject
{
    public partial class MainPage : ContentPage
    {
        private SQLiteConnection _mydatabase;
        int count = 0;
        
        public MainPage()
        {
            InitializeComponent();

            // Creating another fresh connection to the local database
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "mydatabase.db");
            _mydatabase = new SQLiteConnection(dbPath);

            // Make sure tables exist
            _mydatabase.CreateTable<EmployeeAccounts>();
            _mydatabase.CreateTable<InstitutionAccounts>();
        }

        // Account creation button clicked method. Needs to be asynchronous, in order to push navigation 
        // to the account creation page through using await
        private async void OnCreationClicked(object? sender, EventArgs e)
        {
            // Adds visual tactility to button presses
            if (sender is Button btn)
            {
                await btn.ScaleTo(0.8, 100);   // shrink
                await btn.ScaleTo(1, 100);     // bounce back
            }

            Debug.WriteLine("Account creation button was clicked -> AccountCreation page should launch");
            await Shell.Current.GoToAsync(nameof(AccountCreation)); // Shell navigation is what works
            SemanticScreenReader.Announce(CreationBtn.Text);
        }

        // Account loging button clicked method
        private async void OnLoginClicked(object? sender, EventArgs e)
        {
            // Adds visual tactility to button presses
            if (sender is Button btn)
            {
                await btn.ScaleTo(0.8, 100);   // shrink
                await btn.ScaleTo(1, 100);     // bounce back
            }

            string input = EmailPhoneEntry.Text?.Trim();
            string password = PasswordEntry.Text;

            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Error", "Please enter all fields.", "OK");
                return;
            }

            // Find user in EmployeeAccounts
            var employee = _mydatabase.Table<EmployeeAccounts>()
                .FirstOrDefault(u => u.Email == input || u.PhoneNumber == input);

            // Find user in InstitutionAccounts if not found
            var institution = employee == null
                ? _mydatabase.Table<InstitutionAccounts>()
                    .FirstOrDefault(u => u.Email == input || u.PhoneNumber == input)
                : null;

            string storedHash = null;

            if (employee != null)
                storedHash = employee.Password;
            else if (institution != null)
                storedHash = institution.Password;

            if (storedHash == null)
            {
                await DisplayAlert("Login Failed", "Credentials did not match.", "OK");
                return;
            }

            bool isValid = PasswordHelper.VerifyPassword(password, storedHash);

            if (isValid)
            {
                await DisplayAlert("Success", "Login successful!", "OK");

                // Setting variables to contain the displayName
                // and the accountType
                string displayName;
                AccountType accountType;
                string phoneNumber;
                string address;
                string email;
                string degreeType;
                string degreeMajor;
                string degreeMinor;

                // If user created was an employee type and not 
                // an institution
                if (employee != null)
                {
                    displayName = employee.FirstName;
                    accountType = AccountType.Employee;
                    phoneNumber = employee.PhoneNumber;
                    address = ($"{employee.City}, {employee.State}");
                    email = employee.Email;
                    degreeType = employee.DegreeType;
                    degreeMajor = employee.DegreeMajor;
                    degreeMinor = employee.DegreeMinor;
                }
                // Otherwise the user is an institution type
                else
                {
                    displayName = institution.InstName;
                    accountType = AccountType.Institution;
                    phoneNumber = institution.PhoneNumber;
                    address = ($"{institution.City}, {institution.State}");
                    email = institution.Email;
                    // Set these to null because they do not pertain to
                    // institutions
                    degreeType = null;
                    degreeMajor = null;
                    degreeMinor = null;
                }

                // Allow state and data persistence through SessionManager
                SessionManager.Login(displayName, accountType, phoneNumber,
                    address, email, degreeType, degreeMajor, degreeMinor, true);

                // Navigate to dashboard
                await Shell.Current.GoToAsync(nameof(Dashboard));
            }
            else
            {
                await DisplayAlert("Login Failed", "Credentials did not match.", "OK");
            }
            SemanticScreenReader.Announce(LoginBtn.Text);
        }
    }
}
