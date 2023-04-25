using EBOOK.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace EBOOK.Data
{
    public class EbookDbContext : IdentityDbContext
    {
        public EbookDbContext(DbContextOptions<EbookDbContext> options) : base(options)
        {

        }
        public DbSet<Ebook> Ebooks { get; set; }
        public DbSet<EBOOK.Models.projectRole> projectRole { get; set; }
        
    }
}
