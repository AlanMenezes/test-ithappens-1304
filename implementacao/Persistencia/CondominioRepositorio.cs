using Dapper;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Persistencia
{
    public class CondominioRepositorio
    {
        private static string NomeTabela = "dbo.Condominio";
        private static string Id_0 = "IdCondominio";
        private static string InsertCampos = "Nome, NomeReduzido, Cnpj, Email, IdSindicoAtual";
        private static string InsertValues = "@" + InsertCampos.Replace(", ", ", @");
        private static string UpdateCampos = "";
        private static string TodosCampos = Id_0 + ", " + InsertCampos;

        public CondominioRepositorio()
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

        public IEnumerable<Condominio> ObterTodos()
        {
            string sql = $"select {TodosCampos} \n " + //+ Environment.NewLine +
                         $"from {NomeTabela}";

            using (var conexao = Acesso.NovaConexao())
            {
                return conexao.Query<Condominio>(sql).ToList();
            }
        }

        public Condominio Obter(int codigo)
        {
            string sql = $"select {TodosCampos} \n " +
                         $"from {NomeTabela} \n " +
                         $"where {Id_0}=@Codigo";

            using (var conexao = Acesso.NovaConexao())
            {
                return conexao.Query<Condominio>(sql, new { Codigo = codigo }).SingleOrDefault();
            }
        }

        public bool Gravar(ref Condominio item)
        {
            string sql = "";
            if (item.IdCondominio == 0)
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
                    item.IdCondominio,
                    item.Nome,
                    item.NomeReduzido,
                    item.Cnpj,
                    item.Email,
                    item.IdSindicoAtual
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
