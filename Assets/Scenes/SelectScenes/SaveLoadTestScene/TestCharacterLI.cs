using deck;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace testSL
{
    public class TestCharacterLI : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI nameText;

        public void Initialize(PixelCharacter character)
        {
            nameText.text = character.characterName;
        }

        public void destroy()
        {
            Destroy(gameObject);
        }
    }

}
