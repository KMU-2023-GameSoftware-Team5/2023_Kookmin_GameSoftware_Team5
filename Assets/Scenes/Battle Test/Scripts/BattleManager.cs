using System.Collections.Generic;
using UnityEngine;

namespace lee
{
    public class BattleManager : StaticGetter<BattleManager>
    {
        public float maxZ = 6.0f;
        public float minZ = -8.0f;

        public enum EStatus
        {
            Waiting,
            Fighting,
        }

        public enum EDeadOrAlive
        {
            Dead, 
            Alive, 
            All
        }

        [SerializeField] private EStatus status = EStatus.Waiting;
        public EStatus GetStatus() { return status; }

        public void Initialize()
        {
            status = EStatus.Waiting;
            m_entityMap = new Dictionary<uint, PixelCharacter>();
        }

        private Dictionary<uint, PixelCharacter> m_entityMap;
        public PixelCharacter GetEntity(uint id, EDeadOrAlive doa)
        {
            PixelCharacter ret = m_entityMap[id];

            if (ret == null)
            {
                return ret;
            }
            else if (doa == EDeadOrAlive.Alive)
            {
                if (!ret.IsDead())
                    return ret;
                else
                    return null;
            }
            else if (doa == EDeadOrAlive.Dead)
            {
                if (ret.IsDead())
                    return ret;
                else
                    return null;
            }


            return ret;
        }

        private List<PixelHumanoid> m_team0Humanoids;
        private List<PixelHumanoid> m_team1Humanoids;
        public bool StartBattle(List<PixelHumanoid> team0Humanoids, List<PixelHumanoid> team1Humanoids)
        {
            if (status == EStatus.Fighting)
            {
                Debug.LogError("already fighting");
                return false;
            }

            m_team0Humanoids = team0Humanoids;
            m_team1Humanoids = team1Humanoids;

            uint lastEntityNumber = 0;  // 1���� ����
            foreach (PixelHumanoid humanoid in m_team0Humanoids)
            {
                humanoid.entityId = lastEntityNumber++;
                m_entityMap[humanoid.entityId] = humanoid;
            }
            foreach (PixelHumanoid humanoid in m_team1Humanoids)
            {
                humanoid.entityId = lastEntityNumber++;
                m_entityMap[humanoid.entityId] = humanoid;
            }

            Debug.Log("battle started");

            foreach(PixelHumanoid humanoid in m_team0Humanoids)
            {
                humanoid.OnBattleStarted(m_team0Humanoids.ToArray(), m_team1Humanoids.ToArray());
            }

            foreach (PixelHumanoid humanoid in m_team1Humanoids)
            {
                humanoid.OnBattleStarted(m_team1Humanoids.ToArray(), m_team0Humanoids.ToArray());
            }


            status = EStatus.Fighting;
            return true;
        }

        public PixelHumanoid GetClosestAliveEnemy(Transform myTransform, int myTeamIndex, out float squaredDistance)
        {
            PixelHumanoid ret = null;
            if (myTeamIndex == 0)
            {
                float distance = float.MaxValue;
                foreach (PixelHumanoid humanoid in m_team1Humanoids)
                {
                    if (humanoid.IsDead())
                        continue;

                    float iDistance = Utility.GetSquaredDistanceBetween(myTransform, humanoid.transform);
                    if (iDistance < distance)
                    {
                        distance = iDistance;
                        ret = humanoid;
                    }
                }

                squaredDistance = distance;
                return ret;
            }
            else if (myTeamIndex == 1)
            {
                float distance = float.MaxValue;
                foreach (PixelHumanoid humanoid in m_team0Humanoids)
                {
                    if (humanoid.IsDead())
                        continue;

                    float iDistance = Utility.GetSquaredDistanceBetween(myTransform, humanoid.transform);
                    if (iDistance < distance)
                    {
                        distance = iDistance;
                        ret = humanoid;
                    }
                }

                squaredDistance = distance;
                return ret;
            }
            else
            {
                Debug.LogError("Error Team");
                squaredDistance = 0.0f;
                return null;
            }
        }

        public void HandleDefaultAttack(PixelCharacter from, PixelCharacter to)
        {
            // �̹� �׾����� �ƹ� ó���� ���� �ʴ´�. 
            if (to.IsDead())
                return;

            // TODO : ���� ��ü�� �и�
            // TODO: ����, ũ��Ƽ�� �� ���
            int damage = from.damage;
            to.hp -= damage;

            from.mp += 10;
            if (from.mp > 100)
            {
                from.mp = PixelCharacter.MaxMp;

                // TODO: notify mp 100 maybe?
            }

            Color damageTextColor = Color.white;
            if (UnityEngine.Random.Range(0.0f, 1.0f) <= from.criticalRate)
            {
                damageTextColor = Color.yellow;
                damage *= 2;
            }

            // creat damage text
            GameObject damageTextPrefap = StaticLoader.Instance().GetFlatingTextPrefap();
            GameObject damageTextGo = Instantiate(damageTextPrefap, Vector3.zero, Quaternion.identity, to.transform);
            damageTextGo.transform.localPosition = new Vector3(0.0f, 2.0f, 0.0f);
            FloatingText floatingText = damageTextGo.GetComponent<FloatingText>();
            floatingText.Initialize(damage.ToString(), damageTextColor);
            
            // callback on damaged
            if (to.teamIndex == 0)
            {
                to.OnDamaged(from, m_team0Humanoids.ToArray(), m_team1Humanoids.ToArray());
            }
            else if (to.teamIndex == 1)
            {
                to.OnDamaged(from, m_team1Humanoids.ToArray(), m_team0Humanoids.ToArray());
            }

            // callback if dead
            // ���� ������ PixelCharacter���� �˾Ƽ� �ϴϱ� �ݹ鸸 ȣ���Ѵ�. 
            if (to.hp <= 0)
            {
                // call victim's callback
                if (from.teamIndex == 0) 
                {
                    to.OnDead(from, m_team0Humanoids.ToArray(), m_team1Humanoids.ToArray());
                }
                else if (from.teamIndex == 1)
                {
                    to.OnDead(from, m_team1Humanoids.ToArray(), m_team0Humanoids.ToArray());
                }

                // call killer's callback
                if (from.teamIndex == 0)
                {
                    to.OnKill(to, m_team0Humanoids.ToArray(), m_team1Humanoids.ToArray());
                }
                else if (from.teamIndex == 1)
                {
                    to.OnKill(to, m_team1Humanoids.ToArray(), m_team0Humanoids.ToArray());
                }
            }
        }

        public void Pause()
        {
            if(status == EStatus.Waiting)
            {
                Debug.LogError("the battle is paused but not fighting");
                return;
            }

            Time.timeScale = 0.0f;
        }

        public void Play()
        {
            if (Time.timeScale != 0.0f)
            {
                Debug.LogError("the battle is paused but not paused");
                return;
            }

            Time.timeScale = 1.0f;
        }

        public void SetPlaySpeedMultiplier(float multiplier)
        {
            Time.timeScale = multiplier;
        }
    }
}
