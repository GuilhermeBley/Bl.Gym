---
name: bl-gym-workspace
description: "Workspace-wide guidance for Bl.Gym .NET backend and React Native mobile"
---

# Bl.Gym Workspace Instructions

- Scope: These instructions apply only to the Bl.Gym workspace.
- Explanation style: Use a balanced level of detail (concise, but include brief reasoning and next steps when useful).

## C# / .NET Backend (Training API and related services)

- Follow clean architecture principles:
  - Keep Domain and Application layers free of infrastructure concerns.
  - Prefer injecting abstractions (interfaces) over concrete infrastructure types.
  - Respect existing project boundaries (Domain, Application, Infrastructure, Api) and maintain dependency direction.
- When changing backend code:
  - Prefer adding or updating application/domain tests when it is straightforward and clearly beneficial, but do not create large test scaffolding unless explicitly asked.
  - Suggest where tests belong (project and rough test name) rather than creating new projects.
- Match existing patterns in this solution (e.g., minimal APIs, mediators, or extension methods) instead of introducing entirely new architectural styles without being asked.

## React Native / Mobile (ui/Bl-gym-mobile)

- Follow existing patterns in the Bl-gym-mobile app for state management, navigation, and styling.
- Use TypeScript-friendly patterns and strong typing where possible.
- Keep UI examples focused on React Native components and the current tech stack (no unrelated web-only APIs).

## General Behavior

- Prefer small, focused changes that integrate cleanly with the existing structure.
- When there are multiple viable options, briefly compare them and recommend one.
- Default to English responses unless the user explicitly switches language.
