using Dapper;
using Negocio;
using System.Collections.Generic;
using System.Linq;
using Util;

namespace Persistencia
{
    public class FilialRepositorio
    {
        private static string _nomeTabela = "dbo.Filial";
        private static string _id = "Codigo";
        private static string _insertCampos = "Nome";
        private static string _insertValues = "@" + _insertCampos.Replace(", ", ", @");
        private static string _updateCampos = "";
        private static string _todosCampos = _id + ", " + _insertCampos;

        public FilialRepositorio()
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

        public IEnumerable<Filial> ObterTodos()
        {
            string sql = $"SELECT {_todosCampos} \n " +
                         $"FROM {_nomeTabela}";

            using (var conexao = Acesso.NovaConexao())
            {
                return conexao.Query<Filial>(sql).ToList();
            }
        }

        public IEnumerable<Filial> ObterPorNomeOuParteDoNome(string nomeOuParteDoNome)
        {
            string sql = $"SELECT {_todosCampos} \n " +
                         $"FROM {_nomeTabela} \n " +
                         $"WHERE Nome LIKE '%{nomeOuParteDoNome}%'";

            using (var conexao = Acesso.NovaConexao())
            {
                return conexao.Query<Filial>(sql).ToList();
            }
        }

        public Filial Obter(int codigo)
        {
            string sql = $"SELECT {_todosCampos} \n " +
                         $"FROM {_nomeTabela} \n " +
                         $"WHERE {_id}=@Codigo";

            using (var conexao = Acesso.NovaConexao())
            {
                return conexao.Query<Filial>(sql, new { Codigo = codigo }).SingleOrDefault();
            }
        }

        public bool Gravar(ref Filial item)
        {
            string sql = "";
            if (!Existe(item.Codigo))
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
                    item.Codigo,
                    item.Nome
                }).SingleOrDefault();

                if (id > 0)
                {
                    item.AtribuiCodigo(id);
                }

                return ((id == -1) || id > 0);
            }
        }
    }
}
