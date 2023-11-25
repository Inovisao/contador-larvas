using AppContagem.Classes;
using AppContagem.ViewModels;

namespace AppContagem.Pages;

public partial class Resultados : ContentPage
{
    ResultadosViewModel viewModel;
    public Resultados(List<ImageDTO> images)
    {
        BindingContext = viewModel = new ResultadosViewModel(images);
        InitializeComponent();
    }
}