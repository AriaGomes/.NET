using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lab5.Models;

namespace Lab5.Data
{
    public class AnswerImageDataContext : DbContext
    {
        

        public AnswerImageDataContext(DbContextOptions<AnswerImageDataContext> options) : base(options)
        {
             
        }
        public DbSet<AnswerImage> AnswerImage { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnswerImage>().ToTable("AnswerImage");
        }
    }
}
