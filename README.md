# Cannoneer

![C#](https://img.shields.io/badge/C%23-239120?style=flat&logo=csharp&logoColor=white)
![MonoGame](https://img.shields.io/badge/MonoGame-3.8-blue?style=flat)
![.NET](https://img.shields.io/badge/.NET-6%2B-purple?style=flat)

A 2D artillery game built with MonoGame. Aim a cannon, charge your shot, and hit a randomly placed target — with real projectile physics including gravity, angle, and launch velocity.

---

## Overview

Cannoneer is a single-screen skill game. A target appears at a random position each round. You control cannon angle with W/S, charge shot power by holding Space, and release to fire. A direct hit triggers a layered explosion animation and pauses the game. Press R to reset with a new target.

---

## Features

- **Projectile physics** — velocity decomposed from charge level and barrel angle using cos/sin. Gravity applied each frame for parabolic arcs.
- **Charge mechanic** — hold Space to oscillate charge level between 0 and 6.5. A live indicator shows filled squares in real time.
- **Explosion animation** — three expanding concentric rings (red → orange → yellow) animate over 0.5 seconds on target hit.
- **Randomized targets** — each round places the target at a random position in the right-hand portion of the screen.
- **Rotatable barrel** — cannon rotates from 0° (flat) up to −90° (straight up) using radian-based rotation.
- **Unlimited shots** — multiple projectiles can be in flight simultaneously, each checked for collision independently.

---

## Getting Started

**Prerequisites:** .NET 6 SDK or later, MonoGame 3.8+

```bash
git clone https://github.com/mohjo13/Cannoneer
cd Cannoneer
dotnet run
```

---

## Controls

| Key | Action |
|-----|--------|
| `W` / `S` | Rotate cannon barrel up / down |
| `Space` (hold) | Charge shot — power oscillates automatically |
| `Space` (release) | Fire |
| `R` | Reset — new target, clear all projectiles |
| `Escape` | Quit |

---

## Physics Model

Launch velocity is decomposed into X and Y components from the barrel's rotation angle in radians:

```
vx = chargeLevel × 6 × cos(rotation)
vy = chargeLevel × 6 × sin(rotation)

each frame:
  vy += 1.8        // gravity
  position += velocity
```

A constant downward gravity of 1.8 units/frame² produces realistic parabolic arcs. The barrel length (70 px) offsets the projectile spawn point to the cannon tip so shots originate from the correct position regardless of angle.

---

## Project Structure

| File | Description |
|------|-------------|
| `Game1.cs` | Main game loop — input, projectile/target update, charge UI, reset logic |
| `Characters.cs` | Cannon entity — rotation, charge oscillation, barrel-tip spawn position, firing |
| `Projectile.cs` | Projectile entity — velocity, gravity, per-frame movement, collision detection |
| `Target.cs` | Target entity — bounds, explosion trigger, three-ring animation with timer |
| `Primitives2D.cs` | C3.XNA helper — `DrawCircle` and `FillRectangle` extensions |

---

## Built With

- [MonoGame 3.8](https://www.monogame.net/) — cross-platform game framework
- [C3.XNA](https://github.com/Nezz/MonoGame-Primitives2D) — primitive drawing helpers
- .NET 6+

---

## License

MIT
