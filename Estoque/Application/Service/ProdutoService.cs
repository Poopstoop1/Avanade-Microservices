using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Domain.IRepository;


namespace Application.Service
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        public Produto Save(Produto produto)
        {
            Boolean produtoExistente = _produtoRepository.FindAll().Any(p => p.Nome == produto.Nome);

            if (produtoExistente)
                throw new ProdutoJaCadastradoException(produto.Nome);

            if (produto.Quantidade < 0)
                throw new QuantidadeInvalidaException(produto.Quantidade);

            return _produtoRepository.Save(produto);
        }

        public List<Produto> FindAll()
        {
            return _produtoRepository.FindAll();
        }

        public Produto? FindById(Guid id)
        {
            var produtoExistente = _produtoRepository.FindById(id);
            if (produtoExistente == null)
                throw new ProdutoNaoEncontradoException(id);

            return produtoExistente;
        }
    public void UpdateQuantity(Guid id, int quantidadeVendida)
    {
        var produtoExistente = _produtoRepository.FindById(id);
            if (produtoExistente == null)
                throw new ProdutoNaoEncontradoException(id);

            if (produtoExistente.Quantidade < 0)
                throw new QuantidadeInvalidaException(produtoExistente.Quantidade);

             if (produtoExistente.Quantidade < quantidadeVendida)
                throw new EstoqueInsuficienteException(produtoExistente.Nome, quantidadeVendida, produtoExistente.Quantidade);

        produtoExistente.Quantidade -= quantidadeVendida;
        _produtoRepository.Update(produtoExistente);
    }
    
    }

  }
