using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Shop.Repositories;
using Shop.Services;

namespace testeef.Controllers
{
    [ApiController]
    [Route("v1/account")]
    public class ProductController : ControllerBase
    {
            [HttpPost]
            [Route("login")]
            [AllowAnonymous]
            public async Task<ActionResult<dynamic>> Authenticate([FromBody] User model)
            {
                // Recupera o usuário
                var user = UserRepository.Get(model.Username, model.Password);

                // Verifica se o usuário existe
                if (user == null)
                    return NotFound(new { message = "Usuário ou senha inválidos" });

                // Gera o Token
                var token = TokenService.GenerateToken(user);

                // Oculta a senha
                user.Password = "";

                // Retorna os dados
                return new
                {
                    user = user,
                    token = token
                };
            }

            [HttpGet]
            [Route("anonymous")]
            [AllowAnonymous]
            public string Anonymous() => "Anônimo";
            
            [HttpGet]
            [Route("authenticated")]
            [Authorize]
            public string Authenticated() => String.Format("Autenticado - {0}", User.Identity.Name);
            //Você deve ter percebido que a linha acima nós recuperamos o usuário logado. 
            //Isto é feito através da propriedade User.Identity.Name, que é preenchida automaticamente cada vez que um Token é enviado no cabeçalho da requisição.


            [HttpGet]
            [Route("employee")]
            [Authorize(Roles = "employee,manager")]
            public string Employee() => "Funcionário";
            
            [HttpGet]
            [Route("manager")]
            [Authorize(Roles = "manager")]
            public string Manager() => "Gerente";


    }
}