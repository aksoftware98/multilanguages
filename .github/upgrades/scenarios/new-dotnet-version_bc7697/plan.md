# .NET 10 Multi-Target Migration Plan

## Table of Contents

- [Executive Summary](#executive-summary)
- [Migration Strategy](#migration-strategy)
- [Detailed Dependency Analysis](#detailed-dependency-analysis)
- [Project-by-Project Plans](#project-by-project-plans)
- [Package Update Reference](#package-update-reference)
- [Breaking Changes Catalog](#breaking-changes-catalog)
- [Risk Management](#risk-management)
- [Testing & Validation Strategy](#testing--validation-strategy)
- [Complexity & Effort Assessment](#complexity--effort-assessment)
- [Source Control Strategy](#source-control-strategy)
- [Success Criteria](#success-criteria)

---

## Executive Summary

### Scenario Overview

This plan outlines the migration of **AKSoftware.Localization.MultiLanguages** solution to add **.NET 10** support while maintaining existing target frameworks for backward compatibility.

**Scope:**
- **Included:** All non-UWP projects (11 projects)
- **Excluded:** UWP projects (3 projects: `AKSoftware.Localization.MultiLanguages.UWP`, `AKSoftware.Localization.MultiLanguages.UWP.Tests`, `UwpAkLocalization`)

**Key Constraint:** 
- **Library projects** → Multi-target with .NET 10 added
- **App projects** (Blazor/WinUI/Console/Benchmark) → Single-target to .NET 10 only

### Current State

| Metric | Value |
|--------|-------|
| Total Projects | 14 (11 in scope) |
| Total Issues | 49 (21 mandatory) |
| Affected Files | 21 |
| NuGet Packages | 31 (7 need upgrade, 3 incompatible) |
| API Behavioral Changes | 13 (low impact) |

### Migration Complexity

**Overall Difficulty:** 🟢 **Low-Medium**

- ✅ All in-scope projects are SDK-style
- ✅ No binary-incompatible APIs detected
- ⚠️ Package reference inconsistencies need cleanup
- ⚠️ 3 incompatible package findings in assessment (**2 WinUI behaviors findings in scope**, **1 UWP finding out of scope**)
- ⚠️ Deprecated xunit package in test project

### Success Criteria

1. ✅ All non-UWP projects build successfully targeting appropriate .NET 10 configuration
2. ✅ All tests pass (excluding UWP tests)
3. ✅ Sample applications run without runtime errors
4. ✅ Package references use ProjectReference for in-solution dependencies
5. ✅ No breaking API changes in public library surface

---

## Migration Strategy

### Approach: **Incremental Dependency-Order Migration**

We will upgrade projects in dependency order (foundation → dependents) to ensure each project builds against its updated dependencies before moving up the chain.

### UWP Scope Guardrail (Mandatory)

The following UWP projects are **explicitly excluded** and must remain unchanged in this plan:
- `AKSoftware.Localization.MultiLanguages.UWP`
- `AKSoftware.Localization.MultiLanguages.UWP.Tests`
- `UwpAkLocalization`

For these excluded projects:
- Do **not** change target frameworks
- Do **not** update package references
- Do **not** run migration fixes
- Do **not** include them in validation gates for this migration

### Migration Phases

#### Phase 1: Pre-Migration Cleanup (Low Risk)
**Goal:** Fix structural issues before framework changes

1. **Fix package reference inconsistencies**
   - Remove duplicate `PackageReference` in `AKSoftware.Localization.MultiLanguages.Benchmarks.csproj`
   - Replace `PackageReference` with `ProjectReference` in `BlazorWebApp.Sample.csproj`

2. **Verify .NET 10 SDK installation**

**Expected Duration:** 30 minutes  
**Risk Level:** Low

---

#### Phase 2: Foundation Library Migration (Medium Risk)
**Goal:** Multi-target core library with .NET 10

**Projects:**
1. `AKSoftware.Localization.MultiLanguages` (Level 0 - no dependencies)

**Actions:**
- Add `net10.0` to existing multi-target configuration
- Upgrade recommended packages (Microsoft.Extensions.DependencyInjection.Abstractions: 8.0.0 → 10.0.7)
- Remove `System.ComponentModel.Annotations` (included in framework)
- Build and test

**Expected Duration:** 1 hour  
**Risk Level:** Medium (foundation - impacts all dependent projects)

---

#### Phase 3: Level 1 Libraries (Medium Risk)
**Goal:** Multi-target platform-specific and test libraries

**Projects (parallel - no inter-dependencies):**
1. `AKSoftware.Localization.MultiLanguages.Blazor`
2. `AKSoftware.Localization.MultiLanguages.WinUI`
3. `AKSoftware.Localization.MultiLanguages.SourceGenerator`
4. `AKSoftware.Localization.MultiLanguages.Tests`

**Actions per project:**
- Add `net10.0` to existing targets
- Upgrade packages to 10.0.7 versions
- Address incompatible packages:
  - `Microsoft.Xaml.Behaviors.WinUI.Managed` in WinUI projects (known incompatible - requires investigation)
- Replace deprecated `xunit` (2.4.2) in test project
- Build and test

**Expected Duration:** 2 hours  
**Risk Level:** Medium (platform-specific behaviors may differ)

---

#### Phase 4: Application Projects (Low-Medium Risk)
**Goal:** Single-target apps to .NET 10

**Projects (can be done in parallel):**
1. `BlazorAKLocalization`
2. `BlazorServerLocalizationSample`
3. `BlazorWebApp.Sample`
4. `WinUIAkLocalization`
5. `ConsoleAppSample`
6. `AKSoftware.Localization.MultiLanguages.Benchmarks`

**Actions per project:**
- Change `TargetFrameworks` → `TargetFramework` (singular)
- Set `TargetFramework` to `net10.0` (or `net10.0-windows10.0.19041.0` for WinUI)
- Upgrade packages to 10.0.7
- Address behavioral changes (primarily `System.Uri` parsing strictness)
- Build and validate runtime behavior

**Expected Duration:** 2-3 hours  
**Risk Level:** Low-Medium (apps, not libraries - lower blast radius)

---

#### Phase 5: Final Validation & Testing (Low Risk)
**Goal:** End-to-end validation

1. Full solution build
2. Run all tests
3. Run sample applications
4. Validate package generation for libraries
5. Performance baseline comparison (benchmarks)

**Expected Duration:** 1 hour  
**Risk Level:** Low

---

### Rollback Strategy

Each phase commits to Git with descriptive messages. If a phase fails:
1. Review error logs
2. Attempt fix within time-box (30 min)
3. If unresolved, `git revert` the phase commit
4. Document blockers and seek guidance

### Total Estimated Duration

**6-8 hours** (including testing and documentation)

---

## Detailed Dependency Analysis

### Dependency Hierarchy

```
Level 0 (Foundation):
  └─ AKSoftware.Localization.MultiLanguages
      ├─ Current: netstandard2.0;net8.0;net9.0;net10.0
      ├─ Planned: netstandard2.0;net8.0;net9.0;net10.0 (already has net10.0)
      └─ Issues: 4 (2 mandatory: package upgrades)

Level 1 (Platform Libraries):
  ├─ AKSoftware.Localization.MultiLanguages.Blazor
  │   ├─ Depends on: Core library
  │   ├─ Current: net8.0;net9.0;net10.0
  │   ├─ Planned: net8.0;net9.0;net10.0 (already has net10.0)
  │   ├─ Used by: BlazorAKLocalization, BlazorServerLocalizationSample
  │   └─ Issues: 2 (1 mandatory: package upgrade)
  │
  ├─ AKSoftware.Localization.MultiLanguages.WinUI
  │   ├─ Depends on: Core library
  │   ├─ Current: net8.0-windows10.0.19041.0;net10.0-windows10.0.19041.0
  │   ├─ Planned: net8.0-windows10.0.19041.0;net10.0-windows10.0.19041.0 (already has net10.0)
  │   ├─ Used by: WinUIAkLocalization
  │   └─ Issues: 2 (2 mandatory: incompatible XAML behaviors package)
  │
  ├─ AKSoftware.Localization.MultiLanguages.SourceGenerator
  │   ├─ Depends on: Core library
  │   ├─ Current: netstandard2.0;net10.0
  │   ├─ Planned: netstandard2.0;net10.0 (already has net10.0)
  │   └─ Issues: 2 (1 mandatory: package upgrade)
  │
  └─ AKSoftware.Localization.MultiLanguages.Tests
      ├─ Depends on: Core library
      ├─ Current: net8.0;net10.0
      ├─ Planned: net8.0;net10.0 (already has net10.0)
      └─ Issues: 2 (1 mandatory: deprecated xunit)

Level 2 (Applications):
  ├─ BlazorAKLocalization
  │   ├─ Depends on: Core + Blazor libraries
  │   ├─ Current: net8.0;net10.0
  │   ├─ Planned: net10.0 (single-target per user constraint)
  │   └─ Issues: 8 (1 mandatory TFM, 4 package upgrades, 3 API behavioral)
  │
  ├─ BlazorServerLocalizationSample
  │   ├─ Depends on: Blazor library
  │   ├─ Current: net8.0;net10.0
  │   ├─ Planned: net10.0 (single-target per user constraint)
  │   └─ Issues: 2 (1 mandatory TFM, 1 API behavioral)
  │
  ├─ BlazorWebApp.Sample
  │   ├─ Depends on: Core + Blazor (via PackageReference - needs fixing)
  │   ├─ Current: net9.0;net10.0
  │   ├─ Planned: net10.0 (single-target per user constraint)
  │   └─ Issues: 2 (1 mandatory TFM, 1 API behavioral)
  │
  ├─ WinUIAkLocalization
  │   ├─ Depends on: Core + WinUI libraries
  │   ├─ Current: net8.0-windows10.0.19041.0;net10.0-windows10.0.19041.0
  │   ├─ Planned: net10.0-windows10.0.19041.0 (single-target per user constraint)
  │   └─ Issues: 9 (2 mandatory: TFM + incompatible package, 6 API behavioral)
  │
  ├─ ConsoleAppSample
  │   ├─ Depends on: Core library
  │   ├─ Current: net7.0;net10.0
  │   ├─ Planned: net10.0 (single-target per user constraint)
  │   └─ Issues: 1 (1 mandatory: TFM)
  │
  └─ AKSoftware.Localization.MultiLanguages.Benchmarks
      ├─ Depends on: Core library
      ├─ Current: net6.0;net10.0
      ├─ Planned: net10.0 (single-target per user constraint)
      └─ Issues: 1 (1 mandatory: TFM) + duplicate PackageReference

Excluded (UWP - out of scope):
  ├─ AKSoftware.Localization.MultiLanguages.UWP (net5.0)
  ├─ AKSoftware.Localization.MultiLanguages.UWP.Tests (net5.0)
  └─ UwpAkLocalization (net5.0)
```

### Critical Path Analysis

**Blocking Dependencies:**

1. **Phase 2 blocks Phase 3**: Core library must complete before platform libraries
2. **Phase 3 blocks Phase 4**: Platform libraries must complete before apps
3. **No parallelization possible** between phases due to dependency chain

**Within-Phase Parallelization:**

- **Phase 3**: All 4 Level 1 libraries can be done in parallel (no inter-dependencies)
- **Phase 4**: All 6 apps can be done in parallel (depend only on lower levels)

### Risk Propagation

**High-Risk Changes:**
1. Core library (`AKSoftware.Localization.MultiLanguages`) - impacts all 10 dependent projects
2. Blazor library - impacts 3 Blazor apps
3. WinUI library - impacts 1 WinUI app

**Mitigation:** Extensive testing at Level 0 and Level 1 before proceeding to apps.

---

## Project-by-Project Plans

---

### Phase 1: Pre-Migration Cleanup

#### 1.1 Fix Duplicate Reference - AKSoftware.Localization.MultiLanguages.Benchmarks

**File:** `src/AKSoftware.Localization.MultiLanguages.Benchmarks/AKSoftware.Localization.MultiLanguages.Benchmarks.csproj`

**Issue:** Contains both `PackageReference` and `ProjectReference` to core library.

**Action:**
Remove the `PackageReference` line:
```xml
<!-- REMOVE THIS -->
<PackageReference Include="AKSoftware.Localization.MultiLanguages" Version="5.8.0" />

<!-- KEEP THIS -->
<ProjectReference Include="..\AKSoftware.Localization.MultiLanguages\AKSoftware.Localization.MultiLanguages.csproj" />
```

**Validation:** `dotnet build` should succeed with only ProjectReference.

---

#### 1.2 Fix PackageReference → ProjectReference - BlazorWebApp.Sample

**File:** `src/BlazorWebApp.Sample/BlazorWebApp.Sample.csproj`

**Issue:** Uses NuGet packages for in-solution libraries.

**Action:**
Replace:
```xml
<!-- REMOVE THESE -->
<PackageReference Include="AKSoftware.Localization.MultiLanguages.Blazor" Version="6.0.0-alpha" />
<PackageReference Include="AKSoftware.Localization.MultiLanguages.SourceGenerator" Version="6.0.0-alpha" />

<!-- ADD THESE -->
<ProjectReference Include="..\AKSoftware.Localization.MultiLanguages.Blazor\AKSoftware.Localization.MultiLanguages.Blazor.csproj" />
<ProjectReference Include="..\AKSoftware.Localization.MultiLanguages.SourceGenerator\AKSoftware.Localization.MultiLanguages.SourceGenerator.csproj" />
```

**Validation:** `dotnet build src/BlazorWebApp.Sample` should succeed.

---

### Phase 2: Foundation Library Migration

#### 2.1 AKSoftware.Localization.MultiLanguages (Core Library)

**Current State:**
- Target: `netstandard2.0;net8.0;net9.0;net10.0`
- Already has `net10.0` target ✅

**Changes Required:**

1. **Update Package References:**
   ```xml
   <!-- Upgrade from 8.0.0 to 10.0.7 -->
   <PackageReference Include="Microsoft.Extensions.DependencyInjection.Abstractions" Version="10.0.7" />
   <PackageReference Include="Microsoft.AspNetCore.Components.Forms" Version="10.0.7" />
   ```

2. **Remove Framework-Included Package:**
   ```xml
   <!-- DELETE THIS LINE -->
   <PackageReference Include="System.ComponentModel.Annotations" Version="5.0.0" />
   ```

**Build Command:**
```bash
dotnet build src/AKSoftware.Localization.MultiLanguages/AKSoftware.Localization.MultiLanguages.csproj -c Release
```

**Test Command:**
```bash
dotnet test src/AKSoftware.Localization.MultiLanguages.Tests/AKSoftware.Localization.MultiLanguages.Tests.csproj --framework net10.0
```

**Success Criteria:**
- ✅ Builds for all targets (netstandard2.0, net8.0, net9.0, net10.0)
- ✅ Tests pass on net10.0
- ✅ No new warnings

---

### Phase 3: Level 1 Libraries

#### 3.1 AKSoftware.Localization.MultiLanguages.Blazor

**Current State:**
- Target: `net8.0;net9.0;net10.0`
- Already has `net10.0` target ✅

**Changes Required:**

1. **Update Package:**
   ```xml
   <PackageReference Include="Microsoft.AspNetCore.Components.Web" Version="10.0.7" />
   ```

**Build/Test:**
```bash
dotnet build src/AKSoftware.Localization.MultiLanguages.Blazor/AKSoftware.Localization.MultiLanguages.Blazor.csproj -c Release
```

**Success Criteria:** Builds all targets with no warnings.

---

#### 3.2 AKSoftware.Localization.MultiLanguages.WinUI

**Current State:**
- Target: `net8.0-windows10.0.19041.0;net10.0-windows10.0.19041.0`
- Already has `net10.0-windows10.0.19041.0` target ✅

**Changes Required:**

**⚠️ BLOCKER INVESTIGATION:** Test with existing `Microsoft.Xaml.Behaviors.WinUI.Managed` 2.0.9

1. **First: Build and test**
   ```bash
   dotnet build src/AKSoftware.Localization.MultiLanguages.WinUI/AKSoftware.Localization.MultiLanguages.WinUI.csproj --framework net10.0-windows10.0.19041.0
   ```

2. **If incompatibility error occurs:**
   - Check for package updates: `dotnet list package --outdated`
   - Search NuGet.org for newer versions
   - If no solution, suppress warning and runtime test

**Success Criteria:**
- ✅ Builds for net10.0-windows target
- ✅ WinUI XAML behaviors work at runtime (test in WinUIAkLocalization app)

---

#### 3.3 AKSoftware.Localization.MultiLanguages.SourceGenerator

**Current State:**
- Target: `netstandard2.0;net10.0`
- Already has `net10.0` target ✅

**Changes Required:**

1. **Update Package:**
   ```xml
   <PackageReference Include="Microsoft.Extensions.DependencyInjection.Abstractions" Version="10.0.7" />
   ```

**Build:**
```bash
dotnet build src/AKSoftware.Localization.MultiLanguages.SourceGenerator/AKSoftware.Localization.MultiLanguages.SourceGenerator.csproj -c Release
```

**Success Criteria:** Builds both targets (netstandard2.0, net10.0).

---

#### 3.4 AKSoftware.Localization.MultiLanguages.Tests

**Current State:**
- Target: `net8.0;net10.0`
- Already has `net10.0` target ✅

**Changes Required:**

1. **Optional: Upgrade xunit (recommended but not mandatory)**
   ```xml
   <PackageReference Include="xunit" Version="2.9.0" />
   ```

**Test:**
```bash
dotnet test src/AKSoftware.Localization.MultiLanguages.Tests/AKSoftware.Localization.MultiLanguages.Tests.csproj --framework net10.0
```

**Success Criteria:** All tests pass on net10.0.

---

### Phase 4: Application Projects

**User Constraint:** Apps should be **single-targeted** to `.NET 10` only (remove other targets).

---

#### 4.1 BlazorAKLocalization (Blazor WebAssembly App)

**Current State:** `net8.0;net10.0` (multi-targeted)

**Changes Required:**

1. **Change to single-target:**
   ```xml
   <!-- BEFORE -->
   <TargetFrameworks>net8.0;net10.0</TargetFrameworks>

   <!-- AFTER -->
   <TargetFramework>net10.0</TargetFramework>
   ```

2. **Update Packages:**
   ```xml
   <PackageReference Include="Microsoft.AspNetCore.Components.WebAssembly" Version="10.0.7" />
   <PackageReference Include="Microsoft.AspNetCore.Components.WebAssembly.DevServer" Version="10.0.7" />
   <PackageReference Include="System.Net.Http.Json" Version="10.0.7" />
   ```

**Test:**
```bash
dotnet run --project src/BlazorAKLocalization/BlazorAKLocalization.csproj
# Verify app loads and language switching works
```

**Success Criteria:**
- ✅ Builds
- ✅ Runs without exceptions
- ✅ Language switching functional

---

#### 4.2 BlazorServerLocalizationSample (Blazor Server App)

**Current State:** `net8.0;net10.0` (multi-targeted)

**Changes Required:**

1. **Change to single-target:**
   ```xml
   <TargetFramework>net10.0</TargetFramework>
   ```

**Test:**
```bash
dotnet run --project src/BlazorServerLocalizationSample/BlazorServerLocalizationSample.csproj
```

**Success Criteria:** App runs and localizes correctly.

---

#### 4.3 BlazorWebApp.Sample (Blazor Web App)

**Current State:** `net9.0;net10.0` (multi-targeted)

**Changes Required:**

1. **Change to single-target:**
   ```xml
   <TargetFramework>net10.0</TargetFramework>
   ```

**Test:**
```bash
dotnet run --project src/BlazorWebApp.Sample/BlazorWebApp.Sample.csproj
```

**Success Criteria:** App runs with no errors.

---

#### 4.4 WinUIAkLocalization (WinUI App)

**Current State:** `net8.0-windows10.0.19041.0;net10.0-windows10.0.19041.0` (multi-targeted)

**Changes Required:**

1. **Change to single-target:**
   ```xml
   <TargetFramework>net10.0-windows10.0.19041.0</TargetFramework>
   ```

2. **Update Package:**
   ```xml
   <PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="10.0.7" />
   ```

**Test:**
```bash
dotnet build src/WinUIAkLocalization/WinUIAkLocalization.csproj
# Run from Visual Studio or:
# .\src\WinUIAkLocalization\bin\Debug\net10.0-windows10.0.19041.0\WinUIAkLocalization.exe
```

**Success Criteria:**
- ✅ Builds
- ✅ Runs
- ✅ XAML behaviors work (MultiBinding with localization)

---

#### 4.5 ConsoleAppSample

**Current State:** `net7.0;net10.0` (multi-targeted)

**Changes Required:**

1. **Change to single-target:**
   ```xml
   <TargetFramework>net10.0</TargetFramework>
   ```

**Test:**
```bash
dotnet run --project src/ConsoleAppSample/ConsoleAppSample.csproj
```

**Success Criteria:** Console app runs and displays localized text.

---

#### 4.6 AKSoftware.Localization.MultiLanguages.Benchmarks

**Current State:** `net6.0;net10.0` (multi-targeted)

**Changes Required:**

1. **Change to single-target:**
   ```xml
   <TargetFramework>net10.0</TargetFramework>
   ```

**Test:**
```bash
dotnet run --project src/AKSoftware.Localization.MultiLanguages.Benchmarks/AKSoftware.Localization.MultiLanguages.Benchmarks.csproj -c Release
```

**Success Criteria:** Benchmarks run and complete successfully.

---

### Projects Excluded from Migration (UWP - Out of Scope)

The following projects remain unchanged:
- ❌ `AKSoftware.Localization.MultiLanguages.UWP` (net5.0)
- ❌ `AKSoftware.Localization.MultiLanguages.UWP.Tests` (net5.0)
- ❌ `UwpAkLocalization` (net5.0)

These projects will continue targeting UWP and are not part of this migration.

---

## Package Update Reference

### Packages Requiring Upgrades

| Package | Current | Target | Projects Affected | Action |
|---------|---------|--------|-------------------|--------|
| Microsoft.Extensions.DependencyInjection.Abstractions | 8.0.0 | 10.0.7 | Core, SourceGenerator | Upgrade |
| Microsoft.AspNetCore.Components.Forms | 3.1.32 | 10.0.7 | Core | Upgrade |
| Microsoft.AspNetCore.Components.Web | 8.0.0 | 10.0.7 | Blazor library | Upgrade |
| Microsoft.AspNetCore.Components.WebAssembly | 8.0.0 | 10.0.7 | BlazorAKLocalization | Upgrade |
| Microsoft.AspNetCore.Components.WebAssembly.DevServer | 8.0.0 | 10.0.7 | BlazorAKLocalization | Upgrade |
| System.Net.Http.Json | 5.0.0 | 10.0.7 | BlazorAKLocalization | Upgrade |
| Microsoft.Extensions.DependencyInjection | 8.0.0 | 10.0.7 | WinUIAkLocalization | Upgrade |

### Packages to Remove

| Package | Current | Projects | Reason |
|---------|---------|----------|--------|
| System.ComponentModel.Annotations | 5.0.0 | Core | Included in .NET 10 framework |

### Deprecated Packages (Replacement Needed)

| Package | Current | Replacement | Projects | Notes |
|---------|---------|-------------|----------|-------|
| xunit | 2.4.2 | xunit (latest stable) | Tests | Current version deprecated, but compatible. Recommend upgrading to latest 2.x or evaluate 3.x preview |

### Incompatible Packages (In-Scope Blockers)

| Package | Current | Projects | Status | Resolution |
|---------|---------|----------|--------|------------|
| Microsoft.Xaml.Behaviors.WinUI.Managed | 2.0.9 | WinUI library, WinUIAkLocalization | ⚠️ Compatibility warning | Validate on .NET 10 via build + runtime first; upgrade only if needed. |

> Note: A separate incompatible package finding exists for UWP behaviors, but UWP projects are explicitly out of scope for this migration.

### Duplicate/Inconsistent References (Pre-Migration Fix)

| Project | Issue | Resolution |
|---------|-------|------------|
| Benchmarks | Has both PackageReference AND ProjectReference to core library | Remove PackageReference, keep ProjectReference only |
| BlazorWebApp.Sample | Uses PackageReference for in-solution libraries | Replace with ProjectReference |

### Package Update Commands (for reference)

```bash
# Core library
dotnet add src/AKSoftware.Localization.MultiLanguages/AKSoftware.Localization.MultiLanguages.csproj package Microsoft.Extensions.DependencyInjection.Abstractions -v 10.0.7
dotnet add src/AKSoftware.Localization.MultiLanguages/AKSoftware.Localization.MultiLanguages.csproj package Microsoft.AspNetCore.Components.Forms -v 10.0.7
dotnet remove src/AKSoftware.Localization.MultiLanguages/AKSoftware.Localization.MultiLanguages.csproj package System.ComponentModel.Annotations

# Blazor library
dotnet add src/AKSoftware.Localization.MultiLanguages.Blazor/AKSoftware.Localization.MultiLanguages.Blazor.csproj package Microsoft.AspNetCore.Components.Web -v 10.0.7

# BlazorAKLocalization
dotnet add src/BlazorAKLocalization/BlazorAKLocalization.csproj package Microsoft.AspNetCore.Components.WebAssembly -v 10.0.7
dotnet add src/BlazorAKLocalization/BlazorAKLocalization.csproj package Microsoft.AspNetCore.Components.WebAssembly.DevServer -v 10.0.7
dotnet add src/BlazorAKLocalization/BlazorAKLocalization.csproj package System.Net.Http.Json -v 10.0.7

# WinUIAkLocalization
dotnet add src/WinUIAkLocalization/WinUIAkLocalization.csproj package Microsoft.Extensions.DependencyInjection -v 10.0.7
```

---

## Breaking Changes Catalog

### API Behavioral Changes (Low Impact)

#### 1. System.Uri Constructor - Stricter Validation (Api.0003)

**Severity:** 🔵 Low (behavioral change, not binary incompatible)

**Affected Projects:**
- `BlazorAKLocalization` (Program.cs, line 23)
- `WinUIAkLocalization` (generated files - likely safe)
- `UwpAkLocalization` (out of scope)

**Change Description:**
Starting in .NET 5+, `System.Uri` constructor has stricter validation for URI strings. Malformed URIs that were previously accepted may now throw `UriFormatException`.

**Impact Analysis:**
```csharp
// Affected code in BlazorAKLocalization\Program.cs:23
builder.Services.AddScoped(sp => new HttpClient { 
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
});
```

**Risk Assessment:** ✅ **Very Low**
- `builder.HostEnvironment.BaseAddress` is framework-provided, well-formed base URI
- WinUI generated files use `ms-appx:///` protocol URIs (framework-generated)
- No user-provided or constructed URIs

**Action Required:** None (monitor for runtime exceptions during testing)

**Mitigation (if issues occur):**
```csharp
// Add validation/error handling if needed
if (Uri.TryCreate(builder.HostEnvironment.BaseAddress, UriKind.Absolute, out var baseUri))
{
    builder.Services.AddScoped(sp => new HttpClient { BaseAddress = baseUri });
}
```

**References:**
- [System.Uri behavioral changes](https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/5.0/uri-parsing-of-file-paths)

---

#### 2. ASP.NET Core Exception Handler - Default Behavior Change (Api.0003)

**Severity:** 🔵 Low (behavioral change)

**Affected Projects:**
- Blazor apps (if using `UseExceptionHandler`)

**Change Description:**
In .NET 10, `UseExceptionHandler` behavior for status code pages may differ slightly. Default error handling paths have changed.

**Risk Assessment:** ✅ **Very Low**
- No explicit `UseExceptionHandler` calls found in sample code
- Standard Blazor error handling (ErrorBoundary components) unaffected

**Action Required:** None (standard Blazor error handling patterns are compatible)

---

### Package-Related Breaking Changes

#### 3. Microsoft.Xaml.Behaviors.WinUI.Managed - Compatibility Warning from Assessment (NuGet.0001)

**Severity:** 🟡 **Medium-High** (can block build, but not confirmed framework breaking change)

**Affected Projects (in scope):**
- `AKSoftware.Localization.MultiLanguages.WinUI`
- `WinUIAkLocalization`

**Important clarification:**
There is **no known official .NET 9 → .NET 10 breaking change** specifically documented for `Microsoft.Xaml.Behaviors.WinUI.Managed`.

**Issue interpretation:**
The analyzer flag indicates a **package compatibility concern** for the selected target framework, not a confirmed .NET runtime/API breaking change.

**Investigation Required:**
1. Check if newer package version exists with explicit .NET 10 support
2. Verify whether current warning is a false positive via build + runtime validation
3. Confirm `MultiBindingBehavior` and other behavior usage works in `WinUIAkLocalization`

**Recommended Actions:**
1. **First:** Build and test with existing 2.0.9 version (may be acceptable)
2. **If fails:** Check NuGet for updates: `dotnet list package --outdated`
3. **If no updates:** Consider alternatives or targeted warning suppression only after runtime validation

**Workaround (if needed):**
```xml
<!-- Suppress compatibility warnings if runtime testing succeeds -->
<PackageReference Include="Microsoft.Xaml.Behaviors.WinUI.Managed" Version="2.0.9">
  <NoWarn>NU1701</NoWarn>
</PackageReference>
```

---

#### 4. xunit - Deprecated Package (NuGet.0005)

**Severity:** 🟡 **Medium** (optional - works but deprecated)

**Affected Projects:**
- `AKSoftware.Localization.MultiLanguages.Tests`

**Issue:**
xunit 2.4.2 is deprecated (though still functional).

**Recommended Action:**
Upgrade to latest stable xunit 2.x:
```bash
dotnet add src/AKSoftware.Localization.MultiLanguages.Tests/AKSoftware.Localization.MultiLanguages.Tests.csproj package xunit -v 2.9.0
```

**Note:** xunit 3.x is in preview - recommend staying on 2.x stable for now.

---

### Summary of Breaking Changes

| Change | Severity | Projects | Action | Phase |
|--------|----------|----------|--------|-------|
| System.Uri validation | 🔵 Low | Blazor/WinUI apps | Monitor during testing | Phase 4 |
| ASP.NET ExceptionHandler | 🔵 Low | Blazor apps | None (not used) | Phase 4 |
| XAML Behaviors incompatible | 🔴 High | WinUI projects | Investigate/test | Phase 3 |
| xunit deprecated | 🟡 Medium | Tests | Optional upgrade | Phase 3 |

**Overall Risk:** 🟢 **Low** - No showstopper breaking changes identified.

---

## Risk Management

### Risk Assessment Matrix

| Risk | Probability | Impact | Mitigation | Owner Phase |
|------|-------------|--------|------------|-------------|
| XAML Behaviors package incompatible | Medium | High | Test first; suppress warnings if runtime OK; find alternative if broken | Phase 3 |
| System.Uri validation breaks Blazor app | Low | Medium | Monitor runtime; add try-catch if needed | Phase 4 |
| Core library change breaks dependents | Low | High | Extensive testing before proceeding to Phase 3 | Phase 2 |
| Multi-target build configuration errors | Low | Low | Review .csproj carefully; incremental build | All Phases |
| Package version conflicts | Low | Medium | Use consistent 10.0.7 versions across solution | Phase 2-4 |
| Test failures on .NET 10 | Low | Medium | Fix or skip; document; don't block unless critical | Phase 5 |

### Contingency Plans

#### If Phase 2 (Core Library) Fails
**Trigger:** Core library doesn't build or tests fail on .NET 10

**Response:**
1. Revert Phase 2 commit
2. Isolate issue:
   - Build errors → check package versions and SDK
   - Test failures → identify failing tests and assess severity
3. If blocking: pause migration, escalate for investigation
4. If non-critical: document, skip, proceed

---

#### If XAML Behaviors Package Blocks Phase 3
**Trigger:** WinUI projects won't build due to package incompatibility

**Response:**
1. Check for package updates: `dotnet list package --outdated`
2. Search NuGet.org and GitHub for newer versions or forks
3. If no solution exists:
   - **Option A:** Suppress compatibility warning and runtime test
   - **Option B:** Remove XAML behaviors dependency (breaking change - requires code refactor)
   - **Option C:** Keep WinUI projects on .NET 8 target only (partial migration)

**Decision Criteria:** Runtime testing results dictate which option.

---

#### If App Projects Have Runtime Errors (Phase 4)
**Trigger:** Apps build but crash or behave incorrectly at runtime

**Response:**
1. Review error stack traces
2. Check for `System.Uri` issues in Blazor apps
3. Test WinUI XAML behaviors functionality
4. Add error handling/validation as needed
5. If unfixable: rollback that app, document blocker, continue with others

---

### Rollback Triggers

**Immediate Rollback Conditions:**
1. Core library (Phase 2) has critical bugs after 1 hour of troubleshooting
2. More than 3 apps fail with same issue (systemic problem)
3. Security vulnerability discovered in .NET 10 packages (unlikely but possible)

**Rollback Process:**
```bash
# Identify failing phase commit
git log --oneline

# Revert specific commit
git revert <commit-sha>

# Or reset to before migration
git reset --hard feature/winui3-library
```

---

## Testing & Validation Strategy

### Testing Pyramid

```
                    /\
                   /  \
                  / E2E \          Phase 5: Full integration
                 /______\
                /        \
               / Service  \        Phase 4: App validation
              /   Tests    \
             /______________\
            /                \
           /   Unit Tests     \   Phase 2-3: Library testing
          /____________________\

```

---

### Phase-Specific Testing

#### Phase 1: Pre-Migration Cleanup
**Test Type:** Build validation

**Commands:**
```bash
# Verify Benchmarks builds with ProjectReference only
dotnet build src/AKSoftware.Localization.MultiLanguages.Benchmarks/AKSoftware.Localization.MultiLanguages.Benchmarks.csproj

# Verify BlazorWebApp.Sample builds with ProjectReferences
dotnet build src/BlazorWebApp.Sample/BlazorWebApp.Sample.csproj
```

**Success Criteria:** Both projects build without errors.

---

#### Phase 2: Core Library
**Test Type:** Unit + Integration

**Commands:**
```bash
# Build all targets
dotnet build src/AKSoftware.Localization.MultiLanguages/AKSoftware.Localization.MultiLanguages.csproj -c Release

# Run tests on .NET 10
dotnet test src/AKSoftware.Localization.MultiLanguages.Tests/AKSoftware.Localization.MultiLanguages.Tests.csproj --framework net10.0 --logger "console;verbosity=detailed"

# Optional: Run tests on all targets to ensure backward compatibility
dotnet test src/AKSoftware.Localization.MultiLanguages.Tests/AKSoftware.Localization.MultiLanguages.Tests.csproj --logger "console;verbosity=detailed"
```

**Success Criteria:**
- ✅ Builds for netstandard2.0, net8.0, net9.0, net10.0
- ✅ All unit tests pass on net10.0
- ✅ No new warnings

**Critical Tests to Monitor:**
- Language loading from embedded resources
- Culture switching
- DI service registration

---

#### Phase 3: Platform Libraries
**Test Type:** Build + Smoke Test

**Commands:**
```bash
# Blazor library
dotnet build src/AKSoftware.Localization.MultiLanguages.Blazor/AKSoftware.Localization.MultiLanguages.Blazor.csproj -c Release

# WinUI library (CRITICAL - watch for XAML behaviors issue)
dotnet build src/AKSoftware.Localization.MultiLanguages.WinUI/AKSoftware.Localization.MultiLanguages.WinUI.csproj --framework net10.0-windows10.0.19041.0

# SourceGenerator
dotnet build src/AKSoftware.Localization.MultiLanguages.SourceGenerator/AKSoftware.Localization.MultiLanguages.SourceGenerator.csproj -c Release

# Tests
dotnet test src/AKSoftware.Localization.MultiLanguages.Tests/AKSoftware.Localization.MultiLanguages.Tests.csproj --framework net10.0
```

**Success Criteria:**
- ✅ All libraries build
- ✅ Tests pass
- ⚠️ **WinUI library builds** (may have warnings - check if they block dependents)

---

#### Phase 4: Applications
**Test Type:** End-to-End Runtime Validation

**Test Matrix:**

| App | Build Test | Run Test | Functional Test | Priority |
|-----|------------|----------|-----------------|----------|
| BlazorAKLocalization | ✅ `dotnet build` | ✅ `dotnet run` | ✅ Language switching in browser | High |
| BlazorServerLocalizationSample | ✅ `dotnet build` | ✅ `dotnet run` | ✅ Server-side rendering with localization | High |
| BlazorWebApp.Sample | ✅ `dotnet build` | ✅ `dotnet run` | ✅ Blazor Web App functionality | Medium |
| WinUIAkLocalization | ✅ `dotnet build` | ✅ Launch .exe | ✅ XAML behaviors + localization | High |
| ConsoleAppSample | ✅ `dotnet build` | ✅ `dotnet run` | ✅ Console output shows localized text | Low |
| Benchmarks | ✅ `dotnet build` | ✅ `dotnet run -c Release` | ✅ Benchmarks complete | Low |

**Functional Test Scripts:**

**Blazor Apps:**
1. Navigate to `http://localhost:5000` (or assigned port)
2. Verify page loads without errors (check browser console)
3. Click language selector
4. Verify UI text changes to selected language
5. Refresh page - verify language persists (if using LocalStorage)

**WinUI App:**
1. Launch from bin directory or Visual Studio
2. Verify main window opens
3. Interact with UI elements using localized strings
4. Verify XAML behaviors (e.g., `MultiBindingBehavior`) work
5. Check for XAML parsing errors

**Console App:**
1. Run and capture output
2. Verify localized strings appear (not resource keys)

**Benchmarks:**
1. Run and let complete
2. Review performance metrics (not validation target, but should complete)

---

#### Phase 5: Full Solution Validation
**Test Type:** Integration + Regression

**Commands:**
```bash
# Full solution build
dotnet build src/AKSoftware.Localization.MultiLanguages.sln -c Release

# All tests (excluding UWP)
dotnet test src/AKSoftware.Localization.MultiLanguages.sln --framework net10.0

# Package generation test (for libraries)
dotnet pack src/AKSoftware.Localization.MultiLanguages/AKSoftware.Localization.MultiLanguages.csproj -c Release
dotnet pack src/AKSoftware.Localization.MultiLanguages.Blazor/AKSoftware.Localization.MultiLanguages.Blazor.csproj -c Release
dotnet pack src/AKSoftware.Localization.MultiLanguages.WinUI/AKSoftware.Localization.MultiLanguages.WinUI.csproj -c Release
```

**Success Criteria:**
- ✅ Entire solution builds
- ✅ All tests pass (excluding UWP projects)
- ✅ NuGet packages generate successfully for all 4 library projects
- ✅ All sample apps run without errors

**Regression Checks:**
- Compare test results net8.0 vs net10.0 (if multi-targeted tests exist)
- Verify package metadata (version, dependencies)
- Check for unexpected breaking changes in public APIs

---

### Test Data & Scenarios

**Language Files to Test:**
- en-US (primary)
- ar-SA (RTL language)
- ja-JP (non-Latin script)

**Edge Cases:**
- Missing translation keys (fallback behavior)
- Invalid culture codes
- Empty resource files

---

### Performance Baseline (Optional)

Run benchmarks before and after migration:
```bash
# Before (on feature/winui3-library)
dotnet run --project src/AKSoftware.Localization.MultiLanguages.Benchmarks -c Release > baseline-before.txt

# After (on upgrade/net10-multitarget-non-uwp)
dotnet run --project src/AKSoftware.Localization.MultiLanguages.Benchmarks -c Release > baseline-after.txt

# Compare results
```

**Expected:** Performance should be similar or improved (not a gate, but document any regressions).

---

## Complexity & Effort Assessment

### Complexity Score: **3.5 / 10** 🟢 (Low-Medium)

#### Factors Increasing Complexity:
- ❗ XAML Behaviors package incompatibility (uncertainty factor) → +2
- ❗ Multi-target vs single-target constraint adds precision work → +1
- ❗ 11 projects in scope requiring changes → +0.5

#### Factors Decreasing Complexity:
- ✅ All SDK-style projects (no conversion needed) → -2
- ✅ No binary-incompatible APIs → -2
- ✅ Most projects already have .NET 10 targets → -3
- ✅ Clear dependency hierarchy (no circular dependencies) → -1
- ✅ Good test coverage → -1

**Net Complexity:** Low-Medium

---

### Effort Breakdown

| Phase | Task Count | Estimated Time | Confidence |
|-------|------------|----------------|------------|
| **Phase 1: Pre-Migration** | 2 tasks | 30 min | High ⭐⭐⭐ |
| **Phase 2: Core Library** | 1 project | 1 hour | High ⭐⭐⭐ |
| **Phase 3: Platform Libraries** | 4 projects | 2 hours | Medium ⭐⭐ |
| **Phase 4: Apps** | 6 projects | 2-3 hours | Medium ⭐⭐ |
| **Phase 5: Validation** | Full solution | 1 hour | High ⭐⭐⭐ |
| **Buffer/Troubleshooting** | - | 1 hour | - |
| **Total** | 13 work items | **7-8 hours** | Medium ⭐⭐ |

---

### Detailed Effort Per Phase

#### Phase 1: Pre-Migration Cleanup (30 min)
- **Effort:** 🔵 Trivial
- **Tasks:**
  - Edit 2 .csproj files (remove/replace references)
  - Test build (2 projects)
- **Risk:** Very Low

#### Phase 2: Foundation Library (1 hour)
- **Effort:** 🟡 Low-Medium
- **Tasks:**
  - Update 2 packages
  - Remove 1 package
  - Build (4 targets)
  - Run unit tests
  - Investigate any test failures
- **Risk:** Medium (foundation - high impact)

#### Phase 3: Platform Libraries (2 hours)
- **Effort:** 🟡 Medium
- **Tasks:**
  - Update packages in 4 projects
  - Build each (multiple targets)
  - **WinUI investigation** (potentially 30-60 min if issues)
  - Run tests
- **Risk:** Medium (XAML behaviors uncertainty)

#### Phase 4: Applications (2-3 hours)
- **Effort:** 🟡 Medium
- **Tasks:**
  - Edit 6 .csproj files (change multi-target to single-target)
  - Update packages
  - Build each
  - Run and functionally test each app
  - Address any runtime issues
- **Risk:** Low-Medium (apps, not libraries)

#### Phase 5: Validation (1 hour)
- **Effort:** 🔵 Low
- **Tasks:**
  - Full solution build
  - All tests
  - Package generation
  - Documentation update
- **Risk:** Low

---

### Team Skill Requirements

| Skill | Required Level | Phase(s) |
|-------|----------------|----------|
| .csproj editing | Intermediate | All |
| .NET multi-targeting | Intermediate | 2-3 |
| Package management | Intermediate | 2-4 |
| Blazor development | Basic | 4 |
| WinUI/XAML | Intermediate | 3-4 |
| Git version control | Basic | All |
| Debugging | Intermediate | 3-5 |

**Recommended:** Developer with .NET 8+ experience and familiarity with Blazor/WinUI.

---

### Automation Opportunities

**Could be automated (if repeated for other solutions):**
- Package version updates via script
- `TargetFrameworks` → `TargetFramework` conversion
- Batch build/test commands

**Example PowerShell script:**
```powershell
# Update all Microsoft.AspNetCore packages to 10.0.7
Get-ChildItem -Recurse -Filter *.csproj | ForEach-Object {
    $content = Get-Content $_.FullName
    $content = $content -replace 'Version="8\.0\.0"', 'Version="10.0.7"'
    Set-Content $_.FullName $content
}
```

**For this migration:** Manual approach recommended due to:
- Custom constraints (multi-target vs single-target per project type)
- Need for validation at each phase
- Unknown XAML behaviors issue requiring investigation

---

### Confidence Level: **Medium (70%)**

**Unknowns:**
1. Will XAML Behaviors package actually block WinUI projects? (30% chance it's false positive)
2. Will Blazor apps have runtime issues with System.Uri changes? (10% chance)
3. Will tests pass cleanly on .NET 10? (80% chance yes)

**If all goes smoothly:** 6 hours  
**If major issue (e.g., XAML Behaviors):** 10-12 hours  
**Most likely:** 7-8 hours

---

## Source Control Strategy

### Branch Structure

**Current Branch:** `upgrade/net10-multitarget-non-uwp` (created from `feature/winui3-library`)

**Commit Strategy:** One commit per phase for atomic rollback capability.

### Commit Messages Template

```
Phase [N]: [Phase Name]

- [Specific change 1]
- [Specific change 2]
- Build status: [Success/Failed]
- Tests status: [Pass/Fail/Skipped]

Co-authored-by: GitHub Copilot <noreply@github.com>
```

### Expected Commits

1. `Phase 1: Pre-Migration Cleanup - Fix package references`
2. `Phase 2: Foundation Library - Add .NET 10 multi-target to core library`
3. `Phase 3: Level 1 Libraries - Add .NET 10 to Blazor/WinUI/SourceGenerator/Tests`
4. `Phase 4: Application Projects - Single-target apps to .NET 10`
5. `Phase 5: Final Validation - Update documentation and validation results`

### Pull Request Strategy

After all phases complete successfully:
1. Create PR: `upgrade/net10-multitarget-non-uwp` → `feature/winui3-library`
2. PR Description should include:
   - Link to this plan.md
   - Build/test results summary
   - Breaking changes (if any)
   - Migration notes for consumers

---

## Success Criteria

### Overall Migration Success

The migration is considered **successful** when all criteria below are met:

---

### ✅ Build Success Criteria

| Criterion | Target | Verification |
|-----------|--------|--------------|
| **Core library builds** | All 4 targets (netstandard2.0, net8.0, net9.0, net10.0) | `dotnet build` returns 0 exit code |
| **Platform libraries build** | All multi-target configs | `dotnet build` per library |
| **All apps build** | Single net10.0 target each | `dotnet build` per app |
| **Full solution builds** | Release configuration | `dotnet build -c Release` on solution |
| **No new warnings** | 0 new warnings vs. baseline | Compare build output |
| **Package generation** | 4 NuGet packages created | `.nupkg` files in bin/Release |

**Command to verify:**
```bash
dotnet build src/AKSoftware.Localization.MultiLanguages.sln -c Release
```

---

### ✅ Test Success Criteria

| Criterion | Target | Verification |
|-----------|--------|--------------|
| **Unit tests pass** | 100% pass rate on net10.0 | `dotnet test --framework net10.0` |
| **No test regressions** | Same pass rate as net8.0 | Compare test results |
| **Test coverage maintained** | No decrease in coverage | Optional: coverage report |

**Command to verify:**
```bash
dotnet test src/AKSoftware.Localization.MultiLanguages.sln --framework net10.0 --logger "console;verbosity=detailed"
```

---

### ✅ Runtime Success Criteria (Apps)

| App | Criterion | Verification |
|-----|-----------|--------------|
| **BlazorAKLocalization** | Loads in browser without errors | Manual: open http://localhost:5xxx |
| | Language switching works | Manual: change language, verify UI updates |
| **BlazorServerLocalizationSample** | Renders server-side | Manual: open and interact |
| | Localization functional | Manual: verify translations |
| **BlazorWebApp.Sample** | App runs | Manual: `dotnet run` and test |
| **WinUIAkLocalization** | Launches and renders | Manual: run .exe |
| | XAML behaviors work | Manual: verify MultiBinding localization |
| **ConsoleAppSample** | Prints localized text | Manual: `dotnet run`, check output |
| **Benchmarks** | Completes without crash | Manual: `dotnet run -c Release` |

---

### ✅ Code Quality Criteria

| Criterion | Target | Verification |
|-----------|--------|--------------|
| **No duplicate dependencies** | Only ProjectReferences for in-solution libs | Review .csproj files |
| **Consistent package versions** | All Microsoft 10.0.x packages at 10.0.7 | Review .csproj files |
| **No breaking API changes** | Public library APIs unchanged | Review code + tests |
| **Documentation updated** | README.md reflects .NET 10 support | Review markdown |

---

### ✅ Git/Version Control Criteria

| Criterion | Target | Verification |
|-----------|--------|--------------|
| **Atomic commits** | One commit per phase (5 total) | `git log --oneline` |
| **Descriptive messages** | Each commit explains changes | Review git log |
| **Branch clean** | No uncommitted changes | `git status` |
| **PR ready** | Branch ahead of feature/winui3-library | Can create PR |

---

### ⚠️ Known Acceptable Deviations

The following are **acceptable** and do **not** constitute failure:

1. **XAML Behaviors Warning:** If `Microsoft.Xaml.Behaviors.WinUI.Managed` produces compatibility warning but works at runtime → PASS (suppress warning)
2. **xunit Deprecated Notice:** Warning about xunit 2.4.2 deprecation → PASS (optional upgrade, not mandatory)
3. **UWP Projects:** UWP projects not building/testing on .NET 10 → PASS (out of scope)

---

### 🔴 Failure Conditions (Rollback Triggers)

The migration **fails** if any of these occur:

1. ❌ Core library does not build on net10.0
2. ❌ More than 50% of tests fail on net10.0 (vs. passing on net8.0)
3. ❌ Any Blazor app crashes on startup after migration
4. ❌ WinUI app won't launch due to XAML behaviors blocking issue
5. ❌ Breaking changes introduced in public library APIs

**If failure occurs:** Revert, document blocker, escalate for investigation.

---

### Final Checklist (Before Marking Complete)

- [ ] All 11 in-scope projects build successfully
- [ ] Unit tests pass on .NET 10
- [ ] 6 sample apps tested and functional
- [ ] NuGet packages generated
- [ ] Git commits are clean and descriptive
- [ ] `plan.md` and `assessment.md` updated with findings
- [ ] README.md updated to reflect .NET 10 support
- [ ] PR created: `upgrade/net10-multitarget-non-uwp` → `feature/winui3-library`

**Approval:** Mark migration complete when checklist is ✅ and team approves PR.
