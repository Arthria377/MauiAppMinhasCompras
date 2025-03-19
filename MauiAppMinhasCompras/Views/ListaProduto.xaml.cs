using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;
using System.Security.Cryptography;

namespace MauiAppMinhasCompras.Views;


public partial class ListaProduto : ContentPage
{

    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();
    public ListaProduto()
	{
		InitializeComponent();

		lst_produtos.ItemsSource = lista;
	}

    protected async override void OnAppearing()
    {
        try
        {
            lista.Clear();

            List<Produto> tmp = await App.Db.GetAll();

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try
		{

			Navigation.PushAsync(new Views.NovoProduto());

		} catch (Exception ex)
		{

			DisplayAlert("Ops", ex.Message, "OK");

		}
    }

    private async void txt_search_SearchButtonPressed(object sender, EventArgs e)
    {
        try
        {
            string q = txt_search.Text?.Trim(); // Obtém o texto de pesquisa

            // Se o campo de pesquisa estiver vazio, restaura a lista original
            if (string.IsNullOrWhiteSpace(q))
            {
                lista.Clear();
                var produtos = await App.Db.GetAll(); // Pega todos os produtos novamente
                produtos.ForEach(i => lista.Add(i)); // Adiciona os produtos na lista
                return; // Sai sem fazer mais nada
            }

            // Se houver texto na pesquisa, faz a busca
            lista.Clear(); // Limpa a lista antes de adicionar os novos resultados

            List<Produto> tmp = await App.Db.Search(q); // Realiza a busca no banco de dados

            // Verifica se foram encontrados produtos
            if (tmp == null || tmp.Count == 0)
            {
                await DisplayAlert("Produto Não Encontrado", "Nenhum produto encontrado para a busca.", "OK");
            }
            else
            {
                tmp.ForEach(i => lista.Add(i)); // Adiciona os produtos encontrados na lista
            }
        }
        catch (Exception ex)
        {
            // Exibe o erro em caso de exceção
            await DisplayAlert("Erro", $"Ocorreu um erro: {ex.Message}", "OK");
        }
    }

    private async void OnTapOutside(object sender, EventArgs e)
    {
        // Desfoca a SearchBar
        txt_search.Unfocus();

        // Restaura a lista de produtos caso a SearchBar esteja vazia
        string q = txt_search.Text?.Trim();
        if (string.IsNullOrWhiteSpace(q))
        {
            lista.Clear();
            var produtos = await App.Db.GetAll(); // Pega todos os produtos novamente
            produtos.ForEach(i => lista.Add(i)); // Adiciona os produtos na lista
        }
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
		double soma = lista.Sum(i => i.Total);

		string msg = $"O total é {soma:C}";

		DisplayAlert("Total dos Produtos", msg, "Ok");
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            MenuItem selecionado = sender as MenuItem;
            Produto p = selecionado.BindingContext as Produto;

            bool confirm = await DisplayAlert("Tem certeza?", "Remover produto?", "Sim", "Não");

            if (confirm)
            {
                await App.Db.Delete(p.Id);
                lista.Remove(p);

                // Se todos os produtos forem removidos, resetar a contagem do ID
                if (lista.Count == 0)
                {
                    await App.Db.ResetAutoIncrement();
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "Ok");
        }

    }



    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            Produto p = e.SelectedItem as Produto;

            Navigation.PushAsync(new Views.EditarProduto
            {
                BindingContext = p,
            });
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "Ok");
        }
    }
}