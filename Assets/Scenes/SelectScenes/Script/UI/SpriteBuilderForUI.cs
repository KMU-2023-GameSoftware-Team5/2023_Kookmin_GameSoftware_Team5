using Assets.PixelHeroes.Scripts.CharacterScrips;
using data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace deck
{
    /// <summary>
    /// UI를 위한 Character SprietBuilder
    /// </summary>
    public class SpriteBuilderForUI : MonoBehaviour
    {
        [SerializeField] SpriteLibrary spriteLibrary;
        [SerializeField] CharacterBuilder builder;
        [SerializeField] SpriteRenderer spriteRenderer;
        
        [Header("size of Sprite")]
        public int width;
        public int height;


        public void buildCharacter(string characterName)
        {
            builder.SpriteCollection = MyDeckFactory.Instance().GetCollection();
            builder.SpriteLibrary = spriteLibrary;
            MyDeckFactory.Instance().getPixelHumanoidData(characterName).SetOutToBuilder(builder);
            builder.Rebuild();

            resizeSprite();
        }

        public void setSortingOrder(int sortingOrder)
        {
            spriteRenderer.sortingOrder = sortingOrder;
        }

        /// <summary>
        /// 정해진 파라미터에 맞게 sprite 크기 재조정
        /// </summary>
        public void resizeSprite()
        {
            spriteRenderer.size = new Vector2(width, height);
        }

        /// <summary>
        /// select scene에서만 사용. UI와 sprite간 겹침문제 해결을 위해서 sortingOrder 조작
        /// </summary>
        /// <param name="dragMode"></param>
        public void setDragMode(bool dragMode)
        {
            int sortingOrder = dragMode ? 3 : 1;
            setSortingOrder(sortingOrder);
        }
    }
}
