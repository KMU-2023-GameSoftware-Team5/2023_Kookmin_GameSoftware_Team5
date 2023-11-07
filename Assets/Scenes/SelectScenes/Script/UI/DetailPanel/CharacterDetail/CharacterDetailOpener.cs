using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace deck
{
    public class CharacterDetailOpener : MonoBehaviour
    {
        public PixelCharacter character;

        public void openCharacterDetail()
        {
            MyDeckFactory.Instance().detailCanvasManager.openCharacterDetail(character);
        }
    }
}
