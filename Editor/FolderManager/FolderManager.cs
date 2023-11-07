using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System;


namespace Jphoooo.Tools{
    public class FolderManager 
    {

        static string rootPath = Application.dataPath; 
        [MenuItem("FolderManager/DefaultFolders %F1",false,2)]
        static void CreateDefaultFolder(){
            
            List<string> FolderNames = new List<string> { "Scenes","Scripts","Materials","Shaders","Textures","Models","Resources","StreamingAssets"};

            DirectoryInfo[] infos = new DirectoryInfo(rootPath).GetDirectories();
        
            foreach(DirectoryInfo info in infos){
                if(FolderNames.Contains(info.Name)){
                    FolderNames.Remove(info.Name);
                }   
            }
            foreach(string name in FolderNames){
                Directory.CreateDirectory(Path.Combine(rootPath, name));
            }
            AssetDatabase.Refresh();
        } 
        [MenuItem("FolderManager/DeleteEmptyFolder %F2",false,3)]  
        static void DeleteEmptyFolder(){
            DirectoryInfo[] infos = new DirectoryInfo(rootPath).GetDirectories();
            foreach(DirectoryInfo info in infos){
                DirectoryInfo directoryInfo = new DirectoryInfo(info.FullName);
                FileInfo[] fileInfo = directoryInfo.GetFiles();
                if (fileInfo.Length == 0){
                    try
                    {
                        Directory.Delete(info.FullName);
                        File.Delete(Path.Combine(info.Parent.FullName, info.Name + ".meta"));
                    }
                    catch (Exception e)
                    {                            
                        throw new Exception(e.Message) ;
                    }              
                }
            }
            AssetDatabase.Refresh();
        }
    }
}

