using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using data;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.Assertions.Must;

// PixelHumanoid가 가질 수 있는 State를 정의하는 파일

namespace battle
{
    public partial class PixelHumanoid
    {
        // StateFactory는 PixelHumanoid의 inner class이기 때문에 private member에 접근할 수 있다. 
        // 만약 State 객체가 PixelHumanoid의 inner class이면 더 편할까? 
        public partial class StateFactory
        {
            // state 내부에 상태를 저장하지 않는 경우, 
            // 상태 객체를 하나만 만들어 레퍼런스를 공유한다. 
            private static State s_watingState = new State()
            {
                OnEnter = (PixelHumanoid owner) =>
                {
                    owner.m_animator.SetBool("Dead", false);
                    owner.m_animator.SetBool("Walking", false);
                    owner.m_animator.SetBool("Idle", true);
                },
                OnUpdate = (PixelHumanoid owner) =>
                {
                    return EState.None;
                }
            };
            public static State GetWaitingState() { return s_watingState; }

            private static State s_searchingState = new State()
            {
                OnEnter = (PixelHumanoid owner) =>
                {
                    // 안하면 무한루프
                    owner.m_fsm.SetForcedNextState(EState.None);

                    owner.m_animator.SetBool("Idle", false);
                    owner.m_animator.SetBool("Dead", false);
                    owner.m_animator.SetBool("Walking", true);
                },
                OnUpdate = (PixelHumanoid owner) =>
                {
                    float distance;

                    PixelHumanoid enemy = owner.bm.GetClosestAliveEnemy(owner.transform, owner.teamIndex, out distance);
                    distance = Mathf.Sqrt(distance);

                    owner.targetId = enemy.entityId;
                    return EState.Chasing;

                    //// 범위 안의 적을 찾은 경우
                    //if (distance <= owner.searchingRange)
                    //{
                    //    owner.targetId = enemy.entityId;
                    //    return EState.Chasing;
                    //}

                    //Vector3 delta = Vector3.zero;
                    //if (owner.m_direction == Utility.Direction2.Left)
                    //{
                    //    delta = Vector3.left * Time.deltaTime * owner.stats.walkSpeed;
                    //}
                    //else if (owner.m_direction == Utility.Direction2.Right)
                    //{
                    //    delta = Vector3.right * Time.deltaTime * owner.stats.walkSpeed;
                    //}
                    //owner.transform.position += delta;

                    //// consider on paused
                    //if (delta.x != 0)
                    //{
                    //    if (delta.x > 0.0f)
                    //        owner.SetDirection(Utility.Direction2.Right);
                    //    else
                    //        owner.SetDirection(Utility.Direction2.Left);
                    //}

                    //return EState.None;
                }
            };
            public static State GetSearchingState() { return s_searchingState; }

            private static State s_chasingState = new State()
            {
                OnUpdate = (PixelHumanoid owner) =>
                {
                    float distance;

                    PixelCharacter target = owner.bm.GetEntity(owner.targetId, BattleManager.EDeadOrAlive.Alive);
                    if (target == null) // 새로운 타겟을 찾는다. 
                    {
                        target = owner.bm.GetClosestAliveEnemy(owner.transform, owner.teamIndex, out distance);
                        if (target == null) // 새로운 타겟 찾기 실패
                        {
                            return EState.Waiting;
                        }
                        else
                        {
                            owner.targetId = target.entityId;
                            return EState.None;
                        }
                    }
                    else
                    {
                        distance = Utility.GetDistanceBetween(owner.transform, target.transform);
                        if (distance <= owner.attackRange) // 공격 가능 범위에 들어온 경우
                        {
                            if (owner.defaultAttackType == EDefualtAttackType.Melee)
                            {
                                return EState.MeleeAttacking;
                            }
                            else
                            {
                                return EState.RangedAttacking;
                            }

                        }
                        else
                        {
                            Utility.MoveToTarget(owner, target);

                            return EState.None;
                        }
                    }
                }
            };
            public static State GetChasingState() { return s_chasingState; }

            private static State s_meleeAttackingState = new State()
            {
                OnUpdate = (PixelHumanoid owner) =>
                {
                    owner.m_animator.SetBool("Idle", false);
                    owner.m_animator.SetBool("Walking", false);

                    PixelCharacter target = owner.bm.GetEntity(owner.targetId, BattleManager.EDeadOrAlive.Alive);
                    if (target == null)
                    {
                        return EState.Chasing;
                    }
                    else
                    {
                        owner.GetAudioSource().clip = StaticLoader.Instance().GetSoundData().meleeAttack;
                        owner.GetAudioSource().Play();

                        owner.m_animator.SetTrigger("Slash");
                        owner.bm.ApplyDefaultAttack(owner, target);

                        owner.leftAttackDelay = owner.stats.attackDelay;
                        return EState.Delaying;
                    }
                }
            };
            public static State GetMeleeAttackingState() { return s_meleeAttackingState; }

            public static State s_rangedAttackingState = new State()
            {
                OnUpdate = (PixelHumanoid owner) =>
                {
                    owner.m_animator.SetBool("Idle", false);
                    owner.m_animator.SetBool("Walking", false);

                    PixelCharacter target = owner.bm.GetEntity(owner.targetId, BattleManager.EDeadOrAlive.Alive);
                    if (target == null)
                    {
                        return EState.Chasing;
                    }
                    else
                    {
                        owner.GetAudioSource().clip = StaticLoader.Instance().GetSoundData().rangedAttack;
                        owner.GetAudioSource().Play();

                        switch (owner.defaultAttackType)
                        {
                            case EDefualtAttackType.RangedShot:
                                {
                                    owner.m_animator.SetTrigger("Shot");

                                    // TODO: Create Factory for Projectiles
                                    GameObject arrow = StaticLoader.Instance().GetDefaultArrowPrefab();
                                    GameObject arrowGo = Instantiate(arrow, owner.transform.position + Vector3.up, Quaternion.identity, null);
                                    AttackProjectile attackProjectile = arrowGo.GetComponent<AttackProjectile>();
                                    attackProjectile.InitializeAsDefaultAttack(owner.bm, owner, owner.targetId, 0.5f, true, 2.0f, 10.0f);

                                    break;
                                }
                            case EDefualtAttackType.RangedFire1:
                                {
                                    owner.m_animator.SetTrigger("Fire1H");
                                    break;
                                }
                            case EDefualtAttackType.RangedFire2:
                                {
                                    owner.m_animator.SetTrigger("Fire2H");
                                    break;
                                }
                        }

                        owner.leftAttackDelay = owner.stats.attackDelay;
                        return EState.Delaying;
                    }
                }
            };
            public static State GetRangedAttackingState() { return s_rangedAttackingState; }

            private static State s_deadState = new State()
            {
                OnEnter = (PixelHumanoid owner) =>
                {
                    owner.m_animator.SetBool("Idle", false);
                    owner.m_animator.SetBool("Walking", false);
                    owner.m_animator.SetBool("Dead", true);
                },
                OnUpdate = (PixelHumanoid onwer) =>
                {
                    return EState.None;
                }
            };
            public static State GetDeadState() { return s_deadState; }

            private static State s_delayingState = new State()
            {
                OnEnter = (PixelHumanoid owner) =>
                {
                    owner.m_animator.SetBool("Idle", true);
                    owner.m_animator.SetBool("Dead", false);
                    owner.m_animator.SetBool("Walking", false);
                },
                OnUpdate = (PixelHumanoid owner) =>
                {
                    owner.leftAttackDelay -= Time.deltaTime;

                    if (owner.leftAttackDelay <= 0)
                    {
                        return EState.Chasing;
                    }

                    if (owner.stats.mp >= 100)
                        return EState.Skill;
                    else
                        return EState.None;
                }
            };
            public static State GetDelayingState() {  return s_delayingState; }

            public class BeingVulturedState : ExtendedState
            {
                public uint vulturerId;

                public BeingVulturedState()
                {
                    vulturerId = 0;
                }

                protected override EState onUpdate(PixelHumanoid owner)
                {
                    if (!owner.IsDead())
                        return EState.Chasing;

                     PixelCharacter vulture =  owner.bm.GetEntity(vulturerId, BattleManager.EDeadOrAlive.All);
                    if (vulture != null)
                    {
                        float distance = Utility.GetDistanceBetween(owner.transform, vulture.transform);
                        if (distance <= 0.5f)
                        {
                            owner.m_sr.enabled = false;
                            return EState.Waiting;
                        }
                        else
                        {
                            Vector3 delta = vulture.transform.position - owner.transform.position;
                            delta.Normalize();
                            delta = delta * 10.0f * Time.deltaTime;
                            owner.transform.position += delta;

                            return EState.BeingVultured;
                        }
                    }
                    else
                    {
                        if (!owner.IsDead())
                            return EState.Chasing;
                        else 
                            return EState.Dead;
                    }
                }
            }
        }
    }
}
