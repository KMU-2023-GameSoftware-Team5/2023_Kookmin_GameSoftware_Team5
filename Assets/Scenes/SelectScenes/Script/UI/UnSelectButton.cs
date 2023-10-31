using battle;
using deck;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace deck
{
    public class UnSelectButton : MonoBehaviour
    {
        void Awake()
        {
            CharacterSelectManager.Instance().initializePlaecmentEvent.AddListener(destroyUnSelectButton);
        }

        public void onClickUnSelect()
        {
            PixelCharacter character = transform.parent.gameObject.GetComponent<LightCharacterListItem>().character;
            CharacterSelectManager.Instance().unPlaceCharacter(character);
            Destroy(transform.parent.gameObject);
        }

        public void destroyUnSelectButton()
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
