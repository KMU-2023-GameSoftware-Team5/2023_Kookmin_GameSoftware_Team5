using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: 이거 보고 업데이트하기: https://www.youtube.com/watch?v=iD1_JczQcFY&ab_channel=CodeMonkey
public class FloatingText : MonoBehaviour
{
    public float destoryTime = 3.0f;
    public TextMesh textMesh;
    public MeshRenderer mr;

    public void Initialize(string text, Color color)
    {
        textMesh.text = text;
        textMesh.color = color;
        mr.sortingLayerName = "Damage";
    }

    // default white
    public void Initialize(string text)
    {
        textMesh.text = text;
        textMesh.color = Color.white;
    }

    void Start()
    {
        Destroy(gameObject, destoryTime);
    }

    private void FixedUpdate()
    {
        if (Camera.main != null)
        {
            transform.rotation = Camera.main.transform.localRotation;
        }
    }
}
