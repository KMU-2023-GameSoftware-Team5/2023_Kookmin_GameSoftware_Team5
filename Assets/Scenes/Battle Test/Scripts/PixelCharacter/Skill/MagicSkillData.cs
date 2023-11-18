using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using battle;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "magicSkill", menuName ="data/Skill/Magic", order = 1)]
public class MagicSkillData : CustomSkillData
{
    public GameObject effect;
    public int effectCount;
    public int damage;

    public override PixelHumanoid.State CreateSkillState()
    {
        return new SkillState(this);
    }

    public class SkillState : PixelHumanoid.ExtendedState
    {
        public SkillState(MagicSkillData skillData)
        {
            effect = skillData.effect;
            effectCount = skillData.effectCount;
            damage = skillData.damage;
        }

        public GameObject effect;
        public int effectCount;
        public int damage;

        protected override void onEnter(PixelHumanoid owner)
        {
            base.onEnter(owner);

            owner.stats.mp = 0;


            PixelCharacter[] enemies;
            owner.bm.GetAliveEnemiesFromClosest(owner, out enemies);
            for (int i = 0; (i < effectCount && i < enemies.Length); i++)
            {
                GameObject go = Instantiate(effect);
                Destroy(go, 3.0f);
                PixelCharacter target = enemies[i];

                go.transform.position = target.transform.position;
                owner.bm.ApplyDamage(owner, target, damage, true);
            }
        }

        protected override PixelHumanoid.EState onUpdate(PixelHumanoid owner)
        {
            return PixelHumanoid.EState.Chasing;
        }
    }
}
