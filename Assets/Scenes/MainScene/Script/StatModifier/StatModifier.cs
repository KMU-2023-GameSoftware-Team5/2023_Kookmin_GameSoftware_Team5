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
            * ������ ���� (ĳ���Ͱ� ������ �ִ� ��� ����)
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
            * ������ ���� ����(10% ����, +10 ���) 
            * TODO Percent�� ������ �ɷ� ���� 
         */
        Add,
    }

    static class StatEnumWrapper
    {
        // Enum To String�� ���� Class
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
            * ������ � ���� ������ ����� �� �� ������ �����ϴ� ��ü
            * �Ӽ�
                int value
                StatModType modType
                StastType statType
                StatModifierContainer owner : ������ unequip�� �� item�� ���ݸ�����̾ �����ϱ� ���� �ϱ� ���� item�� ���۷���
            * �޼���
                compareXXX : type, owner�� ���� getter ��� compare �Լ��� �����Ͽ� ���� �� �ٸ� ���� Ȯ��
         */
        int value; // ���� ������
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
