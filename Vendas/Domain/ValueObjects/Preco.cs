

namespace Domain.ValueObjects
{
    public class Preco(decimal valor)
    {
        
        public decimal Valor { get; private set; } = Math.Round(valor, 2);
    

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