using jslee;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace jslee
{
    public class CharacaterItemSelect : MonoBehaviour
    {
        /*
         * 캐릭터 선택창의 아이템 선택버튼
            * TODO
                * 아이템 아이콘 구현
            * 속성
                * item
                * itemName
            * 메서드 
                * onClick : 아이템 착용 이벤트 시작
         */
        public int charSelectId;
        public int itemSelectId;
        public EquipItem item;
        public TextMeshProUGUI itemName;

        public void setSelectId(int charSelectId, int itemSelectId)
        {
            this.charSelectId = charSelectId;
            this.itemSelectId = itemSelectId;
        }

        public void setItemInfo(EquipItem item)
        {
            this.item = item;
        }

        void Update()
        {
            // TODO : 아이템 이미지 조절
            if (item == null)
            {
                itemName.text = "Choose Item";
            }
            else
            {
                itemName.text = item.getItemName();
            }
        }

        public void onClick()
        {
            Debug.Log("1. openSelectItem Event from character Item Button");
            CharacterSelectManager.Instance.openSelectEquipItemEvent(charSelectId, itemSelectId);
        }
    }
}
