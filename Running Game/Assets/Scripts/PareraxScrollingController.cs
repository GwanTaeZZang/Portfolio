using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PareraxScrollingController : MonoBehaviour
{
    private SpriteRenderer[] spriteArr;
    private int count;
    private int speed;
    private float minXPos;
    private float maxXPos;
    private float repositionX;
    private float biggerObjectXSize;
    private float moveAmount;
    

    private void Awake()
    {
        spriteArr = transform.GetComponentsInChildren<SpriteRenderer>();
        count = spriteArr.Length;
        speed = spriteArr[0].sortingOrder;
        minXPos = spriteArr[0].transform.position.x;
        maxXPos = spriteArr[count-1].transform.position.x;
        repositionX = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;

        //Debug.Log(maxXPos - minXPos);
        //Debug.Log(repositionX);
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
            if(biggerObjectXSize < spriteArr[i].sprite.texture.texelSize.x)
            {
                biggerObjectXSize = spriteArr[i].sprite.bounds.size.x * spriteArr[i].transform.localScale.x;
                //Debug.Log(biggerObjectXSize);
            }
        }

        if(minXPos < 0)
        {
            moveAmount = maxXPos - minXPos;
        }
        else if(minXPos >= 0)
        {
            moveAmount = maxXPos + minXPos;
        }
    }

    private void Update()
    {
        for(int i =0; i < count; i++)
        {
            Transform trans = spriteArr[i].transform;

            trans.Translate(speed * Time.deltaTime * -0.5f, 0, 0);
            if (trans.position.x < repositionX - biggerObjectXSize)
            {
                trans.position = new Vector2(moveAmount + trans.position.x + biggerObjectXSize, trans.position.y);
            }
        }
    }
}
