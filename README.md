# CubeMixer

## Overview
CubeMixer is a multiplayer game developed as my project, where players strategically select skills and movements before battling in a grid-based arena. 
The project was built using Unity, focusing on implementing networking features, 3D visual effects, and a robust turn-based system.

![截圖 2024-09-17 18.15.19](https://hackmd.io/_uploads/B1nyvALaC.png)


## Features
- **Multiplayer Networking**: Implemented using Unity's Netcode for GameObjects, with Relay and Lobby modules for seamless matchmaking.
- **Strategic Gameplay**: Players plan moves and skills without knowing the opponent's choices, creating dynamic and unpredictable battles.
- **Custom Visual Effects**: Built with Unity Visual Effect Graph to create engaging skill animations and effects.
- **Turn-Based System**: Powered by a Finite State Machine (FSM) to manage game flow and handle transitions between player actions.


## Technologies Used
- **Unity**: Game development platform.
- **Netcode for GameObjects**: Networking SDK.
- **Unity Lobby, Unity Relay**: Unity Lobby to connet players and join in game.
- **Unity Visual Effect Graph**: For creating visual effects.
- **Unity Timeline**: For skill animation sequencing.
- **DoTween**: A plugin about animation engine for Unity
- **Odin inspector**: A plugin that uses Attributes to easily draw editor UI.


## How to Play
1. Open the game and click "Join Game."
2. Wait in the game until the second player joins, then the round will start.
3. Select your skills and movements for the round.
4. Watch as your character and your opponent’s character execute their planned actions.
5. Outplay your opponent with character skills and strategic decisions!

## Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/CubeMixer.git
2. Open the project in Unity (version 2023.2.11 or later).
