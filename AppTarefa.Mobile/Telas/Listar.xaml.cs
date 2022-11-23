using AppTarefa.Mobile.Banco;
using AppTarefa.Mobile.Modelos;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace AppTarefa.Mobile.Telas;

public partial class Listar : ContentPage
{
    public event PropertyChangedEventHandler PropertyChanged;

    // This method is called by the Set accessor of each property.  
    // The CallerMemberName attribute that is applied to the optional propertyName  
    // parameter causes the property name of the caller to be substituted as an argument.  
    private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private ObservableCollection<Tarefa> _lista;
    public ObservableCollection<Tarefa> Lista
    {
        get
        {
            return _lista;
        }
        set
        {
            _lista = value;
            NotifyPropertyChanged(nameof(Lista));
        }
    }


    public Listar()
    {
        InitializeComponent();

        AtualizarDataCalendario(DateTime.Now);

        MessagingCenter.Subscribe<Listar>(this, "OnTarefaCadastrada", async (sender) =>
        {
            if (Lista != null)
            {
                Lista = new ObservableCollection<Tarefa>(
                   await new TarefaDB().PesquisarAsync(DPCalendario.Date));
                CVListaDeTarefas.ItemsSource = Lista;
                lblQuantidadeTarefas.Text = Lista.Count.ToString();
            }
            MessagingCenter.Unsubscribe<Listar, Tarefa>(this, "OnTarefaCadastrada");
        });
    }

    private async void BtnCadastrar(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Cadastrar());
    }

    private async void BtnVisualizar(object sender, TappedEventArgs e)
    {
        var tarefa = (Tarefa)e.Parameter;

        await Navigation.PushAsync(new Visualizar(tarefa));
    }

    private async void BtnExcluir(object sender, EventArgs e)
    {
        bool pergunta = await DisplayAlert("Excluir", "Tem certeza que deseja excluir esseregistro? Essa ação será permanente", "Sim", "Não");

        if (pergunta)
        {
            var swipeItem = (SwipeItem)sender;

            Tarefa tarefa = (Tarefa)swipeItem.CommandParameter;

            var excluido = await new TarefaDB().ExcluirAsync(tarefa.Id);

            if (excluido)
            {
                Lista.Remove(tarefa);
                lblQuantidadeTarefas.Text = Lista.Count.ToString();
            }
        }
    }

    private async void CheckedChangedAction(object sender, CheckedChangedEventArgs e)
    {
        var checkBox = (CheckBox)sender;
        var label = checkBox.Parent.FindByName<StackLayout>("slTarefaDetalhe");
        if (label != null)
        {
            var tapGesture = (TapGestureRecognizer)label.GestureRecognizers[0];

            if (tapGesture != null)
            {
                var tarefa = (Tarefa)tapGesture.CommandParameter;

                if (tarefa != null)
                {
                    var teste = await new TarefaDB().ConsultarAsync(tarefa.Id);

                    if (teste != null && teste.Finalizada != tarefa.Finalizada)
                    {
                        await new TarefaDB().AtualizarAsync(tarefa);
                    }
                    //Tachado(label, tarefa.Finalizada);
                }
            }
        }

    }

    private void AbrirCalendario(object sender, TappedEventArgs e)
    {
        DPCalendario.Focus();
    }

    private void DateSelectedAction(object sender, DateChangedEventArgs e)
    {
        AtualizarDataCalendario(e.NewDate);
    }

    private void AtualizarDataCalendario(DateTime data)
    {
        Task.Run(() =>
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                Lista = new ObservableCollection<Tarefa>(
                        await new TarefaDB().PesquisarAsync(data)
                    );
                CVListaDeTarefas.ItemsSource = Lista;
                lblQuantidadeTarefas.Text = Lista.Count.ToString();
            });
        });

        var idioma = CultureInfo.CurrentCulture;

        lblDia.Text = data.Day.ToString();
        lblMes.Text = PrimeiraLetraMaiuscula(idioma.DateTimeFormat.GetMonthName(data.Month)[..3]);
        lblDiaDaSemana.Text = PrimeiraLetraMaiuscula(idioma.DateTimeFormat.GetDayName(data.DayOfWeek));
    }

    private void Tachado(Label label, bool finalizado)
    {
        if (finalizado)
        {
            label.TextDecorations = TextDecorations.Strikethrough;
        }
        else
        {
            label.TextDecorations = TextDecorations.None;
        }
    }

    private string PrimeiraLetraMaiuscula(string palavra) => char.ToUpper(palavra[0]) + palavra[1..];
}