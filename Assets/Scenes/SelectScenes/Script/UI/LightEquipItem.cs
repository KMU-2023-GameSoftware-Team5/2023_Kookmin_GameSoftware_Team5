using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace deck
{
    public class LightEquipItem : MonoBehaviour
    {
        /// <summary>
        /// 필요한 장비아이템
        /// </summary>
        public EquipItem item;
        /// <summary>
        /// 아이템이름 출력하는 텍스트 컴포넌트
        /// </summary>
        [SerializeField]
        TextMeshProUGUI itemName;
        /// <summary>
        /// 아이템 이미지 출력하는 이미지 컴포넌트
        /// </summary>
        [SerializeField]
        Image itemImage;
        /// <summary>
        /// 아이템 착용여부를 보여주는 이미지 컴포넌트
        /// </summary>
        [SerializeField]
        GameObject itemEquipMark;

        void Update()
        {
            // 아이템이 장착되었다면 장착마크를 표기함
            if (item.isEquip())
            {
                setEquipMark(true);
            }
            else
            {
                setEquipMark(false);
            }
        }

        public void setEquipMark(bool isEquip)
        {
            itemEquipMark.SetActive(isEquip);
        }


        public void Initialize(EquipItem item)
        {
            this.item = item;
            itemName.text = this.item.getItemName();
            itemImage.sprite = this.item.getItemIconImage();
        }
    }
}
