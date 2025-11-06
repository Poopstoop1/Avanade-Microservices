using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.IRepository
{
    public interface IProdutoRepository
    {
        Produto Save(Produto produto);
        List<Produto> FindAll();
        Produto? FindById(Guid id);
        void Update(Produto produto);
        Produto? Delete(Guid id);

    }
}