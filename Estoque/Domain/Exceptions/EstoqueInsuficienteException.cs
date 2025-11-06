using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class EstoqueInsuficienteException : Exception
    {
        public EstoqueInsuficienteException(string nome,int quantidadeDisponivel, int quantidadeSolicitada)
            : base($"Estoque insuficiente para {nome}. Disponível: {quantidadeDisponivel}, Solicitado: {quantidadeSolicitada}")
        {
        }
    }
}