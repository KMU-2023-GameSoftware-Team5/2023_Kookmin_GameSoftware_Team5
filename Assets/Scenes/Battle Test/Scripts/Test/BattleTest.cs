using System;
using System.Collections.Generic;
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

        public bool doRandom;
        public bool loadTeam1FromMobset;
        public BuildTarget[] team0Characters;
        public BuildTarget[] team1Characters;

        private void Start()
        {
            if (loadTeam1FromMobset)
                spawnFromMobSet(SceneParamter.Instance().MobSet);
            else if (doRandom)
                spawnCharactersRandom();
            else
                spawnCharacters();
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

        public void SetTeam0(List<PixelCharacter> characters)
        {
            List<PixelCharacter> team0 = characters;

            foreach (var character in team0)
            {
                if (!character)
                    continue;

                character.bm = BattleManager.Instance();
                character.SetDirection(Utility.Direction2.Right);
            }

            m_team0Humanoids = team0;
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
