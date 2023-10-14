using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using data;

namespace battle
{
    public partial class PixelHumanoid
    {
        public partial class StateFactory: StaticGetter<StateFactory>
        {
            private State s_watingState = new State()
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
            public State GetWaitingState() { return s_watingState; }

            private State s_searchingState = new State()
            {
                OnEnter = (PixelHumanoid owner) =>
                {
                    owner.m_fsm.SetTransitionToSearch(false);

                    owner.m_animator.SetBool("Idle", false);
                    owner.m_animator.SetBool("Dead", false);
                    owner.m_animator.SetBool("Walking", true);
                },
                OnUpdate = (PixelHumanoid owner) =>
                {
                    float distance;

<<<<<<< Updated upstream
                    Debug.Log("owner: " + owner);
                    Debug.Log("owner: " + owner.bm);

=======
>>>>>>> Stashed changes
                    PixelHumanoid enemy = owner.bm.GetClosestAliveEnemy(owner.transform, owner.teamIndex, out distance);
                    distance = Mathf.Sqrt(distance);

                    if (distance <= owner.searchingRange)
                    {
                        owner.targetId = enemy.entityId;
                        return EState.Chasing;
                    }

                    Vector3 delta = Vector3.zero;
                    if (owner.m_direction == Utility.Direction2.Left)
                    {
                        delta = Vector3.left * Time.deltaTime * owner.stats.walkSpeed;
                    }
                    else if (owner.m_direction == Utility.Direction2.Right)
                    {
                        delta = Vector3.right * Time.deltaTime * owner.stats.walkSpeed;
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
            public  State GetSearchingState() { return s_searchingState; }

            private State s_chasingState = new State()
            {
                OnUpdate = (PixelHumanoid owner) =>
                {
                    float distance;

                    PixelCharacter target = owner.bm.GetEntity(owner.targetId, BattleManager.EDeadOrAlive.Alive);
                    if (target == null) 
                    {
                        target = owner.bm.GetClosestAliveEnemy(owner.transform, owner.teamIndex, out distance);
                        if (target == null) 
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
                        if (distance <= owner.attackRange)
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

                            return EState.None;
                        }
                    }
                }
            };
            public State GetChasingState() { return s_chasingState; }

            private State s_meleeAttackingState = new State()
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
                        owner.m_animator.SetTrigger("Slash");
                        owner.bm.ApplyDefaultAttack(owner, target);

                        owner.leftAttackDelay = owner.stats.attackDelay;
                        return EState.Delaying;
                    }
                }
            };
            public State GetMeleeAttackingState() { return s_meleeAttackingState; }

            public State s_rangedAttackingState = new State()
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
                        switch (owner.defaultAttackType)
                        {
                            case EDefualtAttackType.RangedShot:
                                {
                                    owner.m_animator.SetTrigger("Shot");

                                    // TODO: Create Factory for Projectiles
                                    GameObject arrow = StaticLoader.Instance().GetDefaultArrowPrefab();
                                    GameObject arrowGo = Instantiate(arrow, owner.transform.position + Vector3.up, Quaternion.identity, null);
                                    AttackProjectile attackProjectile = arrowGo.GetComponent<AttackProjectile>();
                                    attackProjectile.Initialize(owner.bm, owner, owner.transform.position, owner.targetId, 0.5f, true, 2.0f, 10.0f, owner.stats.damage, false);

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
            public State GetRangedAttackingState() { return s_rangedAttackingState; }

            private State s_deadState = new State()
            {
                OnEnter = (PixelHumanoid owner) =>
                {
                    owner.m_fsm.SetTransitionToDead(false);

                    owner.m_animator.SetBool("Idle", false);
                    owner.m_animator.SetBool("Walking", false);
                    owner.m_animator.SetBool("Dead", true);
                }
            };
            public State GetDeadState() { return s_deadState; }

            private State s_delayingState = new State()
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
            public State GetDelayingState() {  return s_delayingState; }
        }
    }
}
