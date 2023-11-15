using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace deck
{
    public class CharacterDetailOpener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public PixelCharacter character;
        public bool longPush=false;
        float needDownTime =1f;
        float downTime;
        bool isDown=false;

        public void OnPointerDown(PointerEventData eventData)
        {
            if(longPush)
                isDown = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (longPush)
                resetPoint();
        }

        void resetPoint()
        {
            downTime = 0f;
            isDown = false;
        }

        void Update()
        {
            if (longPush)
            {
                if (isDown)
                {
                    downTime += Time.deltaTime;
                    if (downTime > 1f)
                    {
                        openCharacterDetail();
                    }
                }
            }
        }

        public void openCharacterDetail()
        {
            MyDeckFactory.Instance().detailCanvasManager.openCharacterDetail(character);
        }
    }
}
