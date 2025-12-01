using Application.DTOs.ViewModels;
using MediatR;


namespace Application.Queries
{
    public class GetProductById : IRequest<ProdutoViewDTO>
    {
        public GetProductById(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
      
    }
}
