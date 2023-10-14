using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lee
{
    public partial class PixelHumanoid
    {
        public enum ESkill
        {
            Default, 
            LightningPillar, 
        }

        public class SkillFactory : StaticGetter<SkillFactory>
        {
            // do nothing
            private static State s_defaultSkill = new State()
            {
                OnEnter = (PixelHumanoid owner) =>
                {
                    owner.stats.mp = 0;
                },
                OnUpdate = (PixelHumanoid owner) =>
                {
                    return EState.Chasing;
                }
            };
            public static State GetDefaultSkill() { return s_defaultSkill; }

            private static int s_lightingPillarSkillDamage = 3;
            private static State s_lightingPillarSkill = new State()
            {
                OnEnter = (PixelHumanoid owner) =>
                {
                    owner.stats.mp = 0;


                    PixelCharacter[] enemies;
                    owner.bm.GetAliveEnemiesFromClosest(owner, out enemies);
                    for(int i = 0; i < 3 && i < enemies.Length; i++)
                    {
                        GameObject go = Instantiate(StaticLoader.Instance().GetLightningPillar());
                        LightingPillar pillar =  go.GetComponent<LightingPillar>();

                        pillar.Initialize(owner.bm, owner, enemies[i].entityId, s_lightingPillarSkillDamage);
                    }
                    
                },
                OnUpdate = (PixelHumanoid owner) =>
                {
                    return EState.Chasing;
                }
            };
            public static State GetLightingPillarSkill() { return s_lightingPillarSkill; }
        }
    }
}
