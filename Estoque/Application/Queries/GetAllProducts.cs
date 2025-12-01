using Application.DTOs.ViewModels;
using MediatR;


namespace Application.Queries
{
    public class GetAllProducts : IRequest<List<ProdutoViewDTO>>
    {

    }
}
