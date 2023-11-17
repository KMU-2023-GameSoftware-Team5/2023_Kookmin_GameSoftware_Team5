using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace deck
{
    public class SlotResizer : MonoBehaviour
    {
        [SerializeField] RectTransform parentRect;
        [SerializeField] RectTransform myRect;
        public bool useHeight=true;

        private void Start()
        {
            RectTransform rect = parentRect.transform.parent.gameObject.GetComponent<RectTransform>();
            float size;
            if(useHeight )
            {
                size = rect.rect.height;
                Debug.Log($"{rect.rect.height}");
            }
            else
            {
                size = rect.rect.width;
                Debug.Log($"{rect.rect.width}");
            }
            size *= 0.84f;
            parentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
            parentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
            myRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
            myRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);

        }
    }

}
