using lee;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PixelCharacterHeadBar : MonoBehaviour
{
    [Header("Setting")]
    public float deltaY;

    [Header("Reference")]
    public RectTransform rect;
    public PixelCharacter target;
    public Image hpBar;
    public Image mpBar;

    public void Initialize(PixelCharacter target)
    {
        if (target == null)
            return;

        this.target = target;
        updatePositionAndRotationByTarget();
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        float hpScale = target.stats.hp / (float)target.maxHp;
        float mpScale = target.stats.mp / (float)PixelCharacter.MaxMp;

        hpBar.rectTransform.localScale = new Vector3(hpScale, 1.0f, 1.0f);
        mpBar.rectTransform.localScale = new Vector3(mpScale, 1.0f, 1.0f);

        updatePositionAndRotationByTarget();
    }

    private void updatePositionAndRotationByTarget()
    {
        rect.anchoredPosition3D = target.transform.position + transform.up * deltaY;

        if (Camera.main != null)
            rect.rotation = Camera.main.transform.rotation;
    }

}
