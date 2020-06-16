using System;

namespace Negocio
{
    public class Produto
    {
        public int Id { get; private set; }
        public string Descricao { get; private set; }
        public string CodBarra { get; private set; }
        public decimal PrecoCusto { get; private set; }
        public decimal PrecoVenda { get; private set; }


        public Produto() { }

        private Produto(int id, string descricao, string codBarra, decimal precoCusto, decimal precoVenda)
        {            
            Id = id;
            Descricao = descricao;
            CodBarra = codBarra;
            PrecoCusto = precoCusto;
            PrecoVenda = precoVenda;
        }

        public static Produto Criar(int id, string descricao, string codBarra, decimal precoCusto, decimal precoVenda)
        {
            ValidaProduto(id, descricao, codBarra, precoCusto, precoVenda);
            return new Produto(id, descricao, codBarra, precoCusto, precoVenda);
        }

        public void AtribuiId(int id)
        {
            ValidaIdProduto(id);
            Id = id;
        }

        public void AlteraDescricao(string novaDescricao)
        {
            ValidaDescricaoProduto(novaDescricao);
            Descricao = novaDescricao;
        }

        public void AlteraCodBarra(string novoCodBarra)
        {
            ValidaCodBarra(novoCodBarra);
            CodBarra = novoCodBarra;
        }

        public void AlteraPrecoCusto(decimal precoCusto)
        {
            ValidaPrecoCusto(precoCusto);
            PrecoCusto = precoCusto;
        }

        public void AlteraPrecoVenda(decimal precoVenda)
        {
            ValidaPrecoVenda(precoVenda, PrecoVenda);
            PrecoVenda = precoVenda;
        }

        private static void ValidaProduto(int id, string descricao, string codBarra, decimal precoCusto, decimal precoVenda)
        {

            ValidaDescricaoProduto(descricao);
            ValidaCodBarra(codBarra);
            ValidaPrecoCusto(precoCusto);
            ValidaPrecoVenda(precoVenda, precoCusto); 

        }

        private static void ValidaIdProduto(int id)
        {
            if (id <= 0)
                throw new InvalidOperationException("Código identificador do produto deve ser maior que zero");
        }

        private static void ValidaDescricaoProduto(string descricao)
        {

            if (String.IsNullOrWhiteSpace(descricao))
                throw new InvalidOperationException("Descrição do produto é obrigatório");

            if (descricao.Length > 60)
                throw new InvalidOperationException("Descriçao do produto deve possuir no máximo 60 caracteres");
        }

        private static void ValidaCodBarra(string codBarra)
        {

            if (String.IsNullOrWhiteSpace(codBarra))
                throw new InvalidOperationException("Código de barras do produto é obrigatório");

            if (codBarra.Length > 30)
                throw new InvalidOperationException("Código de barras do produto deve possuir no máximo 30 caracteres");
        }

        private static void ValidaPrecoCusto(decimal precoCusto)
        {
            if (precoCusto <= 0.00M)
                throw new InvalidOperationException("Preço de custo do produto deve ser maior que zero");
        }

        private static void ValidaPrecoVenda(decimal precoVenda, decimal precoCusto)
        {
            if (precoVenda <= 0.00M)
                throw new InvalidOperationException("Preço de venda do produto deve ser maior que zero");

            if (precoVenda < precoCusto)
                throw new InvalidOperationException("Preço de venda do produto não pode ser menor que o preço de custo");
        }
    }
}
