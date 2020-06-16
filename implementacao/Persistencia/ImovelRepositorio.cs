using Dapper;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Persistencia
{
    public class ImovelRepositorio
    {
        private static string NomeTabela = "dbo.Imovel";
        private static string Id_0 = "IdImovel";
        private static string InsertCampos = "IdCondominio, IdEdificacao, Tipo, Nome, Ramal";
        private static string InsertValues = "@" + InsertCampos.Replace(", ", ", @");
        private static string UpdateCampos = "";
        private static string TodosCampos = Id_0 + ", " + InsertCampos;

        public ImovelRepositorio()
        {
            UpdateCampos = MontarUpdateCampos();
        }

        private static string MontarUpdateCampos()
        {
            string sOrigem = InsertCampos;
            string sDestino = "";
            string s = "";
            while (sOrigem.IndexOf(",") > 0)
            {
                s = sOrigem.Substring(0, sOrigem.IndexOf(","));

                if (!String.IsNullOrWhiteSpace(sDestino))
                    sDestino += ", ";

                sDestino += s + "=@" + s;

                sOrigem = sOrigem.Substring(s.Length + 1, sOrigem.Length - s.Length - 1).Trim();
            }
            s = sOrigem;
            if (!String.IsNullOrWhiteSpace(sDestino))
                sDestino += ", ";
            sDestino += s + "=@" + s;

            return sDestino;
        }

        public virtual bool Existe(int codigo)
        {
            string sql = $"select 1 from {NomeTabela} where {Id_0} = @Codigo";

            using (var conexao = Acesso.NovaConexao())
            {
                return conexao.Query(sql, new { Codigo = codigo }).Count() > 0;
            }
        }

        public IEnumerable<Imovel> ObterTodos()
        {
            string sql = $"select {TodosCampos} \n " + //+ Environment.NewLine +
                         $"from {NomeTabela}";

            using (var conexao = Acesso.NovaConexao())
            {
                return conexao.Query<Imovel>(sql).ToList();
            }
        }

        public Imovel Obter(int codigo)
        {
            string sql = $"select {TodosCampos} \n " +
                         $"from {NomeTabela} \n " +
                         $"where {Id_0}=@Codigo";

            using (var conexao = Acesso.NovaConexao())
            {
                return conexao.Query<Imovel>(sql, new { Codigo = codigo }).SingleOrDefault();
            }
        }

        public bool Gravar(ref Imovel item)
        {
            string sql = "";
            if (item.IdImovel == 0)
            {
                sql = $"insert into {NomeTabela} ({InsertCampos}) \n " +
                      $"values ({InsertValues}); \n " +
                      "select ID=@@IDENTITY";
            }
            else
            {
                sql = $"update {NomeTabela} set {UpdateCampos} \n " +
                      $"where {Id_0}=@{Id_0}; \n " +
                      "select ID=-1";
            }

            using (var conexao = Acesso.NovaConexao())
            {
                var i = conexao.Query<int>(sql, new
                {
                    item.IdImovel,
                    item.IdCondominio,
                    item.IdEdificacao,
                    item.Tipo,
                    item.Nome,
                    item.Ramal
                }).SingleOrDefault();

                if (i > 0)
                {
                    item.AtualizarCodigo(i);
                }

                return ((i == -1) || i > 0);
            }
        }
    }
}
