using Models;
using Negocio;
using Persistencia;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class FilialController : ApiController
    {
        [HttpPost]
        [Route("filial/gravar")]
        public HttpResponseMessage Gravar(CriarFilialDto dto)
        {
            Filial item = Filial.Criar(
                dto.Codigo,
                dto.Nome
                );

            FilialRepositorio repositorio = new FilialRepositorio();
            if (repositorio.Gravar(ref item))
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, item.Codigo);
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, "Falha ao gravar");
        }

        [HttpGet]
        [Route("filial/obterPorCodigo")]
        public HttpResponseMessage Obter(int codigo)
        {
            FilialRepositorio repositorio = new FilialRepositorio();
            Filial item = repositorio.Obter(codigo);

            if (item == null)
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, $"[{codigo}] Registro não encontrado");
            }

            FilialDto dto = new FilialDto()
            {
                Codigo = item.Codigo,
                Nome = item.Nome
            };


            return Request.CreateResponse(System.Net.HttpStatusCode.OK, dto);

        }

        [HttpGet]
        [Route("filial/obterTodas")]
        public HttpResponseMessage ObterLista()
        {
            FilialRepositorio repositorio = new FilialRepositorio();
            IEnumerable<Filial> lista = repositorio.ObterTodos().OrderBy(o => o.Nome);

            if (lista.ToList().Count == 0)
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, "Nenhum registro encontrado");


            List<FilialDto> dto = new List<FilialDto>();

            foreach (Filial item in lista)
            {
                dto.Add(new FilialDto()
                {
                    Codigo = item.Codigo,
                    Nome = item.Nome
                });
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, dto);
        }

        [HttpGet]
        [Route("filial/obterPorNomeOuParteDoNome")]
        public HttpResponseMessage ObterPorNomeOuParteDoNome(string nomeOuParteDoNome)
        {
            FilialRepositorio repositorio = new FilialRepositorio();
            IEnumerable<Filial> lista = repositorio.ObterPorNomeOuParteDoNome(nomeOuParteDoNome).OrderBy(o => o.Nome);

            if (lista.ToList().Count == 0)
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, "Nenhum registro encontrado");


            List<FilialDto> dto = new List<FilialDto>();

            foreach (Filial item in lista)
            {
                dto.Add(new FilialDto()
                {
                    Codigo = item.Codigo,
                    Nome = item.Nome
                });
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, dto);
        }

    }
}