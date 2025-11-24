using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Query;
using Domain.Enums;
using Application.DTOs;
using Application.Command;
using Microsoft.AspNetCore.Authorization;

namespace Vendas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PedidosController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PedidosController> _logger;


        public PedidosController(IMediator mediator, ILogger<PedidosController> logger)
        {
            _mediator = mediator;
            _logger = logger;

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPedidoById(Guid id)
        {
            try
            {
                var query = new GetByIdPedido(id);
                var pedido = await _mediator.Send(query);
                return Ok(pedido);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Erro ao buscar o pedido com ID: {Id}", id);
                return NotFound();
            }
        }

        [HttpGet]

        public async Task<IActionResult> GetAllPedidos()
        {
            try
            {
                var query = new GetByAllPedido();
                var pedidos = await _mediator.Send(query);
                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar todos os pedidos");
                return BadRequest(ex.Message);

            }
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetPedidosByStatus(PedidoStatus status)
        {
            try
            {
                var query = new GetByStatusPedido(status);
                var pedidos = await _mediator.Send(query);
                return Ok(pedidos);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Status inválido: {Status}", status);
                return BadRequest("Status inválido.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar pedidos com status: {Status}", status);
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreatePedido([FromBody] PedidoDTO pedidoDTO)
        {
            try
            {
                var query = new AddPedido(
                    pedidoDTO.UsuarioId,
                    pedidoDTO.Itens
                    );
                var pedidoId = await _mediator.Send(query);
                return CreatedAtAction(nameof(GetPedidoById), new { id = pedidoId }, pedidoId );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar o pedido");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/status/{status}")]

        public async Task<IActionResult> UpdatePedidoStatus(Guid id, string status)
        {
            try
            {
                var pedidoStatus = Enum.Parse<PedidoStatus>(status, true);
                var command = new UpdatePedido(id, pedidoStatus);
                await _mediator.Send(command);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Status inválido: {Status}", status);
                return BadRequest("Status inválido.");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Erro ao atualizar o pedido com ID: {Id}", id);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar o pedido com ID: {Id}", id);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePedido(Guid id)
        {
            try
            {
                var command = new DeletePedido(id);
                await _mediator.Send(command);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Erro ao deletar o pedido com ID: {Id}", id);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar o pedido com ID: {Id}", id);
                return BadRequest(ex.Message);
            }

        }


        }
}
