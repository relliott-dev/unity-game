using UnityEngine;
using System;
using System.IO;

namespace RDE
{
    /// <summary>
    /// Manages saving and loading of player data to files
    /// This class handles file operations including creation, deletion, and data retrieval
    /// </summary>
    public class SaveFileDataWriter
    {
        private string saveDirectoryPath;

        public SaveFileDataWriter(string savePath)
        {
            saveDirectoryPath = Path.Combine(Application.persistentDataPath, savePath);
        }

        #region Public Methods

        // Creates a new file and writes player save data to it
        public void CreateNewFile(string fileName, PlayerSaveData playerSaveData)
        {
            string savePath = Path.Combine(saveDirectoryPath, fileName);

            try
            {
                string data = JsonUtility.ToJson(playerSaveData, true);

                using (FileStream stream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (StreamWriter fileWriter = new StreamWriter(stream))
                    {
                        fileWriter.Write(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Save Error: Game not saved " + savePath + "\n" + ex);
            }
        }

        // Loads and returns player save data from a specified file
        public PlayerSaveData LoadSaveFile(string fileName)
        {
            PlayerSaveData playerSaveData = null;
            string loadPath = Path.Combine(saveDirectoryPath, fileName);

            if (File.Exists(loadPath))
            {
                try
                {
                    string data = "";

                    using (FileStream stream = new FileStream(loadPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (StreamReader fileReader = new StreamReader(stream))
                        {
                            data = fileReader.ReadToEnd();
                        }
                    }

                    playerSaveData = JsonUtility.FromJson<PlayerSaveData>(data);
                }
                catch (Exception ex)
                {
                    Debug.LogError("Load Error: Game not loaded " + loadPath + "\n" + ex);
                }
            }
            else
            {
                Debug.LogWarning("Load Warning: File not found " + loadPath);
            }

            return playerSaveData;
        }

        // Returns a list of save files
        public string[] GetAllSaveFiles()
        {
            try
            {
                return Directory.GetFiles(saveDirectoryPath);
            }
            catch (Exception ex)
            {
                Debug.LogError("GetAllSaveFiles Error: " + ex);
                return new string[0];
            }
        }

        // Checks if a file exists
        public bool CheckFileExists(string fileName)
        {
            return File.Exists(Path.Combine(saveDirectoryPath, fileName));
        }

        // Deletes a specified file
        public void DeleteSaveFile(string fileName)
        {
            try
            {
                File.Delete(Path.Combine(saveDirectoryPath, fileName));
            }
            catch (Exception ex)
            {
                Debug.LogError("Delete Error: Could not delete file " + fileName + "\n" + ex);
            }
        }

        #endregion
    }
}