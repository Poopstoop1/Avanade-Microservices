using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Exceptions;

namespace Domain.ValueObjects
{
    public class Preco
    {
        
        public decimal Valor { get; private set; }

        public Preco(decimal valor) 
        {
            if (valor < 0)
                throw new PrecoNegativoException(valor);

            Valor = Math.Round(valor, 2);
        }
    

     public override bool Equals(object? obj)
        {
            if (obj is not Preco outro)
                return false;

            return Valor == outro.Valor;
        }

        public override int GetHashCode()
            => Valor.GetHashCode();

    }

}