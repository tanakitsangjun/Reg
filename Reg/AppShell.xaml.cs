using Reg.Pages;

namespace Reg;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
    }
}
