namespace SandersCapstoneProject
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            // Routing must be registered here at AppShell.xaml.cs
            Routing.RegisterRoute(nameof(AccountCreation), typeof(AccountCreation));
            Routing.RegisterRoute(nameof(Dashboard), typeof(Dashboard));

        }
    }
}
