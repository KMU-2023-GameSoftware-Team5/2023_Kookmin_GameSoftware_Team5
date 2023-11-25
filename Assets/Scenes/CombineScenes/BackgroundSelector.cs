using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSelector : MonoBehaviour
{
    private void Start()
    {
        int rand_idex = UnityEngine.Random.Range(0, transform.childCount);
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == rand_idex);
        }
    }
}
