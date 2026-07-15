# Tabiby — Full Setup Guide (Backend + Frontend)

## ⚠️ Critical fix included in this version — read if upgrading from an earlier copy

**Bug:** `DataSeeder.cs` was creating roles as `"PATIENT"`, `"DOCTOR"`, etc. (all
caps), while every `[Authorize(Roles = "...")]` attribute in the app checks
for `"Patient"`, `"Doctor"`, etc. Since that check is case-sensitive, every
single protected endpoint returned `403 Forbidden` for every logged-in user
— appointments, consultations, pharmacy, nursing, all of it.

**Fixed in this package:**
- `backend/DAL/Data/DataSeeder.cs` now creates roles with correct casing —
  this is enough for a **brand-new** database.
- **If you already have an existing database** (you registered/tested
  before this fix), the seeding logic checks "does this role already exist"
  and will skip re-creating it — so the code fix alone won't retroactively
  fix your current database. Run `backend/fix-role-names.sql` once against
  your `Tabibydb` database (SSMS / Azure Data Studio / `sqlcmd`) to correct
  the existing rows directly.
- After running that SQL script, **log out and log back in** in the app —
  your current session's token still has the old, wrong-cased role baked
  into it; only a fresh login issues a corrected one.
- `frontend/src/utils/jwt.js` also normalizes role casing defensively on
  the frontend side, as a second line of defense.

---


This package contains both halves of the project, already wired together:

```
/backend    <- ProjectDEPI (ASP.NET Core API, .NET 9)
/frontend   <- tabiby-frontend (React + Vite)
```

Follow these steps **in order**. Each one only needs to be done once unless noted.

---

## 0. Prerequisites (one-time, per machine)

- **.NET 9 SDK** — https://dotnet.microsoft.com/download/dotnet/9.0
  Verify after installing (close/reopen your terminal first):
  ```powershell
  dotnet --version
  ```
  should print `9.0.x`.

- **Node.js 18+** — https://nodejs.org (LTS version). Verify:
  ```powershell
  node --version
  npm --version
  ```

- **SQL Server** (LocalDB is fine) — comes bundled with Visual Studio, or
  install SQL Server Express separately if you don't have VS.

- **Trust the local HTTPS certificate** (needed for the frontend to talk to
  `https://localhost:7292` without browser errors):
  ```powershell
  dotnet dev-certs https --trust
  ```

---

## 1. Backend setup

```powershell
cd backend
.\run-backend.ps1
```

This script restores NuGet packages (using the included `NuGet.Config`, which
pins package sources to `nuget.org` only — this avoids the "local source
doesn't exist" NU1301 errors some machines hit from stale/broken Visual
Studio NuGet configs) and then starts the API.

**If `run-backend.ps1` won't run** ("scripts disabled on this system"), run
this once, then retry:
```powershell
Set-ExecutionPolicy -Scope CurrentUser -ExecutionPolicy RemoteSigned
```

**First time only — create the database:**
```powershell
cd backend
dotnet tool install --global dotnet-ef   # only if you don't already have it
dotnet ef database update --project DAL --startup-project PL
```
Check `backend/PL/appsettings.json`'s `ConnectionStrings` if your SQL Server
isn't the default local instance — adjust `Server=.` to match yours.

Once running, confirm the console says it's listening on
`https://localhost:7292`. Leave this terminal open.

---

## 2. Frontend setup

Open a **second** terminal:

```powershell
cd frontend
.\run-frontend.ps1
```

This installs npm dependencies (first run only) and starts the dev server.
It's pinned to `http://localhost:3000` in `vite.config.js` — this must match
the backend's CORS allow-list, so don't let anything else grab port 3000
first.

`.env` already points `VITE_API_BASE_URL` at `https://localhost:7292/api` —
only edit this if your backend is running somewhere else.

---

## 3. First-time data: getting past an empty database

The backend's built-in data seeder has a hardcoded file path from the
original developer's machine, so it will likely fail silently — expect an
**empty database** on first run. To get a usable account:

1. Open the frontend at `http://localhost:3000` and **Register** a new
   account (always created as a Patient).
2. To test Doctor/Nurse/Pharmacist/Admin features, you need an Admin
   account, which can only be created by another Admin (chicken-and-egg).
   Promote your new account directly in the database:
   ```sql
   UPDATE AspNetUsers SET Discriminator = 'Admin' WHERE Email = 'your-registered-email';
   ```
   Log out and back in — you should land on the Admin Accounts screen.
3. From there, create Doctor / Nurse / Pharmacist accounts through the UI.

---

## 4. Testing checklist

With both servers running and DevTools' Network tab open:

1. **Register** a Patient → confirms the connection works end-to-end.
2. **Admin → create a Doctor** → log out, log back in as the Patient.
3. **Patient → Find a Doctor → book an appointment** and **request a
   consultation** with that doctor.
4. **Log in as the Doctor** (separate browser/incognito window) → accept
   the consultation → confirms the appointment/consultation flow.
5. **Chat**: with both Patient and Doctor windows open side by side, send
   messages back and forth — they should appear instantly on both sides
   with no refresh (SignalR working live).
6. **Admin → create a Pharmacist** → log in as them → add a medication
   with price/stock/image.
7. **Patient → Pharmacy** → confirm the medication shows up → add to cart
   → checkout → confirm it appears under Orders.
8. **Notifications**: trigger any of the above actions and confirm the
   bell icon badge updates live.

If something breaks, note exactly which step, and check the Network tab for
the failing request's status code and response body — that pinpoints the
issue immediately.

---

## Known backend contract gaps (already adapted for in the frontend)

- Nursing requests return no `Id`, so the nurse-side accept/reject list is
  read-only for now (see `frontend/README.md` for the fix needed).
- No doctor/patient names on some DTOs; no suspend/reactivate for admin
  accounts (only hard delete); medications have no category.
- Full list with code-level detail: `frontend/README.md`.

---

## 5. Recent System Updates & Fixes (Post-Delivery)

This project has been heavily updated with the following critical fixes:

### Security & Admin Controls
- **Admin Permissions Restricted:** Admins can no longer modify Patient accounts or interfere with other Admin accounts (suspension, deletion of Admin role, etc.). 
- **Pharmacist Account Creation:** The `PharmacyName` is no longer a required field. Admins can create Pharmacist accounts without providing a pharmacy name.

### Pharmacy & Orders
- **Order Items Fix:** The backend now eagerly loads medication details with orders using a new `AllOrdersSpecs`. Pharmacists can now correctly see the items inside each order instead of "0 items".

### UI / UX Cleanup
- **Removed Fake Features:** The "Leave Review" button on past appointments and the "Price" display on the doctor booking modal have been removed since they lack backend endpoints.
- **Removed Notifications from Dashboards:** The Notifications bell has been hidden from the Admin and Pharmacist sidebars.

### Codebase Cleanliness
- **Comments Stripped:** All comments (single-line `//` and multi-line `/* */`) have been thoroughly and safely stripped from the entire backend and frontend codebase.
