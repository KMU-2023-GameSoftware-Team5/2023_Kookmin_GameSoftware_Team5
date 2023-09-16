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
            * ĳ������ ������ �����ϰ� ������ 
            * �Ӽ�
                StatType statType : ���� �� �ν��Ͻ��� ĳ������ � �Ӽ��� �����ϴ��� 
                modifiers : ������ � ���� �߰��� ���ݸ�����̾��� ����
                baseValue : ĳ���Ͱ� �⺻������ ������ �ִ� ���ݰ�
                finalValue : ���� ������̾�� baseValue�� ����Ͽ� ���������� ������ ĳ������ ���ݰ�
            * �޼��� 
                addModifier : ���ݸ�����̾������̳��� ������̾� �� �� �ν��Ͻ��� �����ϴ� ������ ����(StatType)�� ���� ������̾ ����
                removeModifier : ĳ���Ϳ��Լ� ���ŵǴ� �����̳ʿ� ���� ������̾ ������
                getValue : finalValue�� ����ϰ� ��ȯ
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
            // ���� ���� Ÿ���� ������ ������̾ �߰���
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
            // ���� ���� Ÿ���� ������ ������̾ ������
            for (int i = modifiers.Count - 1; i >= 0; i--)
            {
                StatModifier modifier = modifiers[i];
                if (modifier.compareOwner(obj)) // ������ ������ ���ŵ� �����۰� ������̾��� Owner�� ������ Ȯ��
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

