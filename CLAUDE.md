# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Regras de Comunicação

1. **Idioma**: Sempre responder em português brasileiro.
2. **Confirmação de entendimento**: Antes de executar qualquer tarefa, explicar o entendimento do pedido e perguntar se está correto. Só prosseguir após confirmação do usuário.

## Project Overview

"Soviet Dream" is a Unity 6 (6000.3.6f1) 2D game project using the Universal Render Pipeline (URP). The project uses the new Input System for player controls.

## Development Environment

- **Unity Version**: 6000.3.6f1
- **Render Pipeline**: Universal Render Pipeline (URP) 2D
- **Input System**: Unity Input System (new, not legacy Input Manager)
- **IDE**: VS Code configured (see `.vscode/settings.json`), Rider and Visual Studio also supported
- **Solution File**: `Soviet Dream.slnx`

## Build and Run

Open the project in Unity Hub by adding the project folder. The main scene is at `Assets/Scenes/SampleScene.unity`.

To build from command line:
```bash
# Linux/macOS
/path/to/Unity -batchmode -projectPath . -buildTarget StandaloneLinux64 -buildLinux64Player build/game

# Windows
"C:\Program Files\Unity\Hub\Editor\6000.3.6f1\Editor\Unity.exe" -batchmode -projectPath . -buildTarget StandaloneWindows64 -buildWindows64Player build/game.exe
```

## Input System

The project uses Unity's new Input System with predefined action maps in `Assets/InputSystem_Actions.inputactions`:

**Player Actions**: Move, Look, Attack, Interact (hold), Crouch, Jump, Previous, Next, Sprint

**UI Actions**: Navigate, Submit, Cancel, Point, Click, RightClick, MiddleClick, ScrollWheel

**Control Schemes**: Keyboard&Mouse, Gamepad, Touch, Joystick, XR

## Project Structure

```
Assets/
├── Scenes/                    # Game scenes (SampleScene.unity is the main scene)
├── Settings/                  # URP renderer and pipeline assets
├── InputSystem_Actions.inputactions  # Input action definitions
├── DefaultVolumeProfile.asset # Post-processing volume profile
└── UniversalRenderPipelineGlobalSettings.asset
```

## Key Packages

- `com.unity.render-pipelines.universal` (17.3.0) - URP rendering
- `com.unity.inputsystem` (1.18.0) - New input system
- `com.unity.2d.animation` (13.0.2) - 2D skeletal animation
- `com.unity.2d.aseprite` (3.0.1) - Aseprite sprite import
- `com.unity.2d.psdimporter` (12.0.1) - PSD layer import
- `com.unity.2d.spriteshape` (13.0.0) - Sprite shape rendering
- `com.unity.2d.tilemap.extras` (6.0.1) - Extended tilemap functionality

## Testing

Run tests via Unity Test Runner (Window > General > Test Runner) or command line:
```bash
/path/to/Unity -batchmode -projectPath . -runTests -testPlatform EditMode -testResults results.xml
```
