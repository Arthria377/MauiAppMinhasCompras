using MauiAppMinhasCompras.Models;
using System.Threading.Tasks;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
	public NovoProduto()
	{
		InitializeComponent();
	}

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try
		{
            Produto p = new Produto
            {
                Descricao = string.IsNullOrWhiteSpace(txt_descricao.Text) ? "Sem descrição" : txt_descricao.Text,
                Quantidade = string.IsNullOrWhiteSpace(txt_quantidade.Text) ? 0 : Convert.ToDouble(txt_quantidade.Text),
                Preco = string.IsNullOrWhiteSpace(txt_preco.Text) ? 0 : Convert.ToDouble(txt_preco.Text)
            };

            await App.Db.Insert(p);
			await DisplayAlert("Sucesso!", "Resgistro Inserido", "OK!");

		}catch(Exception ex)
		{
			await DisplayAlert("Ops", ex.Message, "OK");
		}
    }
}