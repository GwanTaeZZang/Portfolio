using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScroller : MonoBehaviour
{
    public int count;
    public float speedRate;

    private void Start()
    {
        count = transform.childCount;
    }

    private void Update()
    {
        transform.Translate(speedRate * -1f * Time.deltaTime, 0, 0);
    }
}
