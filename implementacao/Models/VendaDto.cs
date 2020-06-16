namespace Models
{
    public class VendaDto
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public string Observacao { get; set; }
        public int FormaDePagamento { get; set; }
        public int IdPedidoEstoque { get; set; }

    }
}
