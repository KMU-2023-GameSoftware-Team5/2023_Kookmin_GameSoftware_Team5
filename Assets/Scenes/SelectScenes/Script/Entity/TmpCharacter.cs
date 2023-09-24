using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using static UnityEditor.Progress;

namespace jslee
{ 
    /// <summary>
    /// 임시로 사용하는 캐릭터 객체. 추후 개선 필요
    /// </summary>
    public class TmpCharacter 
    {
        /// <summary>
        /// 캐릭터의 이름(캐릭터 팩토리에서 사용하는 캐릭터의 이름)
        /// </summary>
        public string characterName { get; private set; }
        /// <summary>
        /// 플레이어가 설정할 캐릭터의 닉네임
        /// </summary>
        string characterNickName;
        /// <summary>
        /// 플레이어가 장착한 아이템 인벤토리
        /// </summary>
        public EquipItem[] Inventory;
        public StatClass characterStat;
        /// <summary>
        /// TODO : 플레이어의 스프라이트로 대체할 것
        /// </summary>
        public Color playerColor;

        public TmpCharacter(string nickname, Color color)
        {
            string[] characterNames = { "Demon", "Skeleton", "Goblin Archor" };
            System.Random random = new System.Random();
            characterName = characterNames[random.Next(0, characterNames.Length)];

            characterNickName = nickname;
            characterStat = new StatClass();
            Inventory = new EquipItem[EquipItemManager.MAX_INVENTORY_SIZE];
            playerColor = color;
        }

        /// <summary>
        /// 캐릭터에게 아이템 장착 메서드
        /// </summary>
        /// <param name="idx">캐릭터가 아이템을 몇번 슬롯에 장착할 것인가</param>
        /// <param name="item">캐릭터가 장착할 아이템</param>
        public void equip(int idx, EquipItem item)
        {
            if (Inventory[idx] != null)
            {
                unEquip(idx);
            }
            Inventory[idx] = item;
            Inventory[idx].equip(idx, this);
        }

        /// <summary>
        /// 캐릭터의 아이템 장착해제 메서드
        /// </summary>
        /// <param name="idx">인벤토리에서의 n번째 슬롯을 제거</param>
        public void unEquip(int idx)
        {
            Inventory[idx].unEquip();
            Inventory[idx] = null;
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
        public string getDescribe() {
            string ret = "Character Description\n";
            ret += $"{getName()}";
            return ret;
        }
    }
}

