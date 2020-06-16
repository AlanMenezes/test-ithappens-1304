using System;

namespace Negocio
{
    public class Condominio
    {
        public int IdCondominio { get; private set; }
        public string Nome { get; private set; }
        public string NomeReduzido { get; private set; }
        public string Cnpj { get; private set; }
        public string Email { get; private set; }
        public int IdSindicoAtual { get; private set; }

        public Condominio() { }

        private Condominio(int idCondominio, string nome, string nomeReduzido, string cnpj, string email, int idSindicoAtual)
        {
            IdCondominio = idCondominio;
            Nome = nome ;
            NomeReduzido = nomeReduzido;
            Cnpj = cnpj ;
            Email = email ;
            IdSindicoAtual = idSindicoAtual;
        }

        public static Condominio Criar(int idCondominio, string nome, string nomeReduzido, string cnpj, string email, int idSindicoAtual)
        {
            return new Condominio(idCondominio, nome, nomeReduzido, cnpj, email, idSindicoAtual);
        }

        public void AtualizarCodigo(int codigo)
        {
            if (codigo <= 0)
                throw new InvalidOperationException("Código inválido");

            IdCondominio = codigo;
        }
    }
}
