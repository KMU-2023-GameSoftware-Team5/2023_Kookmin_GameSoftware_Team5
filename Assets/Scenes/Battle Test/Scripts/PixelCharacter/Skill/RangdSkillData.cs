using battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using battle;

[CreateAssetMenu(fileName = "RangedSkill", menuName = "data/Skill/RangedSkill", order = 1)]
public class RangedSkillData : CustomSkillData
{
    public GameObject projectile;
    public float projectileRadius;
    public int projectileCount;
    public float projectileGapSec;
    public bool rotateProjectileDirection;
    public float projectileSpeed;

    public int damage;
    public float range;         // move towards enemy till in range

    public override PixelHumanoid.State CreateSkillState()
    {
        return new SkillState(this);
    }

    public class SkillState : PixelHumanoid.ExtendedState
    {
        public SkillState(RangedSkillData data) 
        { 
            projectile = data.projectile;
            projectileRadius = data.projectileRadius;
            projectileCount = data.projectileCount;
            projectileGapSec = data.projectileGapSec;
            rotateProjectileDirection = data.rotateProjectileDirection;

            damage = data.damage;
            range = data.range;
        }

        public GameObject projectile;
        public float projectileRadius;
        public int projectileCount;
        public float projectileGapSec;
        public bool rotateProjectileDirection;

        public int damage;
        public float range;         // move towards enemy till in range

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

                for (int i = 0; i < projectileCount; i++)
                {
                    Utility.InstantiateProjectileAsDamageAfter(
                        i * projectileGapSec,
                        projectile,
                        owner.bm,
                        owner,
                        owner.transform.position + Vector3.up,
                        target.entityId,
                        projectileRadius, rotateProjectileDirection, 10, 10, damage);
                }

                return PixelHumanoid.EState.Chasing;
            }
        }
    }
}
