using System;
using System.Collections.Generic;
using System.Linq;

namespace Negocio
{
    public class PedidoEstoque
    {
        public int Id { get; private set; }
        public TipoPedido Tipo{ get; private set; }
        public OrigemPedidoEstoque Origem { get; private set; }
        public int IdUsuario { get; private set; }
        public int IdFilial { get; private set; } 
        public List<ItemPedido> Itens { get; private set; }
        public int QuantidadeDeItens => Itens.Count();
        public decimal Valor => Itens.Where(item => item.Status != StatusItemPedido.Cancelado).Sum(item => item.ValorTotal);
        public Venda Venda { get; private set; }

        public PedidoEstoque()
        {
            Itens = new List<ItemPedido>();
        }

        private PedidoEstoque(int id, TipoPedido tipo, OrigemPedidoEstoque origem, int idUsuario, int idFilial, Venda venda = null)
        {
            Id = id;
            Tipo = tipo;
            Origem = origem;
            IdUsuario = idUsuario;
            IdFilial = idFilial;
            Itens = new List<ItemPedido>();
            Venda = venda;
        }

        public static PedidoEstoque Criar(int id, TipoPedido tipo, OrigemPedidoEstoque origem, int idUsuario, int idFilial, Venda venda = null)
        {
            ValidaPedido(tipo, origem, idUsuario, idFilial);
            return new PedidoEstoque(id, tipo, origem, idUsuario, idFilial, venda);
        }

        public void AdicionarItem(ItemPedido itemPedido)
        {
            ValidaAdicaoDeItemNoPedido(itemPedido);

            ItemPedido itemExistente = null;
            if (Itens.Count() > 0)
            {
                itemExistente = Itens.First(p => p.IdProduto == itemPedido.IdProduto);
            }

            if (itemExistente != null)
            {
                itemExistente.AlterarQuantidade(itemExistente.Quantidade + itemPedido.Quantidade);
                return;
            }

            Itens.Add(itemPedido);
        }

        public void AdicionarItens(List<ItemPedido> itens)
        {
            foreach (ItemPedido itemPedido in itens)
            {
                AdicionarItem(itemPedido);
            }
        }

        public void AdicionarVenda(Venda venda)
        {
            Venda = venda;
        }

        public void AtualizarIdPedido(int idPedido)
        {
            ValidaIdPedido(idPedido);

            Id = idPedido;

            foreach (ItemPedido item in Itens)
            {
                item.AtualizarIdPedido(idPedido);
            }

            if (Venda != null)
            {
                Venda.AtualizarIdPedido(idPedido);

            }
        }

        private static void ValidaPedido(TipoPedido tipo, OrigemPedidoEstoque origem, int idUsuario, int idFilial)      
        {
            ValidaOregemPedido(tipo, origem);
            ValidaIdUsuario(idUsuario);
            ValidaIdFilial(idFilial);
        }

        private static void ValidaOregemPedido(TipoPedido tipo, OrigemPedidoEstoque origem)
        {
            if (tipo == TipoPedido.Entrada && origem == OrigemPedidoEstoque.Venda)
                throw new InvalidOperationException("Um pedido de estoque do tipo entrada não pode ter uma venda como origem");

            if (tipo == TipoPedido.Saida && origem == OrigemPedidoEstoque.Compra)
                throw new InvalidOperationException("Um pedido de estoque do tipo saida não pode ter uma compra como origem");
        }

        private static void ValidaIdPedido(int idPedido)
        {
            if (idPedido <= 0)
                throw new InvalidOperationException("Código identificador do pedido de estoque deve ser maior que zero");
        }

        private static void ValidaIdUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
                throw new InvalidOperationException("Código identificador do usuário deve ser maior que zero");
        }

        private static void ValidaIdFilial(int idFilial)
        {
            if (idFilial <= 0)
                throw new InvalidOperationException("Código identificador da filial deve ser maior que zero");
        }

        private static void ValidaAdicaoDeItemNoPedido(ItemPedido item)
        {
            if (item == null)
                throw new InvalidOperationException("Item do pedido de estoque deve estar com suas informações preenchidas para ser adicionado ao pedido");
        }


    }
}
