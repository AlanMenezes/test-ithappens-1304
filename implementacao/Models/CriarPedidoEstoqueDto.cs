using System.Collections.Generic;

namespace Models
{
    public class CriarPedidoEstoqueDto
    {
        public int Tipo { get; set; }
        public int Origem { get; set; }
        public int IdUsuario { get; set; }
        public int IdFilial { get; set; }
        public List<CriarItemPedidoDto> Itens { get; set; }
        public CriarVendaDto Venda { get; set; }
    }
}
