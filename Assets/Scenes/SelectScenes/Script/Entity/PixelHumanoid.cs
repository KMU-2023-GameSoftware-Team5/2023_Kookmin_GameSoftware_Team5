using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using data;
using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace deck
{
    /// <summary>
    /// PixelCharacter의 구현체. PixelHumanoid와 호환
    /// </summary>
    public class PixelHumanoid : PixelCharacter
    {
        public PixelHumanoid() { }

        /// <summary>
        /// 픽셀 캐릭터 생성자
        /// </summary>
        /// <param name="characterName">픽셀 캐릭터의 이름(유니크한)</param>
        public PixelHumanoid(string characterName, CommonStats characterStat)
        {
            ID = Guid.NewGuid().ToString();
            this.characterName = characterName;
            characterNickName = nickNameMaker();

            // 캐릭터 초기 위치 설정
            worldPosition = characterInitPosition();

            // 캐릭터 설정
            this.characterStat = characterStat;
            
            // 아이템 인벤토리
            Inventory = new EquipItem[PlayerManager.MAX_INVENTORY_SIZE];
        }

        public PixelHumanoid(string characterName, PixelHumanoidData characterData)
        {
            ID = Guid.NewGuid().ToString();
            this.characterName = characterName;
            characterNickName = nickNameMaker();

            // 캐릭터 초기 위치 설정
            worldPosition = characterInitPosition();

            // 캐릭터 설정
            characterStat = new CommonStats();
            characterStat.CopyFrom(characterData);
            defualtAttackType = characterData.defualtAttackType;
            skill = characterData.customSkillName;

            // 아이템 인벤토리
            Inventory = new EquipItem[PlayerManager.MAX_INVENTORY_SIZE];

        }

        /// <summary>
        /// 임시로 닉네임 만드는 메서드
        /// </summary>
        /// <returns>캐릭터 닉네임</returns>
        string nickNameMaker()
        {
            System.Random random = new System.Random();

            string ret = $"용병 {random.Next(0, 999)}호";
            return ret;
        }

        /// <summary>
        /// TODO : 캐릭터 초기위치 설정
        /// </summary>
        /// <returns>캐릭터의 초기 worldspace 좌표</returns>
        Vector3 characterInitPosition()
        {
            Vector3 ret = new Vector3(UnityEngine.Random.Range(-6, -0), UnityEngine.Random.Range(3, -4), 0); 
            return ret;
        }

        public override JObject toJson()
        {
            JObject ret = new JObject();
            ret["id"] = ID;
            ret["name"] = characterName;
            ret["nickname"] = characterNickName;
            ret["stat"] = JsonConvert.SerializeObject(characterStat);
            ret["position"] = new JObject { 
                {"x", worldPosition.x },
                {"y", worldPosition.y},
                {"z", worldPosition.z},
            };
            ret["tier"] = tier;
            JArray jInventory = new JArray();
            foreach (EquipItem item in Inventory)
            {
                JObject tmp = new JObject();
                if(item != null)
                {
                    tmp["item id"] = item.id;
                }
                else
                {
                    tmp["item id"] = "None";
                }
                jInventory.Add(tmp);
            }
            ret["inventory"] = jInventory;
            return ret;
        }

        public override void fromJson(JObject json, Dictionary<string, EquipItem> itemMap)
        {
            ID = (string) json["id"];
            characterName = (string)json["name"];
            characterNickName = (string)json["nickname"];
            characterStat = JsonConvert.DeserializeObject<CommonStats>((string)json["stat"]);
            PixelHumanoidData ph = MyDeckFactory.Instance().getPixelHumanoidData(characterName);
            skill = ph.customSkillName; 
            defualtAttackType = ph.defualtAttackType;
            worldPosition = new Vector3(
                (float)json["position"]["x"],
                (float)json["position"]["y"],
                (float)json["position"]["z"]
            );
            tier = (int) json["tier"];
            Inventory = new EquipItem[PlayerManager.MAX_INVENTORY_SIZE];

            JArray jInventory = (JArray)json["inventory"];
            int cnt = 0;
            foreach(JObject jItem in jInventory)
            {
                if ((string) jItem["item id"] != "None") // jItem["item id"] = null -> but not null. why?
                {
                    if(itemMap.ContainsKey((string)jItem["item id"]))
                    {
                        equip(cnt, itemMap[(string)jItem["item id"]]);
                    }
                }
                cnt++;
            }

        }
    }
}
