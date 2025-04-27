using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Persistence.Contexts;

namespace ETicaretAPI.Persistence.Repositories;

public class InvoiceFileWriteRepository:WriteRepository<Domain.Entities.InvoiceFile>,IInvoiceFileWriteRepository
{
    public InvoiceFileWriteRepository(ETicaretAPIDbContext context) : base(context)
    {
    }
}