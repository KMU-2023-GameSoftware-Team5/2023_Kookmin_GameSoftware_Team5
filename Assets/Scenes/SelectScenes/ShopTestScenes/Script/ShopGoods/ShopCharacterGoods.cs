using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace deck
{
    public class PixelCharacterGoods : MonoBehaviour
    {
        [Header("CharacterInfoUI")]
        [SerializeField] CharacterIcon characterInfo;

        /// <summary>
        /// 캐릭터 가격
        /// </summary>
        [SerializeField] TextMeshProUGUI priceTextUI;

        /// <summary>
        /// 판매완료 마크
        /// </summary>
        [SerializeField] GameObject sellMarkUI;

        /// <summary>
        /// 판매할 캐릭터
        /// </summary>
        public PixelCharacter character {  get; private set; }

        /// <summary>
        /// 판매할 캐릭터 가격 
        /// </summary>
        int price;

        /// <summary>
        /// 상품이 팔렸는지 보여줌
        /// </summary>
        bool isSell = false;

        public Outline[] outlines;

        /// <summary>
        /// 판매할 캐릭터 및 그 가격을 보여주는 객체 초기화 메서드
        /// </summary>
        /// <param name="character">판매할 캐릭터</param>
        /// <param name="price">판매할 캐릭터 가격</param>
        public void initialize(PixelCharacter character, int price)
        {
            this.character = character;
            this.price = price;
            priceTextUI.text = $"${price}";
            characterInfo.Initialize(character);
            isSell = false;
            sellMarkUI.SetActive(false);
            foreach (var outline in outlines)
            {
                outline.effectColor = MyDeckFactory.Instance().tierColors[price - 1];
            }
        }

        /// <summary>
        /// 상품을 클릭하면 구매로직이 실행되는 함수
        /// </summary>
        public void OnClickBuyCharacter()
        {
            if (isSell)
            { // 이미 팔렸으면 작동X
                return;
            }
            bool ret = ShopSceneManager.Instance().buyCharacter(character, price);
            if (ret)
            {
                buyCharacter();
            }
        }

        public void buyCharacter()
        {
            isSell = true;
            sellMarkUI.SetActive(true);
        }
    }
}
