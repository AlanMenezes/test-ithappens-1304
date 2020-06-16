using Dapper;
using Negocio;
using System.Collections.Generic;
using System.Linq;
using Util;

namespace Persistencia
{
    public class UsuarioRepositorio
    {
        private static string _nomeTabela = "dbo.Usuario";
        private static string _id = "Id";
        private static string _insertCampos = "Nome, Login, Senha";
        private static string _insertValues = "@" + _insertCampos.Replace(", ", ", @");
        private static string _updateCampos = "";
        private static string _todosCampos = _id + ", " + _insertCampos;

        public UsuarioRepositorio()
        {
            _updateCampos = UDB.MontarUpdateCampos(_insertCampos);
        }

        public virtual bool Existe(int codigo)
        {
            string sql = $"select 1 from {_nomeTabela} where {_id} = @Codigo";

            using (var conexao = Acesso.NovaConexao())
            {
                return conexao.Query(sql, new { Codigo = codigo }).Count() > 0;
            }
        }

        public IEnumerable<Usuario> ObterTodos()
        {
            string sql = $"SELECT {_todosCampos} \n " +
                         $"FROM {_nomeTabela}";

            using (var conexao = Acesso.NovaConexao())
            {
                return conexao.Query<Usuario>(sql).ToList();
            }
        }

        public IEnumerable<Usuario> ObterPorNomeOuParteDoNome(string nomeOuParteDoNome)
        {
            string sql = $"SELECT {_todosCampos} \n " +
                         $"FROM {_nomeTabela} \n " +
                         $"WHERE Nome LIKE '%{nomeOuParteDoNome}%'";

            using (var conexao = Acesso.NovaConexao())
            {
                return conexao.Query<Usuario>(sql).ToList();
            }
        }

        public Usuario Obter(int codigo)
        {
            string sql = $"SELECT {_todosCampos} \n " +
                         $"FROM {_nomeTabela} \n " +
                         $"WHERE {_id}=@Codigo";

            using (var conexao = Acesso.NovaConexao())
            {
                return conexao.Query<Usuario>(sql, new { Codigo = codigo }).SingleOrDefault();
            }
        }

        public bool Gravar(ref Usuario item)
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
                    item.Nome,
                    item.Login,
                    item.Senha
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
