-- Run this ONCE against your existing Tabibydb database.
--
-- Needed because: DataSeeder.cs originally created role names in all
-- caps ("PATIENT", "DOCTOR", etc). That's now fixed in the source code for
-- any FUTURE database, but the seeding logic checks "does this role already
-- exist" before creating one -- and since your database already has these
-- roles (just with the wrong casing), the code fix alone will NOT correct
-- an existing database. This script does that one-time correction directly.
--
-- Only the Name column needs to change. Leave NormalizedName as-is (it's
-- supposed to stay uppercase -- that's how ASP.NET Identity does
-- case-insensitive lookups internally). It's the Name column that gets
-- embedded in JWTs and checked by [Authorize(Roles = "...")].

UPDATE AspNetRoles SET Name = 'Patient'    WHERE Name = 'PATIENT';
UPDATE AspNetRoles SET Name = 'Doctor'     WHERE Name = 'DOCTOR';
UPDATE AspNetRoles SET Name = 'Admin'      WHERE Name = 'ADMIN';
UPDATE AspNetRoles SET Name = 'Nurse'      WHERE Name = 'NURSE';
UPDATE AspNetRoles SET Name = 'Pharmacist' WHERE Name = 'PHARMACIST';

-- Verify the fix:
SELECT Id, Name, NormalizedName FROM AspNetRoles;
