using System;

namespace Negocio
{
    public class Filial
    {

        public int Codigo { get; private set; }

        public string Nome { get; private set; }

        public Filial() { }

        private Filial(int codigo, string nome)
        {            
            Codigo = codigo;
            Nome = nome;
        }

        public static Filial Criar(int codigo, string nome)
        {
            ValidaNomeFilial(nome);
            return new Filial(codigo, nome);
        }

        public void AtribuiCodigo(int codigo)
        {
            ValidaCodigoFilial(codigo);
            Codigo = codigo;
        }

        public void AlteraNome(string novoNome)
        {
            ValidaNomeFilial(novoNome);
            Nome = novoNome;
        }

        private static void ValidaCliente(int codigo, string nome)
        {
            ValidaCodigoFilial(codigo);
            ValidaNomeFilial(nome);
        }

        private static void ValidaCodigoFilial(int codigo)
        {
            if (codigo <= 0)
                throw new InvalidOperationException("Código identificador da filial deve ser maior que zero");
        }

        private static void ValidaNomeFilial(string nome)
        {

            if (String.IsNullOrWhiteSpace(nome))
                throw new InvalidOperationException("Nome da filial é obrigatório");

            if (nome.Length > 90)
                throw new InvalidOperationException("Nome do filial deve possuir no máximo 60 caracteres");
        }
    }
}
