using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Memoriae.DAL.PostgreSQL.EF.Models
{
    public partial class PersonalContext : DbContext
    {
        public PersonalContext()
        {
        }

        public PersonalContext(DbContextOptions<PersonalContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<PostTagLink> PostTagLinks { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("pgcrypto")
                .HasAnnotation("Relational:Collation", "Russian_Russia.1251");

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("Post", "memoriae");

                entity.Property(e => e.Id).IsRequired().HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.Title)                  
                    .HasMaxLength(512);                
            });

            modelBuilder.Entity<PostTagLink>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("PostTagLink", "memoriae");

                entity.Property(e => e.Id).IsRequired().HasDefaultValueSql("gen_random_uuid()");                
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("Tag", "memoriae");                

                entity.Property(e => e.Id).IsRequired().HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(512);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
