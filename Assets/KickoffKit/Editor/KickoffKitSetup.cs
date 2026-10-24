using System.IO;
using UnityEditor;
using UnityEngine;
//////////////////////////////////////////////////////////////////////////////////////
namespace KickoffKit.Editor
{
    // Adds "Tools > Kickoff Kit > Setup" to the Unity menu bar.
    public static class KickoffKitSetup
    {
        private const string RootFolder = "Assets";

        private static readonly string[] _subFolders =
        {
            "_Project/Art/Animations",
            "_Project/Art/Materials",
            "_Project/Art/Models",
            "_Project/Art/Textures",
            "_Project/Audio/Music",
            "_Project/Audio/Sfx",
            "_Project/Input",
            "_Project/Prefabs",
            "_Project/Scenes",
            "_Project/Scripts",
            "_Project/UI",
            "ThirdParty"
        };
        //////////////////////////////////////////////////////////////////////////////////////
        [MenuItem("Tools/Kickoff Kit/Setup")]
        private static void RunSetup()
        {
            CreateFolders();
            // Tells Unity to look at the disk again, so the new folders show up.
            AssetDatabase.Refresh();
            MoveDefaultFiles();
            CheckGitSettings();
            RemoveTutorialFiles();
        }
        //////////////////////////////////////////////////////////////////////////////////////
        private static void CreateFolders()
        {
            foreach (var folder in _subFolders)
            {
                var path = $"{RootFolder}/{folder}";

                // Does nothing if the folder already exists.
                Directory.CreateDirectory(path);

                // Git does not store empty folders. The .gitkeep file keeps the
                // folder in the repo. Unity ignores files that start with a dot.
                var keepFile = $"{path}/.gitkeep";
                if (!File.Exists(keepFile))
                {
                    File.WriteAllText(keepFile, string.Empty);
                }
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////
        // Moves the files every new Unity 6 project starts with into _Project.
        private static void MoveDefaultFiles()
        {
            MoveIfFree("Assets/Settings", "Assets/_Project/Settings");
            MoveIfFree("Assets/InputSystem_Actions.inputactions",
                "Assets/_Project/Input/InputSystem_Actions.inputactions");
            MoveIfFree("Assets/Scenes/SampleScene.unity",
                "Assets/_Project/Scenes/SampleScene.unity");

            DeleteFolderIfEmpty("Assets/Scenes");
        }
        //////////////////////////////////////////////////////////////////////////////////////
        // New Universal 3D projects come with Unity's tutorial files. A jam does not need
        // them, and their editor script can mess up the Project window on first open.
        private static void RemoveTutorialFiles()
        {
            if (!Exists("Assets/TutorialInfo") && !Exists("Assets/Readme.asset"))
            {
                return;
            }

            var remove = EditorUtility.DisplayDialog(
                "Kickoff Kit",
                "This project still has Unity's tutorial files (TutorialInfo and Readme). " +
                "Move them to the trash?",
                "Move to trash",
                "Keep");

            if (!remove)
            {
                return;
            }

            TrashIfExists("Assets/TutorialInfo");
            TrashIfExists("Assets/Readme.asset");
        }

        //////////////////////////////////////////////////////////////////////////////////////
        // MoveAssetToTrash puts the files in the system trash, so they can be restored.
        private static void TrashIfExists(string path)
        {
            if (!Exists(path))
            {
                return;
            }

            if (AssetDatabase.MoveAssetToTrash(path))
            {
                Debug.Log($"Kickoff Kit: moved {path} to the trash.");
            }
            else
            {
                Debug.LogWarning($"Kickoff Kit: could not move {path} to the trash.");
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////
        // Git needs the .meta files next to the assets and scenes saved as readable text.
        private static void CheckGitSettings()
        {
            // The version control mode is a plain string in Unity.
            // Only "Hidden Meta Files" breaks Git, so we leave other setups
            // like Perforce or Unity Version Control alone.
            if (VersionControlSettings.mode == "Hidden Meta Files")
            {
                VersionControlSettings.mode = "Visible Meta Files";
                Debug.Log("Kickoff Kit: switched Version Control mode to Visible Meta Files.");
            }

            // The serialization mode is an enum, so a typo would not even compile.
            if (EditorSettings.serializationMode != SerializationMode.ForceText)
            {
                var switchNow = EditorUtility.DisplayDialog(
                    "Kickoff Kit",
                    "Asset Serialization is not set to Force Text. Git can only merge scenes " +
                    "and prefabs that are saved as text.\n\nSwitch now? Unity will rewrite " +
                    "all assets, this can take a while in big projects.",
                    "Switch now",
                    "Later");

                if (switchNow)
                {
                    EditorSettings.serializationMode = SerializationMode.ForceText;
                    Debug.Log("Kickoff Kit: switched Asset Serialization to Force Text.");
                }
                else
                {
                    Debug.LogWarning("Kickoff Kit: Asset Serialization is not Force Text. " +
                                     "You can change it in Project Settings > Editor.");
                }
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////
        private static void MoveIfFree(string from, string to)
        {
            // Only move if the file still sits at Unity's default path
            // and nothing is in the way. We never overwrite anything.
            if (!Exists(from) || Exists(to))
            {
                return;
            }
            // MoveAsset takes the .meta file with it. The ID inside stays the
            // same, so every reference to the file keeps working.
            var error = AssetDatabase.MoveAsset(from, to);

            if (string.IsNullOrEmpty(error))
            {
                Debug.Log($"Kickoff Kit: moved {from} to {to}");
            }
            else
            {
                Debug.LogWarning($"Kickoff Kit: could not move {from}. {error}");
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////
        private static void DeleteFolderIfEmpty(string path)
        {
            if (Directory.Exists(path) && Directory.GetFileSystemEntries(path).Length == 0)
            {
                AssetDatabase.DeleteAsset(path);
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////
        private static bool Exists(string path)
        {
            return File.Exists(path) || Directory.Exists(path);
        }
    }
}