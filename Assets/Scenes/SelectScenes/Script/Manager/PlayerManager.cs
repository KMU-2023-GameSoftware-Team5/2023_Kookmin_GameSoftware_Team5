using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace deck
{
    public class PlayerManager : MonoBehaviour
    {
        private static PlayerManager instance;
        public static PlayerManager Instance()
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerManager>();
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

        void Start()
        {
            Initialize();
        }

        void Initialize()
        {
            playerCharacters = new List<PixelCharacter>();
            playerEquipItems = new List<EquipItem>();
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
        /// <returns>성공여부</returns>
        public bool addEquipItemByName(string itemName)
        {
            EquipItem ret;
            ret = MyDeckFactory.Instance().buildEquipItem(itemName);
            playerEquipItems.Add(ret);
            return true;
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
        /// <returns>성공여부</returns>
        public bool addCharacterByName(string characterName) { 
            PixelCharacter ret;
            ret = MyDeckFactory.Instance().buildPixelCharacter(characterName);
            playerCharacters.Add(ret);
            return true; 
        }

        // TODO ?- remove 캐릭터, remove 아이템

        // TODO - save, load
    }
}
