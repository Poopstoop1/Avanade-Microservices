using Application.DTOs;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Query
{
    public class GetProductById : IRequest<ProdutoDTO>
    {
        public GetProductById(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
      
    }
}
