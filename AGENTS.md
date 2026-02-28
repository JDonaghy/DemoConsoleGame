# AGENTS.md

This document provides instructions and guidelines for AI agents and developers working on the `ConsoleGame` repository. It is a C# .NET 10.0 MonoGame Application.

## 1. Environment & Tooling

- **Framework:** .NET 10.0 (`net10.0`)
- **SDK:** Ensure the .NET 10 SDK is installed.
- **Library:** MonoGame Framework (DesktopGL).
- **Project File:** `ConsoleGame/ConsoleGame.csproj`
- **Entry Point:** `ConsoleGame/Program.cs` bootstraps `Game1.cs`.

## 2. Build, Lint, and Test Commands

### Build
To build the project, run the following command from the root or `ConsoleGame` directory:

```bash
dotnet build
```

### Run
To run the game (requires graphical environment):

```bash
# From repository root
dotnet run --project ConsoleGame/ConsoleGame.csproj

# From ConsoleGame directory
dotnet run
```

### Test
**Current Status:** There are currently **no automated tests** in this repository.
- There is no test project (e.g., xUnit/NUnit).
- If you are asked to add tests, create a new xUnit project:
  ```bash
  dotnet new xunit -o ConsoleGame.Tests
  dotnet add ConsoleGame.Tests/ConsoleGame.Tests.csproj reference ConsoleGame/ConsoleGame.csproj
  ```

### Linting & Formatting
The project does not have a strict linter configuration (like `.editorconfig` or `StyleCop`), but relies on standard C# conventions.
- Use `dotnet format` to fix whitespace and import ordering.

## 3. Code Style Guidelines

Adhere strictly to the existing coding patterns found in `GameManager.cs` and `GameObjects/`.

### Formatting
- **Brace Style:** Use **Allman** style (opening braces on a new line).
- **Indentation:** Use **4 spaces**. Do not use tabs.
- **Spacing:** Space after keywords (`if`, `while`, `for`).

### Naming Conventions
- **Classes/Structs/Enums:** `PascalCase` (e.g., `GameManager`, `BaseObject`, `Rect`).
- **Methods:** `PascalCase` (e.g., `Update`, `AddObject`).
- **Public Properties:** `PascalCase` (e.g., `CurrentX`, `IsDead`).
  - *Exception:* `GameManager.objList` is public but `camelCase`. Prefer `PascalCase` for new properties.
- **Private Fields:** `_camelCase` (underscore prefix).
- **Interfaces:** Prefix with `I` (e.g., `IBaseObject`).

### File Structure
- **Namespaces:** Use `ConsoleGame` for core logic and `ConsoleGame.GameObjects` for entities.
- **Imports:** System namespaces first, followed by Microsoft.Xna.Framework, then project namespaces.

### Type Usage
- Use explicit types when the type is simple (int, bool, string).
- Use `var` when the type is obvious from the right-hand side.

## 4. Architecture & Patterns

### Game Loop (MonoGame)
The game loop is managed by `Game1.cs` (inheriting from `Microsoft.Xna.Framework.Game`):
1.  **Initialize:** Sets up graphics and `GameManager`.
2.  **LoadContent:** Loads PNG textures from `Content/` folder into `GameManager.Textures`.
3.  **Update:** Calls `GameManager.Update(GameTime)`, which updates all game objects.
4.  **Draw:** Calls `GameManager.Draw(SpriteBatch)`, which renders all objects.

### Object System
The game uses an entity-component-like structure via inheritance:
- **Base Interface:** `IBaseObject` defines the contract (`Update`, `Draw`, `ProcessInput`).
- **Base Class:** `BaseObject` implements common logic (position, lifecycle, collision).
- **Entities:** Specific objects (`SineObject`, `MissileObject`, `CursorObject`) inherit from `BaseObject`.
- **Display:** `ObjectDisplay` handles rendering `Texture2D` sprites.

### Rendering
- The game renders using `SpriteBatch`.
- Objects use `Texture2D` loaded from PNGs.
- Coordinates are logically 0-indexed (Row, Column) but scaled to pixels during `Draw()`.
- Default resolution is approx 1280x720 (80x45 grid).

## 5. Common Tasks for Agents

### Adding a New Game Object
1.  Create a new class in `GameObjects/` inheriting from `BaseObject`.
2.  Add a texture PNG to `Content/` folder.
3.  Load the texture in `Game1.LoadContent` and add to `GameManager.Textures`.
4.  In the object constructor, initialize `ObjectDisplay` with the texture.
5.  Implement `Update()` for movement logic.
6.  Register the object in `GameManager.AddObject` switch case.

### Modifying Game Logic
- **Input:** Modify `CursorObject.ProcessInput` or `GameManager.ProcessInput`.
- **Collision:** Logic is in `BaseObject.CheckAlive()`.
- **Spawning:** Controlled by `makeDemBuggers` in `GameManager.cs`.

## 6. Rules & Instructions
- **Do not introduce new NuGet packages** unless explicitly requested.
- **Do not change the .NET version** (keep `net8.0`).
- **Always use absolute paths** when reading/writing files in your tool usage.
- **Verify changes** by attempting a build (`dotnet build`) after editing.
