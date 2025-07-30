# 🎮 Unity Match 3 — First Engine Exploration

This is a simple Match-3 puzzle game built in **Unity** as a first foray into the engine. It’s inspired by classic grid-swapping mechanics (Candy Crush, Bejeweled, etc.) but serves mainly as a hands-on introduction to Unity’s 2D workflow, Input System, and component-driven architecture.

> This is not a complete or polished commercial product — it’s a focused technical demo meant to reinforce familiarity with Unity, C#, and match-3 game logic.

---

## 🧠 Project Goals

- Learn the Unity engine from a clean-slate perspective
- Recreate core Match-3 mechanics:
  - Swapping tiles
  - Matching rows/columns of 3+
  - Removing matches and refilling the board
- Experiment with Unity's modern Input System
- Add drag-to-swap functionality
- Add animations and effects using DOTween
- Build for both Windows and macOS

---

## 🧩 Features

- 🎯 Grid-based tile layout (8x8)
- 🖱 Click + Drag input with position clamping and swap validation
- 🔁 Swap-back mechanic if no match is made
- 🔊 Sound effects for match, swap, fail, and drop
- 🌟 Visual feedback for selected tiles (glow effect)
- 💥 Basic explosion VFX on match
- 📦 Custom grid and tile systems using generics (`Grid<T>`, `GridObject<T>`)
- 🛠️ Built-in drag prevention, input lock during animations
- 🔄 **Cascading refill logic**: chain reactions are supported when new matches fall into place

---

## 🧪 How to Run the Build

**🪟 Windows and 🍎 macOS builds included.**

1. Download `Match3.zip` for your OS.
2. Extract it to a folder.
3. Open the folder and run the executable:
   - **Windows**: Run `Match3.exe`
   - **macOS**: Run `Match3.app` (you may need to right-click → Open if Gatekeeper complains)
4. Play!

---

## 🛠 Tech Stack

- 🎮 Unity 2023.x (URP 2D Template)
- 🧠 C# (custom class structure, generics)
- 📦 Unity Input System (click, drag, release)
- 🎞 DOTween for animations
- 🎨 Custom glow outline sprite
- 🎧 Unity AudioSource system

---

## 🚧 Known Limitations

- No special tile types (e.g., bombs, color clears)
- No prevention of initial board spawning matches (pre-matched tiles may appear)
- Fixed screen resolution (720x1280 window)
- Very light UI — no score or move count
- Asset art is placeholder and procedural

---

## 💬 Personal Note

This project marks my first exploration into Unity. Coming from Phaser and C++/JavaScript backgrounds, this helped bridge my understanding of component-based architecture, serialized object patterns, and event-driven input handling in a new environment.

This repo and build are a sandbox — not a polished game — but every piece of logic here represents progress toward becoming more comfortable in Unity.

---

## 📁 Credits

- Glow sprite outline generated with help from ChatGPT
- Audio clips are basic test sounds (free and/or procedural)
- Grid logic and tweening system inspired by Phaser → Unity transitions
