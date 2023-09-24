using UnityEngine;
using System.Collections.Generic;

namespace lee
{
    // TODO:궂이 component여야 할까? 
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

            GameObject characterPrefap = StaticLoader.Instance().GetPixelCharacterPrefap();
            GameObject characterGo =Instantiate(characterPrefap, Vector3.zero, Quaternion.identity, parent);

            characterGo.transform.position = worldPosition;
            
            PixelHumanoid ret = characterGo.GetComponent<PixelHumanoid>();

            // build
            ret.builder.SpriteCollection = StaticLoader.Instance().GetCollection();
            ret.builder.SpriteLibrary = ret.spriteLibrary;
            PixelHumanoidData data = m_humanoidDataMap[name];
            data.SetOutToBuilder(ret.builder);
            ret.builder.Rebuild();

            ret.Initilize(data);

            // head bar
            GameObject characterHeadBarPrefap = StaticLoader.Instance().GetPixelCharacterHeadBarPrefap();
            GameObject characterHeadBarGo = Instantiate(characterHeadBarPrefap, Vector3.zero, Quaternion.identity, WorldCanvas.Instance().transform);

            PixelCharacterHeadBar bar = characterHeadBarGo.GetComponent<PixelCharacterHeadBar>();
            bar.Initialize(ret);
            ret.headBar = bar;

            return ret;
        }
    }
}
