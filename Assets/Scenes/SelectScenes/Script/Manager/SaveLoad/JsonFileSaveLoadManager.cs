using deck;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonFileSaveLoadManager : SaveLoadManager
{
    string makeFilePath(string path)
    {
        return $"{Application.dataPath}/Scenes/SelectScenes/SaveFile/{path}.json";
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
