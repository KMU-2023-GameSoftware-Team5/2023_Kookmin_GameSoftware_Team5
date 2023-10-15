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
        TextMeshProUGUI name;

        public void Initialize(PixelCharacter character)
        {
            name.text = character.characterName;
        }

        public void destroy()
        {
            Destroy(gameObject);
        }
    }

}
