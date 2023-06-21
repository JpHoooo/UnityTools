using Jphoooo.Tools.Runtimes;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Jphoooo.EditorTool
{
    public class EditorTest : Editor
    {    
        [MenuItem("Assets/Create/Config Json File")]
        static void CreateFolder()
        {

            string streamingAssetPath = Path.Combine(Application.dataPath, "StreamingAssets");

            DirectoryInfo dir = new DirectoryInfo(streamingAssetPath);

            if (!dir.Exists)
            {
                dir.Create();
            }

            string configFilePath = Path.Combine(streamingAssetPath, "Config.json");
            FileInfo fileInfo = new FileInfo(configFilePath);

            Debug.Log("Create Config Json File".WithColor(Color.green));

            AssetDatabase.CreateAsset(null, streamingAssetPath + "/Config.json");
        }
    }
}
