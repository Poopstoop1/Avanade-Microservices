using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command
{
    public class DeleteProduct(Guid id) : IRequest<Unit>
    {
        public Guid Id { get; private set; } = id;
    }
}
