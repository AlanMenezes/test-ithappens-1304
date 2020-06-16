namespace Models
{
    public class CriarItemPedidoDto
    {
        public int IdProduto { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Quantidade { get; set; }
        public int Status { get; set; }
    }
}
