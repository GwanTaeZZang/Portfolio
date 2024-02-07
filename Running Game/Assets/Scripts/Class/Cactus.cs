using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cactus
{
    private const float HALF = 0.5f;
    private const int SPRITE_RANDOM_MIN = 0;
    private const int SPRITE_RANDOM_MAX = 3;
    private const int CACTUS_CORRECTION_POS_MIN = 2;
    private const int CACTUS_CORRECTION_POS_MAX = -1;
    private const int CACTUS_C_FLOOR_MINIMUM = 6;
    private const int CREATE_POS_X = -5;
    private const int CREATE_POS_Y = -5;

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
        cactus.position = new Vector2(CREATE_POS_X, CREATE_POS_Y);

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

    public Vector2 GetPos()
    {
        return cactus.position;
    }

    public float GetCactusWidth()
    {
        return width;
    }

    public AABB GetCactusAABB()
    {
        AABB aabb;
        aabb.pos = cactus.position;
        aabb.width = width;
        aabb.height = height;

        return aabb;
    }

    //private void SetWidth(float _width)
    //{
    //    width = _width;
    //}

    //private void SetHeight(float _height)
    //{
    //    height = _height;
    //}

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

        if(cactusRenderer.sprite.name == "Cactus C" && aabb.width < CACTUS_C_FLOOR_MINIMUM)
        {
            return;
        }

        Vector2 pos = cactus.position;
        pos.x = GetRandomValue((aabb.pos.x + aabb.width * -HALF) + width * HALF + CACTUS_CORRECTION_POS_MIN, (aabb.pos.x + aabb.width * HALF) - width * HALF + CACTUS_CORRECTION_POS_MAX);
        pos.y = (aabb.pos.y + aabb.height * HALF) + height * HALF;
        cactus.position = pos;

        SetVisible(true);
    }

    private void ChangeRandomSprite()
    {
        //int random = Random.Range(0, 4);
        cactusRenderer.sprite = cactusSpriteArr[GetRandomValue(SPRITE_RANDOM_MIN, SPRITE_RANDOM_MAX)];
    }

    private int GetRandomValue(float _min, float _max)
    {
        return (int)Random.Range(_min, _max);
    }
}
