namespace MauiAppMinhasCompras.Views
{
    public partial class RelatorioCategoria : ContentPage
    {
        public RelatorioCategoria()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var produtos = await App.Db.ListaProdutos();
            var relatorio = produtos
                .GroupBy(p => p.Categoria)
                .Select(g => new {
                    Categoria = g.Key,
                    Total = g.Sum(p => p.Total)
                }).ToList();

            lst_relatorio.ItemsSource = relatorio;
        }
    }
}
