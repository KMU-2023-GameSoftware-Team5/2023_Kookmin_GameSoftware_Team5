using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using battle;


[CreateAssetMenu(fileName = "MeleeSkill", menuName = "data/Skill/Melee", order = 1)]
public class MeleeSkillData : CustomSkillData
{
    public GameObject effect;
    public int effectRepeatCount;   // 최소 1
    public float effectGapSec;
    public int effectSpawnRange;

    public int damage;
    public float range;         // move towards enemy till in range

    public override PixelHumanoid.State CreateSkillState() 
    {
        return new SkillState(this);
    }

    public class SkillState : PixelHumanoid.ExtendedState
    {
        public SkillState(MeleeSkillData data) 
        {
            effect = data.effect;
            effectRepeatCount = data.effectRepeatCount;
            effectSpawnRange = data.effectSpawnRange;
            damage = data.damage;
            range = data.range;
            effectGapSec = data.effectGapSec;
        }

        public GameObject effect;
        public int effectRepeatCount;
        public float effectSpawnRange;
        public float effectGapSec;

        public int damage;
        public float range;

        protected override PixelHumanoid.EState onUpdate(PixelHumanoid owner) 
        {
            float distance = 0;
            PixelHumanoid target = owner.bm.GetClosestAliveEnemy(owner.transform, owner.teamIndex, out distance);

            if (target == null)
            {
                return PixelHumanoid.EState.Chasing;
            }

            if (distance > range)
            {
                Vector3 delta = target.transform.position - owner.transform.position;
                delta.Normalize();
                delta = delta * owner.stats.walkSpeed * Time.deltaTime;
                owner.transform.position += delta;

                // consider on paused
                if (delta.x != 0.0)
                {
                    if (delta.x > 0.0f)
                        owner.SetDirection(Utility.Direction2.Right);
                    else
                        owner.SetDirection(Utility.Direction2.Left);
                }

                owner.GetAnimator().SetBool("Idle", false);
                owner.GetAnimator().SetBool("Walking", true);

                return PixelHumanoid.EState.None;
            }
            else
            {
                owner.stats.mp = 0;

                for (int i = 0; i < effectRepeatCount; i++)
                {
                    Vector3 world_pos = Vector3.zero;
                    if (effectSpawnRange > 0)
                    {
                        float x = Random.Range(-1.0f, 1.0f);
                        float y = Random.Range(-1.0f, 1.0f);
                        world_pos = new Vector3(x, y, 0.0f);
                        world_pos.Normalize();
                        world_pos *= effectSpawnRange;
                    }

                    world_pos += target.transform.position;
                    Utility.InstantiateAfter(effect, world_pos, effectGapSec * i, 1.0f);
                }

                
                // GameObject skillGo = GameObject.Instantiate(effect);
                // skillGo.transform.position = target.transform.position;

                owner.bm.ApplyDamage(owner, target, this.damage, true);
                // Destroy(skillGo, 1);

                return PixelHumanoid.EState.Chasing;
            }
        }
    }
}
