using System;
using Domain.Entities;
using Domain.Enums;

public interface IPedidoRepository	
{
    Task AddAsync(Pedido pedido, CancellationToken cancellationToken);

    Task<Pedido?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task <List<Pedido>> GetAllAsync(CancellationToken cancellationToken);
    Task<List<Pedido>> GetByStatusAsync(PedidoStatus status, CancellationToken cancellationToken);

    Task UpdateAsync(Pedido pedido, CancellationToken cancellationToken);

    Task DeleteAsync(Pedido pedido, CancellationToken cancellationToken);

}
