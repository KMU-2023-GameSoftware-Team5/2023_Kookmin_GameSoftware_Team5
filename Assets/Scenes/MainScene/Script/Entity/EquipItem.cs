using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jslee
{
    public class EquipItem
    {
        /*
            * 장비 아이템 정보 관리하는 객체 
            * TODO
                * 캐릭터 A가 착용하는 아이템을 캐릭터 B가 착용할 때의 처리 개선
            * 속성
                * ItemName
                * itemStat
                * itemOwner : 현재 아이템 소유자 
                * idx : 현재 아이템 소유자가 인벤토리에서 몇번에 소유하고 있는지
            * 메서드
                * equip
                * unEquip
                * isEquip
        */

        string ItemName;
        StatClass itemStat;
        private TmpCharacter itemOwner;
        private int idx = -1;

        public EquipItem(string itemName)
        {
            this.ItemName = itemName;
            // 테스트를 위한 랜덤 효과 생성
            System.Random random = new System.Random();
            bool hp  = random.NextDouble() > 0.5;
            bool damage = random.NextDouble() > 0.5;
            bool walkSpeed = random.NextDouble() > 0.5;
            bool attackRange = random.NextDouble() > 0.5;
            bool attackDelay = random.NextDouble() > 0.5;
            itemStat = new StatClass(
                hp, 
                damage,
                walkSpeed,
                attackRange,
                attackDelay
            );
        }

        public string getItemName()
        {
            return ItemName;
        }

        public void equip(int idx, TmpCharacter owner)
        {
            if(itemOwner != null) 
            {
                itemOwner.Inventory[idx] = null;
            }
            
            this.idx = idx;
            itemOwner = owner;
        }
        public void unEquip()
        {
            this.idx = -1;
            itemOwner = null;
        }
        public bool isEquip()
        {
            return itemOwner != null;
        }

        public override string ToString()
        {
            string ret = "";
            if (itemStat.hp != 0)
            {
                ret += $"hp : {itemStat.hp}\n";
            }
            if (itemStat.damage != 0)
            {
                ret += $"damage : {itemStat.damage}\n";
            }
            if (itemStat.walkSpeed != 0)
            {
                ret += $"walkSpeed : {itemStat.walkSpeed}\n";
            }
            if (itemStat.attackRange != 0)
            {
                ret += $"attackRange : {itemStat.attackRange}\n";
            }
            if (itemStat.attackDelay != 0)
            {
                ret += $"attackDelay : {itemStat.attackDelay}\n";
            }
            return ret;
        }
    }

}
