using AppContagem.Pages;

namespace AppContagem;

public partial class MainPage : ContentPage
{
    public MainPage()
	{
        InitializeComponent();
	}

    private async void enviarFotosClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SelecionarFotos());
    }

    private async void historico_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Historico());
    }
}

