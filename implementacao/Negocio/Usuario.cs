using System;

namespace Negocio
{
    public class Usuario
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string Login { get; private set; }
        public string Senha { get; private set; }
        

        public Usuario() { }

        private Usuario(int id, string nome, string login, string senha)
        {        
            Id = id;
            Nome = nome;
            Login = login;
            Senha = senha;            
        }

        public static Usuario Criar(int id, string nome, string login, string senha)
        {
            ValidaUsuario(id, nome, login, senha);
            return new Usuario(id, nome, login, senha);
        }

        public void AtribuiId(int id)
        {
            ValidaIdUsuario(id);
            Id = id;
        }

        public void AlteraNome(string novoNome)
        {
            ValidaNomeUsuario(novoNome);
            Nome = novoNome;
        }

        public void AlteraSenha(string novoSenha)
        {
            ValidaSenhaUsuario(novoSenha);
            Senha = novoSenha;
        }


        private static void ValidaUsuario(int id, string nome, string login, string senha)
        {
            ValidaNomeUsuario(nome);
            ValidaLoginUsuario(login);
            ValidaSenhaUsuario(senha);

        }

        private static void ValidaIdUsuario(int id)
        {
            if (id <= 0)
                throw new InvalidOperationException("Código identificador do usuario deve ser maior que zero");
        }

        private static void ValidaNomeUsuario(string nome)
        {

            if (String.IsNullOrWhiteSpace(nome))
                throw new InvalidOperationException("Nome do usuario é obrigatório");

            if (nome.Length > 90)
                throw new InvalidOperationException("Nome do usuario deve possuir no máximo 90 caracteres");
        }

        private static void ValidaLoginUsuario(string login)
        {

            if (String.IsNullOrWhiteSpace(login))
                throw new InvalidOperationException("Login do usuario é obrigatório");

            if (login.Length > 50)
                throw new InvalidOperationException("Login do usuario deve possuir no máximo 50 caracteres");
        }

        private static void ValidaSenhaUsuario(string senha)
        {

            if (String.IsNullOrWhiteSpace(senha))
                throw new InvalidOperationException("Senha do usuario é obrigatório");

            if (senha.Length > 30)
                throw new InvalidOperationException("Senha do usuario deve possuir no máximo 30 caracteres");
        }
    }
}
