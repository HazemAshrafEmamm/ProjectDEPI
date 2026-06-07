using DAL.Models;
using DAL.Models.AppointmentModule;
using DAL.Models.OrderModule;
using DAL.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;

namespace DAL.Data
{
    public class DataSeeder
    {
        private readonly TabibyDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private const string SEED_DATA_FOLDER = "DataSeeding";
        private readonly string _seedPath;

        public DataSeeder(
            TabibyDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager )
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _seedPath = "D:\\projects\\Tabiby Project\\Yuossef-ezzat\\ProjectDEPI\\DAL\\Data\\";
        }
        public async Task SeedDatabaseAsync()
        {
            try
            {
                // Check if data already exists
                if (await _context.Users.AnyAsync())
                {
                    Console.WriteLine("Database already seeded. Skipping...");
                    return;
                }

                Console.WriteLine("Starting database seeding...");

                // 0. Seed Roles 
                await SeedRolesAsync();
                Console.WriteLine(" Roles Schedules seeded successfully");

                // 1. Seed Users
                await SeedUsersAsync();
                Console.WriteLine("✓ Users seeded successfully");

                // 2. Seed Patients
                await SeedPatientsAsync();
                Console.WriteLine("✓ Patients seeded successfully");

                // 3. Seed Doctors
                await SeedDoctorsAsync();
                Console.WriteLine("✓ Doctors seeded successfully");

                // 4. Seed Nurses
                await SeedNursesAsync();
                Console.WriteLine("✓ Nurses seeded successfully");

                // 5. Seed Pharmacists
                await SeedPharmacistsAsync();
                Console.WriteLine("✓ Pharmacists seeded successfully");

                // 6. Seed Medications
                await SeedMedicationsAsync();
                Console.WriteLine("✓ Medications seeded successfully");

                // 7. Seed Doctor Schedules
                await SeedDoctorSchedulesAsync();
                Console.WriteLine(" Doctor Schedules seeded successfully");

                

                Console.WriteLine(" Database seeding completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error during seeding: {ex.Message}");
                throw;
            }
        }

        private async Task SeedUsersAsync()
        {
            var usersSeedPath = Path.Combine(_seedPath, SEED_DATA_FOLDER, "users-seed.json");
            if (!File.Exists(usersSeedPath))
            {
                Console.WriteLine(" Users seed file not found");
                return;
            }

            var json = await File.ReadAllTextAsync(usersSeedPath);
            var seedData = JsonSerializer.Deserialize<List<UserSeedDto>>(json);

            if (seedData== null) return;

            foreach (var userSeed in seedData)
            {
                var existingUser = await _userManager.FindByEmailAsync(userSeed.Email);
                if (existingUser != null) continue;

                var user = new ApplicationUser
                {
                    Id = userSeed.Id,
                    UserName = userSeed.UserName,
                    Email = userSeed.Email,
                    Fullname = userSeed.Fullname,
                    IsActive = userSeed.IsActive
                };

                var result = await _userManager.CreateAsync(user, userSeed.Password);

                if (!result.Succeeded)
                {
                    Console.WriteLine($"User failed {user.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(userSeed.Role))
                {
                    if (await _roleManager.RoleExistsAsync(userSeed.Role))
                    {
                        await _userManager.AddToRoleAsync(user, userSeed.Role);
                    }
                    else
                    {
                        Console.WriteLine($"Role not found: {userSeed.Role}");
                    }
                }
            }
            await _context.SaveChangesAsync();
        }

        private async Task SeedRolesAsync()
        {
            var RolesSeedPath = Path.Combine(_seedPath, SEED_DATA_FOLDER, "Roles.json");
            if (!File.Exists(RolesSeedPath))
            {
                Console.WriteLine(" Roles seed file not found");
                return;
            }

            var json = await File.ReadAllTextAsync(RolesSeedPath);
            var roles = JsonSerializer.Deserialize<List<IdentityRole>>(json);

            if (roles is null)
                return;

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role.Name))
                {
                    await _roleManager.CreateAsync(role);
                }
            }
        }

        private async Task SeedPatientsAsync()
        {
            var patientsSeedPath = Path.Combine(_seedPath, SEED_DATA_FOLDER, "patients-seed.json");
            if (!File.Exists(patientsSeedPath)) return;

            var json = await File.ReadAllTextAsync(patientsSeedPath);
            var seedData = JsonSerializer.Deserialize<List<Patient>>(json);

            if (seedData is null) return;

            foreach (var patientSeed in seedData)
            {
                var existingPatient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == patientSeed.Id);
                if (existingPatient != null) continue;

                _context.Patients.Add(patientSeed);
            }

            await _context.SaveChangesAsync();
        }

        private async Task SeedDoctorsAsync()
        {
            var doctorsSeedPath = Path.Combine(_seedPath, SEED_DATA_FOLDER, "doctors-seed.json");
            if (!File.Exists(doctorsSeedPath)) return;

            var json = await File.ReadAllTextAsync(doctorsSeedPath);
            var seedData = JsonSerializer.Deserialize<List<Doctor>>(json);

            if (seedData == null) return;

            foreach (var doctorSeed in seedData)
            {
                var existingDoctor = await _context.Doctors.FirstOrDefaultAsync(d => d.Id == doctorSeed.Id);
                if (existingDoctor != null) continue;

                _context.Doctors.Add(doctorSeed);
            }
            await _context.SaveChangesAsync();
        }

        private async Task SeedNursesAsync()
        {
            var nursesSeedPath = Path.Combine(_seedPath, SEED_DATA_FOLDER, "nurses-seed.json");
            if (!File.Exists(nursesSeedPath)) return;

            var json = await File.ReadAllTextAsync(nursesSeedPath);
            var seedData = JsonSerializer.Deserialize<List<Nurse>>(json);

            if (seedData == null) return;

            foreach (var nurseSeed in seedData)
            {
                var existingNurse = await _context.Nurses.FirstOrDefaultAsync(n => n.Id == nurseSeed.Id);
                if (existingNurse != null) continue;

                _context.Nurses.Add(nurseSeed);
            }

            await _context.SaveChangesAsync();
        }

        private async Task SeedPharmacistsAsync()
        {
            var pharmacistsSeedPath = Path.Combine(_seedPath, SEED_DATA_FOLDER, "pharmacists-seed.json");
            if (!File.Exists(pharmacistsSeedPath)) return;

            var json = await File.ReadAllTextAsync(pharmacistsSeedPath);
            var seedData = JsonSerializer.Deserialize<List<Pharmacist>>(json);

            if (seedData == null) return;

            foreach (var pharmacistSeed in seedData)
            {
                var existingPharmacist = await _context.Pharmacists.FirstOrDefaultAsync(p => p.Id == pharmacistSeed.Id);
                if (existingPharmacist != null) continue;
                _context.Pharmacists.Add(pharmacistSeed);
            }

            await _context.SaveChangesAsync();
        }

        private async Task SeedMedicationsAsync()
        {
            var medicationsSeedPath = Path.Combine(_seedPath, SEED_DATA_FOLDER, "medications-seed.json");
            if (!File.Exists(medicationsSeedPath)) return;

            var json = await File.ReadAllTextAsync(medicationsSeedPath);
            var seedData = JsonSerializer.Deserialize<List<Medication>>(json);

            if (seedData == null) return;

            foreach (var medicationSeed in seedData)
            {
                var existingMedication = await _context.Medications.FirstOrDefaultAsync(m => m.Id == medicationSeed.Id);
                if (existingMedication != null) continue;

                _context.Medications.Add(medicationSeed);
            }

            await _context.SaveChangesAsync();
        }
        private async Task SeedDoctorSchedulesAsync()
        {
            var schedulesSeedPath = Path.Combine(_seedPath, SEED_DATA_FOLDER, "doctorSchedules-seed.json");
            if (!File.Exists(schedulesSeedPath)) return;

            var json = await File.ReadAllTextAsync(schedulesSeedPath);
            var seedData = JsonSerializer.Deserialize<List<DoctorSchedule>>(json);

            if (seedData == null) return;

            foreach (var scheduleSeed in seedData)
            {
                var existingSchedule = await _context.DoctorSchedules.FirstOrDefaultAsync(s => s.Id == scheduleSeed.Id);
                if (existingSchedule != null) continue;

                _context.DoctorSchedules.Add(scheduleSeed);
            }

            await _context.SaveChangesAsync();
        }

        public class UserSeedDto
        {
            public string Id { get; set; } = null!;

            public string Fullname { get; set; } = null!;

            public string UserName { get; set; } = null!;

            public string Email { get; set; } = null!;

            public string Password { get; set; } = null!;

            public bool IsActive { get; set; }

            public string Role { get; set; } = null!;
        }
    }
}