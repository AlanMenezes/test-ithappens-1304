using System;

namespace Negocio
{
    public class Estoque
    {
        public int IdProduto{ get; private set; }
        public int Quantidade { get; private set; }
        public int CodFilial { get; private set; }

        public Estoque() { }

        private Estoque(int idProduto, int quantidade, int codFilial)
        {            
            IdProduto = idProduto;
            Quantidade = quantidade;
            CodFilial = codFilial;
        }

        public static Estoque Criar(int idProduto, int quantidade, int codFilial)
        {
            ValidaEstoque(idProduto, quantidade, codFilial);
            return new Estoque(idProduto, quantidade, codFilial);
        }

        public void AlteraIdProduto(int novoIdProduto)
        {
            ValidaCodigoDoPruduto(novoIdProduto);
            IdProduto = novoIdProduto;
        }

        public void AlteraQuantidade(int novaQuantidade)
        {
            ValidaQuantidade(novaQuantidade);
            Quantidade = novaQuantidade;
        }

        public void AlteraFilial(int codNovoFilial)
        {
            ValidaCodigoFilial(codNovoFilial);
            CodFilial = codNovoFilial;
        }

        private static void ValidaEstoque(int idProduto, int quantidade, int codFilial)
        {
            ValidaCodigoDoPruduto(idProduto);
            ValidaQuantidade(quantidade);
            ValidaCodigoFilial(codFilial);
        }

        private static void ValidaCodigoDoPruduto(int codigoProduto)
        {
            if (codigoProduto <= 0)
                throw new InvalidOperationException("Código identificador do produto deve ser maior que zero");
        }

        private static void ValidaQuantidade(int quantidade)
        {
            if (quantidade <= 0)
                throw new InvalidOperationException("Quantidade do produto deve ser maior que zero");
        }

        private static void ValidaCodigoFilial(int codigoFilial)
        {
            if (codigoFilial <= 0)
                throw new InvalidOperationException("Código identificador da filial deve ser maior que zero");
        }

    }
}
