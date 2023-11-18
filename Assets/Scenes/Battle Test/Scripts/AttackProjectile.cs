using UnityEngine;
using UnityEngine.UIElements;

namespace battle
{
    // TODO: line Renderer로 궤적 이쁘게 그리기
    public class AttackProjectile : MonoBehaviour
    {
        public BattleManager bm;
        public PixelHumanoid parent;
        public Vector3 birthPosition;
        public uint targetId;
        public float radius;
        public bool rotateDirection;
        public float leftLifeTime;
        public float speed;
        public int damage;
        public bool isAlive;

        [Header("Reference")]
        public SpriteRenderer sr;

        public void InitializeAsDefaultAttack(
            BattleManager bm,
            PixelHumanoid parent,
            uint targetId,
            float radius,
            bool rotateDirection,
            float lifeTime,
            float speed
        )
        {
            isAlive = true;

            this.parent = parent;
            this.bm = bm;
            this.targetId = targetId;
            this.radius = radius;
            this.rotateDirection = rotateDirection;
            leftLifeTime = lifeTime;
            this.speed = speed;
            damage = -1;

            PixelCharacter target = bm.GetEntity(targetId, BattleManager.EDeadOrAlive.Alive);
            if (target == null)
            {
                transform.position += transform.right * speed * Time.deltaTime;
                return;
            }

            Vector3 targetPos = target.transform.position + Vector3.up;
            if (rotateDirection)
            {
                rotate(targetPos);
            }
        }

        public void InitializeAsSkill(
            BattleManager bm, 
            PixelHumanoid parent, 
            uint targetId, 
            float radius, 
            bool rotateDirection, 
            float lifeTime, 
            float speed, 
            int damage
        )
        {
            isAlive = true;

            this.parent = parent;
            this.bm = bm;
            this.targetId = targetId;
            this.radius = radius;
            this.rotateDirection = rotateDirection;
            leftLifeTime = lifeTime;
            this.speed = speed;
            this.damage = damage;

            PixelCharacter target = bm.GetEntity(targetId, BattleManager.EDeadOrAlive.Alive);
            if (target == null)
            {
                transform.position += transform.right * speed * Time.deltaTime;
                return;
            }

            Vector3 targetPos = target.transform.position + Vector3.up;
            if (rotateDirection)
            {
                rotate(targetPos);
            }
        }

        private void Update()
        {
            leftLifeTime  -= Time.deltaTime;
            if (leftLifeTime <= 0)
                Destroy(gameObject);

            if (!isAlive)
                return;

            PixelCharacter target = bm.GetEntity(targetId, BattleManager.EDeadOrAlive.Alive);
            if (target == null)
            {
                transform.position += transform.right * speed * Time.deltaTime;
                return;
            }

            Vector3 targetPos = target.transform.position + Vector3.up;
            if (rotateDirection)
            {
                rotate(targetPos);
            }

            // TODO: clamp delta' size so that not pass by target
            Vector3 delta = targetPos - transform.position;
            transform.position += delta.normalized * speed * Time.deltaTime;

            if (Utility.GetDistanceBetween(transform.position, targetPos) <= radius)
            {
                // TODO: handle HIT
                transform.SetParent(target.transform);

                if (damage == -1f)
                    bm.ApplyDefaultAttack(parent, target);
                else
                    bm.ApplyDamage(parent, target, damage, true);

                Destroy(gameObject);
            }

        } /* end of Update() */

        private void LateUpdate()
        {
            if (Camera.main != null)
            {
            }
        }

        // REF : https://forum.unity.com/threads/set-forward-and-right-of-a-transform.461482/   
        private void rotate(Vector3 target_pos)
        {
            Vector3 forward = Camera.main.transform.forward;
            Vector3 right = target_pos - transform.position;
            Vector3 up = Vector3.Cross(forward, right);
            transform.rotation = Quaternion.LookRotation(forward, up);
        }
    }
}

