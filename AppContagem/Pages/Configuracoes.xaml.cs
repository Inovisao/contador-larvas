using AppContagem.Classes;

namespace Fishlytics.Pages;

public partial class Configuracoes : ContentPage
{
	public Configuracoes()
	{
		InitializeComponent();
        ip_server_entry.Text = Config.Url;
    }

    private void confirm_Clicked(object sender, EventArgs e)
    {
        Config.Url = ip_server_entry.Text.EndsWith("/") ? ip_server_entry.Text : ip_server_entry.Text + "/";
    }
}