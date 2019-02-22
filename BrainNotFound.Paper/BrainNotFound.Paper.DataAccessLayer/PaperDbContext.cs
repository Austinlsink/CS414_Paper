using BrainNotFound.Paper.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrainNotFound.Paper.DataAccessLayer
{
    public class PaperDbContext : DbContext
    {
        public DbSet<Department> Departments { get; set; } // Example
        public DbSet<Course> Courses { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<SectionMeetingTime> SectionMeetingTimes { get; set; }


        // Constructor
        public PaperDbContext(DbContextOptions<PaperDbContext> options) 
            : base(options)
        {  }
    }
}
