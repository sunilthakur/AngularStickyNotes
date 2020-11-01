using Microsoft.EntityFrameworkCore;
using StickyNotesAPIService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StickyNotesAPIService.Data
{
    public class NotesDbContext:DbContext
    {
        public NotesDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Notes> StickyNotes { get; set; }
    }
}
