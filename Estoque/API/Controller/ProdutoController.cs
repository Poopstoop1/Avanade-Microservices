using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Queries;
using Application.Command;
using Microsoft.AspNetCore.Authorization;
using Application.DTOs.InputModels;

namespace API.Controller
{
    [ApiController]
    [Route("api/produtos")]
    [Authorize()]
    public class ProdutoController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly ILogger<ProdutoController> _logger;


        public ProdutoController(IMediator mediator, ILogger<ProdutoController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // Query
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            try
            {
                var command = new GetProductById(id);
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Produto com Id {Id} não encontrado.", id);
                return NotFound();
            }
                
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var query = new GetAllProducts();
                var produtos = await _mediator.Send(query);
                return Ok(produtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os produtos.");
                return BadRequest(new { message = ex.Message });
            }
            
        }
        // Fim da Query


        // Command
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProdutoInputDTO produto)
        {
            try {
                var command = new AddProduct
               (
                   produto.Nome,
                   produto.Descricao,
                   produto.Preco,
                   produto.Quantidade
               );
                var id = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetProductById), new { id }, new { id });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Erro ao criar o Produto");
                return BadRequest(new { message = ex.Message });
            }
           
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduto(Guid id, [FromBody] ProdutoInputDTO produto)
        {
            var command = new UpdateProduct
            (
                id,
                produto.Nome,
                produto.Descricao,
                produto.Preco,
                produto.Quantidade
            );
            try
            {
                await _mediator.Send(command);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Produto com Id {Id} não encontrado para atualização.", id);
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(Guid id)
        {
            var command = new DeleteProduct(id);

            try
            {
                await _mediator.Send(command);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Produto com Id {Id} não encontrado para exclusão.", id);
                return NotFound();
            }
            return NoContent();
        }
        // Fim do Command

    }
}