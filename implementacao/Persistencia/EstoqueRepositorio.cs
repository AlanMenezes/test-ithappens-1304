using Dapper;
using Negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Persistencia
{
    public class EstoqueRepositorio
    {
        private static string _nomeTabela = "dbo.Estoque";
        private static string _id = "IdProduto";
        private static string _insertCampos = "IdProduto, Quantidade, CodFilial";
        private static string _insertValues = "@" + _insertCampos.Replace(", ", ", @");
        private static string _todosCampos = _insertCampos;

        public SqlConnection _conexaoBD;
        public SqlTransaction _transacaoDB;

        public EstoqueRepositorio()
        {

        }

        public virtual bool Existe(int idProduto, int codigoFilial )
        {
            string sql = $"SELECT 1 FROM {_nomeTabela} WHERE {_id} = @IdProduto AND CodFilial = @CodFilial";

            using (var conexao = Acesso.NovaConexao())
            {
                return conexao.Query(sql, new { IdProduto = idProduto, CodFilial = codigoFilial }).Count() > 0;
            }
        }

        public IEnumerable<Estoque> ObterTodos()
        {
            string sql = $"SELECT {_todosCampos} \n " +
                         $"FROM {_nomeTabela}";

            using (var conexao = Acesso.NovaConexao())
            {
                return conexao.Query<Estoque>(sql).ToList();
            }
        }

        public decimal ObterSaldo(int idFilial, int idProduto)
        {
            string sql = $"SELECT Quantidade \n " +
                         $"FROM {_nomeTabela} \n " +
                         $"WHERE IdProduto = @IdProduto \n " +
                         $"AND CodFilial=@CodFilial";

            using (var conexao = Acesso.NovaConexao())
            {
                decimal Quantidade = conexao.Query<decimal>(sql, new { IdProduto = idProduto, CodFilial = idFilial }).SingleOrDefault();
                return Quantidade;
            }
        }

        public bool AtualizarEstoque(ref PedidoEstoque pedidoEstoque)
        {
            string sql = "";

            foreach (ItemPedido item in pedidoEstoque.Itens)
            {
                if (item.Status == StatusItemPedido.Cancelado)
                {
                    continue;
                }

                if (pedidoEstoque.Tipo == TipoPedido.Saida)
                {
                    decimal saldo = ObterSaldo(pedidoEstoque.IdFilial, item.IdProduto);

                    if (saldo < item.Quantidade)
                        throw new InvalidOperationException(String.Format("Produto {0} não tem quantidade suficiente em estoque. Quantidade dispinível: {1}", item.IdProduto, saldo));

                    sql = $"UPDATE {_nomeTabela} \n " +
                          $"SET Quantidade = Quantidade - @Quantidade \n " +
                          $"WHERE IdProduto = @IdProduto \n " +
                          $"AND CodFilial = @CodFilial";
          
                }
                else
                {

                    if (!Existe(item.IdProduto, pedidoEstoque.IdFilial))
                    {
                        sql = $"INSERT INTO {_nomeTabela} \n " +
                              $"({_insertCampos})  \n " +
                              $"VALUES \n " +
                              $"({_insertValues})";
                    }
                    else
                    {
                        sql = $"UPDATE {_nomeTabela} \n " +
                              $"SET Quantidade = Quantidade + @Quantidade \n " +
                              $"WHERE IdProduto = @IdProduto \n " +
                              $"AND CodFilial = @CodFilial";
                    }
                }
                //"IdProduto, Quantidade, CodFilial";
                _conexaoBD.Execute(sql, new
                {
                    IdProduto = item.IdProduto,
                    CodFilial = pedidoEstoque.IdFilial,
                    Quantidade = item.Quantidade
                }, _transacaoDB);
            }

            return true;
        }

        public Estoque Obter(int codigo)
        {
            string sql = $"SELECT {_todosCampos} \n " +
                         $"FROM {_nomeTabela} \n " +
                         $"WHERE {_id}=@Codigo";

            using (var conexao = Acesso.NovaConexao())
            {
                return conexao.Query<Estoque>(sql, new { Codigo = codigo }).SingleOrDefault();
            }
        }
    }
}
