using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    [SerializeField] private int rePosRange;
    [SerializeField] private int rePos;
    private void LateUpdate()
    {
        if(transform.position.x > rePosRange)
        {
            return;
        }

        transform.Translate(rePos, 0, 0, Space.Self);
    }
}
