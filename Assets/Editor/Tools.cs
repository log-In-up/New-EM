using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    public class Tools
    {
        [MenuItem("Tools/Clear Save Data")]
        public static void ClearSaveData()
        {
            string fullPath = Path.Combine(Application.persistentDataPath);

            if (Directory.Exists(fullPath))
            {
                try
                {
                    DirectoryInfo dir = new(fullPath);
                    dir.Attributes &= ~FileAttributes.ReadOnly;
                    dir.Delete(true);

                    Debug.Log("Save data has been deleted.");
                }
                catch (Exception exeption)
                {
                    Debug.LogError($"Error occured when trying to delete data from path: {fullPath}.\n{exeption}");
                }
            }
            else
            {
                Debug.Log("Save data does not exist.");
            }
        }
    }
}