using System;
using UnityEngine;

namespace data
{
    // non-scriptable version
    // it is a struct
    public struct CommonStats
    {
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