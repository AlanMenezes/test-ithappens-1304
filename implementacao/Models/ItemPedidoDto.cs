namespace Models
{
    public class ItemPedidoDto
    {
        public int Id { get; set; }
        public int IdPedido { get; set; }
        public int IdProduto { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Quantidade { get; set; }
        public decimal Valortotal => ValorUnitario * Quantidade;
        public int Status { get; set; }

    }
}
