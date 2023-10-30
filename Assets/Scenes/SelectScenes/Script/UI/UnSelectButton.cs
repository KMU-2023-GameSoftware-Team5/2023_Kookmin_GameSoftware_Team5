using battle;
using deck;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace deck
{
    public class UnSelectButton : MonoBehaviour
    {
        public void onClickUnSelect()
        {
            PixelCharacter character = transform.parent.gameObject.GetComponent<LightCharacterListItem>().character;
            CharacterSelectManager.Instance().unPlaceCharacter(character);
            Destroy(transform.parent.gameObject);
        }
    }

}
