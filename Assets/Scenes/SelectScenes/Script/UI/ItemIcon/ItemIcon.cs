using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace deck
{
    public class ItemIcon : MonoBehaviour
    {
        public TextMeshProUGUI itemName;
        public TextMeshProUGUI itemStat;
        public TextMeshProUGUI itemOwner;
        public Image itemImage;

        public void initialize(EquipItem item)
        {
            if (itemImage != null)
            {
                itemImage.sprite = item.getItemIconImage();
            }
            if(itemName != null)
            {
                itemName.text = item.getItemName();
            }
            if (itemStat != null)
            {
                //
            }
            if(itemOwner != null)
            {
                if( item.ItemOwner != null)
                {
                    itemOwner.text = item.ItemOwner.getName();
                }
                else
                {
                    itemOwner.text = "주인없음";
                }
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
