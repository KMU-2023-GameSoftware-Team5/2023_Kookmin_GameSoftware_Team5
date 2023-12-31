using System;
using System.Collections;
using System.Collections.Generic;
using data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace deck
{
    /// <summary>
    /// 장비 아이템 정보 관리하는 객체 
    /// </summary>
    public class EquipItem
    {
        public string id { get; private set; }
        string itemName;

        /// <summary>
        /// 아이템 정보 객체(scriptable)
        /// </summary>
        ItemData itemData;

        /// <summary>
        /// Item 스텟
        /// </summary>
        public CommonStats itemStat { get; private set; }

        /// <summary>
        /// 아이템 주인에 대한 레퍼런스
        /// </summary>
        private PixelCharacter itemOwner;
        public PixelCharacter ItemOwner { get; private set; }

        /// <summary>
        /// 아이템 주인의 인벤토리 몇번째 칸에 아이템이 저장되어 있는 지 확인하는 변수
        /// </summary>
        private int idx = -1;

        /// <summary>
        /// 생성자에서만 사용하는 아이템 스텟 복사 메서드
        /// CopyFrom이 작동안되서 생성
        /// </summary>
        void copyStat()
        {
            itemStat = new CommonStats()
            {
                sheild = itemData.sheild,
                hp = itemData.hp,
                mp = itemData.mp,
                energy = itemData.energy,
                walkSpeed = itemData.walkSpeed,
                damage = itemData.damage,
                attackDelay = itemData.attackDelay,
                criticalRate = itemData.criticalRate
            };
        }

        public EquipItem()
        {
        }

        public EquipItem(ItemData itemData)
        {
            id = System.Guid.NewGuid().ToString();
            this.itemData = itemData;
            this.itemName = itemData.itemName;
            copyStat();
        }

        public EquipItem(string itemName) {
            id = System.Guid.NewGuid().ToString();
            this.itemName = itemName;
            this.itemData = MyDeckFactory.Instance().getItemData(itemName);
            copyStat();
        }

        /// <summary>
        /// 아이템 이름 반환 함수
        /// </summary>
        /// <remarks>프리펩에서 사용 중</remarks>
        /// <returns> 아이템 이름 </returns>
        public string getItemName()
        {
            return itemName;
        }

        /// <summary>
        /// 아이템에 대한 세부 설명을 출력하는 메서드
        /// </summary>
        /// <returns>아이템에 대한 설명(플레이어를 위한)</returns>
        public string getItemDescription()
        {
            return itemData.description;
        }

        public Sprite getItemIconImage()
        {
            return itemData.iconImage;
        }

        public ItemData getItemData()
        {
            return itemData;
        }

        public String getOwnerName()
        {
            return itemOwner.getName();
        }

        /// <summary>
        /// 아이템 착용 메서드
        /// </summary>
        /// <param name="idx"> 아이템이 플레이어의 인벤토리에서 몇번쨰 칸에 착용할 것인지</param>
        /// <param name="owner">아이템 착용 대상자</param>
        /// <returns>(boolean) 장비 아이템 착용 성공 여부</returns>
        public bool equip(int idx, PixelCharacter owner)
        {
            if(itemOwner != null) 
            {
                itemOwner.Inventory[idx] = null;
            }            
            this.idx = idx;
            itemOwner = owner;
            return true;
        }
        /// <summary>
        /// 아이템 착용해제 코드
        /// </summary>
        /// <returns>(boolean) 장비 아이템 해제 성공 여부</returns>
        public bool unEquip()
        {
            this.idx = -1;
            itemOwner = null;
            return true;
        }
        /// <summary>
        /// 착용하고자하는 아이템이 원래의 주인이 있는지
        /// </summary>
        /// <returns>아이템이 이미 착용상태인가 여부(boolean)</returns>
        public bool isEquip()
        {
            return itemOwner != null;
        }

        /// <summary>
        /// JSON으로 부터 아이템 객체 생성
        /// </summary>
        /// <param name="json">저장된 equipItem에 대한 json</param>
        /// <returns>아이템에 소유자가 있는 경우 반환(아이템 장착처리 관련)</returns>
        public string fromJson(JObject json)
        {
            id = (string) json["id"];
            itemName = (string)json["name"];
            itemData = MyDeckFactory.Instance().getItemData(itemName);
            itemStat = JsonConvert.DeserializeObject < CommonStats > ((string) json["stat"]);
            if (json["owner"] == null)
            {
                return null;
            }
            else
            {
                return (string)json["owner"];
            }
        }

        /// <summary>
        /// EquipItem to Json
        /// </summary>
        /// <returns>JSON화한 아이템 객체</returns>
        public JObject toJson()
        {
            JObject ret = new JObject();
            ret["id"] = id;
            ret["name"] = itemName;
            if (itemOwner != null) {
                ret["owner"] = itemOwner.ID;
            }
            else
            {
                ret["owner"] = null;    
            }
            ret["stat"] = JsonConvert.SerializeObject(itemStat);
            return ret;
        }
    }
}
