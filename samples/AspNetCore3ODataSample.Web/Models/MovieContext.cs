// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace AspNetCore3ODataSample.Web.Models
{
	using Microsoft.EntityFrameworkCore;

	public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieStar> MovieStars { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieStar>().HasKey(_ => new
            {
                _.FirstName,
                _.LastName
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
