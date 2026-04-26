using Microsoft.EntityFrameworkCore;
using VeterinariaApp.Models;

namespace VeterinariaApp.Data
{
    public class MySqlDBContext : DbContext
    {
        public MySqlDBContext(DbContextOptions<MySqlDBContext> options)
            : base(options)
        {
        }

        // 🔹 TABLAS
        public DbSet<Propietario> Propietarios { get; set; }
        public DbSet<Mascota> Mascotas { get; set; }
        public DbSet<Veterinario> Veterinarios { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<Tratamiento> Tratamientos { get; set; }
        public DbSet<Medicamento> Medicamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔥 PROPIETARIO
            modelBuilder.Entity<Propietario>()
                .HasIndex(p => p.Documento)
                .IsUnique();

            modelBuilder.Entity<Propietario>()
                .HasIndex(p => p.Email)
                .IsUnique();

            // 🔥 RELACIÓN Propietario → Mascotas (1:N)
            modelBuilder.Entity<Mascota>()
                .HasOne(m => m.Propietario)
                .WithMany(p => p.Mascotas)
                .HasForeignKey(m => m.PropietarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔥 RELACIÓN Mascota → Citas (1:N)
            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Mascota)
                .WithMany(m => m.Citas)
                .HasForeignKey(c => c.MascotaId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔥 RELACIÓN Veterinario → Citas (1:N)
            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Veterinario)
                .WithMany(v => v.Citas)
                .HasForeignKey(c => c.VeterinarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔥 RELACIÓN Cita → Tratamiento (1:1)
            modelBuilder.Entity<Tratamiento>()
                .HasOne(t => t.Cita)
                .WithOne(c => c.Tratamiento)
                .HasForeignKey<Tratamiento>(t => t.CitaId);

            // 🔥 RELACIÓN Tratamiento → Medicamentos (1:N)
            modelBuilder.Entity<Medicamento>()
                .HasOne(m => m.Tratamiento)
                .WithMany(t => t.Medicamentos)
                .HasForeignKey(m => m.TratamientoId)
                .OnDelete(DeleteBehavior.Cascade);
        } 
    }
}