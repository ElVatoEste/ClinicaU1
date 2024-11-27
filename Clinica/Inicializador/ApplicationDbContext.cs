using Microsoft.EntityFrameworkCore;
using Clinica.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Clinica.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet properties for each entity
        public DbSet<ConsultaMedica> ConsultasMedicas { get; set; }
        public DbSet<Detalle> Detalles { get; set; }
        public DbSet<DetalleFactura> DetallesFactura { get; set; }
        public DbSet<DetalleReceta> DetallesRecetas { get; set; }
        public DbSet<Diagnostico> Diagnosticos { get; set; }
        public DbSet<Expediente> Expedientes { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<FacturaMedicamento> FacturasMedicamentos { get; set; }
        public DbSet<Medicamento> Medicamentos { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Receta> Recetas { get; set; }
        public DbSet<SignosVitales> SignosVitales { get; set; }
        public DbSet<TipoExamen> TiposExamen { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite Primary Key for DetalleRecetas
            modelBuilder.Entity<DetalleReceta>()
                .HasKey(dr => new { dr.RecetaID, dr.MedicamentoID });

            // Unique Constraint on Expediente.PacienteID
            modelBuilder.Entity<Expediente>()
                .HasIndex(e => e.PacienteID)
                .IsUnique();

            // ConsultasMedicas - Expediente (Many-to-One)
            modelBuilder.Entity<ConsultaMedica>()
                .HasOne(cm => cm.Expediente)
                .WithMany(e => e.ConsultasMedicas)
                .HasForeignKey(cm => cm.ExpedienteID)
                .OnDelete(DeleteBehavior.Cascade);

            // ConsultasMedicas - SignosVitales (Many-to-One)
            modelBuilder.Entity<ConsultaMedica>()
                .HasOne(cm => cm.SignosVitales)
                .WithMany(sv => sv.ConsultasMedicas)
                .HasForeignKey(cm => cm.SignosVitalesID);

            // Detalle - TipoExamen (Many-to-One)
            modelBuilder.Entity<Detalle>()
                .HasOne(d => d.TipoExamen)
                .WithMany(te => te.Detalles)
                .HasForeignKey(d => d.TipoExamenID);

            // DetalleFactura - Factura (Many-to-One)
            modelBuilder.Entity<DetalleFactura>()
                .HasOne(df => df.Factura)
                .WithMany(f => f.DetallesFactura)
                .HasForeignKey(df => df.FacturaID)
                .OnDelete(DeleteBehavior.Cascade);

            // DetalleFactura - Detalle (Many-to-One)
            modelBuilder.Entity<DetalleFactura>()
                .HasOne(df => df.Detalle)
                .WithMany(d => d.DetallesFactura)
                .HasForeignKey(df => df.DetalleID)
                .OnDelete(DeleteBehavior.Cascade);

            // Diagnostico - ConsultaMedica (Many-to-One)
            modelBuilder.Entity<Diagnostico>()
                .HasOne(d => d.ConsultaMedica)
                .WithMany(cm => cm.Diagnosticos)
                .HasForeignKey(d => d.ConsultaMedicaID)
                .OnDelete(DeleteBehavior.Cascade);

            // Factura - Paciente (Many-to-One)
            modelBuilder.Entity<Factura>()
                .HasOne(f => f.Paciente)
                .WithMany(p => p.Facturas)
                .HasForeignKey(f => f.PacienteID);

            // FacturaMedicamento - Factura (Many-to-One)
            modelBuilder.Entity<FacturaMedicamento>()
                .HasOne(fm => fm.Factura)
                .WithMany(f => f.FacturaMedicamentos)
                .HasForeignKey(fm => fm.FacturaID)
                .OnDelete(DeleteBehavior.Cascade);

            // FacturaMedicamento - Medicamento (Many-to-One)
            modelBuilder.Entity<FacturaMedicamento>()
                .HasOne(fm => fm.Medicamento)
                .WithMany(m => m.FacturaMedicamentos)
                .HasForeignKey(fm => fm.MedicamentoID)
                .OnDelete(DeleteBehavior.Cascade);

            // Paciente - Expediente (One-to-One)
            modelBuilder.Entity<Paciente>()
                .HasOne(p => p.Expediente)
                .WithOne(e => e.Paciente)
                .HasForeignKey<Expediente>(e => e.PacienteID);

            // Receta - Diagnostico (Many-to-One)
            modelBuilder.Entity<Receta>()
                .HasOne(r => r.Diagnostico)
                .WithMany(d => d.Recetas)
                .HasForeignKey(r => r.DiagnosticoID)
                .OnDelete(DeleteBehavior.Cascade);

            // DetalleReceta - Receta (Many-to-One)
            modelBuilder.Entity<DetalleReceta>()
                .HasOne(dr => dr.Receta)
                .WithMany(r => r.DetallesReceta)
                .HasForeignKey(dr => dr.RecetaID)
                .OnDelete(DeleteBehavior.Cascade);

            // DetalleReceta - Medicamento (Many-to-One)
            modelBuilder.Entity<DetalleReceta>()
                .HasOne(dr => dr.Medicamento)
                .WithMany(m => m.DetalleRecetas)
                .HasForeignKey(dr => dr.MedicamentoID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
