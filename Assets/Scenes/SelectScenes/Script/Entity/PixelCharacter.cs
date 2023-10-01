using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using data;

namespace deck{
    /// <summary>
    /// Deck 편집에서 관리하는 캐릭터 객체
    /// </summary>
    public abstract class PixelCharacter
    {
        protected string characterNickName;
        public string characterName { get; set; }
        public EquipItem[] Inventory;
        protected CommonStats characterStat; 

        public Vector3 worldPosition {  get; set; }

        /// <summary>
        /// 캐릭터에게 아이템 장착 메서드
        /// </summary>
        /// <param name="idx">캐릭터가 아이템을 몇번 슬롯에 장착할 것인가</param>
        /// <param name="item">캐릭터가 장착할 아이템</param>
        public bool equip(int idx, EquipItem item)
        {
            foreach (EquipItem myItem in Inventory)
            {
                if (myItem != null)
                {
                    if (myItem == item) // 똑같은 아이템을 중복 착용하는 것을 방지
                    {
                        return false;
                    }
                }
            }
            if (Inventory[idx] != null) // 해당 아이템 슬롯에 아이템이 이미 있는 경우
            {
                unEquip(idx);
            }
            Inventory[idx] = item;
            Inventory[idx].equip(idx, this);
            return true;
        }

        /// <summary>
        /// 캐릭터의 아이템 장착해제 메서드
        /// </summary>
        /// <param name="idx">인벤토리에서의 n번째 슬롯을 제거</param>
        public bool unEquip(int idx)
        {
            Inventory[idx].unEquip();
            Inventory[idx] = null;
            return true;
        }

        /// <summary>
        /// 캐릭터의 이름 + 별칭 출력 메서드
        /// </summary>
        /// <returns>"캐릭터 별칭(캐릭터 이름)"</returns>
        public string getName()
        {
            string ret = $"{characterNickName}({characterName})\n";
            return ret;
        }

        /// <summary>
        /// 캐릭터에 대한 설명 출력 메서드
        /// </summary>
        /// <returns>캐릭터에 대한 설명</returns>
        public string getDescribe()
        {
            string ret = "Character Description\n";
            ret += $"{getName()}";
            return ret;
        }
        
        public CommonStats getEquipItemStats()
        {
            CommonStats ret = new CommonStats();
            foreach(EquipItem item in Inventory)
            {
                if(item != null)
                {
                    ret += item.itemStat;
                }
            }
            return ret;
        }

        public CommonStats getCharacterStats()
        {
            CommonStats equipItemStat = getEquipItemStats();
            CommonStats ret = characterStat + equipItemStat;
            return ret;
        }
    }
}
