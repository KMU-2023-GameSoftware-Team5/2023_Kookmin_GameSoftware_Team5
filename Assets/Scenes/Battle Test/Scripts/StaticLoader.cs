using Assets.PixelHeroes.Scripts.CollectionScripts;
using UnityEngine;
using data;
using System.Collections.Generic;

namespace battle
{
    public class StaticLoader : StaticComponentGetter<StaticLoader>, IOnStaticFound
    {
        [SerializeField] private SpriteCollection collection;
        public SpriteCollection GetCollection() { return collection; }

        [Header("Prefap")]
        [SerializeField] private GameObject pixelHumanoidPrefap;
        public GameObject GetPixelCharacterPrefap() { return pixelHumanoidPrefap; }

        [SerializeField] private GameObject defaultArrowPrefap;
        public GameObject GetDefaultArrowPrefab() { return defaultArrowPrefap; }

        [SerializeField] private GameObject pixelCharacterHeadBarPrefap;
        public GameObject GetPixelCharacterHeadBarPrefap() { return pixelCharacterHeadBarPrefap; }

        [SerializeField] private GameObject floatingTextPrefap;
        public GameObject GetFlatingTextPrefap() { return floatingTextPrefap; }

        [SerializeField] private GameObject lightningPillar;
        public GameObject GetLightningPillar() { return lightningPillar; }

        [SerializeField] private GameObject chomp;
        public GameObject GetChomp() { return chomp; }

        [SerializeField] private GameObject truePunch;
        public GameObject GetTruePunch() { return truePunch; }

        [Header("Reference")]
        [SerializeField] private GameObject fireOrbit;
        public GameObject GetFireOrbit() { return fireOrbit; }

        [Header("Humanoid")]
        [SerializeField] private List<PixelHumanoidData> pixelHumanoidDatas = new List<PixelHumanoidData>();
        private Dictionary<string, PixelHumanoidData> m_pixelHumanoidDataMap = new Dictionary<string, PixelHumanoidData>();

        public PixelHumanoidData GetPixelHumanoidData(string name)
        {
            if (m_pixelHumanoidDataMap.ContainsKey(name))
            {
                return m_pixelHumanoidDataMap[name];
            }
            else
            {
                Debug.LogError("PixelHumanoidData not registered: " + name);
                return null;
            }
        }
        public int GetPixelHumanoidCount() { return pixelHumanoidDatas.Count; }
        public List<PixelHumanoidData> GetPixelHumanoidDatas() { return pixelHumanoidDatas; }
        public PixelHumanoidData GetPixelHumanoidData(int index)
        {
            if (index >= pixelHumanoidDatas.Count)
            {
                Debug.LogError("invalid index: over the length");
                return null;
            }
            else if (index < 0)
            {
                Debug.LogError("invalid index: minus value");
                return null;
            }

            return pixelHumanoidDatas[index];
        }

        [Header("CustomSkills")]
        [SerializeField] private List<CustomSkillData> m_customSkillDatas = new List<CustomSkillData>();
        private Dictionary<string, CustomSkillData> m_customSkillDataMap = new Dictionary<string, CustomSkillData>();
        private Dictionary<string, PixelHumanoid.State> m_customSkillStateMap = new Dictionary<string, PixelHumanoid.State>();
        
        public CustomSkillData GetCustomSkillData(string skillName) 
        {
            if(m_customSkillDataMap.ContainsKey(skillName))
            {
                return m_customSkillDataMap[skillName];
            }
            else
            {
                Debug.LogError("custom skill not registered: " + skillName);
                return null;
            }
        }

        public PixelHumanoid.State GetCustomSkillState(string skillName)
        {
            if (m_customSkillStateMap.ContainsKey(skillName))
            {
                return m_customSkillStateMap[skillName];
            }
            else
            {
                Debug.LogError("custom skill not registered: " + skillName);
                return null;
            }
        }

        public bool OnStaticFound()
        {
            foreach (var data in m_customSkillDatas)
            {
                if (data != null)
                {
                    if (m_customSkillStateMap.ContainsKey(data.skillName))
                    {
                        Debug.LogError("duplicated custom skill name: " + data.skillName);
                        continue;
                    }

                    m_customSkillDataMap[data.skillName] = data;
                    m_customSkillStateMap[data.skillName] = data.CreateSkillState();
                }
            }

            foreach(PixelHumanoidData data in pixelHumanoidDatas)
            {
                if (m_pixelHumanoidDataMap.ContainsKey(data.characterName))
                {
                    Debug.LogError("duplicated PixelHumanoidData name: " + data.characterName);
                    continue;
                }

                m_pixelHumanoidDataMap[data.characterName] = data;
            }

            return true;
        }

        [SerializeField] private TraitsStats m_traitsStats;
        public TraitsStats GetTraitsStats() { return m_traitsStats; }

        [SerializeField] private SoundData m_soundData;
        public SoundData GetSoundData() {  return m_soundData; }
    }
}
