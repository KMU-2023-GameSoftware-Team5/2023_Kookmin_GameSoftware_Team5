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
            * 스텟 모디파이어를 관리하는 객체 
                예시 ) 아이템
            * 속성
                public bool isEquip; - 플레이어에게 이 컨테이너가 장착되어있는 상태인지
                StatModifier[] modifiers - 스텟 모디파이어의 집합
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
            * 아이템 정보 관리하는 객체 
                스텟모디파이어컨테이너 상속
        */

        string ItemName;

        public Item(String itemName)
        {
            this.ItemName = itemName;
            // 테스트를 위한 랜덤 효과 생성
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
