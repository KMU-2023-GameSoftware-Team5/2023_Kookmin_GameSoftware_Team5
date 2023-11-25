using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

namespace battle
{
    public class BattleTest : StaticComponentGetter<BattleTest>
    {
        [Serializable]
        public class BuildTarget
        {
            public string name;
            public Vector3 position;
        }

        public ESpawnMode spawnMode;

        public BuildTarget[] team0Characters;
        public BuildTarget[] team1Characters;

        public enum ESpawnMode
        {
            Random100, 
            SceneParam,
            MobSet, 
            Inspector, 
        }

        private void Update()
        {
            /*
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // partition test
                int[] ret = Utility.Partition(45, 5, 9);
                Debug.Log("45: " + $"[{string.Join(",", ret)}]");
                ret = Utility.Partition(40, 5, 9);
                Debug.Log("40: " + $"[{string.Join(",", ret)}]");
                ret = Utility.Partition(35, 5, 9);
                Debug.Log("35: " + $"[{string.Join(",", ret)}]");
                ret = Utility.Partition(30, 5, 9);
                Debug.Log("30: " + $"[{string.Join(",", ret)}]");
                ret = Utility.Partition(20, 5, 9);
                Debug.Log("20: " + $"[{string.Join(",", ret)}]");
                ret = Utility.Partition(10, 5, 9);
                Debug.Log("10: " + $"[{string.Join(",", ret)}]");
                ret = Utility.Partition(5, 5, 9);
                Debug.Log("5: " + $"[{string.Join(",", ret)}]");
            }
            */
        }

        private void Start()
        {
            switch (spawnMode)
            {
                case ESpawnMode.Random100:
                    spawnCharactersRandom();
                    break;
                case ESpawnMode.MobSet:
                    spawnFromMobSet(SceneParamter.Instance().MobSet);
                    break;
                case ESpawnMode.SceneParam:
                    {
                        if (SceneParamter.Instance().IsBoss)
                        {

                        }
                        else
                        {
                            spawnEnemyByTotalLevel();
                        }

                        break;
                    }
                case ESpawnMode.Inspector:
                    spawnCharacters();
                    break;
            }
        }

        private void spawnEnemyByTotalLevel()
        {
            int sum  = SceneParamter.Instance().EnemyTotalLevel;
            int[] randomLevels = Utility.Partition(sum, 5, 9);

            if (randomLevels == null)
            {
                Debug.LogError("randomLevels is NULL");
                return;
            }

            for(int i = 0; i < randomLevels.Length; i++)
            {
                int maxExcludeIdex = StaticLoader.Instance().GetPixelHumanoidCount();
                int randomIdx = UnityEngine.Random.Range(0, maxExcludeIdex);

                data.PixelHumanoidData pixelHumanoidData = 
                    StaticLoader.Instance().GetPixelHumanoidData(randomIdx);

                Vector2 pos = new Vector3(4.5f, 0.5f, 0);
                pos += new Vector2(UnityEngine.Random.Range(-2, 3), UnityEngine.Random.Range(-2, 3));

                PixelHumanoid humanoid = MyCharacterFactory.Instance().
                    CreatePixelHumanoid(pixelHumanoidData.characterName, pos, transform);

                humanoid.SetDirection(Utility.Direction2.Left);
                humanoid.teamIndex = 1;
                humanoid.bm = BattleManager.Instance();
                humanoid.upgradeLevel = randomLevels[i];

                m_team1Humanoids.Add(humanoid);
            }
        }

        private void spawnFromMobSet(MobSetData mobset)
        {
            if (mobset == null)
            {
                Debug.LogError("mobset is NULL");
                return;
            }

            for (int i = 0; i < mobset.mobs.Length; i++)
            {
                PixelHumanoid humanoid = MyCharacterFactory.Instance().CreatePixelHumanoid(mobset.mobs[i].CharacterName, mobset.mobs[i].Position, transform);
                humanoid.SetDirection(Utility.Direction2.Left);
                humanoid.teamIndex = 1;
                humanoid.bm = BattleManager.Instance();

                m_team1Humanoids.Add(humanoid);
            }
        }

        List<PixelCharacter> m_team0Humanoids = new List<PixelCharacter>();
        List<PixelCharacter> m_team1Humanoids = new List<PixelCharacter>();
        private void spawnCharacters()
        {
            foreach (BuildTarget target in team0Characters)
            {
                if (target == null)
                    continue;

                MyCharacterFactory factory = MyCharacterFactory.Instance();
                PixelHumanoid humanoid = factory.CreatePixelHumanoid(target.name, target.position, transform);
                humanoid.SetDirection(Utility.Direction2.Right);
                humanoid.teamIndex = 0;
                humanoid.bm = BattleManager.Instance();

                m_team0Humanoids.Add(humanoid);
            }

            foreach (BuildTarget target in team1Characters)
            {
                if (target == null)
                    continue;

                PixelHumanoid humanoid = MyCharacterFactory.Instance().CreatePixelHumanoid(target.name, target.position, transform);
                humanoid.SetDirection(Utility.Direction2.Left);
                humanoid.teamIndex = 1;
                humanoid.bm = BattleManager.Instance();

                m_team1Humanoids.Add(humanoid);
            }
        }

        private void spawnCharactersRandom()
        {
            int humanoidCount = StaticLoader.Instance().GetPixelHumanoidCount();
            int randomHumanoidIndex = 0;
            string name = "";

            for (int i = 0; i < 100; i++)
            {
                Vector3 rand_pos0 = new Vector3(UnityEngine.Random.Range(-8, -2), 0.0f, UnityEngine.Random.Range(-8, 6));
                randomHumanoidIndex = (int)UnityEngine.Random.Range(0, humanoidCount - 0.0001f);
                name = StaticLoader.Instance().GetPixelHumanoidData(randomHumanoidIndex).characterName;
                PixelHumanoid humanoid = MyCharacterFactory.Instance().CreatePixelHumanoid(name, rand_pos0, transform);
                humanoid.SetDirection(Utility.Direction2.Right);
                humanoid.teamIndex = 0;
                humanoid.bm = BattleManager.Instance();
                m_team0Humanoids.Add(humanoid);

                Vector3 rand_pos1 = new Vector3(UnityEngine.Random.Range(2, 8), 0.0f, UnityEngine.Random.Range(-8, 6));
                randomHumanoidIndex = (int)UnityEngine.Random.Range(0, humanoidCount - 0.0001f);
                name = StaticLoader.Instance().GetPixelHumanoidData(randomHumanoidIndex).characterName;
                humanoid = MyCharacterFactory.Instance().CreatePixelHumanoid(name, rand_pos1, transform);
                humanoid.SetDirection(Utility.Direction2.Left);
                humanoid.teamIndex = 1;
                humanoid.bm = BattleManager.Instance();

                m_team1Humanoids.Add(humanoid);
            }
        }

        public void SetTeam0(PixelCharacter[] characters)
        {
            List<PixelCharacter> team0 = new List<PixelCharacter>();

            foreach(var character in characters)
            {
                if (!character)
                    continue;

                team0.Add(character);
                character.bm = BattleManager.Instance();
                character.SetDirection(Utility.Direction2.Right);
            }

            m_team0Humanoids = team0;
        }

        public void SetTeam1(PixelCharacter[] characters)
        {
            List<PixelCharacter> team1 = new List<PixelCharacter>();

            foreach (var character in characters)
            {
                if (!character)
                    continue;

                team1.Add(character);
            }

            m_team1Humanoids = team1;
        }


        public void StartBattle()
        {
            BattleManager.EStatus battleStatus = BattleManager.Instance().GetStatus();
            if (battleStatus == BattleManager.EStatus.Waiting)
            {
                BattleManager.Instance().StartBattle(m_team0Humanoids, m_team1Humanoids);
            }
            else if (battleStatus == BattleManager.EStatus.Fighting)
            {
                Debug.LogError("Battle already started");
            }
        }
    }
}
