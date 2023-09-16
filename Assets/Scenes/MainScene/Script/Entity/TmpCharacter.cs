using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace jslee
{
    public class TmpCharacter
    {
        /* 
            * 아이템 시스템 테스트를 위한 임시 캐릭터 class 
            * TODO 
                스텟 출력하기(아이템으로 증가한 스텟포함)
            * 속성
                Inventory : 플레이어가 소지할 수 있는 아이템
                charStat : 캐릭터의 스텟
                characterName : 캐릭터의 이름
            * 메서드
                equip : 장비 착용
                unequop : 장비 해제
         */

        public EquipItem[] Inventory;
        public StatClass charStat;
        string characterName;

        public TmpCharacter(string name)
        {
            characterName = name;
            charStat = new StatClass();
            Inventory = new EquipItem[3];
        }

        public void equip(int idx, EquipItem item)
        {
            if (Inventory[idx] != null)
            {
                unequip(idx);
            }
            Inventory[idx] = item;
            Inventory[idx].equip(idx, this);
        }

        public void unequip(int idx)
        {
            Inventory[idx].unEquip();
            Inventory[idx] = null;
        }

        public override string ToString()
        {
            string ret = $"name : {characterName}\n";
            return ret;
        }
    }

}