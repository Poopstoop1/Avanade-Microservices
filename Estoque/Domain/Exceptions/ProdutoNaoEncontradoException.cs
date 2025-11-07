using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class ProdutoNaoEncontradoException : Exception
    {   
       public ProdutoNaoEncontradoException(Guid id)
            : base($"Produto com ID '{id}' não foi encontrado.")
        {
        }
    }
}