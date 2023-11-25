using data;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace battle
{
    public class BattleManager : StaticComponentGetter<BattleManager>
    {
        public UnityEvent onBattleEnd;

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

        public class Statistics
        {
            public Statistics(PixelCharacter character)
            {
                this.character = character;
                Clear();
            }

            public void Clear()
            {
                DamageApplied = 0;
                TakenDamage = 0;
                HealApplied = 0;
                TakenHeal = 0;
            }

            public PixelCharacter character;
            public int DamageApplied;
            public int TakenDamage;
            public int HealApplied;
            public int TakenHeal;
        }
        private Dictionary<uint, Statistics> m_statistics;   // key: id

        [SerializeField] private EStatus status = EStatus.Waiting;
        public EStatus GetStatus() { return status; }

        public void Awake()
        {
            status = EStatus.Waiting;
            m_entityMap = new Dictionary<uint, PixelCharacter>();
            m_statistics = new Dictionary<uint, Statistics>(); 
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

        private CommonStats team0TraitStats;
        private CommonStats team1TraitStats;

        public CommonStats GetTraitsStats(PixelHumanoid humanoid)
        {
            if (humanoid.teamIndex == 0)
                return team0TraitStats;
            else
                return team1TraitStats;
        }

        private List<PixelCharacter> m_team0Characters;
        private List<PixelCharacter> m_team1Characters;
        public bool StartBattle(List<PixelCharacter> team0, List<PixelCharacter> team1)
        {
            m_statistics.Clear();

            TraitsCount team0TraitsStats = new TraitsCount(team0);
            TraitsCount team1TraitsStats = new TraitsCount(team1);

            team0TraitStats = team0TraitsStats.GetTraitsStats();
            team1TraitStats = team1TraitsStats.GetTraitsStats();

            if (status == EStatus.Fighting)
            {
                Debug.LogError("already fighting");
                return false;
            }

            m_team0Characters = team0;
            m_team1Characters = team1;

            uint lastEntityNumber = 0;  
            foreach (PixelCharacter character in m_team0Characters)
            {
                character.entityId = lastEntityNumber++;
                m_entityMap[character.entityId] = character;

                m_statistics[character.entityId] = new Statistics(character);

                PixelHumanoid humanoid = (PixelHumanoid)character;
                humanoid.GetSpriteRenderer().sortingLayerName = "Default";
            }
            foreach (PixelCharacter character in m_team1Characters)
            {
                character.entityId = lastEntityNumber++;
                m_entityMap[character.entityId] = character;

                m_statistics[character.entityId] = new Statistics(character);

                PixelHumanoid humanoid = (PixelHumanoid)character;
                humanoid.GetSpriteRenderer().sortingLayerName = "Default";
            }

            Debug.Log("battle started");

            foreach(PixelCharacter character in m_team0Characters)
            {
                PixelHumanoid humanoid = (PixelHumanoid)character;

                // apply upgrade level
                int upgradeLevel = character.GetUpgradelLevel();

                // apply traits synergy
                CommonStats traitsStats = GetTraitsStats(humanoid);
                humanoid.stats += traitsStats;

                // apply item stats
                if (humanoid.deckHumanoid != null)
                {
                    humanoid.stats += humanoid.deckHumanoid.getEquipItemStats();
                }

                humanoid.maxHp = humanoid.stats.hp;

                character.OnBattleStarted(m_team0Characters.ToArray(), m_team1Characters.ToArray());
            }

            foreach (PixelCharacter character in m_team1Characters)
            {
                PixelHumanoid humanoid = (PixelHumanoid)character;

                // apply upgrade level
                int upgradeLevel = character.GetUpgradelLevel();
                humanoid.stats = humanoid.stats.GetUpgradedStats(upgradeLevel);

                // apply traits synergy
                CommonStats traitsStats = GetTraitsStats(humanoid);
                humanoid.stats += traitsStats;

                // apply item stats
                if (humanoid.deckHumanoid != null)
                {
                    humanoid.stats += humanoid.deckHumanoid.getEquipItemStats();
                }

                humanoid.maxHp = humanoid.stats.hp;


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

        public PixelHumanoid GetClosestDead(PixelCharacter from)
        {
            PixelHumanoid ret = null;

            float min_distance = float.MaxValue;
            foreach (PixelHumanoid iCharacter in m_team0Characters)
            {
                if (iCharacter.IsDead())
                {
                    float distance = Utility.GetSquaredDistanceBetween(from.transform, iCharacter.transform);
                    if (distance < min_distance)
                    {
                        min_distance = distance;
                        ret = iCharacter;
                    }
                }
            }

            foreach (PixelHumanoid iCharacter in m_team1Characters)
            {
                if (iCharacter.IsDead())
                {
                    float distance = Utility.GetSquaredDistanceBetween(from.transform, iCharacter.transform);
                    if (distance < min_distance)
                    {
                        min_distance = distance;
                        ret = iCharacter;
                    }
                }
            }

            return ret;
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

            ret.Sort((PixelCharacter x, PixelCharacter y) =>
                {
                    float distX = (from.transform.position - x.transform.position).sqrMagnitude;
                    float distY = (from.transform.position - y.transform.position).sqrMagnitude;

                    return (int)(distX - distY);
                }
            );

            out_enemies = ret.ToArray();
        }

        public void GetAliesFromLowestHp(PixelCharacter from, out PixelCharacter[] out_enemies)
        {
            List<PixelCharacter> ret = new List<PixelCharacter>();
            
            List<PixelCharacter> alies = m_team0Characters;
            if (from.teamIndex == 1)
                alies = m_team1Characters;

            foreach (PixelCharacter iCharacter in alies)
            {
                if (iCharacter.IsDead()) continue;

                ret.Add(iCharacter);
            }

            ret.Sort((PixelCharacter x, PixelCharacter y) =>
            {

                return x.stats.hp - y.stats.hp;
            }
            );

            out_enemies = ret.ToArray();
        }

        public void ApplyDefaultAttack(PixelCharacter from, PixelCharacter to)
        {
            if (from.teamIndex == 0)
            {
                ApplyDamage(from, to, from.stats.damage + team0TraitStats.damage, true);
            }
            else if (from.teamIndex == 1)
            {
                ApplyDamage(from, to, from.stats.damage + team1TraitStats.damage, true);
            }
        }

        public void ApplyHeal(PixelCharacter from, PixelCharacter to, int amount)
        {
            if (to.IsDead())
                return;

            to.stats.hp += amount;
            if (to.stats.hp > to.maxHp)
            {
                amount -= to.stats.hp - to.maxHp;

                to.stats.hp = to.maxHp;
            }
                

            m_statistics[from.targetId].HealApplied += amount;
            m_statistics[to.targetId].TakenHeal += amount;

            UnityEngine.Color damageTextColor = UnityEngine.Color.green;

            // creat damage text
            GameObject damageTextPrefap = StaticLoader.Instance().GetFlatingTextPrefap();
            GameObject damageTextGo = Instantiate(damageTextPrefap, Vector3.zero, Quaternion.identity, to.transform);
            damageTextGo.transform.localPosition = new Vector3(0.0f, 2.0f, 0.0f);
            FloatingText floatingText = damageTextGo.GetComponent<FloatingText>();
            floatingText.Initialize(amount.ToString(), damageTextColor);

            // callback on damaged
            if (to.teamIndex == 0)
            {
                to.OnHealed(from, m_team0Characters.ToArray(), m_team1Characters.ToArray());
            }
            else if (to.teamIndex == 1)
            {
                to.OnHealed(from, m_team1Characters.ToArray(), m_team0Characters.ToArray());
            }
        }

        public void ApplyDamage(PixelCharacter from, PixelCharacter to, int damage, bool checkCritical, UnityEngine.Color color)
        {
            if (to.IsDead())
                return;

            to.stats.hp -= damage;
            if (to.stats.hp < 0)
                damage += to.stats.hp;

            m_statistics[from.targetId].DamageApplied += damage;
            m_statistics[from.targetId].TakenDamage += damage;

            from.stats.mp += 10;
            if (from.stats.mp > 100)
            {
                from.stats.mp = PixelCharacter.MaxMp;

                // TODO: notify mp 100 maybe?
            }

            UnityEngine.Color damageTextColor = color;

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

                checkBattleEndAndHandle(to);
            }
        }

        public void ApplyDamage(PixelCharacter from, PixelCharacter to, int damage, bool checkCritical)
        {
            if (to.IsDead())
                return;

            to.stats.hp -= damage;
            if (to.stats.hp < 0)
                damage += to.stats.hp;

            m_statistics[from.targetId].DamageApplied += damage;
            m_statistics[from.targetId].TakenDamage += damage;

            from.stats.mp += 10;
            if (from.stats.mp > 100)
            {
                from.stats.mp = PixelCharacter.MaxMp;

                // TODO: notify mp 100 maybe?
            }

            UnityEngine.Color damageTextColor = UnityEngine.Color.white;
            if (checkCritical)
            {
                if (UnityEngine.Random.Range(0.0f, 1.0f) <= from.stats.criticalRate)
                {
                    damageTextColor = UnityEngine.Color.yellow;
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

                checkBattleEndAndHandle(to);
            }
        }

        private void checkBattleEndAndHandle(PixelCharacter killed)
        {
            // check if game end
            List<PixelCharacter> killedTeam = m_team0Characters;
            SceneParamter.Instance().isWin = false;
            if (killed.teamIndex == 1)
            {
                killedTeam = m_team1Characters;
                SceneParamter.Instance().isWin = true;
            }

            bool isAllDead = true;
            foreach (var character in killedTeam)
            {
                if (!character.IsDead())
                {
                    isAllDead = false;
                }

            }

            if (isAllDead)
            {
                onBattleEnd.Invoke();
            }
        }

        
        public virtual void Vulture(PixelCharacter from, PixelHumanoid to)
        {
            if (!to.IsDead())
            {
                return;
            }

            PixelHumanoid.FSM fsm = to.GetFsm();

            PixelHumanoid.StateFactory.BeingVulturedState state = (PixelHumanoid.StateFactory.BeingVulturedState)fsm.GetStateSet().Get(PixelHumanoid.EState.BeingVultured);
            state.vulturerId = from.entityId;

            // delete vultured humanoid so that can't be queryed even for DEAD state
            if (to.teamIndex == 0)
                m_team0Characters.Remove(to);
            else if (to.teamIndex == 1)
                m_team1Characters.Remove(to);

            fsm.SetForcedNextState(PixelHumanoid.EState.BeingVultured);
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
