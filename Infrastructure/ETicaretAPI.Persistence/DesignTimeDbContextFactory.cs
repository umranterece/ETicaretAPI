using ETicaretAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ETicaretAPIDbContext>
    {
        // Bu Yapi migrationlari powershell uzerinden yonetmek icindir. Ama biz pm console kullanacagiz. 
        public ETicaretAPIDbContext CreateDbContext(string[] args)
        {
           
            DbContextOptionsBuilder<ETicaretAPIDbContext> dbContextOptionsBuilder = new();
            dbContextOptionsBuilder.UseNpgsql(Configuration.ConnectionString);

            return new ETicaretAPIDbContext(dbContextOptionsBuilder.Options);
        }

    }
}
