using System;
using UnityEngine;

namespace data
{
    // non-scriptable version
    // it is a struct
    [Serializable]
    public struct CommonStats
    {
        public CommonStats(int value)
        {
            sheild = value;
            hp = value;
            mp = value;
            energy = value;
            walkSpeed = value;
            damage = value;
            attackDelay = value;
            criticalRate = value;
        }

        public CommonStats(CommonStats other)
        {
            sheild = other.sheild;
            hp = other.hp;
            mp = other.mp;
            energy = other.energy;
            walkSpeed = other.walkSpeed;
            damage = other.damage;
            attackDelay = other.attackDelay;
            criticalRate = other.criticalRate;
        }

        public void CopyFrom(scriptable.CommonStats other)
        {
            sheild = other.sheild;
            hp = other.hp;
            mp = other.mp;
            energy = other.energy;
            walkSpeed = other.walkSpeed;
            damage = other.damage;
            attackDelay = other.attackDelay;
            criticalRate = other.criticalRate;
        }

        public void Add(CommonStats other)
        {
            sheild += other.sheild;
            hp += other.hp;
            mp += other.mp;
            energy += other.energy;
            walkSpeed += other.walkSpeed;
            damage += other.damage;
            attackDelay += other.attackDelay;
            criticalRate += other.criticalRate;
        }

        public int sheild;
        public int hp;
        public int mp;
        public int energy;
        public float walkSpeed;
        public int damage;
        public float attackDelay;
        public float criticalRate;

        public static CommonStats operator +(CommonStats A, CommonStats B) {
            CommonStats ret = new CommonStats();
            ret.sheild = A.sheild + B.sheild;
            ret.hp = A.hp + B.hp;
            ret.mp = A.mp + B.mp;
            ret.energy = A.energy + B.energy;
            ret.walkSpeed = A.walkSpeed + B.walkSpeed;
            ret.damage = A.damage + B.damage;
            ret.attackDelay = A.attackDelay + B.attackDelay;
            ret.criticalRate = A.criticalRate + B.criticalRate;
            return ret;
        }


        public CommonStats CreateMultiflied(float scale)
        {
            CommonStats ret = new CommonStats(this);

            ret.sheild = (int)(sheild * scale);
            ret.hp = (int)(hp * scale);
            ret.mp = (int)(mp * scale);
            ret.energy = (int)(energy * scale);
            ret.damage = (int)(damage * scale);
            
            // 업그레이드 하지 않아도 변하지 않는 스텟
            ret.criticalRate = criticalRate;
            ret.walkSpeed = walkSpeed;
            ret.attackDelay = attackDelay;

            return ret;
        }

        public CommonStats GetUpgradedStats(int upgradeLevel)
        {
            switch (upgradeLevel)
            {
                case 1:
                    return CreateMultiflied(1.0f);
                case 2:
                    return CreateMultiflied(1.7f);
                case 3:
                    return CreateMultiflied(2.6f);
                case 4:
                    return CreateMultiflied(3.7f);
                case 5:
                    return CreateMultiflied(4.8f);
                case 6:
                    return CreateMultiflied(5.8f);
                case 7:
                    return CreateMultiflied(6.9f);
                case 8:
                    return CreateMultiflied(7.9f);
                case 9:
                    return CreateMultiflied(9.0f);
                default:
                    Debug.LogError("invalid upgrade level: " + upgradeLevel.ToString());
                    return CreateMultiflied(1.0f);
            }
        }
    }

    namespace scriptable
    {
        // scriptable version
        public class CommonStats : ScriptableObject
        {
            public void CopyFrom(CommonStats other)
            {
                sheild = other.sheild;
                hp = other.hp;
                mp = other.mp;
                energy = other.energy;
                walkSpeed = other.walkSpeed;
                damage = other.damage;
                attackDelay = other.attackDelay;
                criticalRate = other.criticalRate;
            }

            [Header("CommonStats")]
            public int sheild;
            public int hp;
            public int mp;
            public int energy;
            public float walkSpeed;
            public int damage;
            public float attackDelay;
            public float criticalRate;
        }
    }
}