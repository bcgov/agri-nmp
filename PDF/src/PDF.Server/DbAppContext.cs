/*
 * REST API Documentation for the  Application
 *
 * 
 *
 * OpenAPI spec version: v1
 * 
 * 
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace PDF.Models
{
    public interface IDbAppContextFactory
    {
        IDbAppContext Create();
    }

    public class DbAppContextFactory : IDbAppContextFactory
    {
        DbContextOptions<DbAppContext> _options;

        public DbAppContextFactory(DbContextOptions<DbAppContext> options)
        {
            _options = options;
        }

        public IDbAppContext Create()
        {
            return new DbAppContext(_options);
        }
    }

    public interface IDbAppContext
    {
           
        int SaveChanges();
    }

    public class DbAppContext : DbContext, IDbAppContext
    {
        /// <summary>
        /// Constructor for Class used for Entity Framework access.
        /// </summary>
        public DbAppContext(DbContextOptions<DbAppContext> options)
                                : base(options)
        { }

        /// <summary>
        /// Override for OnModelCreating - used to change the database naming convention.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // add our naming convention extension
            //modelBuilder.UpperCaseUnderscoreSingularConvention();
        }

        // Add methods here to get and set items in the model.
        // For example:

     
        
    }
}
