using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace placement
{
    /// <summary>
    /// 캐릭터 배치시 캐릭터 위에 위치할 이름
    /// </summary>
    public class PlacementCharacterHeadName : MonoBehaviour
    {
        public float deltaY;
        public RectTransform rect;
        public PlacementCharacter target;
        public TextMeshProUGUI characterName;

        public void Initialize(PlacementCharacter target,string characterNickname)
        {
            if (target == null)
                return;

            this.target = target;
            characterName.text  = characterNickname;
            updatePositionAndRotationByTarget();
        }

        internal void unSelect()
        {
            Destroy(gameObject);
        }

        private void LateUpdate()
        {
            if (target == null)
                return;
            updatePositionAndRotationByTarget();
        }

        private void updatePositionAndRotationByTarget()
        {
            // Vector3 tmpVector = target.transform.position + transform.up * deltaY;
            rect.anchoredPosition3D = target.transform.position + transform.up * deltaY;

            if (Camera.main != null)
                rect.rotation = Camera.main.transform.rotation;
        }
    }

}
