using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class Imovel
    {
        public int IdImovel { get; private set; }
        public TipoImovel Tipo { get; private set; }
        public int? IdEdificacao { get; private set; }
        public string Nome { get; private set; }
        public string Ramal { get; private set; }

        public Imovel() { }

        private Imovel(int idImovel, TipoImovel tipo, int? idEdificacao, string nome, string ramal)
        {
            IdImovel = idImovel;
            Tipo = tipo;
            IdEdificacao = idEdificacao;
            Nome = nome;
            Ramal = ramal;
        }

        public static Imovel Criar(int idImovel, TipoImovel tipo, int? idEdificacao, string nome, string ramal)
        {
            return new Imovel(idImovel, tipo, idEdificacao, nome, ramal);
        }

        public void AtualizarCodigo(int codigo)
        {
            if (codigo <= 0)
                throw new InvalidOperationException("Código inválido");

            IdEdificacao = codigo;
        }
    }
}
