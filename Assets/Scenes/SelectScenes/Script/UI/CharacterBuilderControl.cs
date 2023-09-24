using Assets.PixelHeroes.Scripts.CharacterScrips;
using lee;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace jslee
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

            builder.SpriteCollection = StaticLoader.Instance().GetCollection();
            builder.SpriteLibrary = spriteLibrary;
            MyCharacterFactory.Instance().getPixelHumanoidData(characterName).SetOutToBuilder(builder);
            builder.Rebuild();
        }
    }
}
