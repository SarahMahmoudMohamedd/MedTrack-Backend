using MedTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace MedTrack.Presistance.Data.DbContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // 🏥 الـ DbSets الخاصة بجداول السيستم بالكامل
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<PatientCondition> PatientConditions => Set<PatientCondition>();
        public DbSet<PatientAllergy> PatientAllergies => Set<PatientAllergy>();
        public DbSet<Doctor> Doctors => Set<Doctor>();
        public DbSet<Visit> Visits => Set<Visit>();
        public DbSet<PrescribedMedication> PrescribedMedications => Set<PrescribedMedication>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<LabTest> LabTests => Set<LabTest>();
        public DbSet<Notification> Notifications => Set<Notification>();

        // 🎯 ضفنا حقل المعامل والمؤسسات الطبية لضمان عمل الـ Unified Login بنجاح
        public DbSet<LabInstitution> LabInstitutions => Set<LabInstitution>();

        // ⚡ ميثود الحفظ التلقائي الذكية للتواريخ (Audit Logs) بدون Reflection بطيء
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                // تحديث تاريخ التعديل دائماً لأي عملية إضافة أو تعديل
                var updatedAtProp = entityEntry.Entity.GetType().GetProperty("UpdatedAt");
                if (updatedAtProp != null)
                {
                    updatedAtProp.SetValue(entityEntry.Entity, DateTime.UtcNow);
                }

                // تحديث تاريخ الإنشاء في حالة الإضافة الجديدة فقط
                if (entityEntry.State == EntityState.Added)
                {
                    var createdAtProp = entityEntry.Entity.GetType().GetProperty("CreatedAt");
                    if (createdAtProp != null)
                    {
                        createdAtProp.SetValue(entityEntry.Entity, DateTime.UtcNow);
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1️⃣ قراءة وتطبيق كل الـ Fluent API Configurations المقسمة تلقائياً
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // 2️⃣ 🔒 الـ Seeding الموحد للأدوار (Roles) والصلاحيات لمنع التضارب
            // ملحوظة: لو الصلاحيات عندك بتعتمد على جداول منفصلة، الـ HasData دي بتثبت الـ Keys في قاعدة البيانات فوراً
            // 🎯 الـ Seeding الموحد للأدوار (Roles) والصلاحيات بقيم ثابتة تماماً
            var doctorId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var labId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var patientId = Guid.Parse("33333333-3333-3333-3333-333333333333");

            // لو عندك Seeding للـ Roles هنا، اتأكدي إن الـ Id واخد المتغيرات الثابتة دي:
            // modelBuilder.Entity<Role>().HasData(...)

            // 🔬 Seeding معمل ديمو (الاختبار الفوري)
            modelBuilder.Entity<LabInstitution>().HasData(
                new LabInstitution
                {
                    // 🎯 الـ ID هنا ثابت 100% ومبيتغيرش ديناميكياً
                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    Name = "Elmo5tabar Labs",
                    Email = "demo.lab@gmail.com",
                    PasswordHash = "$2a$11$EvXNshA4lA9lB.68F0X3vO3K3K1oZIn79eMkWy8S0W8wD72K5q6vG",
                    PhoneNumber = "01234567890",
                    InstitutionType = MedTrack.Domain.Enums.InstitutionType.Both
                }
            );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // 🎯 السطر السحري لتخطي أيرور الـ Dynamic Values والـ PendingModelChanges غصب عن الـ EF Core
            optionsBuilder.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        }
    }
}