using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nestly.Model.Entity;

namespace Nestly.Services.Data.Seeders
{
    public static class DoctorProfileSeeder
    {
        public static void SeedData(this EntityTypeBuilder<DoctorProfile> entity)
        {
            entity.HasData(new DoctorProfile
            {
                Id = 1,
                UserId = 2
            });
        }
    }
}