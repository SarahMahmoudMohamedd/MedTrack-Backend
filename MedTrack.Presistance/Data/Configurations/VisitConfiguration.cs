using MedTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Presistance.Data.Configurations
{
    public class VisitConfiguration : IEntityTypeConfiguration<Visit>
    {
        public void Configure(EntityTypeBuilder<Visit> builder)
        {
            builder.HasKey(v => v.Id);
            builder.Property(v => v.Diagnosis).IsRequired().HasMaxLength(1000);
            builder.Property(v => v.BloodPressure).HasMaxLength(20);
            builder.Property(v => v.Temperature).HasColumnType("decimal(4,2)");

            builder.HasOne(v => v.Patient).WithMany(p => p.Visits).HasForeignKey(v => v.PatientId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(v => v.Doctor).WithMany(d => d.Visits).HasForeignKey(v => v.DoctorId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}