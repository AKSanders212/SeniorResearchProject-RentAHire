/* Aaron Keith Sanders
     UA Grantham
     IS498: Senior Research Project
     Senior Research Capstone Project
     Robert Chubbuck
     File: MainPage.xaml.cs
     28 March 2026
*/

using Microsoft.Extensions.DependencyInjection;

namespace SandersCapstoneProject
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();            
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}