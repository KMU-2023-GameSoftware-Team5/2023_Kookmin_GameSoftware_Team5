using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

        private List<PixelCharacter> m_team0Characters;
        private List<PixelCharacter> m_team1Characters;
        public bool StartBattle(List<PixelCharacter> team0, List<PixelCharacter> team1)
        {
            if (status == EStatus.Fighting)
            {
                Debug.LogError("already fighting");
                return false;
            }

            m_team0Characters = team0;
            m_team1Characters = team1;

            uint lastEntityNumber = 0;  // 1���� ����
            foreach (PixelCharacter character in m_team0Characters)
            {
                character.entityId = lastEntityNumber++;
                m_entityMap[character.entityId] = character;
            }
            foreach (PixelCharacter character in m_team1Characters)
            {
                character.entityId = lastEntityNumber++;
                m_entityMap[character.entityId] = character;
            }

            Debug.Log("battle started");

            foreach(PixelCharacter character in m_team0Characters)
            {
                character.OnBattleStarted(m_team0Characters.ToArray(), m_team1Characters.ToArray());
            }

            foreach (PixelCharacter character in m_team1Characters)
            {
                character.OnBattleStarted(m_team1Characters.ToArray(), m_team0Characters.ToArray());
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
                foreach (PixelHumanoid humanoid in m_team1Characters)
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
                foreach (PixelHumanoid humanoid in m_team0Characters)
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

        public void GetAliveEnemiesFromClosest(PixelCharacter from, out PixelCharacter[] out_enemies)
        {
            List<PixelCharacter> ret = new List<PixelCharacter>();
            
            List<PixelCharacter> enemies = m_team1Characters;
            if (from.teamIndex == 1)
                enemies = m_team0Characters;

            foreach (PixelCharacter iCharacter in enemies)
            {
                if (iCharacter.IsDead()) continue;

                ret.Add(iCharacter);
            }

            // ��������
            ret.Sort((PixelCharacter x, PixelCharacter y) =>
                {
                    float distX = (from.transform.position - x.transform.position).sqrMagnitude;
                    float distY = (from.transform.position - y.transform.position).sqrMagnitude;

                    return (int)(distX - distY);
                }
            );

            out_enemies = ret.ToArray();
        }

        public void ApplyDefaultAttack(PixelCharacter from, PixelCharacter to)
        {
            ApplyDamage(from, to, from.stats.damage, true);
        }

        public void ApplyDamage(PixelCharacter from, PixelCharacter to, int damage, bool checkCritical)
        {
            // �̹� �׾����� �ƹ� ó���� ���� �ʴ´�. 
            if (to.IsDead())
                return;

            to.stats.hp -= damage;

            from.stats.mp += 10;
            if (from.stats.mp > 100)
            {
                from.stats.mp = PixelCharacter.MaxMp;

                // TODO: notify mp 100 maybe?
            }

            Color damageTextColor = Color.white;
            if (checkCritical)
            {
                if (UnityEngine.Random.Range(0.0f, 1.0f) <= from.stats.criticalRate)
                {
                    damageTextColor = Color.yellow;
                    damage *= 2;
                }
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
                to.OnDamaged(from, m_team0Characters.ToArray(), m_team1Characters.ToArray());
            }
            else if (to.teamIndex == 1)
            {
                to.OnDamaged(from, m_team1Characters.ToArray(), m_team0Characters.ToArray());
            }

            // callback if dead
            // ���� ������ PixelCharacter���� �˾Ƽ� �ϴϱ� �ݹ鸸 ȣ���Ѵ�. 
            if (to.stats.hp <= 0)
            {
                // call victim's callback
                if (from.teamIndex == 0)
                {
                    to.OnDead(from, m_team0Characters.ToArray(), m_team1Characters.ToArray());
                }
                else if (from.teamIndex == 1)
                {
                    to.OnDead(from, m_team1Characters.ToArray(), m_team0Characters.ToArray());
                }

                // call killer's callback
                if (from.teamIndex == 0)
                {
                    to.OnKill(to, m_team0Characters.ToArray(), m_team1Characters.ToArray());
                }
                else if (from.teamIndex == 1)
                {
                    to.OnKill(to, m_team1Characters.ToArray(), m_team0Characters.ToArray());
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
