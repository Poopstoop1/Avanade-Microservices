using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command
{
    public class DeleteProduct: IRequest<Unit>
    {
        public DeleteProduct(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
    }
}
