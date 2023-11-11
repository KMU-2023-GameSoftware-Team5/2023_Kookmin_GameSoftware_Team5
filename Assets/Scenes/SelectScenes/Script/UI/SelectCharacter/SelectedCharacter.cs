using deck;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace deck
{
    public class SelectedCharacter : MonoBehaviour
    {
        [SerializeField] PixelCharacter character;
        [SerializeField] CharacterIcon characterIcon;
        public void Initialize(PixelCharacter character)
        {
            CharacterSelectManager.Instance().initializePlaecmentEvent.AddListener(destroyUnSelect);
            this.character= character;
            characterIcon.Initialize(character);
        }

        public void onClickUnSelect()
        {
            CharacterSelectManager.Instance().unPlaceCharacter(character);
            destroyUnSelect();
        }

        public void destroyUnSelect()
        {
            Destroy(transform.parent.gameObject);
        }

    }

}
