using System;

namespace Negocio
{
    public class Cliente
    {

        public int Id { get; private set; }

        public string Nome { get; private set; }

        public Cliente() { }

        private Cliente(int id, string nome)
        {
            Id = id;
            Nome = nome;
        }

        public static Cliente Criar(int id, string nome)
        {
            ValidaNomeCliente(nome);
            return new Cliente(id, nome);
        }

        public void AtribuiId(int id)
        {
            ValidaIdCliente(id);
            Id = id;
        }

        public void AlteraNome(string novoNome)
        {
            ValidaNomeCliente(novoNome);
            Nome = novoNome;
        }

        private static void ValidaCliente(int id, string nome)
        {
            ValidaIdCliente(id);
            ValidaNomeCliente(nome);
        }

        private static void ValidaIdCliente(int id)
        {
            if (id <= 0)
                throw new InvalidOperationException("Código identificador do cliente deve ser maior que zero");
        }

        private static void ValidaNomeCliente(string nome)
        {

            if (String.IsNullOrWhiteSpace(nome))
                throw new InvalidOperationException("Nome do cliente é obrigatório");

            if (nome.Length > 90)
                throw new InvalidOperationException("Nome do cliente deve possuir no máximo 90 caracteres");
        }

    }
}
