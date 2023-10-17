using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace battle
{
    public partial class PixelHumanoid
    {
        public enum ESkill
        {
            None,
            LightPillar,
            FireOrbit,
        }

        public class SkillFactory : StaticGetter<SkillFactory>, IOnStaticFound
        {
            private Dictionary<ESkill, State> s_skills;

            public bool OnStaticFound()
            {
                s_skills = new Dictionary<ESkill, State>();
                s_skills[ESkill.None] = s_noneSkill;
                s_skills[ESkill.LightPillar] = s_lightingPillarSkill;
                s_skills[ESkill.FireOrbit] = s_fireOrbitSkill;

                return true;
            }

            public State GetSkill(ESkill skill)
            {
                if (!s_skills.ContainsKey(skill))
                {
                    Debug.LogError("FATAL: Skill not found");
                    return null;
                }

                return s_skills[skill];
            }

            // do nothing
            private static State s_noneSkill = new State()
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
            public static State GetDefaultSkill() { return s_noneSkill; }

            private static int s_lightingPillarSkillDamage = 1;
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

            private static int s_fireOrbitSkillDamage = 1;
            private State s_fireOrbitSkill = new State()
            {
                OnEnter = (PixelHumanoid owner) =>
                {
                    owner.stats.mp = 0;

                    GameObject skillPrefap = StaticLoader.Instance().GetFireOrbit();
                    GameObject go = Instantiate(skillPrefap, Vector3.zero, Quaternion.identity, null);
                    FireOrbit orbit = go.GetComponent<FireOrbit>();

                    float life = 2.0f;
                    int repeat = 3;
                    orbit.Initialize(owner, s_fireOrbitSkillDamage, life, 1.0f, life / repeat);
                }
                ,
                OnUpdate = (PixelHumanoid owner) =>
                {
                    return EState.Chasing;
                }
            };
        }
    }
}
