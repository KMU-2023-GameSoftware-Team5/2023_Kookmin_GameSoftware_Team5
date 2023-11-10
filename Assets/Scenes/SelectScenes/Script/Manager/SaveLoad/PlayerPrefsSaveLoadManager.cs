using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace deck
{ 
    /// <summary>
    /// PlayerPref을 사용하여 playerManager를 저장하는 객체
    /// </summary>
    public class PlayerPrefsSaveLoadManager : SaveLoadManager
    {
        public override void load(PlayerManager playerManager, string path="PlayerManager")
        {
            if (PlayerPrefs.HasKey("PlayerManager"))
            {
                JObject loadJObject = JObject.Parse(PlayerPrefs.GetString("PlayerManager"));
                playerManager.fromJson(loadJObject);
            }
            else
            {
                playerManager.fromJson(null);
            }
        }

        public override void save(PlayerManager playerManager, string path= "PlayerManager")
        {
            JObject saveJObject = playerManager.toJson();
            PlayerPrefs.SetString(path, saveJObject.ToString());
        }

        public override void delete(PlayerManager playerManager, string path = "PlayerManager")
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
