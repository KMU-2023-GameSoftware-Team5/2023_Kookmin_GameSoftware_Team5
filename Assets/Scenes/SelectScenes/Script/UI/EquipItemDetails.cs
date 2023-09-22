using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace jslee
{
    /// <summary>
    /// 아이템 세부 정보창
    /// </summary>
    public class EquipItemDetails : MonoBehaviour
    {
        /// <summary>
        /// 아이템 세부 정보창 컴포넌트
        /// </summary>
        [SerializeField]
        GameObject detailUI;
        /// <summary>
        /// 아이템 이미지를 보여줄 컴포넌트
        /// </summary>
        [SerializeField]
        Image itemImage;
        /// <summary>
        /// 아이템 이름 출력할 컴포넌트
        /// </summary>
        [SerializeField]
        TextMeshProUGUI itemName;
        /// <summary>
        /// 아이템 설명 출력할 컴포넌트
        /// </summary>
        [SerializeField]
        TextMeshProUGUI itemDescription;

        /// <summary>
        /// 캐릭터 세부창이 보여줄 아이템 객체
        /// </summary>
        EquipItem item;


        /// <summary>
        /// 아이템 세부창을 여는 메서드
        /// </summary>
        /// <param name="item">아이템 세부 정보창이 보여줘야하는 아이템 객체</param>
        public void openItemDetail(EquipItem item)
        {
            this.item = item;
            itemName.text = this.item.getItemName();
            itemDescription.text = this.item.getItemDescription();
            itemImage.color = this.item.itemColor;
            detailUI.SetActive(true);
        }

    }

}

