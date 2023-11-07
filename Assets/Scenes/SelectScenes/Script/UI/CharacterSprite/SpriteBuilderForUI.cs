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
        [SerializeField] RectTransform rect;

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
        /// rect transform의 크기를 기반으로 sprite resize
        /// </summary>
        public void resizeSprite()
        {
            Vector3[] v = new Vector3[4];
            if (rect == null)
            {
                return;
            }
            rect.GetWorldCorners(v);

            float l1 = Vector3.Distance(v[1], v[2]);
            float l2 = Vector3.Distance(v[0], v[1]);
            float l = (l1 < l2 ? l1 : l2) *250; // 158은 그냥 상수. 오차범위가 좀 넓음

            spriteRenderer.size = new Vector2(l, l);
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
