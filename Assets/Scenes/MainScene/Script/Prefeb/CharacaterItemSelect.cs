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
         * ĳ���� ����â�� ������ ���ù�ư
            * TODO
                * ������ ������ ����
            * �Ӽ�
                * item
                * itemName
            * �޼��� 
                * onClick : ������ ���� �̺�Ʈ ����
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
            // TODO : ������ �̹��� ����
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
