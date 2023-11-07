using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sprite의 위치 및 크기 재조절 컴포넌트
/// </summary>
public class SpriteMaskResizer : MonoBehaviour
{
    /// <summary>
    /// Sprite Mask가 추적할 UI의 RectTransform
    /// </summary>
    [SerializeField] RectTransform rect;

    void Start()
    {
        resize();
    }

    /// <summary>
    /// 위치 및 크기 재조절
    /// </summary>
    void resize()
    {
        Vector3[] v = new Vector3[4];
        if(rect == null ) {
            return;
        }
        rect.GetWorldCorners(v);

        Vector3 center = (v[0] + v[1] + v[2] + v[3]) / 4f;
        Vector3 size = new Vector3(
            Vector3.Distance(v[1], v[2]) * 2,
            Vector3.Distance(v[0], v[1]) * 2,
            v[0].z
        );
        // V0 : -1, -1
        // V1 : -1, 1
        // V2 : 1, 1
        // V3 : 1, -1 

        transform.position = center;
        transform.localScale = size;
    }
}
