# 🏰 Monster Marches

> A 2D modular tower defense game inspired by Kingdom Rush, built with Unity and designed for scalability and live data updates.
> The world of nature is under siege.

Dark monsters are marching from the edge of the forest, corrupting everything in their path — trees wither, crops die, and villages fall.  
As a defender of the land, you must lead the resistance: build towers, command brave soldiers, and hold the line at all costs.

**Monster Marches** is not just another tower defense game. It features flexible soldier control, live-updatable maps and enemies via GitHub, and a rich visual style handcrafted in Adobe Illustrator.

Can you stop the withering tide before nature falls completely?

## 🎮 Play the Game
👉 [Play on Itch.io](https://vinhdang.itch.io/monstermarches)

## 🎥 Demo Video
📺 [Watch on YouTube](https://youtu.be/F_NlWcMJ2zI)

## 📋 Devlog – Adding a New Remote Map (Map 3)
📺 [Watch on YouTube](https://youtu.be/rejVyvOZs_8)

Added a new map (Map 3) to the remote game data system, making it available in the game on Itch.io.
Game logic: After a player finishes a map, the next one will be unlocked.
(Currently, the game has 2 maps.)

1. Start by copying the background sprite for Map 3 from the asset folder into the project, and mark it as Addressable.
2. Open the scene used for data creation and generate the JSON file for Map 3.
3. Replace the old JSON files with the new ones (these files were already marked as Addressables).
4. Rebuild the Addressable bundles and upload them to the server (GitHub Pages).
5. Before building, make sure you're using the correct build target (e.g., WebGL for Itch.io).
6. Wait for GitHub Pages to finish updating (usually takes 1–2 minutes).
7. Refresh the game on Itch.io to test if the new map displays and works correctly.

## 📖 How to Play

<table>
  <tr>
    <td><img src="https://github.com/vinhdang15/Monster-Marches/raw/main/Screenshots/Build-Tower.png" width="300"/></td>
    <td><b>Build Towers:</b><br>Place towers on empty plots to stop incoming enemies. Use gold to build.</td>
  </tr>
  <tr>
    <td><img src="https://github.com/vinhdang15/Monster-Marches/raw/main/Screenshots/Upgrade-Tower.png" width="300"/></td>
    <td><b>Upgrade Towers:</b><br>Improve towers and barrack to unlock new projectiles, stronger units, and special effects.</td>
  </tr>
  <tr>
    <td><img src="https://github.com/vinhdang15/Monster-Marches/raw/main/Screenshots/Control-Soldiers.png" width="300"/></td>
    <td><b>Control Soldiers:</b><br>Deploy and move soldiers to strategic locations to block enemy paths and delay their advance.</td>
  </tr>
  <tr>
    <td><img src="https://github.com/vinhdang15/Monster-Marches/raw/main/Screenshots/Call-Enemy-Waves.png" width="300"/></td>
    <td><b>Call Enemy Waves:</b><br>A countdown shows before each wave. Pressing the wave button early spawns enemies instantly and grants BONUS GOLD — the sooner you press, the more you earn. Otherwise, the wave starts automatically.</td>
  </tr>
  <tr>
    <td><img src="https://github.com/vinhdang15/Monster-Marches/raw/main/Screenshots/Beware-of-Bosses.png" width="300"/></td>
    <td><b>Beware of Bosses:</b><br>Some boss monsters can wither grass and crops as they pass, turning nature against you.</td>
  </tr>
  <tr>
    <td><img src="https://github.com/vinhdang15/Monster-Marches/raw/main/Screenshots/Defend-Your-Base.png" width="300"/></td>
    <td><b>Defend Your Base:</b><br>Don’t let too many enemies reach the end. Lose all lives and it's game over!</td>
  </tr>
</table>

## 🧠 Features

- MVP architecture with clean separation of logic and presentation
- GameFlowManager initializes all game systems in order
- Object Pooling system for optimized performance
- Loads game data (towers, enemies, maps) via JSON hosted on GitHub
- Uses Addressables for dynamic prefab loading (soldiers, enemies, maps)
- All art drawn with Adobe Illustrator

## 📦 Technologies

- Unity 2D
- C#
- Addressables System
- JSON Data Management
- GitHub Pages (data hosting)
- Adobe Illustrator

## 📂 Related Repository
This game reads all external game data from:  
👉 [Monster-Marches-Data](https://github.com/vinhdang15/Monster-Marches-Data)

## ✍️ Author

**Đặng Toàn Vinh**  
[GitHub Profile](https://github.com/vinhdang15)"
