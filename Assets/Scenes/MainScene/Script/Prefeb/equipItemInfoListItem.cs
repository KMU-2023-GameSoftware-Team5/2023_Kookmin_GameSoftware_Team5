using jslee;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace jslee
{
    public class ItemInfoListItem : MonoBehaviour
    {

        // 장비 아이템 리스트 아이템
        // TODO
        // 아이템을 선택할 수 있게 하여 캐릭터에게 장착하도록 하기 

        public EquipItem itemInfo;
        public TextMeshProUGUI itemName;
        public TextMeshProUGUI itemDescription;
        public GameObject itemSelectButton;
        bool isSelect = false;

        // Start is called before the first frame update
        private void Start()
        {
            // TODO - 개선해야함
            EquipItemManager.Instance.openItemSelect.AddListener(onEquipItemButton);
            EquipItemManager.Instance.closeItemSelect.AddListener(offEquipItemButton);
        }

        public void setItemInfo(EquipItem item)
        {
            itemInfo = item;
        }

        // Update is called once per frame
        void Update()
        {
            itemName.text = itemInfo.getItemName();
            itemDescription.text = itemInfo.ToString();
        }

        public void onEquipItemButton()
        {
            Debug.Log("4. openSelectItem Event By item info list Set Active");
            isSelect = true;
        }
        public void offEquipItemButton()
        {
            isSelect = false;
        }

        public void selectItem()
        {
            if (isSelect)
            {
                Debug.Log("5. openSelectItem Event By item info list - select");
                EquipItemManager.Instance.selectEquipItemEvent(itemInfo);
            }
        }

    }
}
