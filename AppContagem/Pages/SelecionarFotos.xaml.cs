using AppContagem.ViewModels;

namespace AppContagem.Pages;

public partial class SelecionarFotos : ContentPage
{
    SelecionarFotosViewModel viewModel { get; set; }
    public SelecionarFotos()
	{
        BindingContext = viewModel = new SelecionarFotosViewModel();
        InitializeComponent();
	}
    private void capturaFotoClicked(object sender, EventArgs e)
    {
        viewModel.CapturarFoto();
    }

    private void selecioneFotoClicked(object sender, EventArgs e)
    {
        viewModel.SelecionarFoto();
    }
}