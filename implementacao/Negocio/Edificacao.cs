using System;

namespace Negocio
{
    public class Edificacao
    {
        public int IdEdificacao { get; private set; }
        public int IdCondominio { get; private set; }
        public TipoEdificacao Tipo { get; private set; }
        public string Nome { get; private set; }
        public string ComplementoEndereco { get; private set; }

        public Edificacao() { }

        private Edificacao(int idEdificacao, int idCondominio, TipoEdificacao tipo, string nome, string complementoEndereco)
        {
            IdEdificacao = idEdificacao;
            IdCondominio = idCondominio;
            Tipo = tipo;
            Nome = nome;
            ComplementoEndereco = complementoEndereco;
        }

        public static Edificacao Criar(int idEdificacao, int idCondominio, TipoEdificacao tipo, string nome, string complementoEndereco)
        {
            return new Edificacao(idEdificacao, idCondominio, tipo, nome, complementoEndereco);
        }

        public void AtualizarCodigo(int codigo)
        {
            if (codigo <= 0)
                throw new InvalidOperationException("Código inválido");

            IdEdificacao = codigo;
        }
    }
}
