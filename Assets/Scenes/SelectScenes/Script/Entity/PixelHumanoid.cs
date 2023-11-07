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
        /// <summary>
        /// 임시로 모아놓은 캐릭터 닉네임들
        /// </summary>
        static string[] characterNickNames = {
                "blue",
                "magenta",
                "yellow",
                "cyan",
                "red",
                "green",
            };

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

        /// <summary>
        /// 임시로 닉네임 만드는 메서드
        /// </summary>
        /// <returns>캐릭터 닉네임</returns>
        string nickNameMaker()
        {
            System.Random random = new System.Random();

            string ret = characterNickNames[random.Next(0, characterNickNames.Length)];
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
            worldPosition = new Vector3(
                (float)json["position"]["x"],
                (float)json["position"]["y"],
                (float)json["position"]["z"]
            );

            Inventory = new EquipItem[PlayerManager.MAX_INVENTORY_SIZE];

            JArray jInventory = (JArray)json["inventory"];
            int cnt = 0;
            foreach(JObject jItem in jInventory)
            {
                if ((string) jItem["item id"] != "None") // jItem["item id"] = null -> but not null. why?
                {
                    Inventory[cnt] = itemMap[(string)jItem["item id"]];
                }
                cnt++;
            }

        }
    }
}
