using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace battle
{
    public partial class PixelHumanoid
    {
        public enum ESkill
        {
            None,
            LightPillar,
            FireOrbit,
            SingleHeal,
            Vulture, 
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
                s_skills[ESkill.SingleHeal] = s_singleHealSkill;
                s_skills[ESkill.Vulture] = s_vulture;

                return true;
            }

            public State GetStaticSkill(ESkill skill)
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

            private static int s_singleHealAmount = 20;
            private static State s_singleHealSkill = new State()
            {
                OnEnter = (PixelHumanoid owner) =>
                {
                    owner.stats.mp = 0;

                    PixelCharacter[] characters;
                    owner.bm.GetAliesFromLowestHp(owner, out characters);

                    // heal self 50% of skill heal amount
                    owner.bm.ApplyHeal(owner, owner, s_singleHealAmount / 2);
                    if (characters.Length > 0)
                    {
                        owner.bm.ApplyHeal(owner, characters[0], s_singleHealAmount);
                    }
                }
                ,
                OnUpdate = (PixelHumanoid owner) =>
                {
                    return EState.Chasing;
                }
            };

            private static State s_vulture = new State()
            {
                OnUpdate = (PixelHumanoid owner) =>
                {
                    PixelCharacter dead = owner.bm.GetClosestDead(owner);
                    if (dead == null)
                    {
                        owner.stats.mp /= 3;

                        return EState.Chasing;
                    }
                    else
                    {
                        float distance = Utility.GetDistanceBetween(owner.transform, dead.transform);
                        if (distance <= 1.0f)
                        {
                            owner.stats.mp = 0;

                            owner.bm.ApplyHeal(owner, owner, dead.maxHp / 10);
                            owner.stats.damage += 2;

                            GameObject chompGo = Instantiate(StaticLoader.Instance().GetChomp());
                            chompGo.transform.position = dead.transform.position + Vector3.up * 0.6f;
                            Destroy(chompGo, 0.5f);

                            owner.bm.Vulture(owner, (PixelHumanoid)dead);   

                            return EState.Chasing;
                        }
                        else
                        {
                            Vector3 delta = dead.transform.position - owner.transform.position;
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

                            owner.m_animator.SetBool("Idle", false);
                            owner.m_animator.SetBool("Walking", true);

                            return EState.Skill;
                        }
                    }
                }
            };
        }
    }
}
