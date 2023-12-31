using deck;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonFileSaveLoadManager : SaveLoadManager
{
    #if (UNITY_EDITOR)
    string directoryPath = $"{Application.dataPath}/Scenes/SelectScenes/SaveFile"; // 오로지 디버깅상황에서만
#else
    string directoryPath = $"{Application.persistentDataPath}/SaveFile"; // 빌드상황에서만 
#endif
    string makeFilePath(string path)
    {
        
        return $"{directoryPath}/{path}.json";
    }

    public override void load(PlayerManager playerManager, string path = "PlayerManager")
    {
        string filePath = makeFilePath(path);

        if (File.Exists(filePath))
        {
            string loadString = File.ReadAllText(filePath);
            JObject loadJObject; // load
            loadJObject = JObject.Parse(loadString);
            playerManager.fromJson(loadJObject);
        }
        else
        {
            playerManager.fromJson(null);
        }
    }

    public override void save(PlayerManager playerManager, string path = "PlayerManager")
    {
        if(!Directory.Exists(directoryPath))
        {
            Debug.Log($"{directoryPath}"); 
            Directory.CreateDirectory(directoryPath);
        }
        string filePath = makeFilePath(path);
        JObject saveJObject = playerManager.toJson();
        // save
        File.WriteAllText(filePath, saveJObject.ToString());
    }

    public override void delete(PlayerManager playerManager, string path = "PlayerManager")
    {
        string filePath = makeFilePath(path);

        if (File.Exists(filePath))
        {
            string loadString = File.ReadAllText(filePath);
            JObject loadJObject; // load
            loadJObject = JObject.Parse(loadString);
            playerManager.fromJson(loadJObject);
        }
        else
        {
            playerManager.fromJson(null);
        }
    }
}
