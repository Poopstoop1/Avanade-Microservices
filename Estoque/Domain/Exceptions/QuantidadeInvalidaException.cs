using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class QuantidadeInvalidaException : Exception
    {
       public QuantidadeInvalidaException(int quantidade)
            : base($"A quantidade informada ({quantidade}) é inválida. Deve ser maior que zero.") { }
    }
}