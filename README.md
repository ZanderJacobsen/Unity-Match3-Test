# Unity Match 3

This is a simple Match-3 puzzle game built in Unity as a first pass at learning the engine. It's a self-contained prototype recreating core match-3 mechanics while exploring Unity's 2D workflow, Input System, and scripting architecture.

This is not a production-ready product. It's a technical demo focused on learning and experimentation.

## Features

- Grid-based match-3 layout
- Click and drag swapping with directional clamping
- Swap-back logic if no match is made
- Cascading matches supported
- Drag prevention and input locking during animations
- Simple outline visual for selected tile
- Basic VFX and sound feedback
- Custom grid implementation using generics

## How to Run

The build includes~~ both~~ Windows~~ and macOS versions~~.

1. Download `Match3.zip` and extract
2. For Windows: run `Match3.exe`
~~3. For macOS: run `Match3.app` (you may need to right-click > Open)~~

## Tech Stack

- Unity Input System
- DOTween for basic animation

## Known Limitations

- No special tile types (e.g. bombs, powerups)
- Initial board generation does not avoid pre-existing matches
- No UI for score, moves, or level tracking
- Fixed 720x1280 resolution
- All assets are placeholders

## Notes

This project served as a hands-on intro to Unity. Coming from Phaser and lower-level environments like C++ and JS, this was about learning how Unity structures input, component logic, coroutines, and scene flow.

