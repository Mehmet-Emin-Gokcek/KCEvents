using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KCEvents.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

    namespace KCEvents.Data
    {
        public class EventDbContext : IdentityDbContext<IdentityUser>
    {
            public DbSet<Event> Events { get; set; }
            public DbSet<EventCategory> Categories { get; set; }
            public DbSet<Tag> Tags { get; set; }
             public DbSet<EventTag> EventTags { get; set; }
             public DbSet<EventAddress> Addresses{ get; set; }



        public EventDbContext(DbContextOptions<EventDbContext> options)
                  : base(options)
            {
            }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventTag>()
                  .HasKey(et => new { et.EventId, et.TagId });
            base.OnModelCreating(modelBuilder);

        }
    }
  }