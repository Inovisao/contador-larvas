using AppContagem.Classes;
using AppContagem.ViewModels;

namespace AppContagem.Pages;

public partial class Historico : ContentPage
{
	HistoricoViewModel viewModel;
	public Historico()
	{
		InitializeComponent();
        BindingContext = viewModel = new HistoricoViewModel();
    }

    async void VerFotos_Tapped(object sender, ItemTappedEventArgs e)
    {
        ImageGroup selectedItem = (ImageGroup)e.Item;
        viewModel.Ver_Fotos(selectedItem);
    }
}