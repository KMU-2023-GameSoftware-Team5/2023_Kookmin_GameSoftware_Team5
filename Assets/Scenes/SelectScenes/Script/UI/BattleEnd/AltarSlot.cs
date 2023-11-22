using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace deck
{
    public class AltarSlot : MonoBehaviour
    {
        public GameObject noScarificeMark;
        public CharacterIcon characterIcon;
        public PixelCharacter character;

        public void select(PixelCharacter character)
        {
            this.character = character;
            characterIcon.Initialize(character);
            noScarificeMark.SetActive(false);
        }

        public void unSelect()
        {
            this.character = null;
            noScarificeMark.SetActive(true);
        }
    }

}
