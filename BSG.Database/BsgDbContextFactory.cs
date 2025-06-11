using BSG.BackEnd.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BSG.Database;

public class BsgDbContextFactory: IDesignTimeDbContextFactory<BsgDbContext>
{
    public BsgDbContext CreateDbContext(string[] args)
    {
        // Build config
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath( Path.Combine( Directory.GetCurrentDirectory(), "..\\Bsg.Api" ) )
            .AddJsonFile( "appsettings.json", optional: false, reloadOnChange: true )
            .AddEnvironmentVariables()
            .Build();

        // Get connection string
        var optionBuilder = new DbContextOptionsBuilder<BsgDbContext>();
        var connectionString = config.GetConnectionString( nameof( BsgDbContext ) );
        //optionBuilder.UseSqlServer( connectionString!, b => b.MigrationsAssembly( "BSG.Database" ) );
        optionBuilder.UseSqlite( connectionString!, b => b.MigrationsAssembly( "BSG.Database" ) );

        // ToDo: Add Migration User Resolver
        return new BsgDbContext( optionBuilder.Options, new DateConverterService() );
    }
}