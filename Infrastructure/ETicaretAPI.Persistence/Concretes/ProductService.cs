using ETicaretAPI.Application.Abstractions;
using ETicaretAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Concretes
{
    public class ProductService : IProductService
    {
        public List<Product> GetProducts() => new() 
        {
            new(){ Id=Guid.NewGuid(),Name="Product 1", Price=100,Stock=10,CreatedDate=DateTime.Now }, 
            new(){ Id=Guid.NewGuid(),Name="Product 2", Price=673,Stock=4,CreatedDate=DateTime.Now }, 
            new(){ Id=Guid.NewGuid(),Name="Product 3", Price=425,Stock=2,CreatedDate=DateTime.Now }, 
            new(){ Id=Guid.NewGuid(),Name="Product 4", Price=335,Stock=7,CreatedDate=DateTime.Now }
        };
       
    }
}
