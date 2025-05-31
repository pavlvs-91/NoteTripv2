using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoteTrip.Models;

namespace NoteTrip.Data
{
    public class NoteTripContext : DbContext
    {
        public NoteTripContext (DbContextOptions<NoteTripContext> options)
            : base(options)
        {
        }

        public DbSet<NoteTrip.Models.User> User { get; set; } = default!;
        public DbSet<NoteTrip.Models.Country> Country { get; set; } = default!;
        public DbSet<NoteTrip.Models.Region> Region { get; set; } = default!;
        public DbSet<NoteTrip.Models.City> City { get; set; } = default!;
        public DbSet<NoteTrip.Models.TouristAttraction> TouristAttraction { get; set; } = default!;
    }
}
