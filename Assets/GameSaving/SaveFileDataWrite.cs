using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace SG{
    public class SaveFileDataWrite : MonoBehaviour
    {
        public string saveDataDirectoryPath= Application.persistentDataPath;
        public string saveFileName = "";

        public bool CheckToSeeIfFileExists()
        {
            if(File.Exists(Path.Combine(saveDataDirectoryPath,saveFileName)))
            {
                return true;
            }
            else
            {
                return false;
            } 
        }

        public void CreateNewCharacterSaveFile(CharacterSaveData characterData)
        {
            //Make a path to save file
            string savePath = Path.Combine(saveDataDirectoryPath,saveFileName);

            try
            {
                //Create the directory the file will be written to if it does not already 
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                Debug.Log("CREATING SAVEFILE, AT SAVE PATH :" + savePath);

                //SERIALIZE THE C3 GAME DATA OBJECT INTO JSON
                string dataToStore = JsonUtility.ToJson(characterData,true);

                //Write the file to Our System
                using(FileStream stream = new FileStream(savePath, FileMode.Create))
                {
                    using(StreamWriter fileWriter = new StreamWriter(stream))
                    {
                        fileWriter.Write(dataToStore);
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.LogError("Error Trying to save character data, not saved" + savePath + "\n" + ex);
            }
        }

        public CharacterSaveData LoadSaveFile()
        {
            CharacterSaveData characterData = null;
            string loadPath = Path.Combine(saveDataDirectoryPath,saveFileName);

            if (File.Exists(loadPath))
            {
                try
                {
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(loadPath,FileMode.Open))
                    {
                        using(StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }
                characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
                }
                catch (Exception ex)
                {
                   // characterData = nuell;
                }
            }
            return characterData;
        }
    }
}

