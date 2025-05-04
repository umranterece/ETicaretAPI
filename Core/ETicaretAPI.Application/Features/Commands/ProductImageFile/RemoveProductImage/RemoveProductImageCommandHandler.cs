using ETicaretAPI.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Application.Features.Commands.ProductImageFile.RemoveProductImage;

public class RemoveProductImageCommandHandler: IRequestHandler<RemoveProductImageCommandRequest, RemoveProductImageCommandResponse>
{
    readonly IProductReadRepository _productReadRepository;
    readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
    public RemoveProductImageCommandHandler(IProductReadRepository productReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository)
    {
        _productReadRepository = productReadRepository;
        _productImageFileWriteRepository = productImageFileWriteRepository;
    }
    public async Task<RemoveProductImageCommandResponse> Handle(RemoveProductImageCommandRequest request, CancellationToken cancellationToken)
    {
        Domain.Entities.Product? product=await _productReadRepository.Table.Include(p=>p.ProductImageFiles)
            .FirstOrDefaultAsync(p=>p.Id==Guid.Parse(request.Id));
        
        Domain.Entities.ProductImageFile productImageFile= product?.ProductImageFiles.FirstOrDefault(p=>p.Id==Guid.Parse(request.ImageId));
        if (productImageFile != null)
            product?.ProductImageFiles.Remove(productImageFile);
        
        await _productImageFileWriteRepository.SaveAsync();
        return new() { };
    }
}