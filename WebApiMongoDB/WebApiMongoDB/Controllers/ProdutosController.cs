using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApiMongoDB.Models;
using WebApiMongoDB.Services;

namespace WebApiMongoDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly ProdutoServices _produtoServices;

        public ProdutosController(ProdutoServices produtoServices)
        {
            _produtoServices = produtoServices;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<Produto>> GetProdutos()
            => await _produtoServices.GetAsync();
     
        [HttpPost]
        public async Task<ActionResult<Produto>> PostProduto(Produto produto)
        {
            try
            {
                if (ProdutoExiste(produto))
                {
                    return NotFound("Esse código já existe!");
                }

                await _produtoServices.CreateAsync(produto);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            
            return Ok(produto);
        }

        private bool ProdutoExiste(Produto produto)
        {
            return _produtoServices.ProdutoExiste(produto).Result;

        }

        [HttpPut]
        public async Task<IActionResult> PutProduto(string id, [FromBody] Produto produto)
        {
            try
            {
                await _produtoServices.UpdateAsync(produto.Id = id, produto);

                if (id != produto.Id)
                {
                    return NotFound();
                }
            }
            catch
            {
                return BadRequest();
            }
            
            return Ok(produto);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduto(string id, Produto produto)
        {
            await _produtoServices.RemoveAsync(produto.Id = id);
            if (produto == null)
            {
                return NotFound();
            }

            return Ok(produto);
        }

        [HttpGet("{id}", Name ="GetProduto")]
        public async Task<ActionResult<Produto>> GetProdutoId(string id)
        {
            try
            {
                var produto = await _produtoServices.GetAsync(id); 

                if (produto is null)
                    return NotFound($"Não existe produto com id={id}");
                return Ok(produto);
            }
            catch
            {
                return BadRequest("Request inválido");
            }

        }

    }
}
