# JamStarter

A small Unity 6 starter for game jam teams. It gives you a clean folder structure, Git with LFS that works right away and a setup script, so your team spends the first hour on the game and not on project setup.

## What's inside

- A folder structure for 3D projects that keeps your own files (`_Project`) apart from downloaded assets (`ThirdParty`)
- `.gitignore` for Unity, Rider, Visual Studio and VS Code
- `.gitattributes` that normalizes line endings and stores large files (images, models, audio, video, fonts) with Git LFS
- A setup script under **Tools > Jam Starter > Setup**

That's it. No menus, no managers, no sample code. How your game works is up to you.

## Tested with

Unity 6.3 LTS (6000.3.10f1), Universal Render Pipeline, 3D.

## Quick start: use the template

1. Install [Git LFS](https://git-lfs.com) and run this once on every machine:
   ```
   git lfs install
   ```
2. On GitHub, click **Use this template** > **Create a new repository**.
3. Clone your new repository.
4. In Unity Hub, click **Add** > **Add project from disk** and pick the folder.
5. Open `_Project/Scenes/SampleScene` and start working.

Everyone on the team needs step 1 before cloning. Without Git LFS, images and models arrive as tiny text files instead of the real thing.

## Quick start: add it to a new or existing project

1. Create a project in Unity Hub with the **Universal 3D** template, or open the one you already have.
2. Copy `.gitignore` and `.gitattributes` into the project root, next to the `Assets` folder. Both names start with a dot, so your file manager might hide them.
3. Copy the `JamStarter` folder into `Assets`.
4. In Unity, click **Tools > Jam Starter > Setup**.
5. Open a terminal in the project folder and run:
   ```
   git init -b main
   git lfs install
   git add .
   git commit -m "Initial commit"
   ```

## What Setup does

- Creates the folders listed below. Every empty folder gets a `.gitkeep` file, because Git doesn't store empty folders.
- Moves the files every new Unity 6 project starts with into `_Project`: `Settings`, `InputSystem_Actions` and `Scenes/SampleScene`. It only moves them if they are still where Unity put them and nothing is in the way. It never overwrites anything.
- Checks two settings Git depends on. Version Control mode must not be **Hidden Meta Files**, and Asset Serialization should be **Force Text**. Before switching the serialization it asks you first, because Unity rewrites every asset when it does that.
- Logs every change to the Console.

You can run it as often as you like. If everything is already in place, nothing happens. Once your project is set up, you can delete `Assets/JamStarter`.

## Folder structure

```
Assets/
├── _Project/
│   ├── Art/
│   │   ├── Animations/
│   │   ├── Materials/
│   │   ├── Models/
│   │   └── Textures/
│   ├── Audio/
│   │   ├── Music/
│   │   └── Sfx/
│   ├── Input/
│   ├── Prefabs/
│   ├── Scenes/
│   ├── Scripts/
│   ├── Settings/
│   └── UI/
├── JamStarter/
│   └── Editor/
└── ThirdParty/
```

`_Project` holds everything your team makes. The underscore keeps it at the top of the Project window.

`ThirdParty` is for things you drop in by hand, like a sound pack or a Kenney asset pack. Packages from the Asset Store import into their own folders. Leave them there, some of them break when you move them.

## Good to know

**GPU Resident Drawer is turned off.** The Universal 3D template turns it on, but it doesn't work in WebGL builds or with OpenGL, which a lot of Linux setups use. It only pays off in scenes with lots of objects anyway. To turn it back on, select `_Project/Settings/PC_RPAsset` and set **Rendering > GPU Resident Drawer** to **Instanced Drawing**.

**Git LFS has storage limits.** Large files count against the LFS quota of your Git host. Check what your plan includes before you push a few hundred megabytes of audio.

## License

LICENSE_PLACEHOLDER

## Links

itch.io: ITCH_LINK_PLACEHOLDER
