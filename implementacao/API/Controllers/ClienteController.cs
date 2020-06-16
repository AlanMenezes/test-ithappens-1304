using Models;
using Negocio;
using Persistencia;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class ClienteController : ApiController
    {
        [HttpPost]
        [Route("cliente/gravar")]
        public HttpResponseMessage Gravar(CriarClienteDto dto)
        {
            Cliente item = Cliente.Criar(
                dto.Id,
                dto.Nome
                );

            ClienteRepositorio repositorio = new ClienteRepositorio();
            if (repositorio.Gravar(ref item))
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, item.Id);
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, "Falha ao gravar");
        }

        [HttpGet]
        [Route("cliente/{id}")]
        public HttpResponseMessage Obter(int id)
        {
            ClienteRepositorio repositorio = new ClienteRepositorio();
            Cliente item = repositorio.Obter(id);

            if (item == null)
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, $"[{id}] Registro não encontrado");
            }

            ClienteDto dto = new ClienteDto()
            {
                Id= item.Id,
                Nome = item.Nome
            };


            return Request.CreateResponse(System.Net.HttpStatusCode.OK, dto);

        }

        [HttpGet]
        [Route("cliente/obterTodos")]
        public HttpResponseMessage ObterLista()
        {
            ClienteRepositorio repositorio = new ClienteRepositorio();
            IEnumerable<Cliente> lista = repositorio.ObterTodos().OrderBy(o => o.Nome);

            if (lista.ToList().Count == 0)
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, "Nenhum registro encontrado");


            List<ClienteDto> dto = new List<ClienteDto>();

            foreach (Cliente item in lista)
            {
                dto.Add(new ClienteDto()
                {
                    Id= item.Id,
                    Nome = item.Nome
                });
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, dto);
        }

        [HttpGet]
        [Route("cliente/obterPorNomeOuParteDoNome")]
        public HttpResponseMessage ObterPorNomeOuParteDoNome(string nomeOuParteDoNome)
        {
            ClienteRepositorio repositorio = new ClienteRepositorio();
            IEnumerable<Cliente> lista = repositorio.ObterPorNomeOuParteDoNome(nomeOuParteDoNome).OrderBy(o => o.Nome);

            if (lista.ToList().Count == 0)
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, "Nenhum registro encontrado");


            List<ClienteDto> dto = new List<ClienteDto>();

            foreach (Cliente item in lista)
            {
                dto.Add(new ClienteDto()
                {
                    Id= item.Id,
                    Nome = item.Nome
                });
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, dto);
        }

    }
}