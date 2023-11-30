using Microsoft.EntityFrameworkCore;

namespace RainfallAPI.Models.Errors;

public class ErrorResponseDbContext : DbContext
{
    public ErrorResponseDbContext(DbContextOptions<ErrorResponseDbContext> options)
        : base(options)
    {

    }
    public DbSet<ErrorResponse> ErrorResponses { get; set; }
}
