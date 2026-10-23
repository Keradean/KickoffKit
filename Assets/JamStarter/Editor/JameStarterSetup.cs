using System.IO;
using UnityEditor;
using UnityEngine;
//////////////////////////////////////////////////////////////////////////////////////
namespace JamStarter
{
    // Adds "Tools > Jam Starter > Setup" to the Unity menu bar.
    public static class JamStarterSetup
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
        [MenuItem("Tools/Jam Starter/Setup")]
        private static void RunSetup()
        {
            CreateFolders();

            // Tells Unity to look at the disk again, so the new folders show up.
            AssetDatabase.Refresh();

            MoveDefaultFiles();
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
                Debug.Log($"Jam Starter: moved {from} to {to}");
            }
            else
            {
                Debug.LogWarning($"Jam Starter: could not move {from}. {error}");
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