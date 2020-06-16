using Dapper;
using Negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Util;

namespace Persistencia
{
    public class PedidoEstoqueRepositorio
    {
        private static string _nomeTabela = "dbo.PedidoEstoque";
        private static string _id = "Id";
        private static string _insertCampos = "Tipo, Origem, Usuario, Filial, QuantidadeDeItens, Valor";
        private static string _insertValues = "@" + _insertCampos.Replace(", ", ", @");
        private static string _todosCampos = _id + ", " + _insertCampos;
        private static string _updateCampos = "";

        public SqlConnection _conexaoBD;
        public SqlTransaction _transacaoDB;

        public PedidoEstoqueRepositorio()
        {
            _updateCampos = UDB.MontarUpdateCampos(_insertCampos);
        }

        public List<PedidoEstoque> ObterTodos()
        {
            
            List<PedidoEstoque> listaDePedido = new List<PedidoEstoque>();
            ItemPedidoRepositorio itemPedidoRepositorio = new ItemPedidoRepositorio();
            VendaRepositorio vendaRepositorio = new VendaRepositorio();

            string sql = $"SELECT {_todosCampos} \n " +
                         $"FROM {_nomeTabela} \n " +
                        // $"WHERE {_id} =@{_id} \n " +
                         $"ORDER BY id DESC";

            using (var conexao = Acesso.NovaConexao())
            {
                listaDePedido = conexao.Query<PedidoEstoque>(sql).ToList();
            }

            if (listaDePedido.Count() > 0)
            {
                foreach (PedidoEstoque pedido in listaDePedido)
                {
                    pedido.AdicionarItens(itemPedidoRepositorio.ObterPorPedido(pedido.Id).ToList());

                    if (pedido.Origem == OrigemPedidoEstoque.Venda)
                    {
                        pedido.AdicionarVenda(vendaRepositorio.ObterPorPedido(pedido.Id));
                    }
                }
            }

            return listaDePedido;
        }

        public PedidoEstoque Obter(int IdPedido)
        {
            PedidoEstoque pedidoEstoque;
            ItemPedidoRepositorio itemPedidoRepositorio = new ItemPedidoRepositorio();
            VendaRepositorio vendaRepositorio = new VendaRepositorio();
            

            string sql = $"SELECT Id \n " +
                         $"      ,Tipo \n " +
                         $"      ,Origem \n " +
                         $"      ,Usuario AS IdUsuario \n " +
                         $"      ,Filial AS IdFilial \n " +
                         $"      ,Origem\n " +
                         $"FROM {_nomeTabela} \n " +
                         $"WHERE { _id}=@{ _id} ";

            using (var conexao = Acesso.NovaConexao())
            {
                pedidoEstoque = conexao.Query<PedidoEstoque>(sql, new { Id = IdPedido }).Single();

                pedidoEstoque.AdicionarItens(itemPedidoRepositorio.ObterPorPedido(pedidoEstoque.Id).ToList());

                if (pedidoEstoque.Origem == OrigemPedidoEstoque.Venda)
                {
                    pedidoEstoque.AdicionarVenda(vendaRepositorio.ObterPorPedido(pedidoEstoque.Id));
                }
            }

            return pedidoEstoque;
        }

        public bool Gravar(ref PedidoEstoque pedidoEstoque)
        {
            if (pedidoEstoque.Itens == null || pedidoEstoque.Itens.Count() == 0)
                throw new InvalidOperationException("O pedido não possui itens");

            if (pedidoEstoque.Origem == OrigemPedidoEstoque.Venda && pedidoEstoque.Venda == null)
                throw new InvalidOperationException("Venda inválida");

            string sql;
            int id;
            ItemPedidoRepositorio itemPedidoRepositorio = new ItemPedidoRepositorio();
            EstoqueRepositorio estoqueRepositorio = new EstoqueRepositorio();

            _conexaoBD = Acesso.NovaConexao();
            _transacaoDB = _conexaoBD.BeginTransaction();

            try
            {
                if (pedidoEstoque.Id== 0)
                {
                    sql = $"INSERT INTO {_nomeTabela} ({_insertCampos}) \n " +
                          $"VALUES ({_insertValues}); \n " +
                          "SELECT SCOPE_IDENTITY() AS Id";
                }
                else
                {
                    sql = $"UPDATE {_nomeTabela} SET {_updateCampos} \n " +
                          $"WHERE { _id}=@{ _id} ";

                }
                //"Tipo, Origem, Usuario, Filial, QuantidadeDeItens, Valor";

                id = _conexaoBD.Query<int>(sql, new
                {
                    Id = pedidoEstoque.Id,
                    Tipo = pedidoEstoque.Tipo,
                    Origem = pedidoEstoque.Origem,
                    Usuario = pedidoEstoque.IdUsuario,
                    Filial = pedidoEstoque.IdFilial,
                    QuantidadeDeItens = pedidoEstoque.QuantidadeDeItens,
                    Valor = pedidoEstoque.Valor
                }, _transacaoDB).SingleOrDefault();

                if (id > 0)
                {
                    pedidoEstoque.AtualizarIdPedido(id);
                }

                itemPedidoRepositorio._conexaoBD = _conexaoBD;
                itemPedidoRepositorio._transacaoDB = _transacaoDB;
                itemPedidoRepositorio.Gravar(ref pedidoEstoque);

                estoqueRepositorio._conexaoBD = _conexaoBD;
                estoqueRepositorio._transacaoDB = _transacaoDB;
                estoqueRepositorio.AtualizarEstoque(ref pedidoEstoque);

                if (pedidoEstoque.Origem == OrigemPedidoEstoque.Venda)
                {
                    VendaRepositorio vendaRepositorio = new VendaRepositorio();
                    vendaRepositorio._conexaoBD = _conexaoBD;
                    vendaRepositorio._transacaoDB = _transacaoDB;
                    vendaRepositorio.Gravar(pedidoEstoque.Venda);
                }

                _transacaoDB.Commit();
            }
            catch (Exception excessao)
            {
                _transacaoDB.Rollback();
                throw new InvalidOperationException("Falha ao gravar: \n" + Environment.NewLine + excessao.Message);
            }

            return ((id == -1) || id > 0);
        }



    }
}
