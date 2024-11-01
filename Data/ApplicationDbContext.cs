using LetterKnowledgeAssessment.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LetterKnowledgeAssessment.Data
{
    public class ApplicationDbContext : IdentityDbContext<Teacher>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ClassList> ClassLists { get; set; }
        public DbSet<Pupil> Pupils { get; set; }
        public DbSet<LetterSoundKnowledgeTestResult> LetterKnowledgeTestResults { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Teacher>()
                .HasMany(t => t.ClassLists)
                .WithOne(c => c.Teacher)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ClassList>()
                .HasMany(c => c.Pupils)
                .WithOne(p => p.ClassList)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Pupil>()
                .HasMany(p => p.LetterSoundKnowledgeTestResults)
                .WithOne(r => r.Pupil)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<LetterSoundKnowledgeTestResult>()
                .HasMany(t => t.LetterTestResult)
                .WithOne(letter => letter.TestResult)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}