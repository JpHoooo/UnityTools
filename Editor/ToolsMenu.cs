using UnityEngine;
using UnityEditor;
using static System.IO.Directory;
using static UnityEditor.AssetDatabase;
using static System.IO.Path;
using static UnityEngine.Application;

namespace Jphoooo.Tools
{
    public static class ToolsMenu
    {
        [MenuItem("Tools/Setup/Create Default Folders")]
        public static void CreateDefaultFolders()
        {
            CreateDirectories("_Project", "Scripts", "Arts", "Scenes", "Plugins");
            Refresh();
        }

        public static void CreateDirectories(string root, params string[] dir)
        {
            var fullpath = Combine(dataPath, root);

            foreach (string name in dir)
                CreateDirectory(Combine(fullpath, name));
        }
    }
}

