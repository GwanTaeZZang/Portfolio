using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PareraxScrollingController : MonoBehaviour
{
    private const float HALF = 0.5f;

    private SpriteRenderer[] spriteArr;
    private int count;
    private int speed;
    private float minPosX;
    private float maxPosX;
    private float repositionX;
    private float biggerObjectSizeX;
    private float moveAmount;
    

    private void Awake()
    {
        spriteArr = transform.GetComponentsInChildren<SpriteRenderer>();
        count = spriteArr.Length;
        speed = spriteArr[0].sortingOrder;
        minPosX = spriteArr[0].transform.position.x;
        maxPosX = spriteArr[count-1].transform.position.x;
        repositionX = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;

        //Debug.Log(maxXPos - minXPos);
        //Debug.Log(repositionX);
    }

    private void Start()
    {
        for(int i =0; i < count; i++)
        {
            float xPos = spriteArr[i].transform.position.x;
            if (xPos > maxPosX)
            {
                maxPosX = xPos;
            }
            if (xPos < minPosX)
            {
                minPosX = xPos;
            }
            if(biggerObjectSizeX < spriteArr[i].sprite.texture.texelSize.x)
            {
                biggerObjectSizeX = spriteArr[i].sprite.bounds.size.x * spriteArr[i].transform.localScale.x;
                //Debug.Log(biggerObjectXSize);
            }
        }

        if(minPosX < 0)
        {
            moveAmount = maxPosX - minPosX;
        }
        else if(minPosX >= 0)
        {
            moveAmount = maxPosX + minPosX;
        }
    }

    private void Update()
    {
        for(int i =0; i < count; i++)
        {
            Transform trans = spriteArr[i].transform;
            Vector2 pos = trans.position;
            pos.x += speed * Time.deltaTime * -HALF;
            trans.position = pos;

            //trans.Translate(speed * Time.deltaTime * -0.5f, 0, 0);
            if (trans.position.x < repositionX - biggerObjectSizeX)
            {
                trans.position = new Vector2(moveAmount + trans.position.x + biggerObjectSizeX, trans.position.y);
            }
        }
    }
}
