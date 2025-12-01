# Silent Greens 

## 1. Overview
Silent Greens is a calm and minimalistic 2D casual golf-inspired game where players guide a small ball into a hole using simple drag-and-release controls. The game removes strict golf rules, focusing instead on smooth physics, relaxed pacing, and cozy visuals across handcrafted levels.

Players can take multiple shots, learn the terrain, and progress at their own pace, making Silent Greens ideal for short, peaceful play sessions.

---

## 2. Core Gameplay

### 2.1 Controls
- Drag to aim  
- Release to shoot  
- Trajectory dots help visualize the shot  
- Ball moves with smooth physics and gentle rolling behavior  

### 2.2 Objective
Reach the hole using as few shots as possible while navigating simple obstacles and terrain variations.

### 2.3 Progression
- Each completed level unlocks the next  
- No penalties for retrying  
- Designed to feel relaxing instead of competitive  

---

## 3. Level Design

### 3.1 Philosophy
Levels are handcrafted to promote:
- Calm pacing  
- Soft difficulty curves  
- Gentle experimentation  

### 3.2 Terrain & Structure
Levels may include:
- Flat platforms  
- Slopes  
- Simple barriers  
- Unique props based on theme  
- Safe drop-zones / reset zones  

### 3.3 Themes
- **Day Mode** – bright, soft colors  
- **Evening Mode** – warm gradients  
- **Night Mode** – cool, soothing tones  

---

## 4. Visual & Audio Atmosphere

### 4.1 Art Style
- Minimalistic  
- Clean shapes  
- Soft color palettes  
- Smooth UI animations  

### 4.2 Tone / Mood
The game focuses on:
- Cozy vibes  
- Calm pacing  
- No stress or punishment  
- Ambient feel without overwhelming effects  

### 4.3 Camera Behavior
- Smooth follow  
- Subtle zoom-in near the hole  
- No aggressive movement  

---

## 5. Features
- Intuitive drag-and-release shooting  
- Clean and responsive physics  
- Handcrafted small levels  
- Relaxed, non-competitive gameplay  
- Level progression with saved progress  
- Minimal UI for a clutter-free experience  
- Light effects and gentle animations  
- Smooth camera zoom near target  

---

## 6. Technical Architecture

### 6.1 Engine
**Unity (2D)**

### 6.2 Core Systems
- **Ball Physics System**  
  Handles velocity, friction, bounce, slopes, and rolling slowdown.

- **Shooting System**  
  Drag input, angle calculation, trajectory prediction, release handling.

- **Trajectory System**  
  Renders prediction points until shot is taken.

- **Camera System**  
  Smooth follow + zoom-in when near the hole.

- **Level Manager**  
  Loads levels based on unlocked index.

- **Progression System**  
  PlayerPrefs stores `LastUnlockedLevel`.

- **UI System**  
  Clean, minimal, responsive.

---

## 7. Tech Stack

### 7.1 Software / Tools
- Unity 2021+ / 2022+  
- C#  
- Unity Tilemap (optional)  
- Unity Input System  
- Photoshop / Figma / Illustrator  
- Git / GitHub  

### 7.2 Optional Packages
- Cinemachine (smooth camera)  
- DOTween (UI animations)  
- TextMeshPro (UI text)  

---

## 8. Level Flow
1. Player selects a level  
2. LevelManager loads the level prefab  
3. Player drags → aims → shoots  
4. Ball interacts with terrain and props  
5. Ball enters hole → level complete  
6. Next level unlocks  
7. Player continues or returns to menu  

---

## 9. Data Storage
- `LastUnlockedLevel` – stored using PlayerPrefs  

Possible future data:
- Best shots  
- Total shots  
- Player settings  

---

## 10. Future Possibilities
- Power-ups (slow motion, free retry, double jump)  
- New themes: forest, snow, neon, desert  
- Obstacles: moving blocks, bumpers, teleporters  
- Endless mode  
- Leaderboards (optional to keep casual vibe)  

