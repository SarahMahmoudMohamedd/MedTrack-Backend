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
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.NationalId).IsRequired().HasMaxLength(14);
            builder.HasIndex(p => p.NationalId).IsUnique(); // لمنع التكرار تماماً كما طلب الـ UI

            builder.Property(p => p.FullName).IsRequired().HasMaxLength(200);
            builder.Property(p => p.PhoneNumber).HasMaxLength(20);
            builder.Property(p => p.Email).HasMaxLength(150);
            builder.Property(p => p.BloodType).HasMaxLength(5);

            builder.Property(p => p.EmergencyContactName).HasMaxLength(200);
            builder.Property(p => p.EmergencyRelationship).HasMaxLength(50);
            builder.Property(p => p.EmergencyPhoneNumber).HasMaxLength(20);
        }
    }
}
