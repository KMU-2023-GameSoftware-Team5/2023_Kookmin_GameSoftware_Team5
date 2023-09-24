using UnityEngine;
using System.Collections.Generic;

namespace lee
{
    public class MyCharacterFactory : StaticGetter<MyCharacterFactory>
    {
        // find PixelCharacterData by characterName
        private Dictionary<string, PixelHumanoidData> m_humanoidDataMap;

        public bool Initialize(PixelHumanoidData[] pixelHumanoidDatas)
        {
            m_humanoidDataMap = new Dictionary<string, PixelHumanoidData>();
            foreach (var humanoidData in pixelHumanoidDatas)
            {
                if (humanoidData != null)
                    m_humanoidDataMap[humanoidData.characterName] = humanoidData;
            }

            return true;
        }

        public PixelHumanoid CreatePixelHumanoid(string name, Vector3 worldPosition, Transform parent)
        {
            if (!m_humanoidDataMap.ContainsKey(name))
            {
                Debug.LogError("There is no Pixel Humanoid Data. Register it in Static Loader: " + name);
                return null;
            }

            GameObject prefap = StaticLoader.Instance().GetPixelCharacterPrefap();
            GameObject go =Instantiate(prefap, Vector3.zero, Quaternion.identity, parent);

            go.transform.position = worldPosition;
            
            PixelHumanoid ret = go.GetComponent<PixelHumanoid>();

            // build
            ret.builder.SpriteCollection = StaticLoader.Instance().GetCollection();
            ret.builder.SpriteLibrary = ret.spriteLibrary;
            PixelHumanoidData data = m_humanoidDataMap[name];
            data.SetOutToBuilder(ret.builder);
            ret.builder.Rebuild();

            ret.Initilize(data);

            return ret;
        }

        public PixelHumanoidData getPixelHumanoidData(string name)
        {
            return m_humanoidDataMap[name];
        }
    }
}
