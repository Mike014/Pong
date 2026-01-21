# Pong 2D - Unity Project

This repository contains a modern reimplementation of the classic arcade game **Pong**, developed in Unity 2022 LTS.

## Project Description

The game is a 2D table tennis simulation that utilizes Unity's physics engine for collision and movement handling. Unlike standard tutorial versions, this implementation features a hybrid Player/AI controller and a ball acceleration system that increases the intensity of prolonged rallies.

## Implemented Mechanics & Features

### 1. Dynamic Difficulty System

The ball does not maintain a constant speed. To increase the challenge during gameplay:

* **Difficulty Multiplier:** Every time the ball hits a paddle, its velocity is multiplied by a factor of 1.3 (configurable via Inspector in `Ball.cs`).
* **Randomization:** Upon spawning, the ball receives an initial velocity with randomized X and Y components and a random direction, ensuring no two serves are identical.
* **Tunneling Prevention:** The collision logic verifies the velocity vector direction before applying the bounce, preventing physics errors where the ball might get stuck inside a paddle.

### 2. Artificial Intelligence (AI)

The paddle control system is handled by a single hybrid script (`Paddle.cs`):

* **Player Mode:** Uses Unity's Input Manager (Vertical Axis) for fluid movement.
* **AI Mode:** Activated via a boolean flag. The CPU uses a predictive algorithm rather than perfectly tracking the ball:
* **Reaction Time:** An internal timer delays the AI's decision-making process to simulate human reflexes.
* **Prediction Offset:** The AI calculates the future Y-position of the ball based on its current vertical velocity.
* **Tolerance:** A margin of error prevents the paddle from jittering when aligned with the ball.



### 3. Game Flow Management

The `GameController` acts as the state machine for the entire application:

* **Scoring System:** Detects goals based on the ball's X coordinates rather than using physical triggers.
* **Scene Reset:** Upon match completion, the scene is completely reloaded using `SceneManager` to ensure a clean reset of memory and physics states.
* **Advanced Pause System:** Implements a "True Pause" (`Time.timeScale = 0`). This freezes physics but correctly toggles the Pause Text and the dedicated **Pause Image/Panel** overlay.
* **Coroutine Management:** Utilizes `WaitForSecondsRealtime` to handle UI delays (e.g., Victory Screen) even when the game time is frozen.

### 4. Technical & Graphics Setup

* **Camera:** Orthographic Projection with Size adapted for the target resolution (640x360).
* **Canvas Scaler:** Configured to `Match Height`, ensuring the UI remains proportionate across different screen aspect ratios.
* **Rendering:** Utilizes Sorting Layers to ensure sprites are correctly layered above the background.

## Controls

### Gameplay

* **W / Up Arrow:** Move paddle Up.
* **S / Down Arrow:** Move paddle Down.

### System

* **TAB:** Toggles Pause (displays Pause Text and Pause Image).
* **ESC:** Quits the application (or stops Play Mode in the Editor).

## Technical Requirements

* **Unity Version:** 2022 LTS (or higher).
* **Packages:** Unity UI (Legacy) is required for text and interface management.

## Installation

1. Clone this repository to your local machine.
2. Open Unity Hub and ensure you have a Unity 2022 LTS version installed.
3. Add the project to Unity Hub and open it.
4. Navigate to `Assets/Scenes/` and open `Game.unity`.
5. Press the Play button in the Editor to start.

## Key File Structure

* `Scripts/GameController.cs`: Handles game logic, UI updates, Pause states (including Image toggling), and application lifecycle.
* `Scripts/Ball.cs`: Handles ball physics, collision logic, audio playback, and difficulty scaling.
* `Scripts/Paddle.cs`: Handles movement logic for both Human input and AI prediction.
* `Prefabs/Ball.prefab`: Pre-configured ball object with Rigidbody2D and AudioSource.

## Credits

This project is based on the mechanics of the original Pong, significantly extended with custom C# logic for AI, advanced physics handling, and state management.
