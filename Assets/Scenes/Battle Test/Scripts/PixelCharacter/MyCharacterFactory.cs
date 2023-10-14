using UnityEngine;
using System.Collections.Generic;
using data;

namespace battle
{
    public class MyCharacterFactory : StaticGetter<MyCharacterFactory>, IOnStaticFound
    {
        // find PixelCharacterData by characterName
        private static Dictionary<string, PixelHumanoidData> s_humanoidDataMap;

        public bool OnStaticFound()
        {
            s_humanoidDataMap = new Dictionary<string, PixelHumanoidData>();
            foreach (var humanoidData in StaticLoader.Instance().GetPixelHumanoidDatas())
            {
                if (humanoidData != null)
                    s_humanoidDataMap[humanoidData.characterName] = humanoidData;
            }

            return true;
        }

        public PixelHumanoid CreatePixelHumanoid(string name, Vector3 worldPosition, Transform parent)
        {
            if (!s_humanoidDataMap.ContainsKey(name))
            {
                Debug.LogError("There is no Pixel Humanoid Data. Register it in Static Loader: " + name);
                return null;
            }

            GameObject characterPrefap = StaticLoader.Instance().GetPixelCharacterPrefap();
            GameObject characterGo = GameObject.Instantiate(characterPrefap, Vector3.zero, Quaternion.identity, parent);

            characterGo.transform.position = worldPosition;
            
            PixelHumanoid ret = characterGo.GetComponent<PixelHumanoid>();

            // build
            ret.builder.SpriteCollection = StaticLoader.Instance().GetCollection();
            ret.builder.SpriteLibrary = ret.spriteLibrary;
            PixelHumanoidData data = s_humanoidDataMap[name];
            data.SetOutToBuilder(ret.builder);
            ret.builder.Rebuild();
            ret.Initilize(data);

            // head bar
            GameObject characterHeadBarPrefap = StaticLoader.Instance().GetPixelCharacterHeadBarPrefap();
            GameObject characterHeadBarGo = GameObject.Instantiate(characterHeadBarPrefap, Vector3.zero, Quaternion.identity, WorldCanvas.Instance().transform);

            PixelCharacterHeadBar bar = characterHeadBarGo.GetComponent<PixelCharacterHeadBar>();
            bar.Initialize(ret);
            ret.headBar = bar;

            return ret;
        }

        public Dictionary<string, PixelHumanoidData> getPixelHumanoidDataMap()
        {
            return s_humanoidDataMap;
        }
    }
}
