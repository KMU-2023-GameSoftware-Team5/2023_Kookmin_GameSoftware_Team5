using Assets.PixelHeroes.Scripts.CharacterScrips;
using data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace deck
{
    public class SpritePoolMember : MonoBehaviour
    {

        [SerializeField] SpriteLibrary spriteLibrary;
        [SerializeField] CharacterBuilder builder;
        [SerializeField] SpriteRenderer spriteRenderer;
        public Sprite buildCharacter(PixelHumanoidData pixelHumanoidData)
        {
            builder.SpriteCollection = MyDeckFactory.Instance().GetCollection();
            builder.SpriteLibrary = spriteLibrary;
            pixelHumanoidData.SetOutToBuilder(builder);
            builder.Rebuild();
            spriteRenderer.size = new Vector2(500,500);
            
            return spriteLibrary.GetSprite("Idle", "0");
        }        
    }
}
