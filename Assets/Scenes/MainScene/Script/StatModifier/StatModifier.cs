using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using jslee;

namespace jslee
{

    public enum StatType
    {
        /*
            * 스텟의 종류 (캐릭터가 가지고 있는 모든 스텟)
         */
        Sheild,
        Hp,
        Energy,
        WalkSpeed,
        Damage,
        AttackDelay,
        LeftAttackDelay
    }   

    public enum StatModType
    {
        /*
            * 스텟의 증감 형태(10% 증가, +10 등등) 
            * TODO Percent로 아이템 능력 증감 
         */
        Add,
    }

    static class StatEnumWrapper
    {
        // Enum To String을 위한 Class
        public static string StatTypeToString(StatType type)
        {
            switch (type)
            {
                case StatType.Sheild:
                    return "Sheild";
                case StatType.Hp:
                    return "Hp";
                case StatType.Energy:
                    return "Energy";
                case StatType.WalkSpeed:
                    return "WalkSpeed";
                case StatType.Damage:
                    return "Damage";
                case StatType.AttackDelay:
                    return "AttackDelay";
                case StatType.LeftAttackDelay:
                    return "LeftAttackDelay";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static string StatModTypeToString(StatModType statMod, int value)
        {

            switch (statMod)
            {
                case StatModType.Add:
                    if (value > 0)
                    {
                        return $"+{value}";
                    }
                    else if (value < 0)
                    {
                        return $"{value}";
                    }
                    else
                    {
                        return "+0";
                    }
                /*
                case StatModType.Percent:
                    if (value > 0)
                    {
                        return $"+{value}%";
                    }
                    else if (value < 0)
                    {
                        return $"{value}%";
                    }
                    else
                    {
                        return "0%";
                    }
                */
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }
    }

    public class StatModifier
    {
        /*
            * 아이템 등에 의해 스텟이 변경될 때 그 정보를 관리하는 객체
            * 속성
                int value
                StatModType modType
                StastType statType
                StatModifierContainer owner : 아이템 unequip할 때 item의 스텟모디파이어를 제거하기 쉽게 하기 위한 item의 레퍼런스
            * 메서드
                compareXXX : type, owner의 값은 getter 대신 compare 함수를 지원하여 같은 지 다른 지만 확인
         */
        int value; // 스텟 증감량
        StatModType modType;
        StatType statType;
        StatModifierContainer owner;

        public StatModifier(int value, StatModType modType, StatType statType, StatModifierContainer owner)
        {
            this.value = value;
            this.modType = modType;
            this.statType = statType;
            this.owner = owner;
        }

        public int getValue()
        {
            return value;
        }
        public bool compareOwner(StatModifierContainer obj)
        {
            return ReferenceEquals(owner, obj);
        }
        public bool compareStatType(StatType obj)
        {
            return ReferenceEquals(statType, obj);
        }
        public bool compareModType(StatModType obj)
        {
            return ReferenceEquals(modType, obj);
        }

        public override string ToString()
        {
            string ret = $"{StatEnumWrapper.StatTypeToString(statType)} : {StatEnumWrapper.StatModTypeToString(modType, value)} \n";
            return ret;
        }
    }
}
