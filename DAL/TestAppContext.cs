using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CognitoTest.DAL
{
    public class TestAppContext : IdentityDbContext
    {
        
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(
                "data source=.; Initial Catalog=AuctionDB; Trusted_Connection=True;");
        }
    }
}
