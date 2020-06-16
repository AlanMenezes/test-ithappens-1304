using Models;
using Negocio;
using Persistencia;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class PedidoEstoqueController: ApiController
    {
        [HttpPost]
        [Route("pedidos")]
        public HttpResponseMessage Criar(CriarPedidoEstoqueDto dto)
        {
            Venda venda = null;
            if (dto.Origem == (int)OrigemPedidoEstoque.Venda)
            {
                venda = Venda.Criar(0, dto.Venda.IdCliente, dto.Venda.Observacao, (FormaDePagamento)dto.Venda.FormaDePagamento, 0);
            }

            PedidoEstoque pedidoEstoque = PedidoEstoque.Criar(0, (TipoPedido)dto.Tipo, (OrigemPedidoEstoque)dto.Origem, dto.IdUsuario, dto.IdFilial, venda);

            foreach (CriarItemPedidoDto itemDto in dto.Itens)
            {
                pedidoEstoque.AdicionarItem(ItemPedido.Criar(0, 0, itemDto.IdProduto, itemDto.ValorUnitario, itemDto.Quantidade, (StatusItemPedido)itemDto.Status));
            }

            PedidoEstoqueRepositorio pedidoEstoqueRepositorio = new PedidoEstoqueRepositorio();
            if (pedidoEstoqueRepositorio.Gravar(ref pedidoEstoque))
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, pedidoEstoque.Id);
            }
            return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, "Falha ao gravar");
        }

        [HttpGet]
        [Route("pedidos/{id}")]
        public HttpResponseMessage Obter(int id)
        {
            PedidoEstoqueRepositorio pedidoEstoqueRepositorio = new PedidoEstoqueRepositorio();
            PedidoEstoque pedidoEstoque = pedidoEstoqueRepositorio.Obter(id);

            if (pedidoEstoque == null)
                return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, "Pedido não encontrado");

            PedidoEstoqueDto dto = new PedidoEstoqueDto()
            {
                Id= pedidoEstoque.Id,
                Tipo = (int)pedidoEstoque.Tipo,
                IdFilial = pedidoEstoque.IdFilial,
                IdUsuario = pedidoEstoque.IdUsuario,
                Origem = (int)pedidoEstoque.Origem
            };

            foreach (ItemPedido item in pedidoEstoque.Itens)
            {
                dto.Itens.Add(new ItemPedidoDto
                {
                    Id = item.Id,
                    IdPedido = item.IdPedido,
                    IdProduto = item.IdProduto,
                    Quantidade = item.Quantidade,
                    ValorUnitario = item.ValorUnitario,
                    Status = (int)item.Status
                });
            }

            if (pedidoEstoque.Venda != null)
            {
                dto.Venda = new VendaDto
                {
                    Id= pedidoEstoque.Venda.Id,
                    IdPedidoEstoque = pedidoEstoque.Id,
                    IdCliente = pedidoEstoque.Venda.IdCliente,
                    Observacao = pedidoEstoque.Venda.Observacao,
                    FormaDePagamento = (int)pedidoEstoque.Venda.FormaDePagamento
                };
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, dto);
        }
    }
}