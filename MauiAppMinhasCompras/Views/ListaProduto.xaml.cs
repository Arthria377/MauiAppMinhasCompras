using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

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
        // Quando a tela aparece, os produtos são carregados do banco de dados
        var produtos = await App.Db.GetAll(); // Obter todos os produtos do banco

        // Adiciona os produtos na ObservableCollection, que automaticamente atualiza a interface
        lista.Clear(); // Limpa a lista antes de adicionar os novos produtos
        produtos.ForEach(i => lista.Add(i)); // Adiciona os produtos na lista
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
            // Obter o produto da célula clicada
            var menuItem = (MenuItem)sender;
            var produto = (Produto)menuItem.BindingContext;

            // Excluir o produto do banco de dados
            var resultado = await App.Db.Delete(produto.Id);

            // Verificar se a exclusão foi bem-sucedida
            if (resultado > 0)
            {
                // Obter todos os produtos restantes
                var produtosRestantes = await App.Db.GetAll();

                // Se a lista de produtos estiver vazia, reiniciar o contador de auto incremento
                if (produtosRestantes.Count == 0)
                {
                    await App.Db.ResetAutoIncrement();  // Resetar o contador para 1
                }

                // Atualizar a lista de exibição
                lista.Clear();
                produtosRestantes.ForEach(i => lista.Add(i));

                // Exibir um alerta com o produto removido
                await DisplayAlert("Produto Removido", $"O produto {produto.Descricao} foi removido.", "OK");
            }
            else
            {
                await DisplayAlert("Erro", "Não foi possível excluir o produto.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }

    }

}