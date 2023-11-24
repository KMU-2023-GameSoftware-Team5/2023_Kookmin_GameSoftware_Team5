using data;
using deck;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace deck
{
    /// <summary>
    /// 아이템 세부 정보창
    /// </summary>
    public class EquipItemDetails : MonoBehaviour
    {
        /// <summary>
        /// 아이템 이미지를 보여줄 컴포넌트
        /// </summary>
        public Image itemImage;
        /// <summary>
        /// 아이템 이름 출력할 컴포넌트
        /// </summary>
        public TextMeshProUGUI itemName;
        /// <summary>
        /// 아이템 설명 출력할 컴포넌트
        /// </summary>
        public TextMeshProUGUI itemDescription;
        /// <summary>
        /// 아이템에 의해 변동되는 스텟
        /// </summary>
        public TextMeshProUGUI itemStat;
        /// <summary>
        /// 아이템의 주인
        /// </summary>
        public TextMeshProUGUI itemOwner;
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

            if (itemImage != null)
            {
                itemImage.sprite = this.item.getItemIconImage();
            }
            if (itemName != null)
            {
                itemName.text = this.item.getItemName();
            }
            if (itemStat != null)
            {
                itemStat.text = itemStatToString();
            }
            if (itemOwner != null)
            {
                if (item.ItemOwner != null)
                {
                    itemOwner.text = item.ItemOwner.getName();
                }
                else
                {
                    itemOwner.text = "주인없음";
                }
            }
            if(itemDescription != null)
            {
                itemDescription.text = this.item.getItemDescription();
            }


            //itemImage.color = this.item.itemColor;
        }

        string itemStatToString()
        {
            CommonStats itemStat = item.itemStat;
            string ret = "";
            if(itemStat.hp > 0)
                ret += $"HP {statToString(itemStat.hp)}\n";
            if (itemStat.mp > 0)
                ret += $"MP {statToString(itemStat.mp)}\n";
            if (itemStat.damage > 0)
                ret += $"Damage {statToString(itemStat.damage)}\n";
            if (itemStat.sheild > 0)
                ret += $"sheild  {statToString(itemStat.sheild)}\n";
            if (itemStat.walkSpeed > 0)
                ret += $"Attack Delay {statToString(itemStat.walkSpeed)}\n";
            if (itemStat.energy > 0)
                ret += $"Energy {statToString(itemStat.energy)}\n";
            if (itemStat.criticalRate > 0)
                ret += $"CriticalRate {statToString(itemStat.criticalRate)}\n";

            return ret;
        }

        string statToString(int value)
        {
            if(value < 0)
            {
                return value.ToString();
            }
            else {
                return $"+{value}";
            }
        }
        string statToString(float value)
        {
            if (value < 0)
            {
                return value.ToString();
            }
            else
            {
                return $"+{value}";
            }
        }

    }

}

