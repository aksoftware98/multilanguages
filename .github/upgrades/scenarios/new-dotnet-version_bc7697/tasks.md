# AKSoftware.Localization.MultiLanguages .NET 10 Multi-Target Upgrade Tasks

## Overview

This document tracks the execution of adding .NET 10 support to the AKSoftware.Localization.MultiLanguages solution while maintaining existing target frameworks. All non-UWP projects (11 projects) will be upgraded using an all-at-once approach with package updates and validation.

**Progress**: 1/3 tasks complete (33%) ![0%](https://progress-bar.xyz/33)

---

## Tasks

### [✓] TASK-001: Pre-migration cleanup and prerequisites *(Completed: 2026-05-07 00:23)*
**References**: Plan §Phase 1, Plan §Package Update Reference

- [✓] (1) Remove duplicate PackageReference to AKSoftware.Localization.MultiLanguages from `src/AKSoftware.Localization.MultiLanguages.Benchmarks/AKSoftware.Localization.MultiLanguages.Benchmarks.csproj` (keep ProjectReference only)
- [✓] (2) Build Benchmarks project to verify change
- [✓] (3) Benchmarks project builds successfully (**Verify**)
- [✓] (4) Replace PackageReference with ProjectReference in `src/BlazorWebApp.Sample/BlazorWebApp.Sample.csproj` for AKSoftware.Localization.MultiLanguages.Blazor and AKSoftware.Localization.MultiLanguages.SourceGenerator packages
- [✓] (5) Build BlazorWebApp.Sample project to verify change
- [✓] (6) BlazorWebApp.Sample builds successfully (**Verify**)
- [✓] (7) Verify .NET 10 SDK installed per Plan §Phase 1
- [✓] (8) .NET 10 SDK version meets requirements (**Verify**)
- [✓] (9) Check global.json compatibility if file exists in repository
- [✓] (10) global.json compatible with .NET 10 or updated (**Verify**)
- [✓] (11) Commit changes with message: "TASK-001: Pre-migration cleanup and prerequisites"

---

### [▶] TASK-002: Atomic framework and package upgrade across all projects
**References**: Plan §Phase 2-4, Plan §Package Update Reference, Plan §Breaking Changes Catalog, Plan §Project-by-Project Plans

- [ ] (1) Update package references across all in-scope projects per Plan §Package Update Reference (Microsoft.Extensions.DependencyInjection.Abstractions, Microsoft.AspNetCore.Components packages, System.Net.Http.Json to version 10.0.7)
- [ ] (2) Remove System.ComponentModel.Annotations package from AKSoftware.Localization.MultiLanguages core library per Plan §Phase 2.1 (included in .NET 10 framework)
- [ ] (3) Change TargetFrameworks to single TargetFramework=net10.0 in application projects per Plan §Phase 4 (BlazorAKLocalization, BlazorServerLocalizationSample, BlazorWebApp.Sample, ConsoleAppSample, Benchmarks; net10.0-windows10.0.19041.0 for WinUIAkLocalization)
- [ ] (4) All project file changes completed (**Verify**)
- [ ] (5) Restore dependencies for entire solution
- [ ] (6) All dependencies restored successfully (**Verify**)
- [ ] (7) Build entire solution and fix all compilation errors per Plan §Breaking Changes Catalog (focus on System.Uri validation strictness, ASP.NET Core exception handler changes, XAML behaviors compatibility in WinUI projects)
- [ ] (8) Solution builds with 0 errors (**Verify**)
- [ ] (9) Commit changes with message: "TASK-002: Atomic .NET 10 framework and package upgrade"

---

### [ ] TASK-003: Execute test suite and validate upgrade
**References**: Plan §Phase 5, Plan §Testing & Validation Strategy

- [ ] (1) Run tests in AKSoftware.Localization.MultiLanguages.Tests project targeting net10.0 framework
- [ ] (2) Fix any test failures per Plan §Breaking Changes Catalog (address System.Uri parsing strictness, ASP.NET Core behavioral changes if applicable)
- [ ] (3) Re-run tests after fixes
- [ ] (4) All tests pass with 0 failures (**Verify**)
- [ ] (5) Commit test fixes with message: "TASK-003: Complete .NET 10 testing and validation"

---


