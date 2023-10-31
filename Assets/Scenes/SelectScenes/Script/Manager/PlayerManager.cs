using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace deck
{
    public class PlayerManager
    {
        private static PlayerManager instance;
        public static void Initialize(PlayerManager playerManager)
        {
            instance = playerManager;
            // Debug.Log($"singleton : {playerManager.playerCharacters.Count}");
        }
        public static PlayerManager Instance()
        {
            if(instance == null)
            {
                instance = new PlayerManager();
                instance.Initialize();
            }
            return instance;
        }

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

        public void Initialize()
        {
            playerCharacters = new List<PixelCharacter>();
            playerEquipItems = new List<EquipItem>();
        }

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

        // TODO ?- remove 캐릭터, remove 아이템

        // TODO - save, load
    }
}
