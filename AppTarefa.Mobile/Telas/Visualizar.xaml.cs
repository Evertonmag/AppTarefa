using AppTarefa.Mobile.Modelos;

namespace AppTarefa.Mobile.Telas;

public partial class Visualizar : ContentPage
{
	public Visualizar()
	{
		InitializeComponent();
	}

	public Visualizar(Tarefa tarefa)
	{
        InitializeComponent();

		BindingContext = tarefa;

		if (tarefa.Nota == null || tarefa.Nota != null && tarefa.Nota.Length == 0)
		{
			lblTituloNota.IsVisible = false;
		}
    }

    private async void BtnVoltar(object sender, TappedEventArgs e)
    {
		await Navigation.PopAsync();
    }
}