using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace deck
{
    public class CharacterDetailOpener : MonoBehaviour
    {
        public PixelCharacter character;
        public bool longPush=false;
        float downTime =0.1f;
        bool isDown=false;

        void Update()
        {
            if (longPush)
            {
                if (Input.GetMouseButtonDown(0)) // 마우스 좌클릭이 눌린 그 순간 -> 모바일도 이 이벤트? 
                {
                    downTime = 0f;
                    isDown = true;
                }
                if(isDown & Input.GetMouseButton(0))
                {
                    downTime += Time.deltaTime;
                    if(downTime > 1f)
                    {
                        Debug.Log("long Click");
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
