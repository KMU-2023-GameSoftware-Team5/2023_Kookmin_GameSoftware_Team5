using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jslee
{
    public class EquipItem
    {
        /*
            * ��� ������ ���� �����ϴ� ��ü 
            * TODO
                * ĳ���� A�� �����ϴ� �������� ĳ���� B�� ������ ���� ó�� ����
            * �Ӽ�
                * ItemName
                * itemStat
                * itemOwner : ���� ������ ������ 
                * idx : ���� ������ �����ڰ� �κ��丮���� ����� �����ϰ� �ִ���
            * �޼���
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
            // �׽�Ʈ�� ���� ���� ȿ�� ����
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
