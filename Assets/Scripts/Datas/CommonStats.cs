using System;
using Unity.VisualScripting;
using UnityEngine;

namespace data
{
    // non-scriptable version
    // it is a struct
    public struct CommonStats
    {
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