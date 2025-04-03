using ETicaretAPI.Application.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductReadRepository _productReadRepository;

        public ProductsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
        }

        [HttpGet]
        public async void Get()
        {
            await _productWriteRepository.AddRangeAsync(new()
            {
                new(){Id=Guid.NewGuid(), Name="Product 1", Price=100,Stock=10, CreatedDate=DateTime.UtcNow},
                new(){Id=Guid.NewGuid(), Name="Product 2", Price=300,Stock=20, CreatedDate=DateTime.UtcNow},
                new(){Id=Guid.NewGuid(), Name="Product 3", Price=500,Stock=30, CreatedDate=DateTime.UtcNow},
            });
            await _productWriteRepository.SaveAsync();

            
        }
    }
}
