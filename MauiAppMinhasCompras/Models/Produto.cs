using SQLite;

namespace MauiAppMinhasCompras.Models
{
    public class Produto
    {
        string _descricao;
        private double _quantidade;
        private double _preco;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Descricao {
            get => _descricao;
            set
            {
                if(value == string.Empty)
                {
                    throw new Exception("Por favor, preencha a descrição");
                }

                _descricao = value;
            }
        }
        public double Quantidade
        {
            get => _quantidade;
            set
            {
                if (value <= 0)
                {
                    throw new Exception("A quantidade deve ser maior que zero");
                }
                _quantidade = value; // Agora está atribuindo ao campo privado correto
            }
        }

        public double Preco
        {
            get => _preco;
            set
            {
                if (value <= 0)
                {
                    throw new Exception("O preço deve ser maior que zero");
                }
                _preco = value; // Agora está atribuindo ao campo privado correto
            }
        }
        public double Total { get => Quantidade * Preco; }

    }
}
