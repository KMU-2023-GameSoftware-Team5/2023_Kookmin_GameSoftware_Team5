using Assets.PixelHeroes.Scripts.CharacterScrips;
using data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace deck
{
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

        public void resizeSprite()
        {
            spriteRenderer.size = new Vector2(width, height);
        }

        public void setDragMode(bool dragMode)
        {
            if (dragMode)
            {
                spriteRenderer.sortingOrder = 3;
            }
            else
            {
                spriteRenderer.sortingOrder = 1;
            }
        }
    }
}
