# DoCard: CubeMixer
 ### 3D multiplayer turn-based strategy game

## 📄 Overview
![gameplay](https://hackmd.io/_uploads/B1nyvALaC.png)

>CubeMixer is a multiplayer game developed as my project, the project was built using Unity, focusing on implementing networking features, 3D visual effects, and a robust turn-based system. 
>
>Players can select grids to move or use skills, but neither player knows the other’s actions until the action round starts. Use your strategy to win the game. Have fun!

> Demonstration Video: 
> https://www.youtube.com/watch?v=EniTK79sF3A

## Features
- **Multiplayer Networking**: Implemented using Unity's Netcode for GameObjects, with Relay and Lobby modules for seamless matchmaking.
- **Strategic Gameplay**: Players plan moves and skills without knowing the opponent's choices, creating dynamic and unpredictable battles.
- **Custom Visual Effects**: Built with Unity Visual Effect Graph to create engaging skill animations and effects.
- **Turn-Based System**: Powered by a Finite State Machine (FSM) to manage game flow and handle transitions between player actions.


## 📡 Technologies Used
- **Unity**: Game development platform.
- **Unity Visual Effect Graph**: For creating visual effects.
- **Unity Timeline**: For skill animation sequencing.
- **Cinemachine**: A plugin for Unity camera control. In this project, the plugin uses Cinemachine Tracks component to help me create a scrollbar that controls the camera's sight direction.
- **[Netcode for GameObjects](https://unity.com/products/netcode)**: Networking SDK.
- **[Unity Lobby](https://unity.com/products/lobby), [Unity Relay](https://unity.com/products/relay)**: Unity Lobby to connet players and join in game.

- **[DoTween](https://dotween.demigiant.com/)**: A plugin about animation engine for Unity
- **[Odin inspector](https://odininspector.com/)**: A plugin that uses Attributes to easily draw editor UI


## 🎮 How to Play
1. Open the game and click "Join Game."
2. Wait in the game until the second player joins, then the round will start.
3. Select your skills and movements for the round.
4. Watch as your character and your opponent’s character execute their planned actions.
5. Outplay your opponent with character skills and strategic decisions!

## Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/Unity_DoCard-CubeMixer.git
2. Open the project in Unity (version 2023.2.11 or later).
