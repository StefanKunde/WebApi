// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

namespace AspNetCore3ODataSample.Web.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Microsoft.AspNet.OData;
	using Microsoft.AspNetCore.Mvc;
	using Models;

	public class MoviesController : ODataController
    {
        private readonly MovieContext _context;

        private readonly IList<Movie> _inMemoryMovies;

        public MoviesController(MovieContext context)
        {
            this._context = context;

            if (!this._context.Movies.Any())
            {
                Movie conanMovie = new Movie
                {
                    Title = "Conan",
                    ReleaseDate = new DateTimeOffset(new DateTime(2017, 3, 3)),
                    Genre = Genre.Comedy,
                    Price = 1.99m
                };
                Movie dieHardMovie = new Movie
                {
                    Title = "Die Hard",
                    ReleaseDate = new DateTimeOffset(new DateTime(2014, 1, 3)),
                    Genre = Genre.Comedy,
                    Price = 1.89m
                };
                this._context.Movies.Add(conanMovie);
                this._context.Movies.Add(dieHardMovie);
                this._context.SaveChanges();
                MovieStar s = new MovieStar
                {
                    FirstName = "Arnold",
                    LastName = "Schwarzenegger",
                    MovieId = conanMovie.ID
                };
                this._context.MovieStars.Add(s);
                MovieStar b = new MovieStar
                {
                    FirstName = "Bruce",
                    LastName = "Willis",
                    MovieId = dieHardMovie.ID
                };
                this._context.MovieStars.Add(b);
                this._context.SaveChanges();
            }

            this._inMemoryMovies = new List<Movie>
            {
                new Movie
                {
                    ID = 1,
                    Title = "Conan",
                    ReleaseDate = new DateTimeOffset(new DateTime(2018, 3, 3)),
                    Genre = Genre.Comedy,
                    Price = 1.99m,
                    Stars = new List<MovieStar>{
                        new MovieStar
                        {
                            FirstName = "Arnold",
                            LastName = "Schwarzenegger",
                            MovieId = 1
                        },
                        new MovieStar
                        {
                            FirstName = "Jackie",
                            LastName = "Chan",
                            MovieId = 1
                        }
                    }
                },
                new Movie
                {
                    ID = 2,
                    Title = "James",
                    ReleaseDate = new DateTimeOffset(new DateTime(2017, 3, 3)),
                    Genre = Genre.Adult,
                    Price = 91.99m,
                    Stars = new List<MovieStar>{
                        new MovieStar
                        {
                            FirstName = "Bruce",
                            LastName = "Willis",
                            MovieId = 2
                        }
                    }
                }
            };
        }

        [EnableQuery]
        public IActionResult Get()
        {
            if (Request.Path.Value.Contains("efcore"))
            {
                return Ok(this._context.Movies);
            }
            else
            {
                return Ok(this._inMemoryMovies);
            }
        }

        [EnableQuery]
        public IActionResult Get(int key)
        {
            Movie m;
            if (Request.Path.Value.Contains("efcore"))
            {
                m = this._context.Movies.FirstOrDefault(c => c.ID == key);
            }
            else
            {
                m = this._inMemoryMovies.FirstOrDefault(c => c.ID == key);
            }

            if (m == null)
            {
                return NotFound();
            }

            return Ok(m);
        }

		[EnableQuery]
        public IActionResult Post([FromBody]Movie movie)
		{
			if (Request.Path.Value.Contains("efcore"))
			{
				this._context.Movies.Add(movie);
				this._context.SaveChanges();
			}
			else
			{
				movie.ID = new Random().Next(3, int.MaxValue);
				this._inMemoryMovies.Add(movie);
            }

			return this.Created(movie);
		}
    }
}
