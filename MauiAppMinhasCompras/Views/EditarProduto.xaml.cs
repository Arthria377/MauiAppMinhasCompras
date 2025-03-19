using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
	public EditarProduto()
	{
		InitializeComponent();
	}

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Produto produto_anexado = BindingContext as Produto;

            Produto p = new Produto
            {
                Id = produto_anexado.Id,
                Descricao = string.IsNullOrWhiteSpace(txt_descricao.Text) ? "Sem descrição" : txt_descricao.Text,
                Quantidade = string.IsNullOrWhiteSpace(txt_quantidade.Text) ? 0 : Convert.ToDouble(txt_quantidade.Text),
                Preco = string.IsNullOrWhiteSpace(txt_preco.Text) ? 0 : Convert.ToDouble(txt_preco.Text)
            };

            await App.Db.Update(p);
            await DisplayAlert("Sucesso!", "Resgistro Atualizado", "OK!");
            await Navigation.PopToRootAsync();

        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}