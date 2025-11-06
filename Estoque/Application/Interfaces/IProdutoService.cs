using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProdutoService
    {
        Produto Save(Produto produto);
        List<Produto> FindAll();
        Produto? FindById(Guid id);
        void UpdateQuantity(Guid id, int quantidadeVendida);
    }
}