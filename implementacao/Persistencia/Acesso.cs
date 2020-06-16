using System;
using System.Data.SqlClient;

namespace Persistencia
{
    public static class Acesso
    {
        public static string Servidor { get; private set; }
        public static string Banco { get; private set; }
        public static string Usuario { get; private set; }
        public static string Senha { get; private set; }
        public static bool Inicializado { get; private set; }

        static Acesso()
        {
            Inicializar("localhost", "CondominioDB", "sa", "sa123");
        }

        private static void Atualizar(string servidor, string banco, string usuario, string senha)
        {
            Servidor = servidor;
            Banco = banco;
            Usuario = usuario;
            Senha = senha;
        }

        public static void Inicializar(string servidor, string banco, string usuario, string senha)
        {
            if (string.IsNullOrWhiteSpace(servidor))
                throw new InvalidOperationException("O servidor é obrigatório");
            if (string.IsNullOrWhiteSpace(banco))
                throw new InvalidOperationException("O banco de dados é obrigatório");
            if (string.IsNullOrWhiteSpace(usuario))
                throw new InvalidOperationException("O usuario é obrigatório");
            if (string.IsNullOrWhiteSpace(senha))
                throw new InvalidOperationException("A senha é obrigatória");

            if (ValidarConexao(servidor, banco, usuario, senha))
            {
                Atualizar(servidor, banco, usuario, senha);
                Inicializado = true;
            }
        }

        private static bool ValidarConexao(string servidor, string banco, string usuario, string senha)
        {
            try
            {
                SqlConnection conexao = NovaConexao(servidor, banco, usuario, senha);

                return conexao != null;
            }
            catch (Exception)
            {
                return false; // throw new InvalidOperationException("Falha ao inicializar");
            }
        }

        public static SqlConnection NovaConexao()
        {
            return NovaConexao(Servidor, Banco, Usuario, Senha);
        }

        public static SqlConnection NovaConexao(string servidor, string banco, string usuario, string senha)
        {
            string connectionString;
            SqlConnection conexao;

            connectionString = $@"Data Source={servidor};Initial Catalog={banco};User ID={usuario};Password={senha}";
            conexao = new SqlConnection(connectionString);

            try
            {
                conexao.Open();
            }
            catch (Exception excessao)
            {
                throw new InvalidOperationException("Falha ao conectar ao banco de dados:" + Environment.NewLine + excessao.Message);
            }

            return conexao;
        }
    }

}
