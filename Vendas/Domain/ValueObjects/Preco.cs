using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public class Preco
    {
        
        public decimal Valor { get; private set; }

        public Preco(decimal valor) 
        {
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