
namespace ProjetoCompletoLocadora.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }

        public List<Veiculo> Veiculos { get; set; } = new();
    }
}
