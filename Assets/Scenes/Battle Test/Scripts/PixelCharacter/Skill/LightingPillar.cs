using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace battle
{
    public class LightingPillar: MonoBehaviour
    {
        public BattleManager bm;
        public PixelHumanoid parent;
        public Vector3 birthPosition;
        public uint targetId;
        public int damage;

        public void Initialize(BattleManager bm, PixelHumanoid parent, uint targetId, int dmamage)
        {
            this.bm = bm;
            this.parent = parent;
            this.targetId = targetId;
            this.damage = dmamage;

            PixelCharacter target = bm.GetEntity(targetId, BattleManager.EDeadOrAlive.Alive);

            if (target == null)
            {
                Destroy(gameObject);
            }
            else
            {
                bm.ApplyDamage(parent, target, damage, true, Color.red);
                transform.position = target.transform.position;
            }
        }

        [SerializeField]
        private float m_leftTime;
        private void Update()
        {
            m_leftTime -= Time.deltaTime;
            if (m_leftTime <= 0)
                Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            if (Camera.main != null)
            {
                transform.rotation = Camera.main.transform.localRotation;
            }
        }
    }
}