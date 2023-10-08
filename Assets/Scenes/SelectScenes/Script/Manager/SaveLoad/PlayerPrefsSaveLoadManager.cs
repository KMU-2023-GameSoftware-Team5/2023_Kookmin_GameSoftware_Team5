using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace deck
{ 
    public class PlayerPrefsSaveLoadManager : SaveLoadManager
    {

        public override void load()
        {
            if (PlayerPrefs.HasKey("PlayerManager"))
            {
                PlayerManager playerManager = new PlayerManager();
                
                // 캐릭터 목록 
                string loadPlayerCharacters = PlayerPrefs.GetString("playerCharacters");
                Debug.Log($"playerJson = {loadPlayerCharacters}");
                List<PixelHumanoid> playerCharacters = JsonConvert.DeserializeObject<List<PixelHumanoid>>(loadPlayerCharacters);

                // 아이템 목록
                string loadPlayerEquipItems = PlayerPrefs.GetString("playerEquipItems");
                Debug.Log($"itemJson = {loadPlayerEquipItems}");
                List<EquipItem> playerEquipItems = JsonConvert.DeserializeObject<List<EquipItem>>(loadPlayerEquipItems);

                foreach (PixelCharacter character in playerCharacters)
                {
                    character.loadForJson();
                }
                foreach (EquipItem item in playerEquipItems)
                {
                    item.loadForJson();   
                }

                // 플레이어 돈
                int playerGold = PlayerPrefs.GetInt("playerGold");
                int playerLife = PlayerPrefs.GetInt("playerLife");

                playerManager.Initialize(playerGold, playerLife, playerCharacters, playerEquipItems);
                PlayerManager.Initialize(playerManager);
            }
            else
            {
                PlayerManager playerManager = new PlayerManager();
                playerManager.Initialize();
                PlayerManager.Initialize(playerManager);
                Debug.Log($"new Manager");
            }
        }

        public override void save()
        {
            PlayerManager playerManager = PlayerManager.Instance();
            foreach (PixelCharacter character in playerManager.playerCharacters)
            {
                character.saveForJson();
            }
            PlayerPrefs.SetInt("playerGold", playerManager.playerGold);
            PlayerPrefs.SetInt("playerLife", playerManager.playerLife);

            PlayerPrefs.SetString("playerCharacters", JsonConvert.SerializeObject(playerManager.playerCharacters));
            PlayerPrefs.SetString("playerEquipItems", JsonConvert.SerializeObject(playerManager.playerEquipItems));

            PlayerPrefs.SetString("PlayerManager", "save");
        }

        public override void delete()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
