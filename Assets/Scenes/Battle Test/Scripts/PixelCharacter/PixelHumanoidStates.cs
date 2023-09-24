using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PixelHumanoid가 가질 수 있는 State를 정의하는 파일

namespace lee
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
                Start = (PixelHumanoid owner) =>
                {
                    owner.m_animator.SetBool("Dead", false);
                    owner.m_animator.SetBool("Walking", false);
                    owner.m_animator.SetBool("Idle", true);
                },
                Update = (PixelHumanoid owner) =>
                {
                    return EState.None;
                }
            };
            public static State GetWaitingState() { return s_watingState; }

            private static State s_searchingState = new State()
            {
                Start = (PixelHumanoid owner) =>
                {
                    // 안하면 무한루프
                    owner.m_fsm.SetTransitionToSearch(false);

                    owner.m_animator.SetBool("Idle", false);
                    owner.m_animator.SetBool("Dead", false);
                    owner.m_animator.SetBool("Walking", true);
                }, 
                Update = (PixelHumanoid owner) =>
                {
                    float distance;
                    PixelHumanoid enemy = owner.bm.GetClosestAliveEnemy(owner.transform, owner.teamIndex, out distance);
                    distance = Mathf.Sqrt(distance);

                    // 범위 안의 적을 찾은 경우
                    if (distance <= owner.searchingRange)
                    {
                        owner.targetId = enemy.entityId;
                        return EState.Chasing;
                    }

                    Vector3 delta = Vector3.zero;
                    if (owner.m_direction == Utility.Direction2.Left)
                    {
                        delta = Vector3.left * Time.deltaTime * owner.walkSpeed;
                    }
                    else if (owner.m_direction == Utility.Direction2.Right)
                    {
                        delta = Vector3.right * Time.deltaTime * owner.walkSpeed;
                    }
                    owner.transform.position += delta;

                    // consider on paused
                    if (delta.x != 0)
                    {
                        if (delta.x > 0.0f)
                            owner.SetDirection(Utility.Direction2.Right);
                        else
                            owner.SetDirection(Utility.Direction2.Left);
                    }

                    return EState.None;
                }
            };
            public static State GetSearchingState() { return s_searchingState; }

            private static State s_chasingState = new State()
            {
                Update = (PixelHumanoid owner) =>
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
                            Vector3 delta = target.transform.position - owner.transform.position;
                            delta.Normalize();
                            delta = delta * owner.walkSpeed * Time.deltaTime;
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

                            return EState.None;
                        }
                    }
                }
            };
            public static State GetChasingState() { return s_chasingState; }

            private static State s_meleeAttackingState = new State()
            {
                Update = (PixelHumanoid owner) =>
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
                        owner.m_animator.SetTrigger("Slash");
                        owner.bm.HandleDefaultAttack(owner, target);

                        owner.leftAttackDelay = owner.attackDelay;
                        return EState.Delaying;
                    }
                }
            };
            public static State GetMeleeAttackingState() { return s_meleeAttackingState; }

            public static State s_rangedAttackingState = new State()
            {
                Update = (PixelHumanoid owner) =>
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
                        switch (owner.defaultAttackType)
                        {
                            case EDefualtAttackType.RangedShot:
                                {
                                    owner.m_animator.SetTrigger("Shot");

                                    // TODO: Create Factory for Projectiles
                                    GameObject arrow = StaticLoader.Instance().GetDefaultArrowPrefab();
                                    GameObject arrowGo = Instantiate(arrow, owner.transform.position + Vector3.up, Quaternion.identity, null);
                                    AttackProjectile attackProjectile = arrowGo.GetComponent<AttackProjectile>();
                                    attackProjectile.Initialize(owner.bm, owner, owner.transform.position, owner.targetId, 0.5f, true, 2.0f, 10.0f, owner.damage, false);

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

                        owner.leftAttackDelay = owner.attackDelay;
                        return EState.Delaying;
                    }
                }
            };
            public static State GetRangedAttackingState() { return s_rangedAttackingState; }

            private static State s_deadState = new State()
            {
                Start = (PixelHumanoid owner) =>
                {
                    owner.m_fsm.SetTransitionToDead(false);

                    owner.m_animator.SetBool("Idle", false);
                    owner.m_animator.SetBool("Walking", false);
                    owner.m_animator.SetBool("Dead", true);
                }
            };
            public static State GetDeadState() { return s_deadState; }

            private static State s_delayingState = new State()
            {
                Start = (PixelHumanoid owner) =>
                {
                    owner.m_animator.SetBool("Idle", true);
                    owner.m_animator.SetBool("Dead", false);
                    owner.m_animator.SetBool("Walking", false);
                },
                Update = (PixelHumanoid owner) =>
                {
                    owner.leftAttackDelay -= Time.deltaTime;

                    if (owner.leftAttackDelay <= 0)
                    {
                        return EState.Chasing;
                    }

                    return EState.None;
                }
            };
            public static State GetDelayingState() {  return s_delayingState; }
        }
    }
}
