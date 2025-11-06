using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Domain.IRepository;
using Infrastructure.DB;

namespace Infrastructure.Repositories
{
  public class ProdutoRepository : IProdutoRepository
  {
    private readonly EstoqueDBContext _context;

     public ProdutoRepository(EstoqueDBContext context)
        {
            _context = context;
        }

        public List<Produto> FindAll()
        {
            return _context.Produtos.ToList();
        }

        public Produto? FindById(Guid id)
        {
            return _context.Produtos.FirstOrDefault(p => p.Id == id);
        }

        public Produto Save(Produto produto)
        {
            _context.Produtos.Add(produto);
            _context.SaveChanges();
            return produto;
        }

        public void Update(Produto produto)
        {
            _context.Produtos.Update(produto);
            _context.SaveChanges();
        }

        public Produto? Delete(Guid id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.Id == id);
            if (produto == null) return null;

            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return produto;
        }

  }
  
  
}