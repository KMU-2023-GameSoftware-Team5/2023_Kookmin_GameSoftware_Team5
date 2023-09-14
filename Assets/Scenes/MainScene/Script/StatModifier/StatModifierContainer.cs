using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jslee;
using System;

namespace jslee
{
    public class StatModifierContainer
    {
        /*
            * ���� ������̾ �����ϴ� ��ü 
                ���� ) ������
            * �Ӽ�
                public bool isEquip; - �÷��̾�� �� �����̳ʰ� �����Ǿ��ִ� ��������
                StatModifier[] modifiers - ���� ������̾��� ����
         */
        private bool isEquip = false;

        protected StatModifier[] modifiers { get; }
        public StatModifier[] getStatModifier()
        {
            return modifiers;
        }

        public StatModifierContainer()
        {
            modifiers = new StatModifier[2];
        }

        public override string ToString()
        {
            string ret = "";
            foreach (StatModifier modifier in modifiers)
            {
                ret += modifier.ToString();
            }
            return ret;
        }

        public void equip()
        {
            isEquip = true;
        }
        public void unequip()
        {
            isEquip = false;
        }
    }

    public class Item : StatModifierContainer
    {
        /*
            * ������ ���� �����ϴ� ��ü 
                ���ݸ�����̾������̳� ���
        */

        string ItemName;

        public Item(String itemName)
        {
            this.ItemName = itemName;
            // �׽�Ʈ�� ���� ���� ȿ�� ����
            System.Random random = new System.Random();
            Array StatTypes = Enum.GetValues(typeof(StatType));
            Array StatModTypes = Enum.GetValues(typeof(StatModType));
            for (int i = 0; i < 2; i++)
            {
                modifiers[i] = new StatModifier
                (
                    random.Next(-10, 10),
                    (StatModType)StatModTypes.GetValue(random.Next(StatModTypes.Length)),
                    (StatType)StatTypes.GetValue(random.Next(StatTypes.Length)),
                    this
                );
            }
        }

        public string getItemName()
        {
            return ItemName;
        }
    }

}
