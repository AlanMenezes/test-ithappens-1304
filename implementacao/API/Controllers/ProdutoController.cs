using Models;
using Negocio;
using Persistencia;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class ProdutoController : ApiController
    {
        [HttpPost]
        [Route("produto/gravar")]
        public HttpResponseMessage Gravar(CriarProdutoDto dto)
        {
            Produto item = Produto.Criar(
                dto.Id,
                dto.Descricao,
                dto.CodBarra,
                dto.PrecoCusto,
                dto.PrecoVenda
                );

            ProdutoRepositorio repositorio = new ProdutoRepositorio();
            if (repositorio.Gravar(ref item))
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, item.Id);
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, "Falha ao gravar");
        }

        [HttpGet]
        [Route("produto/{id}")]
        public HttpResponseMessage Obter(int id)
        {
            ProdutoRepositorio repositorio = new ProdutoRepositorio();
            Produto item = repositorio.Obter(id);

            if (item == null)
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, $"[{id}] Registro não encontrado");
            }

            ProdutoDto dto = new ProdutoDto()
            {
                Id = item.Id,
                Descricao = item.Descricao,
                CodBarra = item.CodBarra,
                PrecoCusto = item.PrecoCusto,
                PrecoVenda = item.PrecoVenda
            };


            return Request.CreateResponse(System.Net.HttpStatusCode.OK, dto);

        }

        [HttpGet]
        [Route("produto/PorCodBarra")]
        public HttpResponseMessage ObterPorCodBarra(string codBarra)
        {
            ProdutoRepositorio repositorio = new ProdutoRepositorio();
            Produto item = repositorio.ObterPorCodigoDeBarras(codBarra);

            if (item == null)
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, $"[{codBarra}] Registro não encontrado");
            }

            ProdutoDto dto = new ProdutoDto()
            {
                Id = item.Id,
                Descricao = item.Descricao,
                CodBarra = item.CodBarra,
                PrecoCusto = item.PrecoCusto,
                PrecoVenda = item.PrecoVenda
            };


            return Request.CreateResponse(System.Net.HttpStatusCode.OK, dto);

        }

        [HttpGet]
        [Route("produto/obterTodos")]
        public HttpResponseMessage ObterLista()
        {
            ProdutoRepositorio repositorio = new ProdutoRepositorio();
            IEnumerable<Produto> lista = repositorio.ObterTodos().OrderBy(o => o.Descricao);

            if (lista.ToList().Count == 0)
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, "Nenhum registro encontrado");


            List<ProdutoDto> dto = new List<ProdutoDto>();

            foreach (Produto item in lista)
            {
                dto.Add(new ProdutoDto()
                {
                    Id = item.Id,
                    Descricao = item.Descricao,
                    CodBarra = item.CodBarra,
                    PrecoCusto = item.PrecoCusto,
                    PrecoVenda = item.PrecoVenda
                });
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, dto);
        }

        [HttpGet]
        [Route("produto/obterPorDescricaoOuParteDaDescricao")]
        public HttpResponseMessage ObterPorDescricaoOuParteDaDescricao(string descricaoOuParteDaDescricao)
        {
            ProdutoRepositorio repositorio = new ProdutoRepositorio();
            IEnumerable<Produto> lista = repositorio.ObterPorDescricaoOuParteDaDescricao(descricaoOuParteDaDescricao).OrderBy(o => o.Descricao);

            if (lista.ToList().Count == 0)
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, "Nenhum registro encontrado");


            List<ProdutoDto> dto = new List<ProdutoDto>();

            foreach (Produto item in lista)
            {
                dto.Add(new ProdutoDto()
                {
                    Id = item.Id,
                    Descricao = item.Descricao,
                    CodBarra = item.CodBarra,
                    PrecoCusto = item.PrecoCusto,
                    PrecoVenda = item.PrecoVenda
                });
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, dto);
        }

    }
}