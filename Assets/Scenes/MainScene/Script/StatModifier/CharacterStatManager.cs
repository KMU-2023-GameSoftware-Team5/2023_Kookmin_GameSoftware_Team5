using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jslee;
using System;

namespace jslee
{
    public class CharacterStatManager
    {
        /*
            * 캐릭터의 스텟을 관리하고 결정함 
            * 속성
                StatType statType : 현재 이 인스턴스가 캐릭터의 어떤 속성을 관리하는지 
                modifiers : 아이템 등에 의해 추가된 스텟모디파이어의 집합
                baseValue : 캐릭터가 기본적으로 가지고 있는 스텟값
                finalValue : 스텟 모디파이어와 baseValue를 고려하여 최종적으로 결정된 캐릭터의 스텟값
            * 메서드 
                addModifier : 스텟모디파이어컨테이너의 모디파이어 중 이 인스턴스와 관리하는 스텟의 종류(StatType)이 같은 모디파이어를 저장
                removeModifier : 캐릭터에게서 제거되는 컨테이너에 속한 모디파이어를 제거함
                getValue : finalValue를 계산하고 반환
         */

        public StatType statType;
        public List<StatModifier> modifiers;

        public int baseValue
        {
            get;
        }

        public int finalValue;
        public bool isUpate = true;

        public CharacterStatManager(StatType statType)
        {
            modifiers = new List<StatModifier>();
            this.statType = statType;
            System.Random random = new System.Random();
            baseValue = random.Next(0, 20);
        }

        public override string ToString()
        {
            return $"{statType} : {getValue()} ({baseValue})";
        }

        public void addModifier(StatModifierContainer obj)
        {
            // 같은 스텟 타입을 가지면 모디파이어를 추가함
            foreach (StatModifier modifier in obj.getStatModifier())
            {
                if (modifier.compareStatType(this.statType))
                {
                    isUpate = true;
                    modifiers.Add(modifier);
                }
            }
        }

        public void removeModifier(StatModifierContainer obj)
        {
            // 같은 스텟 타입을 가지면 모디파이어를 제거함
            for (int i = modifiers.Count - 1; i >= 0; i--)
            {
                StatModifier modifier = modifiers[i];
                if (modifier.compareOwner(obj)) // 제거의 기준은 제거된 아이템과 모디파이어의 Owner가 같은지 확인
                {
                    isUpate = true;
                    modifiers.RemoveAt(i);
                }
            }
        }

        public int getValue()
        {
            if (isUpate)
            {
                finalValue = baseValue;
                foreach (StatModifier modifier in modifiers)
                {
                    if (modifier.compareModType(StatModType.Add))
                    {
                        finalValue += modifier.getValue();
                    }
                }
                isUpate = false;
            }
            return finalValue;
        }
    }


}

