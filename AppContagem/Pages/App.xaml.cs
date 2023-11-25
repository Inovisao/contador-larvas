
namespace AppContagem;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        var appNav = new NavigationPage(new MainPage() { BackgroundColor = Color.FromArgb("#000000") });
        MainPage = appNav;
    }
}
