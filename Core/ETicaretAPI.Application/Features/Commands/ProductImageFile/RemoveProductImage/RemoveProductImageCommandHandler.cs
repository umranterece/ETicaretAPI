using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Application.Features.Commands.ProductImageFile.RemoveProductImage;

public class RemoveProductImageCommandHandler: IRequestHandler<RemoveProductImageCommandRequest, RemoveProductImageCommandResponse>
{
    readonly IStorageService _storageService;
    readonly IProductReadRepository _productReadRepository;
    readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
    public RemoveProductImageCommandHandler(IProductReadRepository productReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository, IStorageService storageService)
    {
        _productReadRepository = productReadRepository;
        _productImageFileWriteRepository = productImageFileWriteRepository;
        _storageService = storageService;
    }
    public async Task<RemoveProductImageCommandResponse> Handle(RemoveProductImageCommandRequest request, CancellationToken cancellationToken)
    {
        Domain.Entities.Product? product=await _productReadRepository.Table.Include(p=>p.ProductImageFiles)
            .FirstOrDefaultAsync(p=>p.Id==Guid.Parse(request.Id));
        
        Domain.Entities.ProductImageFile productImageFile= product?.ProductImageFiles.FirstOrDefault(p=>p.Id==Guid.Parse(request.ImageId));
        if (productImageFile != null)
        {
            product?.ProductImageFiles.Remove(productImageFile);

            await _storageService.DeleteAsync(productImageFile.Path,productImageFile.FileName);
        }
        
        await _productImageFileWriteRepository.SaveAsync();
        return new() { };
    }
}