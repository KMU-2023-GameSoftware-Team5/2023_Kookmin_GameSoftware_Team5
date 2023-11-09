using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotResizer : MonoBehaviour
{
    [SerializeField] RectTransform parentRect;
    [SerializeField] RectTransform myRect;


    private void Update()
    {
        float parentWidth = parentRect.rect.width;
        float parentHeight = parentRect.rect.height;
        myRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parentWidth);
        myRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, parentHeight);

    }
}
