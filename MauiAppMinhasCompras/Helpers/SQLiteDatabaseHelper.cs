using MauiAppMinhasCompras.Models;
using SQLite;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _conn;

        public SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait();
        }

        public Task<int> Insert(Produto p)
        {
            Console.WriteLine($"Inserindo: {p.Descricao}, {p.Quantidade}, {p.Preco}");
            return _conn.InsertAsync(p);
        }

        public Task<List<Produto>> Update(Produto p)
        {
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=? WHERE Id=?";
            return _conn.QueryAsync<Produto>(sql, p.Descricao, p.Quantidade, p.Preco, p.Id);
        }

        public async Task<int> Delete(int id)
        {
            // Exclui o produto
            var result = await _conn.Table<Produto>().DeleteAsync(i => i.Id == id);

            return result;
        }

        public Task<List<Produto>> GetAll()
        {
            return _conn.Table<Produto>().ToListAsync();
        }

        public Task<List<Produto>> Search(string query)
        {
            string sql = "SELECT * FROM Produto WHERE Descricao LIKE '%' || ? || '%'";
            return _conn.QueryAsync<Produto>(sql, query);
        }

        public Task<int> ExecuteAsync(string query)
        {
            return _conn.ExecuteAsync(query);
        }

        public async Task ResetAutoIncrement()
        {
            await _conn.ExecuteAsync("DELETE FROM sqlite_sequence WHERE name='Produto'");
            await _conn.ExecuteAsync("VACUUM");
        }
    }
}
