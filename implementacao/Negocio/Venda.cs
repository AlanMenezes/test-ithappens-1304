using System;
namespace Negocio
{
    public class Venda
    {
        public int Id { get; private set; }
        public int IdCliente { get; private set; }
        public string Observacao { get; private set; }
        public FormaDePagamento FormaDePagamento { get; private set; }
        public int IdPedidoEstoque { get; private set; }

        public Venda() { }

        private Venda(int id, int idCliente, string observacao, FormaDePagamento formaDePagamento, int idPedidoEstoque)
        {
            Id = id;
            IdCliente = idCliente;
            Observacao = observacao;
            FormaDePagamento = formaDePagamento;
            IdPedidoEstoque = idPedidoEstoque;
        }

        public static Venda Criar(int id, int idCliente, string observacao, FormaDePagamento formaDePagamento, int idPedidoEstoque)
        {
            ValidaVenda(idCliente, observacao);
            return new Venda(id, idCliente, observacao, formaDePagamento, idPedidoEstoque);
        }

        public void AlterarObservacao(string novaObservacao)
        {
            ValidaObservacao(novaObservacao);
            Observacao = novaObservacao;
        }

        public void AlterarFormaPagamento(FormaDePagamento formaDePagamento)
        {
            FormaDePagamento = formaDePagamento;
        }

        public void AtualizarIdVenda(int idVenda)
        {
            ValidaIdVenda(idVenda);
            Id = idVenda;
        }

        public void AtualizarIdPedido(int idPedidoEstoque)
        {
            ValidaIdPedido(idPedidoEstoque);
            IdPedidoEstoque = idPedidoEstoque;
        }

        private static void ValidaVenda(int idCliente, string observacao)
        {
            ValidaIdCliente(idCliente);
            ValidaObservacao(observacao);
        }

        private static void ValidaIdVenda(int idVenda)
        {
            if (idVenda <= 0)
                throw new InvalidOperationException("Código identificador da venda deve ser maior que zero");
        }

        private static void ValidaIdPedido(int idPedidoEstoque)
        {
            if (idPedidoEstoque <= 0)
                throw new InvalidOperationException("Código identificador do pedido de estoque deve ser maior que zero");
        }

        private static void ValidaIdCliente(int idCliente)
        {
            if (idCliente <= 0)
                throw new InvalidOperationException("Código identificador do cliente deve ser maior que zero");
        }

        private static void ValidaObservacao(string observacao)
        {
            if (observacao.Length > 100)
                throw new InvalidOperationException("Observacao da venda deve possuir no máximo 100 caracteres");
        }

    }
}
