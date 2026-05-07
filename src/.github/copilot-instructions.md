# Copilot Instructions

## Project Guidelines
- User prefers updating this solution to the latest installed WinAppSDK version on the dev box.
- User prefers to perform upgrade work in a new branch.
- For the upgrade, Blazor and WinUI app projects should be single-targeted to net10.0, while consumed library projects should be multi-targeted with net10.0 included.
- Until moving to planning, any user-requested changes should be recorded in the assessment document.
- Do not treat UWP tests as relevant validation for WinUI API refactors; focus on cleaning up the WinUI API first.