using GameMap;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Purchasing;

namespace deck
{
    public class PlayerManager
    {
        /// <summary>
        /// 현재 스테이지 정보 
        /// </summary>
        GameMap.MapStageData stageData;

        /*
         * Static Pref Field
         */
        public static int MAX_INVENTORY_SIZE = 2;
        // public static int MAX_SELECTED_CHARACTER = 5;

        ///////////////////
        /* start of static */
        ///////////////////

        /// <summary>
        /// 싱글턴 객체 
        /// </summary>
        private static PlayerManager instance;

        /// <summary>
        /// playerManager를 저장=불러오기하는 객체
        /// </summary>
        private static SaveLoadManager saveLoadManager;

        /// <summary>
        /// PlayerManager 저장
        /// </summary>
        public static void save()
        {
            saveLoadManager.save(instance);
        }

        /// <summary>
        /// PlayerManager 불러오기 
        /// </summary>
        /// <returns>새로 갱신된 playerManager</returns>
        public static PlayerManager load()
        {
            saveLoadManager.load(instance);
            return instance;
        }

        /// <summary>
        /// Save 파일 삭제 및 현재 플레이어 객체 초기화
        /// <returns>새로 갱신된 playerManager</returns>
        /// </summary>
        public static PlayerManager delete()
        {
            saveLoadManager.delete(instance);
            instance = new PlayerManager();
            return instance;
        }

        public static void replace(PlayerManager playerManager)
        {
            instance = playerManager;
            saveLoadManager.save(instance);
        }

        public static PlayerManager Instance()
        {
            if(instance == null)
            {
                // 인스턴스가 없으면 새로 만들기 
                instance = new PlayerManager();

                // SaveLoadManager도 같이 만들기 
                saveLoadManager = new JsonFileSaveLoadManager();

                // player상태 불러오기
                saveLoadManager.load(instance);
            }
            return instance;
        }

        ///////////////////
        /* end of static */
        ///////////////////

        /// <summary>
        /// 플레이어가 가지고 있는 캐릭터 모음
        /// </summary>
        public List<PixelCharacter> playerCharacters { get; private set; }

        /// <summary>
        /// 플레이어가 가지고 있는 장비아이템 모음
        /// </summary>
        public List<EquipItem> playerEquipItems { get; private set; }

        /// <summary>
        /// 플레이어가 가지고 있는 골드 
        /// </summary>
        public int playerGold;

        /// <summary>
        /// 플레이어의 현재 라이프
        /// </summary>
        public int playerLife;

        /// <summary>
        /// 연승, 연패 카운트
        /// </summary>
        public int playerWinCount;

        /// <summary>
        /// 플레이어의 총 전투 수
        /// </summary>
        public int playerBattleCount;
        /// <summary>
        /// 플레이어의 패배횟수
        /// </summary>
        public int playerLoseCount;

        /// <summary>
        /// 플레이어의 최대 배치 가능 개수
        /// </summary>
        public int max_selectable = 3;
        /// <summary>
        /// 보유가능 캐릭터 개수
        /// </summary>
        public int max_character = 3;

        /// <summary>
        /// stage가 몇번 진행되었는가
        /// </summary>
        public int stageCount=0;
        public int StageCount { get { return stageCount; } }
        /// <summary>
        /// shopCount
        /// </summary>
        public int shopCount = 0;

        public int playerScore;
        
        /// <summary>
        /// 배치 정보 프리셋
        /// </summary>
        public JArray selectedCharacters;

        /* 생성자 모음 */
        public PlayerManager()
        {
            playerCharacters = new List<PixelCharacter>();
            playerEquipItems = new List<EquipItem>();
            selectedCharacters = new JArray();
        }

        public PlayerManager(List<PixelCharacter> playerCharacters, List<EquipItem> playerEquipItems, int playerGold, int playerLife)
        {
            this.playerCharacters = playerCharacters;
            this.playerEquipItems = playerEquipItems;
            selectedCharacters = new JArray();
            this.playerGold = playerGold;
            this.playerLife = playerLife;
        }

        public PlayerManager(List<PixelCharacter> playerCharacters, int playerGold, int playerLife) 
        {
            this.playerGold = playerGold;
            this.playerLife = playerLife;
            this.playerCharacters = playerCharacters;
            playerEquipItems = new List<EquipItem>();
            selectedCharacters = new JArray();
        }


        // 쓸데없이 많은 Initialze 삭제 

        void Intialize(int playerGold, int playerLife,  List<EquipItem> equipItems)
        {
            this.playerGold = playerGold;
            this.playerLife = playerLife;
            if (equipItems != null)
            {
                this.playerEquipItems = equipItems;
            }
            else
            {
                playerEquipItems = new List<EquipItem>();
            }
        }

        public void Initialize(int playerGold, int playerLife, List<PixelCharacter> characters, List<EquipItem> equipItems)
        {
            if (characters == null)
            {
                Debug.LogError("playerCharacters is NOT NULL");
                return;
            }

            Intialize(playerGold, playerLife, equipItems);
            this.playerCharacters = characters;
        }

        public void Initialize(int playerGold, int playerLife, List<PixelHumanoid> characters, List<EquipItem> equipItems)
        {
            if (characters == null)
            {
                Debug.LogError("playerCharacters is NOT NULL");
                return;
            }

            Intialize(playerGold, playerLife, equipItems);
            this.playerCharacters = new List<PixelCharacter>(characters);
        }

        /// <summary>
        /// 아이템 객체를 플레이어 인벤토리에 추가
        /// </summary>
        /// <param name="item">추가할 아이템 객체</param>
        /// <returns>성공여부</returns>
        public bool addEquipItem(EquipItem item)
        {
            playerEquipItems.Add(item);
            return true;
        }

        /// <summary>
        /// 아이템 이름으로 아이템 객체 생성 후 플레이어 인벤토리에 추가할 때 호출하는 함수
        /// </summary>
        /// <param name="itemName">추가할 아이템 이름</param>
        /// <returns>생성 후 리스트에 추가된 아이템</returns>
        public EquipItem addEquipItemByName(string itemName)
        {
            EquipItem ret;
            ret = MyDeckFactory.Instance().buildEquipItem(itemName);
            playerEquipItems.Add(ret);
            return ret;
        }

        /// <summary>
        /// 캐릭터 객체를 플레이어 인벤토리에 추가
        /// </summary>
        /// <param name="character">추가할 캐릭터 객체</param>
        /// <returns>성공여부</returns>
        public bool addCharacter(PixelCharacter character)
        {
            character.playerOwned = true;
            playerCharacters.Add(character);
            return true;
        }

        /// <summary>
        /// 캐릭터 이름으로 캐릭터 생성 후 플레이어 인벤토리에 추가할 때 호출하는 함수
        /// </summary>
        /// <param name="characterName">추가할 캐릭터 이름</param>
        /// <returns>생성 후 리스트에 추가된 캐릭터</returns>
        public PixelCharacter addCharacterByName(string characterName) { 
            PixelCharacter ret;
            ret = MyDeckFactory.Instance().buildPixelCharacter(characterName);
            playerCharacters.Add(ret);
            return ret; 
        }

        /// <summary>
        /// Save-Load 로직을 위해 Json화하는 메서드
        /// </summary>
        /// <returns>JSON화된 플레이어 매니저 객체</returns>
        public JObject toJson()
        {
            JObject saveJson = new JObject();

            // gold & life
            saveJson["playerGold"] = playerGold;
            saveJson["playerLife"] = playerLife;
            saveJson["playerWinCount"] = playerWinCount;
            saveJson["selectable"] = max_selectable;
            saveJson["stageCount"] = stageCount;
            saveJson["playerScore"] = playerScore;
            saveJson["playerBattleCount"] = playerBattleCount;
            saveJson["playerLoseCount"] = playerLoseCount;
            saveJson["shopCount"] = shopCount;

            // item save
            JArray itemArray = new JArray();
            if(playerEquipItems != null)
            {
                foreach (EquipItem item in playerEquipItems)
                {
                    itemArray.Add(item.toJson());
                }
                saveJson["items"] = itemArray;
            }
            else
            {
                saveJson["items"] = itemArray;
            }

            // character save
            JArray characterArray = new JArray();
            foreach (PixelCharacter character in playerCharacters)
            {
                characterArray.Add(character.toJson());
            }
            saveJson["characters"] = characterArray;

            // 캐릭터 배치정보 저장
            saveJson["selectedCharacterInfo"] = selectedCharacters;
            if(stageData != null)
                saveJson["stageData"] = stageData.toJson();
            return saveJson;
        }

        /// <summary>
        /// Save-Load 로직을 위해 Json을 입력받아 객체를 복원하는 메서드
        /// </summary>
        public void fromJson(JObject json)
        {
            if(json == null)// 불러올 거 없으면 return
            {
                return;
            }

            // gold & life 불러오기 
            if (json.ContainsKey("playerGold"))
            {
                playerGold = (int)json["playerGold"];
            }
            else
            {
                playerGold = 5;
            }
            if (json.ContainsKey("playerWinCount"))
            {
                playerWinCount = (int)json["playerWinCount"];
            }
            else
            {
                playerWinCount = 5;
            }
            if (json.ContainsKey("selectable"))
            {
                max_selectable = (int)json["selectable"];
            }
            else
            {
                max_selectable = 1;
            }
            if (json.ContainsKey("max_character"))
            {
                max_character = (int)json["max_character"];
            }
            else
            {
                max_character = 5;
            }
            if (json.ContainsKey("stageCount"))
            {
                stageCount = (int)json["stageCount"];
            }
            else
            {
                stageCount = 0;
            }
            if (json.ContainsKey("playerScore"))
            {
                playerScore = (int)json["playerScore"];
            }
            else
            {
                playerScore = 5;
            }
            if (json.ContainsKey("playerBattleCount"))
            {
                playerBattleCount = (int)json["playerBattleCount"];
            }
            else
            {
                playerBattleCount = 5;
            }
            if (json.ContainsKey("playerLoseCount"))
            {
                playerLoseCount = (int)json["playerLoseCount"];
            }
            else
            {
                playerLoseCount = 5;
            }
            if (json.ContainsKey("shopCount"))
            {
                shopCount = (int)json["shopCount"];
            }
            else
            {
                shopCount = 5;
            }




            // 아이템 장착처리를 위한 map 
            Dictionary<string, EquipItem> itemMap = new Dictionary<string, EquipItem>();

            // 아이템 불러오기
            List<EquipItem> equipItems = new List<EquipItem>();
            JArray jitems = (JArray)json["items"];
            foreach (JObject jitem in jitems)
            {
                EquipItem equipItem = new EquipItem();
                string ownerID = equipItem.fromJson(jitem);
                equipItems.Add(equipItem);
                if (ownerID != null)
                {
                    itemMap[equipItem.id] = equipItem;
                }
            }

            // 캐릭터 불러오기
            JArray jcharacters = (JArray)json["characters"];
            List<PixelCharacter> characters = new List<PixelCharacter>();
            foreach (JObject jcharacter in jcharacters)
            {
                PixelHumanoid character = new PixelHumanoid();
                character.playerOwned = true;
                character.fromJson(jcharacter, itemMap);
                characters.Add(character);
            }

            // 아이템-캐릭터를 manager로 반영하기 
            playerEquipItems = equipItems;
            playerCharacters = characters;

            // 캐릭터 배치 정보
            if(json["selectedCharacterInfo"].Type == JTokenType.Null)
            {
                selectedCharacters = new JArray();
            }
            else
            {
                selectedCharacters = (JArray)json["selectedCharacterInfo"];
            }

            if (!json.ContainsKey("stageData") || json["stageData"].Type == JTokenType.Null)
            {
                stageData = new MapStageData();
            }
            else
            {
                stageData = new MapStageData();
                stageData.fromJson((JObject)json["stageData"]);
            }
        }

        /// <summary>
        /// 캐릭터에게 아이템 장착 이벤트
        /// </summary>
        /// <param name="character">아이템을 장착할 캐릭터</param>
        /// <param name="equipId">캐릭터가 몇번 인벤토리에 아이템을 장착할 것인지</param>
        /// <param name="item">장착할 아이템</param>
        public bool equip(PixelCharacter character, int equipId, EquipItem item)
        {
            return character.equip(equipId, item);
        }

        /// <summary>
        /// 캐릭터 아이템 장착 해제 이벤트
        /// </summary>
        /// <param name="character">아이템을 해제할 캐릭터</param>
        /// <param name="equipId">해제될 아이템의 인벤토리상 위치</param>
        public bool unEquip(PixelCharacter character, int equipId)
        {
            return character.unEquip(equipId);
        }

        /// <summary>
        /// 캐릭터 구매관련 함수
        /// </summary>
        /// <param name="character">구매할 캐릭터</param>
        /// <param name="price">구매할 캐릭터 가격</param>
        /// <returns>구매 성공여부</returns>
        public bool buyCharacter(PixelCharacter character, int price)
        {
            if (useGold(price))
            {
                // 돈계산 
                // playerGold -= price;

                // 캐릭터 추가
                addCharacter(character);

                // 저장로직 호출
                save();
                return true;
            }
            else
            {
                // 구매 실패
                return false;
            }
        }

        public void removeCharacter(PixelCharacter character) {
            PixelCharacter removeTarget = null;
            for (int i = playerCharacters.Count - 1; i >= 0; i--)
            {
                if (playerCharacters[i].ID == character.ID)
                {
                    removeTarget = playerCharacters[i];
                    playerCharacters.RemoveAt(i);
                    break;
                }
            }
            if (removeTarget == null)
            {
                return;
            }
            else
            {
                for (int i = 0; i < removeTarget.Inventory.Length; i++)
                {
                    if (removeTarget.Inventory[i] != null) // 안정성을 위해 장착 아이템 해제 
                    {
                        removeTarget.unEquip(i);
                    }
                }
                save();
                return;
            }

        }

        public bool sellCharacter(PixelCharacter character, int price)
        {
            if(playerCharacters.Count == 1) // 캐릭터 하나 남기면 게임오버
            {
                return false;
            }
            else
            {
                PixelCharacter sellTarget = null;
                for(int i=playerCharacters.Count -1; i>=0; i--)
                {
                    if (playerCharacters[i].ID == character.ID)
                    {
                        sellTarget = playerCharacters[i];
                        playerCharacters.RemoveAt(i);
                        break;
                    }
                }
                if(sellTarget == null)
                {
                    return false;
                }
                else
                {
                    playerGold += price;
                    for(int i = 0; i < sellTarget.Inventory.Length; i++)
                    {
                        if (sellTarget.Inventory[i] != null) // 안정성을 위해 장착 아이템 해제 
                        {
                            sellTarget.unEquip(i);
                        }
                    }
                    save();
                    return true;
                }
            }
        }


        public List<PixelCharacter> sortingCharacter()
        {
            playerCharacters = playerCharacters.OrderByDescending(c => c.tier).ToList();
            return playerCharacters;
        }

        /// <summary>
        /// 플레이어가 돈을 소모해야할 때 사용. 돈이 충분한지 검사 및 돈소모까지 진행
        /// </summary>
        /// <param name="gold">지불해야할 돈</param>
        /// <returns>플레이어가 지불을 했는가?</returns>
        public bool useGold(int gold)
        {
            if(playerGold < gold)
            {
                return false;
            }
            else
            {
                playerGold -= gold;
                return true;
            }
        }



    }
}
  