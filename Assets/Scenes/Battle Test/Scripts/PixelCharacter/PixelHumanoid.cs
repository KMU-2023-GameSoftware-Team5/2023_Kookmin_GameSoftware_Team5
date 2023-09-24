using Assets.PixelHeroes.Scripts.CharacterScrips;
using System;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UIElements;

namespace lee
{
    public partial class PixelHumanoid : PixelCharacter
    {
        [Header("Status: PixelHumanoid")]
        public EState state; 

        [Header("Setting: PixelCharacter")]
        public float searchingRange = 5.0f;
        public float attackRange;
        public EDefualtAttackType defaultAttackType;
        public bool scaleByZ;

        [Header("Reference: PixelHumanoid")]
        public SpriteLibrary spriteLibrary;
        public CharacterBuilder builder;
        [SerializeField] Animator m_animator;
        [SerializeField] Transform m_bodyScaler;
        [SerializeField] SpriteRenderer m_sr;

        public override void SetDirection(Utility.Direction2 direction)
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

        // TODO: PixelHumanoidData와 PixelCharacterData 구분하기
        public void Initilize(PixelHumanoidData data)
        {
            walkSpeed = data.walkSpeed;
            maxHp = data.hp;
            hp = data.hp;
            mp = 0;
            attackRange = data.attackRange;
            attackDelay = data.attackDelay;
            defaultAttackType = data.defualtAttackType;
            damage = data.damage;
            criticalRate = data.criticalRate;

            m_fsm = new FSM(StateSet.CreateTestStateSet0(), EState.Waiting);
            m_isDead = false;
        }

        private FSM m_fsm;
        // public override bool IsDead() { return m_fsm.GetCurrentState() == EState.Dead; }
        public override bool IsDead() { return m_isDead; }

        private void FixedUpdate()
        {
            if (Time.timeScale != 0.0f)
                m_fsm.Update(this);
        }

        private void LateUpdate()
        {
            if (Camera.main != null)
            {
                transform.rotation = Camera.main.transform.localRotation;
            }

            state = m_fsm.GetCurrentState();

            if (scaleByZ)
            {
                float minZ = BattleManager.Instance().minZ;
                float maxZ = BattleManager.Instance().maxZ;
                float z = transform.position.z;
                float scale = 1.0f - ((z - minZ) / (maxZ - minZ));   // max에 가까울 수록 값이 작고, min에 가까울 수록 값이 크다. 
                scale = 0.7f + scale * 0.3f;    // 합은 1.0f
                m_bodyScaler.localScale = new Vector3(scale, scale, scale);
            }
            else
            {
                m_bodyScaler.localScale = Vector3.one;
            }
        }

        /// <summary>
        /// callback on battle started
        /// don't call this by your self: it is called by battle manager
        /// </summary>
        /// <param name="allies">includeing self and dead</param>
        /// <param name="enemies">includeing all enemies even already dead</param>
        public override void OnBattleStarted(PixelCharacter[] allies, PixelCharacter[] enemies)
        {
            targetId = 0;
            m_fsm.SetTransitionToSearch(true);
        }

        private bool m_isDead;

        /// <summary>
        /// callback on this character dead
        /// don't call this by your self: it is called by battle manager
        /// </summary>
        /// <param name="allies">includeing self and dead</param>
        /// <param name="enemies">includeing all enemies even already dead</param>
        public override void OnDead(PixelCharacter killer, PixelCharacter[] allies, PixelCharacter[] enemies)
        {
            base.OnDead(killer, allies, enemies);   // invalidate headBar
            m_fsm.SetTransitionToDead(true);
            m_isDead = true;

            m_sr.sortingOrder = -1;
            Color newColor = m_sr.color;
            newColor.a = newColor.a * 0.75f;
            m_sr.color = newColor;
        }

        /// <summary>
        /// callback on kill
        /// /// don't call this by your self: it is called by battle manager
        /// </summary>
        /// <param name="allies">includeing self and dead</param>
        /// <param name="enemies">includeing all enemies even already dead</param>
        public override void OnKill(PixelCharacter killed, PixelCharacter[] allies, PixelCharacter[] enemies)
        {

        }

        /// <summary>
        /// callback on kill
        /// /// don't call this by your self: it is called by battle manager
        /// </summary>
        /// <param name="allies">includeing self and dead</param>
        /// <param name="enemies">includeing all enemies even already dead</param>
        public override void OnDamaged(PixelCharacter byWho, PixelCharacter[] allies, PixelCharacter[] enemies)
        {
            m_animator.SetTrigger("Hit");
        }
    }
}
