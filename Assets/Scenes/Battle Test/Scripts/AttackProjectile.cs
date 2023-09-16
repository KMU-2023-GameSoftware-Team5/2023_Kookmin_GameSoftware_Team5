using UnityEngine;

namespace lee
{
    // TODO: line Renderer�� ���� �̻ڰ� �׸���
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
        public bool lodge;

        [Header("Reference")]
        public SpriteRenderer sr;

        public void Initialize(BattleManager bm, PixelHumanoid parent, Vector3 birthPosition, uint targetId, float radius, bool rotateDirection, float lifeTime, float speed, int damage, bool lodge)
        {
            isAlive = true;

            this.parent = parent;
            this.bm = bm;
            this.birthPosition = birthPosition;
            this.targetId = targetId;
            this.radius = radius;
            this.rotateDirection = rotateDirection;
            leftLifeTime = lifeTime;
            this.speed = speed;
            this.damage = damage;
            this.lodge = lodge;
        }

        private void Update()
        {
            leftLifeTime  -= Time.deltaTime;
            if (leftLifeTime <= 0)
                Destroy(gameObject);

            if (!isAlive)
                return;

            PixelHumanoid target = bm.GetEntity(targetId, BattleManager.EDeadOrAlive.Alive);
            if (target == null)
            {
                transform.position += transform.forward * speed * Time.deltaTime;
                return;
            }

            if (rotateDirection)
            {
                transform.LookAt(target.transform.position + Vector3.up);
            }

            // TODO: clamp delta' size so that not pass by target
            // �� ������
            transform.position += transform.forward * speed * Time.deltaTime;

            if (Utility.GetDistanceBetween(transform.position, target.transform.position + Vector3.up) <= radius)
            {
                // TODO: handle HIT
                transform.SetParent(target.transform);
                bm.HandleDefaultAttack(parent, target);

                if (lodge)
                {
                    isAlive = false;
                    leftLifeTime = 5.0f;    // Ÿ�ٿ� �浹�ϸ� Ÿ�� ���� n�ʵ��� �پ��ִ´�. 
                    sr.sortingOrder = -1;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}

