using Dapper;
using Negocio;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Util;

namespace Persistencia
{
    public class ItemPedidoRepositorio
    {
        private static string _nomeTabela = "dbo.ItemPedido";
        private static string _id = "Id";
        private static string _insertCampos = "Pedido, Produto, ValorUnitario, Quantidade, ValorTotal, Status";
        private static string _insertValues = "@" + _insertCampos.Replace(", ", ", @");
        private static string _todosCampos = _id + ", " + _insertCampos;
        private static string _updateCampos = "";

        public SqlConnection _conexaoBD;
        public SqlTransaction _transacaoDB;

        public ItemPedidoRepositorio()
        {
            _updateCampos = UDB.MontarUpdateCampos(_insertCampos);
        }

        public virtual bool Existe(int idPedidoEstoque, int idProduto)
        {
            string sql = $"SELECT 1 FROM {_nomeTabela} WHERE Pedido = @IdPedido AND Produto = @IdProduto";

            using (var conexao = Acesso.NovaConexao())
            {
                return conexao.Query(sql, new { IdPedido = idPedidoEstoque, IdProduto = idProduto }).Count() > 0;
            }
        }

        public IEnumerable<ItemPedido> ObterPorPedido(int idPedidoEstoque)
        {
            string sql = $"SELECT Id \n " +
              $"      ,Pedido AS IdPedido\n " +
              $"      ,Produto AS IdProduto \n " +
              $"      ,ValorUnitario\n " +
              $"      ,Quantidade \n " +
              $"      ,ValorTotal\n " +
              $"      ,Status\n " +
              $"FROM {_nomeTabela} \n " +
              $"WHERE Pedido = @IdPedidoEstoque";

            using (var conexao = Acesso.NovaConexao())
            {
                return conexao.Query<ItemPedido>(sql, new { IdPedidoEstoque = idPedidoEstoque }).ToList();
            }

        }

        public bool Gravar(ref PedidoEstoque pedidoEstoque)
        {
            string sql="";

            foreach (ItemPedido item in pedidoEstoque.Itens)
            {
                if (item.Status == StatusItemPedido.Cancelado)
                {
                    continue;
                }

                if (!Existe(pedidoEstoque.Id, item.IdProduto))
                {
                    sql = $"INSERT INTO {_nomeTabela} ({_insertCampos}) \n " +
                          $"VALUES ({_insertValues}); \n " +
                          "SELECT SCOPE_IDENTITY() AS Id";
                }
                else
                {
                    sql = $"UPDATE {_nomeTabela} SET {_updateCampos} \n " +
                          $"WHERE Pedido = @IdPedido \n " +
                          $"AND Produto = @IdProduto \n " +
                          "SELECT -1 AS Id";
                }

                //"Pedido, Produto, ValorUnitario, Quantidade, ValorTotal, Status";
                int id = _conexaoBD.Execute(sql, new
                {
                    Pedido = pedidoEstoque.Id,
                    Produto = item.IdProduto,
                    ValorUnitario = item.ValorUnitario,
                    Quantidade = item.Quantidade,
                    ValorTotal = item.ValorTotal,
                    Status = StatusItemPedido.Processado
                }, _transacaoDB);

                if (id > 0)
                {
                    item.AtualizarIdItemPedido(id);
                }

                item.AlterarStatus(StatusItemPedido.Processado);
            }

            return true;
        }
    }

}
