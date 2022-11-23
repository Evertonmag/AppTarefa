using AppTarefa.Mobile.Telas;

namespace AppTarefa.Mobile;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new NavigationPage(new Listar());
	}
}
