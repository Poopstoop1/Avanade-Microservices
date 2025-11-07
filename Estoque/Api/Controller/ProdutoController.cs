using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;

namespace API.Controller
{
    [ApiController]
    [Route("api/produtos")]
    public class ProdutoController : ControllerBase
    { 
        private readonly IProdutoService _produtoService;

        public ProdutoController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        // GET: api/produtos
        [HttpGet]
        public ActionResult<List<ProdutoDTO>> GetAllProducts()
        {
            var produtos = _produtoService.FindAll();

            return Ok(produtos);
        }

        // GET: api/produtos/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<ProdutoDTO> GetById(Guid id)
        {
            try
            {
                var produto = _produtoService.FindById(id);
                var produtoDTO = new ProdutoDTO
                {
                    Nome = produto!.Nome,
                    Descricao = produto.Descricao,
                    Preco = produto.Preco.Valor,
                    Quantidade = produto.Quantidade
                };
                return Ok(produtoDTO);
            }
            catch (ProdutoNaoEncontradoException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // POST: api/produtos
        [HttpPost]
        public ActionResult<ProdutoDTO> Create([FromBody] ProdutoDTO produtoDTO)
        {
            try
            {
                var produto = new Produto(
                    produtoDTO.Nome,
                    produtoDTO.Descricao,
                    produtoDTO.Preco,
                    produtoDTO.Quantidade
                );

                var novoProduto = _produtoService.Save(produto);

                var novoProdutoDTO = new ProdutoDTO
                {
                    Nome = novoProduto.Nome,
                    Descricao = novoProduto.Descricao,
                    Preco = novoProduto.Preco.Valor,
                    Quantidade = novoProduto.Quantidade
                };

                return CreatedAtAction(nameof(GetById), new { id = novoProduto.Id }, novoProdutoDTO);
            }
            catch (ProdutoJaCadastradoException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (QuantidadeInvalidaException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/produtos/{id}/quantidade
        [HttpPut("{id:guid}/quantidade")]
        public IActionResult UpdateQuantity(Guid id, [FromQuery] int quantidadeVendida)
        {
            try
            {
                _produtoService.UpdateQuantity(id, quantidadeVendida);
                return NoContent();
            }
            catch (ProdutoNaoEncontradoException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (QuantidadeInvalidaException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (EstoqueInsuficienteException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        } 
    }
}