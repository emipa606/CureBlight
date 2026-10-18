# GitHub Copilot Instructions for RimWorld Modding: Cure Blight (Continued)

## Introduction

Welcome to the development guide for the **Cure Blight (Continued)** mod, an update to the original Vicakis mod for RimWorld. This mod enhances the interaction with blighted plants by adding new gizmos and introducing an order system for marking blighted plants, allowing colonists to cure them.

## Mod Overview and Purpose

The primary purpose of the **Cure Blight (Continued)** mod is to provide players with a more efficient way to manage blighted plants in their colonies. It extends the gameplay by offering a mechanism to mark and cure blighted plants, integrating seamlessly with the game.

## Key Features and Systems

- **Gizmos for Blighted Plants**: Adds new gizmos for blighted plants, allowing players to cure individual plants or all affected plants, similar to existing cut-gizmos.
- **Marking and Curing System**: Introduces a method to mark blighted plants. Colonists will prioritize curing these marked plants, with the speed depending on their plant work skills.
- **Integration with Existing Game Mechanics**: The mod fully integrates with RimWorld's existing plant work mechanics, enhancing player's management options without disrupting the balance.

## Coding Patterns and Conventions

For maintaining consistency and readability within the mod:
- Follow C# naming conventions, using PascalCase for types and methods, and camelCase for local variables.
- Use XML definitions for any statically defined game elements, such as jobs and designations.
- Group related logic into classes, focusing on separation of concerns.

## XML Integration

XML is used extensively to define new game objects and patch existing behaviors:
- **DesignationDef**: For creating new plant designations.
- **JobDef**: For defining new jobs related to curing blight.
- **WorkGiverDef**: For assigning colonists to blight-cure work tasks.

Ensure that all XML files are correctly formatted and placed in the appropriate directories to align with RimWorld's modding structure.

## Harmony Patching

The mod uses Harmony for runtime method patching:
- **Postfix Patch**: The `Plant_GetGizmos` class contains a postfix method to append new gizmos to blighted plants. Make sure to correctly apply Harmony annotations and ensure patches do not conflict with other mods.

## Suggestions for Copilot

When utilizing GitHub Copilot for this mod development:
- Leverage Copilot to automate routine code patterns, like properties and typical method structures.
- Use Copilot for bulk XML editing, especially when adding or updating similar entries across different files.
- Be mindful of the context in which Copilot is used. Always review and understand the generated code for compatibility with both existing game systems and mod functionality.

By following these structured guidelines, the development of the **Cure Blight (Continued)** mod should be manageable, helping maintain consistent quality and feature set as intended.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

