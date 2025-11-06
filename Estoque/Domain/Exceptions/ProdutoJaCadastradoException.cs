using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class ProdutoJaCadastradoException : Exception
    {
         public ProdutoJaCadastradoException(string nome)
                : base($"O produto com o nome '{nome}' já está cadastrado.") { }
    }
}