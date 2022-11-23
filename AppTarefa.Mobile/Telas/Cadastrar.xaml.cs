using AppTarefa.Mobile.Banco;
using AppTarefa.Mobile.Modelos;
using CommunityToolkit.Mvvm.Messaging;

namespace AppTarefa.Mobile.Telas;

public partial class Cadastrar : ContentPage
{
    public string Prioridade;

    public Cadastrar()
    {
        InitializeComponent();

        lblDataSelecionada.Text = DateTime.Now.ToString("d");
        rbMedia.IsChecked = true;
    }

    private async void FecharModal(object sender, TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void SalvarTarefa(object sender, EventArgs e)
    {
        Tarefa tarefa = new()
        {
            Nome = Nome.Text,
            Nota = Nota.Text,
            Data = Data.Date,
            HorarioInicial = HorarioInicial.Time,
            HorarioFinal = HorarioFinal.Time,
            Finalizada = false,
            Prioridade = Prioridade,
        };

        if (await ValidacaoAsync(tarefa))
        {
            if (await new TarefaDB().CadastrarAsync(tarefa))
            {
                MessagingCenter.Send(new Listar(), "OnTarefaCadastrada");

                await Navigation.PopAsync();
            }
        }
    }

    private async Task<bool> ValidacaoAsync(Tarefa tarefa)
    {
        bool validacao = true;

        if (tarefa.Data < DateTime.Now.Date)
        {
            await DisplayAlert("Erro", "A data não pode ser inferior a atual", "Ok");
            validacao = false;
        }
        if (tarefa.HorarioInicial > tarefa.HorarioFinal)
        {
            await DisplayAlert("Erro", "O horário inicial não pode ser maior que o horário de término", "Ok");
            validacao = false;
        }
        if (tarefa.Nome == null)
        {
            await DisplayAlert("Erro", "O nome da tarefa precisa ser preenchido", "Ok");
            validacao = false;
        }
        if (tarefa.Nome != null && tarefa.Nome.Length < 5)
        {
            await DisplayAlert("Erro", "O nome da tarefa precisa ter no minimo 5 caracteres", "Ok");
            validacao = false;
        }

        return validacao;
    }


    private void DataSelecionadaAction(object sender, DateChangedEventArgs e)
    {
        lblDataSelecionada.Text = e.NewDate.ToString("d");
    }

    private void RBBaixa_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        Prioridade = ((RadioButton)sender).Content.ToString();
    }

    //private void HorarioInicial_Unfocused(object sender, FocusEventArgs e)
    //{
    //    lblHorarioInicial.Text = ((TimePicker)sender).Time.ToString("t");
    //    HorarioFinal.Focus();
    //}

    //private void HorarioFinal_Unfocused(object sender, FocusEventArgs e)
    //{
    //    lblHorarioFinal.Text = ((TimePicker)sender).Time.ToString("t");
    //}
}