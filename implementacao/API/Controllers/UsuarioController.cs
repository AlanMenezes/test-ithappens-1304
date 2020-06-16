using Models;
using Negocio;
using Persistencia;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class UsuarioController : ApiController
    {
        [HttpPost]
        [Route("usuario/gravar")]
        public HttpResponseMessage Gravar(CriarUsuarioDto dto)
        {
            Usuario item = Usuario.Criar(
                dto.Id,
                dto.Nome,
                dto.Login,
                dto.Senha
                );

            UsuarioRepositorio repositorio = new UsuarioRepositorio();
            if (repositorio.Gravar(ref item))
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, item.Id);
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, "Falha ao gravar");
        }

        [HttpGet]
        [Route("usuario/{id}")]
        public HttpResponseMessage Obter(int id)
        {
            UsuarioRepositorio repositorio = new UsuarioRepositorio();
            Usuario item = repositorio.Obter(id);

            if (item == null)
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, $"[{id}] Registro não encontrado");
            }

            UsuarioDto dto = new UsuarioDto()
            {
                Id = item.Id,
                Nome = item.Nome,
                Login = item.Login,
                Senha = "*********"
            };


            return Request.CreateResponse(System.Net.HttpStatusCode.OK, dto);

        }

        [HttpGet]
        [Route("usuario/obterTodos")]
        public HttpResponseMessage ObterLista()
        {
            UsuarioRepositorio repositorio = new UsuarioRepositorio();
            IEnumerable<Usuario> lista = repositorio.ObterTodos().OrderBy(o => o.Nome);

            if (lista.ToList().Count == 0)
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, "Nenhum registro encontrado");


            List<UsuarioDto> dto = new List<UsuarioDto>();

            foreach (Usuario item in lista)
            {
                dto.Add(new UsuarioDto()
                {
                    Id = item.Id,
                    Nome = item.Nome,
                    Login = item.Login,
                    Senha = "*********"
                });
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, dto);
        }

        [HttpGet]
        [Route("usuario/obterPorNomeOuParteDoNome")]
        public HttpResponseMessage ObterPorNomeOuParteDoNome(string nomeOuParteDoNome)
        {
            UsuarioRepositorio repositorio = new UsuarioRepositorio();
            IEnumerable<Usuario> lista = repositorio.ObterPorNomeOuParteDoNome(nomeOuParteDoNome).OrderBy(o => o.Nome);

            if (lista.ToList().Count == 0)
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, "Nenhum registro encontrado");


            List<UsuarioDto> dto = new List<UsuarioDto>();

            foreach (Usuario item in lista)
            {
                dto.Add(new UsuarioDto()
                {
                    Id = item.Id,
                    Nome = item.Nome,
                    Login = item.Login,
                    Senha = "*********"
                });
            }

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, dto);
        }

    }
}