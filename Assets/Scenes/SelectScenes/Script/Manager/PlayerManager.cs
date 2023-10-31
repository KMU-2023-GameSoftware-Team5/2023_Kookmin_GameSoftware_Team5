using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace deck
{
    public class PlayerManager
    {
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
            this.playerGold = playerGold;
            this.playerLife = playerLife;
        }

        public PlayerManager(List<PixelCharacter> playerCharacters, int playerGold, int playerLife)
        {
            this.playerGold = playerGold;
            this.playerLife = playerLife;
            this.playerCharacters = playerCharacters;
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
            playerGold = (int)json["playerGold"];
            playerLife = (int)json["playerLife"];

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
                character.fromJson(jcharacter, itemMap);
                characters.Add(character);
            }

            // 아이템-캐릭터를 manager로 반영하기 
            playerEquipItems = equipItems;
            playerCharacters = characters;

            // 캐릭터 배치 정보
            selectedCharacters = (JArray) json["selectedCharacterInfo"];
            if(selectedCharacters == null)
            {
                selectedCharacters = new JArray();
            }
        }
    }
}
