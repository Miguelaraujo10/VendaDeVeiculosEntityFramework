
namespace ProjetoCompletoLocadora.Models
{
    public class Fabricante
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        public List<Veiculo> Veiculos { get; set; } = new();

    }
}
