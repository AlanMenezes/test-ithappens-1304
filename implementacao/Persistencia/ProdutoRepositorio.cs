using Dapper;
using Negocio;
using System.Collections.Generic;
using System.Linq;
using Util;

namespace Persistencia
{
    public class ProdutoRepositorio
    {
        private static string _nomeTabela = "dbo.Produto";
        private static string _id = "Id";
        private static string _insertCampos = "Descricao, CodBarra, PrecoCusto, PrecoVenda";
        private static string _insertValues = "@" + _insertCampos.Replace(", ", ", @");
        private static string _updateCampos = "";
        private static string _todosCampos = _id + ", " + _insertCampos;

        public ProdutoRepositorio()
        {
            _updateCampos = UDB.MontarUpdateCampos(_insertCampos);
        }

        public virtual bool Existe(int codigo)
        {
            string sql = $"SELECT 1 FROM {_nomeTabela} WHERE {_id} = @Codigo";

            using (var conexao = Acesso.NovaConexao())
            {
                return conexao.Query(sql, new { Codigo = codigo }).Count() > 0;
            }
        }

        public IEnumerable<Produto> ObterTodos()
        {
            string sql = $"SELECT {_todosCampos} \n " +
                         $"FROM {_nomeTabela}";

            using (var conexao = Acesso.NovaConexao())
            {
                return conexao.Query<Produto>(sql).ToList();
            }
        }

        public IEnumerable<Produto> ObterPorDescricaoOuParteDaDescricao(string descricaoOuParteDaDescricao)
        {
            string sql = $"SELECT {_todosCampos} \n " +
                         $"FROM {_nomeTabela} \n " +
                         $"WHERE Descricao LIKE '%{descricaoOuParteDaDescricao}%'";

            using (var conexao = Acesso.NovaConexao())
            {
                return conexao.Query<Produto>(sql).ToList();
            }
        }

        public Produto Obter(int codigo)
        {
            string sql = $"SELECT {_todosCampos} \n " +
                         $"FROM {_nomeTabela} \n " +
                         $"WHERE {_id}=@Codigo";

            using (var conexao = Acesso.NovaConexao())
            {
                return conexao.Query<Produto>(sql, new { Codigo = codigo }).SingleOrDefault();
            }
        }

        public Produto ObterPorCodigoDeBarras(string codigoDeBarras)
        {
            string sql = $"SELECT {_todosCampos} \n " +
                         $"FROM {_nomeTabela} \n " +
                         $"WHERE CodBarra=@CodigoDeBarras";

            using (var conexao = Acesso.NovaConexao())
            {
                return conexao.Query<Produto>(sql, new { CodigoDeBarras = codigoDeBarras }).SingleOrDefault();
            }
        }

        public bool Gravar(ref Produto item)
        {
            string sql = "";
            if (!Existe(item.Id))
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

            using (var conexao = Acesso.NovaConexao())
            {
                var id = conexao.Query<int>(sql, new
                {
                    item.Id,
                    item.Descricao,
                    item.CodBarra,
                    item.PrecoCusto,
                    item.PrecoVenda,
                }).SingleOrDefault();

                if (id > 0)
                {
                    item.AtribuiId(id);
                }

                return ((id == -1) || id > 0);
            }
        }
    }
}
