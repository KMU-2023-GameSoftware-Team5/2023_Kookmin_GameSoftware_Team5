using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jslee
{
    /// <summary>
    /// 장비 아이템 정보 관리하는 객체 
    /// </summary>
    public class EquipItem
    {
        /*
            이하의 속성들은 추후 개편 예정
        */
        string itemName;
        StatClass itemStat;
        /// <summary>
        /// TODO 아이템 스프라이트로 대체할 것
        /// </summary>
        public Color itemColor;
        /// <summary>
        /// 아이템 주인에 대한 레퍼런스
        /// </summary>
        private TmpCharacter itemOwner;
        /// <summary>
        /// 아이템 주인의 인벤토리 몇번째 칸에 아이템이 저장되어 있는 지 확인하는 변수
        /// </summary>
        private int idx = -1;
        /// <summary>
        /// 장비아이템 객체 생성자. 추후 개선해야함
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="color"></param>
        public EquipItem(string itemName, Color color)
        {
            this.itemName = itemName;
            itemColor = color;
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
            string ret = "";
            ret += $"{getItemName()}\n";
            if (itemOwner != null)
            {
                ret += $"now owner is {itemOwner.getName()}\n";
            }

            return ret;
        }

        /// <summary>
        /// 아이템 착용 메서드
        /// </summary>
        /// <param name="idx"> 아이템이 플레이어의 인벤토리에서 몇번쨰 칸에 착용할 것인지</param>
        /// <param name="owner">아이템 착용 대상자</param>
        public void equip(int idx, TmpCharacter owner)
        {
            if(itemOwner != null) 
            {
                itemOwner.Inventory[idx] = null;
            }
            
            this.idx = idx;
            itemOwner = owner;
        }
        /// <summary>
        /// 아이템 착용해제 코드
        /// </summary>
        public void unEquip()
        {
            this.idx = -1;
            itemOwner = null;
        }
        /// <summary>
        /// 착용하고자하는 아이템이 원래의 주인이 있는지
        /// </summary>
        /// <returns>아이템이 이미 착용상태인가 여부(boolean)</returns>
        public bool isEquip()
        {
            return itemOwner != null;
        }
    }
}
