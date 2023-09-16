using Assets.PixelHeroes.Scripts.CharacterScrips;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace lee
{
    public class PixelHumanoid : MonoBehaviour
    {
        [Header("Status")]
        // DON'T SET THESE VALUES DIRECTLY: it is maganged by BattleManager, not byt self
        public int sheild;
        public int hp;
        public int energy;
        public uint targetId;
        public float walkSpeed;
        public int damage;
        public float attackDelay;
        public float leftAttackDelay;

        [Header("Setting")]
        public int teamIndex;
        // All the entities that participating in battle should be assigned an ID by battle manager
        // ZERO means NONE
        public uint entityId;
        public float searchingRange = 5.0f;
        public float attackRange;
        public EDefualtAttackType defaultAttackType;

        [Header("Reference")]
        public SpriteLibrary spriteLibrary;
        public CharacterBuilder builder;
        public Animator animator;
        public BattleManager bm;

        private Utility.Direction2 m_direction;
        public void SetDirectionLeft(Utility.Direction2 direction)
        {
            m_direction = direction;
            if (m_direction == Utility.Direction2.Left)
            {
                spriteLibrary.transform.localScale = new Vector3(
                        -Mathf.Abs(spriteLibrary.transform.localScale.x),
                        spriteLibrary.transform.localScale.y,
                        spriteLibrary.transform.localScale.z
                );
            }
            else
            {
                spriteLibrary.transform.localScale = new Vector3(
                        Mathf.Abs(spriteLibrary.transform.localScale.x),
                        spriteLibrary.transform.localScale.y,
                        spriteLibrary.transform.localScale.z
                );
            }
        }

        public enum EStatus
        {
            Waiting,
            Searching, // 바라보고 있는 방향으로 이동하면서 범위 안의 적을 찾는다. 
            Chasing, // 공격 타깃에 공격 범위 밖에 있어서 따라간다. 
            MeleeAttacking,
            RangedAttacking,
            Delaying,
            Dead,
        }
        public EStatus status = EStatus.Waiting;

        public void Initilize(PixelHumanoidData data)
        {
            walkSpeed = data.walkSpeed;
            hp = data.hp;
            attackRange = data.attackRange;
            attackDelay = data.attackDelay;
            defaultAttackType = data.defualtAttackType;
            damage = data.damage;
        }

        // TODO: 제대로 된 FSM class 만들기
        private void Update()
        {
            if (Camera.main != null)
            {
                transform.rotation = Camera.main.transform.localRotation;
            }

            if (status == EStatus.Waiting)
            {
                animator.SetBool("Idle", true);
                animator.SetBool("Walking", false);
                return;
            }
            else if (status == EStatus.Searching)
            {
                float distance;
                PixelHumanoid enemy = bm.GetClosestAliveEnemy(transform, teamIndex, out distance);
                distance = Mathf.Sqrt(distance);

                // 범위 안의 적을 찾은 경우
                if (distance <= searchingRange)
                {
                    status = EStatus.Chasing;
                    targetId = enemy.entityId;
                    return;
                }


                Vector3 delta = Vector3.zero;
                if (m_direction == Utility.Direction2.Left)
                {
                    delta = Vector3.left * Time.deltaTime * walkSpeed;
                }
                else if (m_direction == Utility.Direction2.Right)
                {
                    delta = Vector3.right * Time.deltaTime * walkSpeed;
                }
                transform.position += delta;

                if (delta.x > 0.0f)
                    SetDirectionLeft(Utility.Direction2.Right);
                else
                    SetDirectionLeft(Utility.Direction2.Left);

                animator.SetBool("Idle", false);
                animator.SetBool("Walking", true);
            }
            else if (status == EStatus.Chasing)
            {
                float distance;

                PixelHumanoid target = bm.GetEntity(targetId, BattleManager.EDeadOrAlive.Alive);
                if (target == null) // 새로운 타겟을 찾는다. 
                {
                    target = bm.GetClosestAliveEnemy(transform, teamIndex, out distance);
                    if (target == null) // 새로운 타겟 찾기 실패
                    {
                        status = EStatus.Waiting;
                    }
                    else
                    {
                        targetId = target.entityId;
                    }
                }
                else
                {
                    distance = Utility.GetDistanceBetween(transform, target.transform);
                    if (distance <= attackRange) // 공격 가능 범위에 들어온 경우
                    {
                        if (defaultAttackType == EDefualtAttackType.Melee)
                        {
                            status = EStatus.MeleeAttacking;
                        }
                        else
                        {
                            status = EStatus.RangedAttacking;
                        }

                    }
                    else
                    {
                        Vector3 delta = target.transform.position - transform.position;
                        delta.Normalize();
                        delta = delta * walkSpeed * Time.deltaTime;
                        transform.position += delta;

                        if (delta.x > 0.0f)
                            SetDirectionLeft(Utility.Direction2.Right);
                        else
                            SetDirectionLeft(Utility.Direction2.Left);

                        animator.SetBool("Idle", false);
                        animator.SetBool("Walking", true);
                    }
                }
            }
            else if (status == EStatus.MeleeAttacking)
            {
                animator.SetBool("Idle", false);
                animator.SetBool("Walking", false);

                PixelHumanoid target = bm.GetEntity(targetId, BattleManager.EDeadOrAlive.Alive);
                if (target == null)
                {
                    status = EStatus.Chasing;
                }
                else
                {
                    animator.SetTrigger("Slash");
                    bm.HandleDefaultAttack(this, target);

                    leftAttackDelay = attackDelay;
                    status = EStatus.Delaying;
                }
            }
            else if (status == EStatus.RangedAttacking)
            {
                animator.SetBool("Idle", false);
                animator.SetBool("Walking", false);

                PixelHumanoid target = bm.GetEntity(targetId, BattleManager.EDeadOrAlive.Alive);
                if (target == null)
                {
                    status = EStatus.Chasing;
                }
                else
                {
                    switch (defaultAttackType)
                    {
                        case EDefualtAttackType.RangedShot:
                            {
                                animator.SetTrigger("Shot");

                                // TODO: Create Factory for Projectiles
                                GameObject arrow = StaticLoader.Instance().GetDefaultArrowPrefab();
                                GameObject arrowGo =  Instantiate(arrow, transform.position + Vector3.up, Quaternion.identity, null);
                                AttackProjectile attackProjectile =  arrowGo.GetComponent<AttackProjectile>();
                                attackProjectile.Initialize(bm, this, transform.position, targetId, 0.5f, true, 2.0f, 10.0f, damage, true);

                                break;
                            }
                        case EDefualtAttackType.RangedFire1:
                            {
                                animator.SetTrigger("Fire1H");
                                break;
                            }
                        case EDefualtAttackType.RangedFire2:
                            {
                                animator.SetTrigger("Fire2H");
                                break;
                            }
                    }

                    leftAttackDelay = attackDelay;
                    status = EStatus.Delaying;
                }
            }
            else if (status == EStatus.Delaying)
            {
                animator.SetBool("Idle", false);
                animator.SetBool("Walking", false);

                leftAttackDelay -= Time.deltaTime;

                if (leftAttackDelay <= 0)
                {
                    status = EStatus.Chasing;
                }
            }
            else if(status == EStatus.Dead)
            {

            }
        }

        /// <summary>
        /// callback on battle started
        /// don't call this by your self: it is called by battle manager
        /// </summary>
        /// <param name="allies">includeing self and dead</param>
        /// <param name="enemies">includeing all enemies even already dead</param>
        public virtual void OnBattleStarted(PixelHumanoid[] allies, PixelHumanoid[] enemies)
        {
            targetId = 0;
            status = EStatus.Searching;
        }

        /// <summary>
        /// callback on this character dead
        /// /// don't call this by your self: it is called by battle manager
        /// </summary>
        /// <param name="allies">includeing self and dead</param>
        /// <param name="enemies">includeing all enemies even already dead</param>
        public virtual void OnDead(PixelHumanoid killer, PixelHumanoid[] allies, PixelHumanoid[] enemies)
        {
            status = EStatus.Dead;

            animator.SetBool("Idle", false);
            animator.SetBool("Walking", false);
            animator.SetBool("Dead", true);
        }

        /// <summary>
        /// callback on kill
        /// /// don't call this by your self: it is called by battle manager
        /// </summary>
        /// <param name="allies">includeing self and dead</param>
        /// <param name="enemies">includeing all enemies even already dead</param>
        public virtual void OnKill(PixelHumanoid killed, PixelHumanoid[] allies, PixelHumanoid[] enemies)
        {

        }
    }
}
