using System.Collections.Generic;
using UnityEngine;

namespace lee
{
    public class BattleManager : StaticGetter<BattleManager>
    {
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
            m_entityMap = new Dictionary<uint, PixelHumanoid>();
        }

        private Dictionary<uint, PixelHumanoid> m_entityMap;
        public PixelHumanoid GetEntity(uint id, EDeadOrAlive doa)
        {
            PixelHumanoid ret = m_entityMap[id];

            if (ret == null)
            {
                return ret;
            }
            else if (doa == EDeadOrAlive.Alive)
            {
                if (ret.status != PixelHumanoid.EStatus.Dead)
                    return ret;
                else
                    return null;
            }
            else if (doa == EDeadOrAlive.Dead)
            {
                if (ret.status == PixelHumanoid.EStatus.Dead)
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

            uint lastEntityNumber = 0;  // 1부터 시작
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
                    if (humanoid.status == PixelHumanoid.EStatus.Dead)
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
                    if (humanoid.status == PixelHumanoid.EStatus.Dead)
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

        public void HandleDefaultAttack(PixelHumanoid from, PixelHumanoid to)
        {
            // TODO : 전략 객체로 분리
            // TODO: 방어력, 크리티컬 등 고려
            to.hp -= from.damage;

            // callback if dead
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
                    to.OnDead(to, m_team1Humanoids.ToArray(), m_team0Humanoids.ToArray());
                }
            }
        }
    }
}
