using System;

namespace Negocio
{
    public class ItemPedido
    {
        public int Id { get; private set; }
        public int IdPedido { get; private set; }
        public int IdProduto { get; private set; }   
        public decimal ValorUnitario { get; private set; }
        public decimal Quantidade { get; private set; }
        public decimal ValorTotal => Quantidade * ValorUnitario;
        public StatusItemPedido Status { get; private set; }

        public ItemPedido() { }

        private ItemPedido(int id, int idPedido, int idProduto, decimal valorUnitario, decimal quantidade, StatusItemPedido status)
        {
            Id = id;
            IdPedido = idPedido;
            IdProduto = idProduto;
            ValorUnitario = valorUnitario;
            Quantidade = quantidade;
            Status = status;
        }
        public static ItemPedido Criar(int id, int idPedido, int idProduto, decimal valorUnitario, decimal quantidade, StatusItemPedido status)
        {
            ValidaItemPedido(idProduto, quantidade);
            return new ItemPedido(id, idPedido, idProduto, valorUnitario, quantidade, status);
        }

        public void AlterarQuantidade(decimal quantidade)
        {
            ValidaQuantidade(quantidade);
            Quantidade = quantidade;
        }

        public void AlterarStatus(StatusItemPedido statusItemPedido)
        {
            Status = statusItemPedido;
        }

        public void AtualizarIdItemPedido(int idItemPedido)
        {
            ValidaIdItemPedido(idItemPedido);
            Id= idItemPedido;
        }

        public void AtualizarIdPedido(int idPedido)
        {
            ValidaIdPedido(idPedido);
            IdPedido = idPedido;
        }
        private static void ValidaItemPedido(int idProduto, decimal quantidade)
        {
            ValidaIdProduto(idProduto);
            ValidaQuantidade(quantidade);
        }

        private static void ValidaIdProduto(int idProduto)
        {
            if (idProduto <= 0)
                throw new InvalidOperationException("Código identificador do produto deve ser maior que zero");
        }

        private static void ValidaQuantidade(decimal quantidade)
        {
            if (quantidade <= 0)
                throw new InvalidOperationException("Quantidade do produto deve ser maior que zero");
        }

        private static void ValidaIdItemPedido(int idItemPedido)
        {
            if (idItemPedido <= 0)
                throw new InvalidOperationException("Código identificador do item do pedido deve ser maior que zero");
        }
        private static void ValidaIdPedido(int idPedido)
        {
            if (idPedido <= 0)
                throw new InvalidOperationException("Código identificador do pedido deve ser maior que zero");
        }
    }
}
