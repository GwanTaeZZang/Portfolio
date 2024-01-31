using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PareraxScrollingController : MonoBehaviour
{
    private const int XPOS_GAP = 1;
    private SpriteRenderer[] spriteArr;
    private int count;
    private int speed;
    private float minXPos;
    private float maxXPos;

    private void Awake()
    {
        spriteArr = transform.GetComponentsInChildren<SpriteRenderer>();
        count = spriteArr.Length;
        speed = spriteArr[0].sortingOrder;
        minXPos = spriteArr[0].transform.position.x;
        maxXPos = spriteArr[count-1].transform.position.x;
    }

    private void Start()
    {
        for(int i =0; i < count; i++)
        {
            float xPos = spriteArr[i].transform.position.x;
            if (xPos > maxXPos)
            {
                maxXPos = xPos;
            }
            if (xPos < minXPos)
            {
                minXPos = xPos;
            }
        }
    }

    private void Update()
    {
        for(int i =0; i < count; i++)
        {
            Transform trans = spriteArr[i].transform;

            trans.Translate(speed * Time.deltaTime * -1, 0, 0);
            if (trans.position.x < minXPos)
            {
                trans.position = new Vector2(maxXPos, trans.position.y);
            }
        }
    }
}
