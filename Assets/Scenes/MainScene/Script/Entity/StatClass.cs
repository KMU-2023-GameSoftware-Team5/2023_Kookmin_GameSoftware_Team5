using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jslee
{
    public class StatClass
    {
        /*
         * ĳ���� �� ���������� ������ �����ϴ� Ŭ����
         * �޼���
            * public StatClass(bool hp, bool damage, bool walkSpeed, bool attackRange, bool attackDelay)
                * �������� ���� ������(�ӽ�)
            * absStr
                * +10�� ���ڿ��� +10���� ����ϱ� ���� �޼���
            * strForItem
                * �ӽ� ����Լ�
        */

        public int hp;
        public int damage;
        public float walkSpeed;
        public float attackRange;
        public float attackDelay;

        public StatClass(bool hp, bool damage, bool walkSpeed, bool attackRange, bool attackDelay)
        {
            System.Random random = new System.Random();

            while (hp && this.hp == 0)
            {
                this.hp = random.Next(-20, 21);
            }
            while (damage && this.damage == 0)
            {
                this.damage = random.Next(-20, 21);
            }
            while (walkSpeed && this.walkSpeed == 0)
            {
                this.walkSpeed = random.Next(-20, 21);
            }
            while (attackRange && this.attackRange == 0)
            {
                this.attackRange = random.Next(-20, 21);
            }
            while (attackDelay && this.attackDelay == 0)
            {
                this.attackDelay = random.Next(-20, 21);
            }

        }

        public StatClass()
        {

        }

        public string strForItem()
        {
            string ret = "";
            if (hp != 0)
            {
                ret += $"hp : {absStr(hp)}\n";
            }
            if (damage != 0)
            {
                ret += $"damage : {absStr(damage)}\n";
            }
            if (walkSpeed != 0)
            {
                ret += $"walkSpeed : {absStr(walkSpeed)}\n";
            }
            if (attackRange != 0)
            {
                ret += $"attackRange : {absStr(attackRange)}\n";
            }
            if (attackDelay != 0)
            {
                ret += $"attackDelay : {absStr(attackDelay)}\n";
            }
            return ret;
        }

        static public string absStr(int value)
        {
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
                return "";
            }
        }

        static public string absStr(float value)
        {
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
                return "";
            }
        }

    }

}