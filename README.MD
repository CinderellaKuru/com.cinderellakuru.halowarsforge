# 🎮 Halo Wars Forge 🎮

*Made by [CinderellaKuru](https://github.com/CinderellaKuru/com.cinderellakuru.halowarsforge)*

The ``Halo Wars - Forge`` package is a powerful Unity in-editor tool designed for modders to easily visualize and modify scenario files within Unity. With an intuitive interface and seamless integration, this package enhances your workflow by providing a dedicated Scenario Editor under the Halo Wars > Scenario Editor tab.

## 🛠️ Known Issues

### 1. Rotation Display Bug
- **Issue** Certain objects with specific rotation values (e.g., `30.5689633`) may not render correctly.
- **Workaround**
  1. Click `[CLEAR SCENE]` to reset the scene.
  2. Reload the scene using `[LOAD SCENARIO INTO SCENE]`.
  3. Save your progress with `[SAVE SCENARIO]`.

### 2. Scene File Read-Only Error
- **Issue** Opening the `Main.unity` scene might trigger a `READ-ONLY` access error.
- **Workaround** Manually drag `Main.unity` into the `Assets` folder to resolve the issue.

## 🎉 Version 1.0.2

### 📦 Changes
   - Refactored the Hierarchy of the package.

  ```plaintext
  Editor
   └── ScenarioWindow.cs
  Runtime
    ├── Scenario
    │   ├── Components
    │   │   ├── DesignSphere.cs
    │   │   ├── Light.cs
    │   │   ├── Object.cs
    │   │   ├── Position.cs
    │   │   └── SHLightParams.cs
    │   └── Scenario.cs
    └── Unity
        ├── Components
        │   ├── DesignSphere.cs
        │   ├── Light.cs
        │   ├── Object.cs
        │   ├── Position.cs
        ├── ScenarioManager.cs
        └── TerrainManager.cs
  ```
  - Renamed `Tools > Scenario Editor` to `Halo Wars > Scenario Editor`.
  - Added a scrollbar to the Editor window for easier navigation.


## 🎉 Version 1.0.1

### 📦 Changes
- Removed Post Processing dependencies for a cleaner build.
- Tested and verified the UPM update feature for seamless package management.


## 🎉 Version 1.0.0

### 📦 Changes
- Initial release, serving as a test bed with UPM.
- Included all necessary scripts to ensure the package is fully functional.


