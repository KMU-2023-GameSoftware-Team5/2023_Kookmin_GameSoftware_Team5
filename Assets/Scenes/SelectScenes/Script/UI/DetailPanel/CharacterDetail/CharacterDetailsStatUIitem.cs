using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace deck
{
    /// <summary>
    /// HP 등 일부 캐릭터 스텟을 보여주기 위한 컴포넌트
    /// </summary>
    public class CharacterDetailsStatUIitem : MonoBehaviour
    {
        /// <summary>
        /// 아이템 등의 영향을 받아 계산된 캐릭터의 최종스텟을 보여줌
        /// </summary>
        [SerializeField] TextMeshProUGUI characterStat;
        /// <summary>
        /// 아이템에 의한 변동치를 보여줌
        /// </summary>
        [SerializeField] TextMeshProUGUI itemStat;

        [SerializeField] public Color basicColor;
        [SerializeField] public Color plusColor;
        [SerializeField] public Color minusColor;

        public void setStat(int characterStat, int itemStat)
        {
            int characterStatFortext= characterStat > 0 ? characterStat : 0;
            int itemStatFortext = itemStat > 0 ? itemStat : -itemStat;

            string characterStatString = characterStatFortext.ToString();
            string itemStatString = itemStatFortext.ToString();
            int colorFlag;
            if(itemStat > 0)
            {
                colorFlag = 1;
            }else if(itemStat < 0)
            {
                colorFlag = -1;
            }
            else
            {
                colorFlag = 0;
            }
            setStat(characterStatString, itemStatString, colorFlag);
        }

        public void setStat(float characterStat, float itemStat)
        {
            float characterStatFortext = characterStat > 0 ? characterStat : 0;
            float itemStatFortext = itemStat > 0 ? itemStat : -itemStat;

            string characterStatString = characterStatFortext.ToString();
            string itemStatString = itemStatFortext.ToString();
            int colorFlag;
            if (itemStat > 0)
            {
                colorFlag = 1;
            }
            else if (itemStat < 0)
            {
                colorFlag = -1;
            }
            else
            {
                colorFlag = 0;
            }

            setStat(characterStatString, itemStatString, colorFlag);
        }

        /// <summary>
        /// 텍스트 UI에 스텟 수치 출력하는 메서드
        /// </summary>
        /// <param name="characterStat">캐릭터의 최종 스텟</param>
        /// <param name="itemStat">아이템에 의해 증감되는 스텟</param>
        /// <param name="colorFlag">텍스트 색깔 설정을 위한 속성 : 아이템에 의해 스텟이 증가되는가? 증가, 변동없음, 감소 순으로 1, 0, -1</param>
        void setStat(string characterStat, string itemStat, int colorFlag)
        {
            string plusMinusText = "";
            //this.characterStat.overrideColorTags = true;
            //this.itemStat.overrideColorTags = true;
            
            if(colorFlag == 1)
            {
                plusMinusText = "+";
                this.characterStat.color = plusColor;
                this.itemStat.color = plusColor;
            }
            else if(colorFlag == -1)
            {
                plusMinusText = "-";
                this.characterStat.color = minusColor;
                this.itemStat.color = minusColor;
            }
            else
            {
                this.characterStat.color = basicColor;
                this.itemStat.color = basicColor;
            }
            this.characterStat.text = characterStat;
            this.itemStat.text = $"({plusMinusText}{itemStat})";

        }
    }

}
