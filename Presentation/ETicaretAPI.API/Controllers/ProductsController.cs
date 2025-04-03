using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities;
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
        private readonly IOrderReadRepository _orderReadRepository;
        private readonly IOrderWriteRepository _orderWriteRepository;
        private readonly ICustomerWriteRepository _customerWriteRepository;

        public ProductsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository, IOrderWriteRepository orderWriteRepository, ICustomerWriteRepository customerWriteRepository, IOrderReadRepository orderReadRepository)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _orderWriteRepository = orderWriteRepository;
            _customerWriteRepository = customerWriteRepository;
            _orderReadRepository = orderReadRepository;
        }

        [HttpGet]
        public async Task Get()
        {
            await _productWriteRepository.AddRangeAsync(new()
            {
                new(){Id=Guid.NewGuid(), Name="Product 1", Price=100,Stock=10},
                new(){Id=Guid.NewGuid(), Name="Product 2", Price=300,Stock=20},
                new(){Id=Guid.NewGuid(), Name="Product 3", Price=500,Stock=30},
            });
          


            //Product p=await _productReadRepository.GetByIdAsync("eab4d3e7-ad97-4e5f-8b89-db79e0d30e26",false);
            //p.Name = "UMRAN TERECE";
            //_productWriteRepository.SaveAsync();


            //await _productWriteRepository.AddAsync(new() { Name = "Product --- 1", Price = 1.500F, Stock = 10, CreatedDate = DateTime.UtcNow });
            //await _productWriteRepository.SaveAsync();
            var custmerId=Guid.NewGuid();
            await _customerWriteRepository.AddAsync(new() { Id = custmerId, Name = "Muiidin" });


            await _orderWriteRepository.AddAsync(new() { Id = Guid.NewGuid(), Address="test",Description="aciklama",CustomerId= custmerId });
            await _orderWriteRepository.AddAsync(new() { Id = Guid.NewGuid(), Address="test2",Description="aciklama2",CustomerId= custmerId });
            await _orderWriteRepository.SaveAsync();

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            Order order = await _orderReadRepository.GetByIdAsync(id);  
            order.Address= "Yeni Adres";
            //_orderWriteRepository.Update(order);
            await _orderWriteRepository.SaveAsync();
            return Ok(order);
        }
    }
}
