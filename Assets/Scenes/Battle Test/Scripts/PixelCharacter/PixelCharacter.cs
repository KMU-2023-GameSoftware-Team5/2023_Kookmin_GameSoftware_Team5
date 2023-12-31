using data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace battle
{
    public abstract class PixelCharacter : MonoBehaviour
    {
        public static int MaxMp = 100;

        [Header("Status: PixelCharacter")]
        public CommonStats stats;
        public int maxHp;
        public uint targetId;
        public int upgradeLevel = 1;
        public ETraits traits;

        [Header("Setting: PixelCharacter")]
        public int teamIndex;
        // All the entities that participating in battle should be assigned an ID by battle manager
        // ZERO means NONE
        public uint entityId;
        public BattleManager bm;

        [Header("Reference: PixelCharacter")]
        public PixelCharacterHeadBar headBar;

        public virtual bool IsDead() { return true; }

        public virtual int GetUpgradelLevel()
        {
            return upgradeLevel;
        }

        [SerializeField] protected Utility.Direction2 m_direction;
        public virtual void SetDirection(Utility.Direction2 direction) { }

        /// <summary>
        /// callback on battle started
        /// don't call this by your self: it is called by battle manager
        /// </summary>
        /// <param name="allies">includeing self and dead</param>
        /// <param name="enemies">includeing all enemies even already dead</param>
        public virtual void OnBattleStarted(PixelCharacter[] allies, PixelCharacter[] enemies) {}

        /// <summary>
        /// callback on this character dead
        /// don't call this by your self: it is called by battle manager
        /// </summary>
        /// <param name="allies">includeing self and dead</param>
        /// <param name="enemies">includeing all enemies even already dead</param>
        public virtual void OnDead(PixelCharacter killer, PixelCharacter[] allies, PixelCharacter[] enemies) 
        {
            if (headBar != null)
                headBar.gameObject.SetActive(false);
        }

        /// <summary>
        /// callback on kill
        /// /// don't call this by your self: it is called by battle manager
        /// </summary>
        /// <param name="allies">includeing self and dead</param>
        /// <param name="enemies">includeing all enemies even already dead</param>
        public virtual void OnKill(PixelCharacter killed, PixelCharacter[] allies, PixelCharacter[] enemies)
        {

        }

        /// <summary>
        /// callback on damaged
        /// /// don't call this by your self: it is called by battle manager
        /// </summary>
        /// <param name="allies">includeing self and dead</param>
        /// <param name="enemies">includeing all enemies even already dead</param>
        public virtual void OnDamaged(PixelCharacter byWho, PixelCharacter[] allies, PixelCharacter[] enemies)
        {

        }

        /// <summary>
        /// callback on healed
        /// /// don't call this by your self: it is called by battle manager
        /// </summary>
        /// <param name="allies">includeing self and dead</param>
        /// <param name="enemies">includeing all enemies even already dead</param>
        public virtual void OnHealed(PixelCharacter byWho, PixelCharacter[] allies, PixelCharacter[] enemies)
        {

        }
    }
}
