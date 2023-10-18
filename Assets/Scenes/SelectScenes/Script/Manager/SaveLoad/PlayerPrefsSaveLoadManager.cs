using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
                JObject jplayerManager = JObject.Parse(PlayerPrefs.GetString("PlayerManager"));

                // gold & life
                int playerGold = (int)jplayerManager["playerGold"];
                int playerLife = (int)jplayerManager["playerLife"];

                // 장착처리를 위한 map 
                Dictionary<string, EquipItem> itemMap = new Dictionary<string, EquipItem>(); 

                // 아이템 
                JArray jitems = (JArray) jplayerManager["items"];

                List<EquipItem> equipItems = new List<EquipItem>();
                foreach(JObject jitem in jitems)
                {
                    EquipItem equipItem = new EquipItem();
                    string ownerID = equipItem.fromJson(jitem);
                    equipItems.Add(equipItem);
                    if(ownerID != null)
                    {
                        itemMap[equipItem.id] = equipItem;
                    }
                }

                // 캐릭터  
                JArray jcharacters = (JArray)jplayerManager["characters"];
                List<PixelCharacter> characters = new List<PixelCharacter>();
                foreach (JObject jcharacter in jcharacters)
                {
                    PixelHumanoid character = new PixelHumanoid();
                    character.fromJson(jcharacter, itemMap);
                    characters.Add(character);
                }
                playerManager.Initialize(playerGold, playerLife, characters, equipItems);
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
            JObject jplayerManager = new JObject();

            // gold & life
            jplayerManager["playerGold"] = playerManager.playerGold;
            jplayerManager["playerLife"] = playerManager.playerLife;

            // item save
            JArray itemArray = new JArray();
            foreach (EquipItem item in playerManager.playerEquipItems)
            {
                itemArray.Add(item.toJson());
            }
            jplayerManager["items"] = itemArray;

            // character save
            JArray characterArray = new JArray();
            foreach(PixelCharacter character in playerManager.playerCharacters)
            {
                characterArray.Add(character.toJson());
            }
            jplayerManager["characters"] = characterArray;

            Debug.Log(jplayerManager);
            PlayerPrefs.SetString("PlayerManager", jplayerManager.ToString());
        }

        public override void delete()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
