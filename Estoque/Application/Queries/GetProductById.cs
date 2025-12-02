using Application.DTOs.ViewModels;
using MediatR;


namespace Application.Queries
{
    public class GetProductById(Guid id) : IRequest<ProdutoViewDTO>
    {
        public Guid Id { get; private set; } = id;
      
    }
}
