namespace ProjetoCompletoLocadora.Models
{
    public class Aluguel
    {
        public int Id { get; set; }

        // Instancia os objetos para evitar que sejam nulos, mas com IDs
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; } = new Cliente();  // Instanciando Cliente

        public int VeiculoId { get; set; }
        public Veiculo Veiculo { get; set; } = new Veiculo();  // Instanciando Veiculo

        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public DateTime? DataDevolucao { get; set; }

        public int QuilometragemInicial { get; set; }
        public int QuilometragemFinal { get; set; }

        public decimal ValorDiaria { get; set; }
        public decimal ValorTotal { get; set; }
    }
}
