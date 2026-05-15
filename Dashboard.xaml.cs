/* Aaron Keith Sanders
     UA Grantham
     IS498: Senior Research Project
     Senior Research Capstone Project
     Robert Chubbuck
     File: Dashboard.xaml.cs
     19 April 2026
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using SQLite;

namespace SandersCapstoneProject
{
    public partial class Dashboard : ContentPage
    {

        public Dashboard()
        {
            InitializeComponent();            
        }

        protected override void OnAppearing()
        {
            // Allows modification of the OnAppearing() method
            // behaviors
            base.OnAppearing();

            // Retrieving the values from the persisted state using SessionManager
            string displayName = SessionManager.DisplayName;
            var accountType = SessionManager.CurrentAccountType;

            // The label text is changed from its default "Welcome"
            // to the new text containing the users displayName and 
            // accountType
            WelcomeLabel.Text =
                $"Hello, {displayName}, welcome to your {accountType} Dashboard! Please feel free to explore.";

            if (accountType == AccountType.Employee)
            {
                EmployeeOrientationBtn.IsVisible = true;
                ViewHireListBtn.IsVisible = false;
            }
            else if (accountType == AccountType.Institution)
            {
                EmployeeOrientationBtn.IsVisible = false;
                ViewHireListBtn.IsVisible = true;
            }
        }

        private async void OnLogoutClicked(object? sender, EventArgs e)
        {
            // Adds visual tactility to button presses
            if (sender is Button btn)
            {
                await btn.ScaleTo(0.8, 100);   // shrink
                await btn.ScaleTo(1, 100);     // bounce back
            }

            // Call the Logout method from the SessionManager
            // then send the user back to the home page
            SessionManager.Logout();
            await DisplayAlert("Success", "You have logged out", "OK");
            await Navigation.PopToRootAsync();
        }

        private async void OnAccountSettingsClicked(object sender, EventArgs e)
        {
            // Retrieving the values from the persisted state using SessionManager
            string displayName = SessionManager.DisplayName;
            var accountType = SessionManager.CurrentAccountType;
            string phoneNumber = SessionManager.PhoneNumber;
            string address = SessionManager.Address;
            string email = SessionManager.Email;
            string degreeType = SessionManager.DegreeType;
            string degreeMajor = SessionManager.DegreeMajor;
            string degreeMinor = SessionManager.DegreeMinor;

            // Adds visual tactility to button presses
            if (sender is Button btn)
            {
                await btn.ScaleTo(0.8, 100);   // shrink
                await btn.ScaleTo(1, 100);     // bounce back
            }            

            // Altering the SettingsLbl to be set to the current user's 
            // persisted login session save state data
            if (accountType == AccountType.Employee)
            {
                // Employee settings
                SettingsLbl.Text = ($"Name: {displayName} \n" +
                $"Phone Number: {phoneNumber} \n" +
                $"Address: {address} \n" +
                $"Email: {email} \n" +
                $"Degree Type: {degreeType} \n" +
                $"Degree Major: {degreeMajor} \n" +
                $"Degree Minor: {degreeMinor} ");
            }
            else if (accountType == AccountType.Institution)
            {
                // Institution settings
                SettingsLbl.Text = ($"Name: {displayName} \n" +
                $"Phone Number: {phoneNumber} \n" +
                $"Address: {address} \n" +
                $"Email: {email} ");
            }

            // Make the settings section toggleable - visible\invisible
            SettingsSection.IsVisible = !SettingsSection.IsVisible;            
        }

        private async void OnEmployeeOrientationClicked(object sender, EventArgs e)
        {
            // Retrieving the values from the persisted state using SessionManager
            string displayName = SessionManager.DisplayName;
            var accountType = SessionManager.CurrentAccountType;
            string phoneNumber = SessionManager.PhoneNumber;
            string address = SessionManager.Address;
            string email = SessionManager.Email;

            // Adds visual tactility to button presses
            if (sender is Button btn)
            {
                await btn.ScaleTo(0.8, 100);   // shrink
                await btn.ScaleTo(1, 100);     // bounce back
            }

            await DisplayAlert("Employee Orientation", "Please answer the following questions.", "OK");
            // Toggleable view section
            OrientationSection.IsVisible = !OrientationSection.IsVisible;
        }

        private async void OnViewHireListClicked(object sender, EventArgs e)
        {
            // Adds visual tactility to button presses
            if (sender is Button btn)
            {
                await btn.ScaleTo(0.8, 100);   // shrink
                await btn.ScaleTo(1, 100);     // bounce back
            }

            await DisplayAlert("Hire List", "Coming soon!", "OK");
        }

        private async void FinishOrientation(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                await DisplayAlert("Employee Orientation", "You've completed your orientation questions!", "SUBMIT");

            }
        }
    }
}
