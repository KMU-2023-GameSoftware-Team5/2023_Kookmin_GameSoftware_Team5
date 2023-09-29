using Assets.PixelHeroes.Scripts.CharacterScrips;
using data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace deck
{
    public class CharacterBuilderControl : MonoBehaviour
    {
        [SerializeField] SpriteLibrary spriteLibrary;
        PixelHumanoidData humanoidData;
        [SerializeField] CharacterBuilder builder;
        [SerializeField] UnityEngine.Transform spriteTransform;
        [SerializeField] RectTransform parent;

        public void buildCharacter(string characterName)
        {

            builder.SpriteCollection = MyDeckFactory.Instance().GetCollection();
            builder.SpriteLibrary = spriteLibrary;
            MyDeckFactory.Instance().getPixelHumanoidData(characterName).SetOutToBuilder(builder);
            builder.Rebuild();
        }
    }
}
