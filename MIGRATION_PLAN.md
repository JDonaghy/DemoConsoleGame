# Migration Plan: Console to GUI (MonoGame)

This document outlines the steps required to convert the existing terminal-based `ConsoleGame` into a graphical application using the **MonoGame** framework (an open-source implementation of XNA). This approach allows us to preserve the majority of the existing game logic while replacing the presentation layer with modern graphics (PNG assets).

## 1. Framework & Project Structure
*   **Current State:** `Console Application` (Linear `Main` execution with a manual `while` loop and `Thread.Sleep`).
*   **Target State:** `MonoGame Game` class structure.
    *   Create a new class inheriting from `Microsoft.Xna.Framework.Game`.
    *   The `Program.cs` will simply bootstrap this `Game` class.
    *   The framework handles the loop timing, update frequency, and rendering cycle, eliminating the need for `Thread.Sleep(10)`.

## 2. Asset Management
*   **Current State:** `ObjectDisplay` holds `List<char[,]>` (character arrays) for animation frames.
*   **Target State:** `ObjectDisplay` will hold `List<Texture2D>`.
    *   Replace character definitions (e.g., `new char[,] { { '|' } }`) with external PNG files (e.g., `missile.png`, `enemy_frame1.png`).
    *   Implement a content loading mechanism: `Content.Load<Texture2D>("asset_name")`.

## 3. Rendering Logic
*   **Current State:** "Dirty Rect" rendering. Explicit calls to `Erase()` (to clear old characters) and `Draw()` (to write new ones) using `Console.SetCursorPosition`.
*   **Target State:** "Immediate Mode" rendering.
    *   **Clear Screen:** The entire screen is cleared every frame (`GraphicsDevice.Clear`).
    *   **Remove Erase:** The `Erase()` methods become obsolete and will be removed.
    *   **Update Draw:** `Draw()` in `ObjectDisplay` will accept a `SpriteBatch` object and position vectors.
        ```csharp
        public void Draw(SpriteBatch spriteBatch, Vector2 position) {
            spriteBatch.Draw(_currentTexture, position, Color.White);
        }
        ```

## 4. Coordinate System
*   **Current State:** Grid-based integers (Row 0-29, Column 0-118).
*   **Target State:** Pixel-based coordinates (e.g., 1280x720).
    *   **Strategy:** Maintain the existing logic coordinates (0-118) but apply a **scaling factor** during rendering.
    *   Example: If sprites are 16x16 pixels, draw at `x * 16`, `y * 16`.
    *   This preserves the movement logic in `SineObject`, `MissileObject`, and `GameManager`.

## 5. Input Handling
*   **Current State:** Event-based/Blocking `Console.KeyAvailable` and `Console.ReadKey`.
*   **Target State:** Polling state.
    *   Refactor `ProcessKeys()` to use `Keyboard.GetState()`.
    *   Example: `if (Keyboard.GetState().IsKeyDown(Keys.Space)) ...`

## 6. Execution Steps

1.  **Add Dependencies:** Add `MonoGame.Framework.DesktopGL` (or similar) to the project via NuGet.
2.  **Refactor `ObjectDisplay`:**
    *   Replace `char[,]` with `Texture2D`.
    *   Replace Console colors with the `Color` struct.
    *   Add `LoadContent` functionality.
3.  **Refactor `GameManager`:**
    *   Remove console-specific calls: `_clearConsole`, `_writeStatus`, `InitGame`.
    *   Implement `Draw(SpriteBatch batch)` to iterate `objList` and render entities.
4.  **Refactor `BaseObject`:**
    *   Remove `Erase()`.
    *   Update `Draw()` signature to accept `SpriteBatch`.
5.  **Refactor Game Loop:**
    *   Move `ProcessKeys` logic into the MonoGame `Update()` override.
    *   Move `ProcessObjects` logic into the MonoGame `Update()` override.
    *   Move `GameManager.Draw` call into the MonoGame `Draw()` override.
