using MauiAppMinhasCompras.Helpers;
using System.Globalization;

namespace MauiAppMinhasCompras
{
    public partial class App : Application
    {
        public static SQLiteDatabaseHelper Db { get; private set; }

        public App()
        {
            InitializeComponent();

            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");

            string caminho = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "banco_sqlite_compras.db3"
            );

            Db = new SQLiteDatabaseHelper(caminho);

            MainPage = new NavigationPage(new Views.ListaProduto());
        }
    }
}
