using Dapper;
using Negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Util;

namespace Persistencia
{
    class VendaRepositorio
    {
        private static string _nomeTabela = "dbo.Venda";
        private static string _id = "Id";
        private static string _insertCampos = "Cliente, Observacao, FormaDePagamento, PedidoEstoque";
        private static string _insertValues = "@" + _insertCampos.Replace(", ", ", @");
        private static string _todosCampos = _id + ", " + _insertCampos;
        private static string _updateCampos = "";

        public SqlConnection _conexaoBD;
        public SqlTransaction _transacaoDB;

        public VendaRepositorio()
        {
            _updateCampos = UDB.MontarUpdateCampos(_insertCampos);
        }

        public Venda ObterPorPedido(int idPedidoEstoque)
        {
            string sql = $"SELECT Id \n " +
                         $"      ,Cliente AS IdCliente \n " +
                         $"      ,Observacao \n " +
                         $"      ,FormaDePagamento \n " +
                         $"      ,PedidoEstoque AS IdPedidoEstoque \n " +
                         $"FROM {_nomeTabela} \n " +
                         $"WHERE PedidoEstoque = @IdPedidoEstoque";

            using (var conexao = Acesso.NovaConexao())
            {
                return conexao.Query<Venda>(sql, new { IdPedidoEstoque = idPedidoEstoque }).SingleOrDefault();
            }
        }

        public bool Gravar(Venda venda)
        {
            string sql;

            if (venda.Id == 0)
            {
                sql = $"INSERT INTO {_nomeTabela} ({_insertCampos}) \n " +
                      $"VALUES ({_insertValues}); \n " +
                      "SELECT SCOPE_IDENTITY() AS Id";
            }
            else
            {
                sql = $"UPDATE {_nomeTabela} SET {_updateCampos} \n " +
                      $"WHERE {_id}=@{_id}; \n " +
                      "SELECT -1 AS Id";
            }

            var id = _conexaoBD.Query<int>(sql, new
            {
                Id = venda.Id,
                Cliente = venda.IdCliente,
                Observacao = venda.Observacao,
                FormaDePagamento = venda.FormaDePagamento,
                PedidoEstoque = venda.IdPedidoEstoque
            }, _transacaoDB).SingleOrDefault();

            if (id > 0)
            {
                venda.AtualizarIdVenda(id);
            }

            return ((id == -1) || id > 0);
           

        }

    }
}
