using AbySalto.Junior.Models;
using Microsoft.EntityFrameworkCore;

namespace AbySalto.Junior.Infrastructure.Database
{
    public interface IApplicationDbContext
    {
        DbSet<Narudzba> Narudzbe { get; set; }
        DbSet<StavkaNarudzbe> StavkeNarudzbe { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}