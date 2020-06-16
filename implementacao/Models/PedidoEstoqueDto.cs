using System.Collections.Generic;

namespace Models
{
    public class PedidoEstoqueDto
    {
        public int Id { get; set; }
        public int Tipo { get; set; }
        public int Origem { get; set; }
        public int IdUsuario { get; set; }
        public int IdFilial { get; set; }
        public List<ItemPedidoDto> Itens { get; set; }
        public VendaDto Venda { get; set; }

        public PedidoEstoqueDto()
        {
            Itens = new List<ItemPedidoDto>();
        }
    }
}
