using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cactus
{
    private const float HALF = 0.5f;

    private Transform cactus;
    private SpriteRenderer cactusRenderer;
    private Sprite[] cactusSpriteArr;
    private float width;
    private float height;
    private float speed;
    private float repositionX;


    public Cactus(SpriteRenderer _cactus, Transform _cactusGroup)
    {
        cactusRenderer = GameObject.Instantiate<SpriteRenderer>(_cactus, _cactusGroup);
        cactus = cactusRenderer.transform;
        cactusSpriteArr = new Sprite[] { Resources.Load<Sprite>("Prefab/Sprite/Cactus/Cactus A"),
                                         Resources.Load<Sprite>("Prefab/Sprite/Cactus/Cactus B"),
                                         Resources.Load<Sprite>("Prefab/Sprite/Cactus/Cactus C")};
        speed = cactusRenderer.sortingOrder;
        repositionX = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        SetVisible(false);
        }

    public void MoveCactus()
    {
        if(cactus.position.x + width * HALF < repositionX)
        {
            SetVisible(false);
        }
        else
        {
            Vector2 pos = cactus.position;
            pos.x += Time.deltaTime * speed * -HALF;
            cactus.position = pos;
        }
    }

    private void SetWidth(float _width)
    {
        width = _width;
    }

    private void SetHeight(float _height)
    {
        height = _height;
    }

    public void SetVisible(bool _visible)
    {
        cactus.gameObject.SetActive(_visible);
    }

    public void SetPosition(Floor _floor)
    {
        ChangeRandomSprite();
        width = cactusRenderer.bounds.size.x;
        height = cactusRenderer.bounds.size.y;

        //Debug.Log("스프라이트 : " + cactusRenderer.sprite + "  높이 : " + height + "  길이 : " + width);

        AABB aabb = _floor.GetAABB();
        Vector2 pos = cactus.position;
        //pos.x = aabb.pos.x + GetRandomValue(aabb.pos.x - aabb.width * HALF, aabb.pos.x + aabb.width * HALF);
        //pos.y = (aabb.pos.y + aabb.height * HALF) + height * HALF;
        //Debug.Log(GetRandomValue(aabb.pos.x - (aabb.width * HALF + 1), aabb.pos.x + (aabb.width * HALF - 1)));
        //int randPos = GetRandomValue(aabb.width * HALF - 1, aabb.width * HALF + 1);
        //Debug.Log(randPos);
        pos.x = GetRandomValue((aabb.pos.x + aabb.width * -HALF) + width * HALF + 1, (aabb.pos.x + aabb.width * HALF) - width * HALF);
        pos.y = (aabb.pos.y + aabb.height * HALF) + height * HALF;
        cactus.position = pos;

        SetVisible(true);


        //AABB aabb = _floor.GetAABB();
        //if(aabb.width > 3)
        //{
        //    Vector2 pos = cactus.position;
        //    pos.x = aabb.pos.x + GetRandomValue(aabb.pos.x - aabb.width * HALF, aabb.pos.x + aabb.width * HALF);
        //    pos.y = (aabb.pos.y + aabb.height * HALF) + height * HALF;
        //    cactus.position = pos;

        //    SetVisible(true);
        //}
        //else
        //{
        //    return;
        //}
    }

    private void ChangeRandomSprite()
    {
        //int random = Random.Range(0, 4);
        cactusRenderer.sprite = cactusSpriteArr[GetRandomValue(0,3)];
    }

    private int GetRandomValue(float _min, float _max)
    {
        return (int)Random.Range(_min, _max);
    }
}
