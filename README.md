# Galaxy Shooter: Air Force War

A classic top-down space shooter built in Unity 2D (URP). Control your ship, shoot enemies, and survive as long as you can.

## Quick Start

1. Open the project in Unity.
2. Open **Assets/Scenes/SampleScene**.
3. Press **Play**.

The scene already contains **GameBootstrap**, which creates the player, enemies, bullets, UI, and background when the game starts. No extra setup or menu steps are required.

## Controls

- **WASD** or **Arrow keys** – Move
- **Space** or **Left mouse button** – Shoot

## Features

- **Player ship** – Move within screen bounds, continuous fire
- **Enemies** – Spawn from the top and move down; destroy them for score
- **Lives** – Start with 3 lives; lose one when hit by an enemy or enemy bullet
- **Score** – 100 points per enemy
- **Game Over** – Restart with the on-screen button
- **Scrolling background** – Simple starfield
- **Object pooling** – Bullets and enemies are pooled for performance

## Project Structure

- **Assets/Scripts/**
  - `GameBootstrap.cs` – Creates the full game at runtime if nothing is in the scene
  - `GameManager.cs` – Score, lives, game over, restart
  - `PlayerController.cs` – Movement and shooting
  - `EnemyController.cs` – Enemy movement and optional shooting
  - `Projectile.cs` – Bullet behavior and collision
  - `ProjectilePool.cs` / `EnemyPool.cs` – Object pools
  - `SpawnManager.cs` – Enemy spawn timing and position
  - `ScrollingBackground.cs` – Starfield scroll
  - `GameUI.cs` – Score, lives, game over panel
  - `PlaceholderSprite.cs` – Runtime placeholder sprites
- **Assets/Scripts/Editor/**
  - `GalaxyShooterSetup.cs` – Menu item to add GameBootstrap and tags

## Customization

- **Use your own art**: Create prefabs for Player, Enemy, and Player/Enemy bullets with the same components (colliders, scripts). Assign them in the scene and disable **Create If Missing** on GameBootstrap, or remove GameBootstrap and place your own GameManager, Player, Pools, SpawnManager, and UI.
- **Difficulty**: Adjust `SpawnManager.spawnInterval`, `EnemyController.moveSpeed` / `canShoot` / `shootInterval`, and `GameManager.maxLives` in the Inspector.
- **Player**: Tune `PlayerController.moveSpeed`, `fireRate`, and `maxHealth`.

## Requirements

- Unity 2022.3+ (project uses URP 2D and Input System)
- Tags **Player** and **Enemy** are already set in the project.
- If you remove **GameBootstrap** from the scene, use menu **Galaxy Shooter → Setup Scene** to add it back.

Enjoy the game!
