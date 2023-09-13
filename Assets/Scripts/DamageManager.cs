using System;

namespace DamageManager
{
    public static class Types{
        public static string[] TypeIndex = {
            "기본", //에러처리용, damage 0 고정
            "물리",
            "마법",
            "회복"
        };
    }

    public class Damage
    {
        int type;
        int damage;

        //데미지공식
        public Damage(Attack attack, Defense defense)
        {

        }
    }

    public class Attack
    {
        int type = 0;    //공격 타입 - 물리, 마법 등
        int damage = 0;     //기본 데미지
        float criticalRate = 0.1f; //치명확률
        float criticalDamage = 1.5f;   //치명데미지 배율 
        float penetrate = 0;    //관통
        int repeat = 1;     //반복

        string description; // 설명

        public Attack(
                string typeName,
                int damage,
                float criticalRate,
                float criticalDamage,
                float penetrate
            )
        {
            int type = Array.IndexOf(DamageManager.Types.TypeIndex, typeName);
            if (type > 0)
            {
                this.type = type;
                this.damage = damage;
            }
            if (criticalRate > 0)
                this.criticalRate = criticalRate;
            if (criticalDamage > 0)
                this.criticalDamage = criticalDamage;
            if (penetrate != 0)
                this.penetrate = penetrate;
        }
    }
    public class Defense
    {
        private string[] typeIndex = {
            "기본", //에러처리용, value 0 고정
            "물리",
            "마법",
            "회복"
    };
        int type = 0;    //타입 - 물리, 마법 등
        int defense = 0;     //방어력
        float criticalReduce = 0; //치명확률 감소
        float criticalDamageReduce = 0;   //치명데미지 배율 감소
        float reduce = 0;    //피해감소 - 관통과 상쇄

        public Defense(
                string typeName,
                int defense,
                float criticalReduce,
                float criticalDamageReduce,
                float reduce
            )
        {
            int type = Array.IndexOf(DamageManager.Types.TypeIndex, typeName);
            if (type > 0)
            {
                this.type = type;
                this.defense = defense;
            }
            this.criticalReduce = criticalReduce;
            if (criticalDamageReduce > 0)
                this.criticalDamageReduce = criticalDamageReduce;
            if (reduce != 0)
                this.reduce = reduce;
        }
    }
}