using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class PrecoNegativoException : Exception
    {
        public PrecoNegativoException(decimal preco)
            : base($"O preço não pode ser negativo: {preco}")
        {
        }
    }
}