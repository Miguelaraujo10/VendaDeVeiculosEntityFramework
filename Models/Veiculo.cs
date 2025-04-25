namespace ProjetoCompletoLocadora.Models
{
    public class Veiculo
    {
        public int Id { get; set; }
        public string Modelo { get; set; } = string.Empty;
        public int AnoFabricacao { get; set; }
        public int Quilometragem { get; set; }

        public int FabricanteId { get; set; }
        public Fabricante Fabricante { get; set; } = new Fabricante();  // Corrigido: instanciando Fabricante

        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = new Categoria();  // Garantindo que a categoria também seja inicializada

        public ICollection<Aluguel> Alugueis { get; set; } = new List<Aluguel>();  // Inicializando Alugueis
    }
}
