using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySchedulerWork.Models;

namespace MySchedulerWork.Data
{
    public class MyAppContext : DbContext
    {
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Audience> Audiences { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Pair> Pairs { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<CourseProgram> CourseProgram { get; set; }
        public DbSet<Program_Subject> Program_Subjects { get; set; }
        //public DbSet<ProgramToGroup> ProgramToGroups { get; set; }
        public DbSet<PairToFGroup> PairToFGroup { get; set; }


        public MyAppContext(DbContextOptions<MyAppContext> options)
        : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies();
        }
    }
}
