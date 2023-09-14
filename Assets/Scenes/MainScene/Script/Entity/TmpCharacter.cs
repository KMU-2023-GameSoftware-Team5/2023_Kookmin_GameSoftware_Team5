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
            * ������ �ý��� �׽�Ʈ�� ���� �ӽ� ĳ���� class 
            * TODO 
                ���� ����ϱ�(���������� ������ ��������)
            * �Ӽ�
                Inventory : �÷��̾ ������ �� �ִ� ������
                charStat : ĳ������ ����
                characterName : ĳ������ �̸�
            * �޼���
                equip : ��� ����
                unequop : ��� ����
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